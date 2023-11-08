using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Services;
using Infrastructure.Logging;
using Presentation.Web.Context;

using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using WebConfig = Domain.Infrastructure.WebConfigValues;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controller principal MGF
    /// </summary>
    [SiteContext]
    public class MainController : GlobalController
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IMailAppServices appMail = new MailAppServices();
        private readonly Commons active = new Commons();

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {
                if (WebConfig.IsAccesoPublico)
                {
                    Session.Abandon();
                    Session.Clear();
                    Response.Cookies.Clear();

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["profile"]))
                    {
                        int profile = int.Parse(Request.QueryString["profile"]);

                        if (profile == (int)Enums.TipoAcceso.ClaveUnica)
                        {
                            return RedirectToRoute("VistaPublicaAcceso");
                        }
                    }

                    return RedirectToRoute("LoginAnonymous");

                }
                else
                {
                    SsoActionResult sso = new SsoActionResult();
                    sso.ExecuteResult(ControllerContext);

                    bool acceso = sso.IsTDPI() || sso.IsINAPI() || sso.IsSAG() || sso.IsAdministrador();
                    if (!acceso)
                        return Redirect(WebConfig.LogOffAuthenticationSystem);

                    if (sso.IsAdministrador() && !sso.IsTDPI())
                        return RedirectToRoute("ActionInitialAdmin");

                    return RedirectToRoute("EscritorioCausas");
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                if (WebConfig.IsAppDebug)
                {
                    throw;
                }
                else
                {
                    return RedirectToAction("Mensaje");
                }

            }

        }


        /// <summary>
        /// Dashboard Externo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Dashboard()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool IsInvitado = sso.IsInvitado();
            bool IsClaveUnica = sso.IsClaveUnica();
            bool IsRequiereMail = false;
            bool IsINAPI = sso.IsINAPI();
            bool IsSAG = sso.IsSAG();
            bool IsAbogado = sso.IsAbogado();

            bool acceso = (IsInvitado || IsINAPI || IsSAG || IsAbogado);
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            if (IsClaveUnica)
                IsRequiereMail = (sso.UserActive.UsuarioID > 0 && string.IsNullOrWhiteSpace(sso.UserActive.Mail));

            IList<DTO.Models.TipoCausa> TipoCausaList = appCommon.GetTipoCausa(true);

            ViewBag.IsRequiereMail = IsRequiereMail;
            ViewBag.TipoCausa = active.GetTipoCausaByUserActive(TipoCausaList, sso);

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetMiFirma()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool IsTDPI = sso.IsTDPI();

            bool acceso = (IsTDPI || !WebConfig.IsAccesoPublico);
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            try
            {
                DTO.Models.Usuario model = sso.UserActive;
                model.SignerEncrypted = string.Empty;

                DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(1);

                string signer = model.Signer.Trim();

                if (!string.IsNullOrWhiteSpace(signer))
                {
                    try
                    {
                        Infrastructure.Utils.TheHash xEncode = new Infrastructure.Utils.TheHash(enc.Cadena.Trim());

                        model.SignerEncrypted = xEncode.DecryptData(model.Signer.Trim());
                    }
                    catch
                    {
                        model.SignerEncrypted = string.Empty;
                    }
                }

                return PartialView("_MiFirma", model);

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// SaveMiFirma
        /// </summary>
        /// <param name="SignerUserActive"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveMiFirma(string SignerUserActive)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool IsTDPI = sso.IsTDPI();

            bool acceso = (IsTDPI || !WebConfig.IsAccesoPublico);
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(1);
                Infrastructure.Utils.TheHash xEncode = new Infrastructure.Utils.TheHash(enc.Cadena);

                string EncodeEnd = active.GetStringValueForm(Request.Form["SignerUserActive"]);
                string strEncode = xEncode.EncryptData(EncodeEnd);

                appCommon.SaveSigner(UsuarioActive, strEncode);

                dbLog.TipoLog = Enums.TipoLog.SaveSigner;
                dbLog.Save();

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }


        #region Email Usuario externo
        /// <summary>
        /// Cambiar email para ciudadnao Logueado.
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MisDatos(string lblEmail)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                bool IsUsuarioRegistrado = (UsuarioActive > 0);
                DTO.Models.Usuario model = new DTO.Models.Usuario();

                if (WebConfig.IsAccesoPublico && IsUsuarioRegistrado)
                {
                    model = appCommon.GetUsuarios().FirstOrDefault(x => x.UsuarioID == UsuarioActive);
                }

                ViewBag.lblEmail = lblEmail;

                return PartialView("_CambiarEmail", model);

            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Actualizar Email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailOld"></param>
        /// <param name="telefono"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveMisDatos(string email, string emailOld, string telefono = "")
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                bool IsCiudadanoActive = (UsuarioActive > 0);
                if (WebConfig.IsAccesoPublico && IsCiudadanoActive)
                {
                    DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioActive);
                    user.Mail = email;
                    user.Telefono = telefono.Trim();
                    user.UsuarioID = appCommon.SaveUser(user);

                    dbLog.Error = emailOld;
                    dbLog.TipoLog = Enums.TipoLog.CambiarEmailExterno;
                    dbLog.Save();
                }

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        #endregion

        #region Registro Abodago

        /// <summary>
        /// Cambiar email para ciudadnao Logueado.
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RegistroAbogado()
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                bool IsUsuarioRegistrado = (UsuarioActive > 0);
                DTO.Models.Usuario model = new DTO.Models.Usuario();

                if (WebConfig.IsAccesoPublico && IsUsuarioRegistrado)
                {
                    model = appCommon.GetUsuarios().FirstOrDefault(x => x.UsuarioID == UsuarioActive);
                }

                ViewBag.FormatosPermitidos = appCommon.GetTipoDocumentoAdjuntoByID((int)Enums.TipoDocumento.CertificadoTituloAbogado);

                ViewBag.IsAbogado = model.IsAbogado();

                return PartialView("_RegistroAbogado", model);

            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Actualizar Email
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SolicitudRegistroAbogado()
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
                        
            try
            {
                bool IsSolicita = active.GetBoolValueForm(Request.Form["IsSolicitaHabilitacion"]);
                string Telefono = active.GetStringValueForm(Request.Form["Telefono"]);
                bool IsTDPI = sso.IsTDPI();
                bool IsSAG = sso.IsSAG();
                bool PuedeSolicitar = (IsSolicita && (!IsTDPI && !IsSAG));

                if (WebConfig.IsAccesoPublico && PuedeSolicitar)
                {
                    DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioActive);
                    user.Telefono = Telefono.Trim();
                    user.UsuarioID = appCommon.SaveUser(user);

                    IList<DTO.Models.AsocDocumentoUsuario> asoc= appCommon.GetAsocDocumentoUsuario(UsuarioActive);
                    string hash = asoc.OrderByDescending(x => x.AsocDocumentoUsuarioID).FirstOrDefault().DocumentoSistema.Hash.Trim();

                    appMail.SolicitudRegistroAbogado(UsuarioActive, hash);
                    
                    dbLog.TipoLog = Enums.TipoLog.RegistroAbogado;
                    dbLog.Save();
                }

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        //

        #endregion
    }
}