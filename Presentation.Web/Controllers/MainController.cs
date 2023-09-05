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

                    return RedirectToRoute("VistaPublicaAcceso");

                }
                else
                {
                    SsoActionResult sso = new SsoActionResult();
                    sso.ExecuteResult(ControllerContext);

                    bool acceso = sso.IsTDPI() || sso.IsINAPI() || sso.IsSAG() || sso.IsAdministrador();
                    if (!acceso)
                        return Redirect(WebConfig.LogOffAuthenticationSystem);

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

            bool acceso = (IsInvitado || IsINAPI || IsSAG);
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            if (IsClaveUnica)
                IsRequiereMail = (sso.UserActive.UsuarioID > 0 && string.IsNullOrWhiteSpace(sso.UserActive.Mail));

            IList<DTO.Models.TipoCausa> TipoCausaList = appCommon.GetTipoCausa(true);
            IList<DTO.Models.TipoCausa> TipoCausaFilter = new List<DTO.Models.TipoCausa>();

            #region Tipos Permitidos
            List<int> TiposPermitidosINAPI = new List<int>();
            TiposPermitidosINAPI.Add((int)Enums.TipoCausa.Marca);
            TiposPermitidosINAPI.Add((int)Enums.TipoCausa.RecursoHechoMarca);
            TiposPermitidosINAPI.Add((int)Enums.TipoCausa.RecursoHechoPatente);
            TiposPermitidosINAPI.Add((int)Enums.TipoCausa.Patente);
            TiposPermitidosINAPI.Add((int)Enums.TipoCausa.ProteccionSuplementaria);

            List<int> TiposPermitidosSAG = new List<int>();
            TiposPermitidosSAG.Add((int)Enums.TipoCausa.VariedadVegetal);
            #endregion

            if (IsINAPI)
            {
                TipoCausaFilter = TipoCausaList.Where(x => TiposPermitidosINAPI.Contains(x.TipoCausaID)).ToList();
            }

            if (IsSAG)
            {
                TipoCausaFilter = TipoCausaList.Where(x => TiposPermitidosSAG.Contains(x.TipoCausaID)).ToList();
            }

            ViewBag.IsRequiereMail = IsRequiereMail;
            ViewBag.TipoCausa = TipoCausaFilter;

            return View();
        }

        #region Email Usuario externo
        /// <summary>
        /// Cambiar email para ciudadnao Logueado.
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CambiarEmail(string lblEmail)
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
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveCambiarEmail(string email, string emailOld)
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
                    DTO.Models.Usuario user = appCommon.GetUsuarios().FirstOrDefault(x => x.UsuarioID == UsuarioActive);
                    user.Mail = email;

                    appCommon.SaveUser(user);

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
    }
}