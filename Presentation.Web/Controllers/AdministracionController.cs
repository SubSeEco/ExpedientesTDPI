using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Logging;
using Application.Services;
using Presentation.Web.Context;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using Infrastructure.Utils.Extensions;
using Domain.Infrastructure;
using System.Text;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controller para acciones de adinistración/mantenedores.
    /// </summary>
    [SiteContext]
    public class AdministracionController : GlobalController
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();

        private readonly Commons active = new Commons();
        
        #region Mantenedor de usuarios y perfiles
        /// <summary>
        /// Conjunto de acciones para gestionar los Usuarios y Perfiles del sistema
        /// </summary>
        /// <returns></returns>
        public ActionResult Usuarios()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");
            
            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                ViewBag.Perfiles = appCommon.GetPerfil(true);

                return View();
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Usuarios y Perfiles del sistema
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="PerfilID"></param>
        /// <param name="TipoAcceso"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetUsers(string Usuario, int PerfilID, int TipoAcceso)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;


            IList<DTO.UserGrid> lista = new List<DTO.UserGrid>();

            try
            {
                var usuarios = appCommon.GetUsuarios();

                IList<DTO.Models.Usuario> filter =
                    (from u in usuarios
                     where
                      //title.Contains("string", StringComparison.OrdinalIgnoreCase)
                      (string.IsNullOrEmpty(Usuario) || (!string.IsNullOrEmpty(Usuario) && u.Nombres.ToUpper().Contains(Usuario.ToUpper()))) &&
                      (PerfilID == 0 || (PerfilID != 0 && u.AsocUsuarioPerfil.Any(x => x.PerfilID == PerfilID)))
                     select u).ToList();

                foreach (var item in filter)
                {
                    DTO.UserGrid dto = new DTO.UserGrid();
                    dto.UsuarioID = item.UsuarioID;
                    dto.AdID = item.AdID;
                    dto.Rut = item.Rut;
                    dto.Nombres = item.Nombres;
                    dto.Apellidos = item.Apellidos;
                    dto.Perfiles = item.GetPerfiles();
                    dto.IsClaveUnica = item.IsClaveUnica;

                    dto.User = (item.IsClaveUnica) ? dto.GetRUT() : dto.AdID;
                    
                    #region bt1: Editar
                    Link a = new Link();
                    a.title = "Editar";
                    a.xicon = "x-icon x-icon-user2";
                    a.href = "javascript:;";
                    a.click = "GetUser(" + item.UsuarioID + ", this)";
                    a.data_txt = "";
                    string bt4 = a.Generate(false);

                    StringBuilder str = new StringBuilder();
                    str.Append(bt4);

                    #endregion

                    dto.Actions = str.ToString();

                    lista.Add(dto);
                }

                JsonResult jsonResult = Json(lista);
                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                return Json(lista);
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Usuarios y Perfiles del sistema
        /// </summary>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetUser(int UsuarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;


            try
            {
                bool IsNew = (UsuarioID < 0);

                DTO.Models.Usuario dto = new DTO.Models.Usuario();
                dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                dto.UsuarioID = UsuarioID;

                if (!IsNew)
                {
                    var usuarios = appCommon.GetUsuarios();
                    var user = usuarios.FirstOrDefault(x => x.UsuarioID == dto.UsuarioID);
                    if (user != null)
                    {
                        dto = user;
                    }
                }

                ViewBag.TipoGeneroList = appCommon.GetTipoGenero(true);
                ViewBag.Perfiles = appCommon.GetPerfil(true);

                return PartialView("_User", dto);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Usuarios y Perfiles del sistema
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveUser(DTO.Models.Usuario dto)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;            
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                int Update = 0;
                Enums.TipoLog tipoLog = Enums.TipoLog.AgregarUsuario;

                dto.AdID = active.GetStringValueForm(Request.Form["AdID"]); 
                dto.Rut = active.GetIntValueForm(Request.Form["Rut"]); 
                dto.Nombres = active.GetStringValueForm(Request.Form["Nombres"]); 
                dto.Apellidos = active.GetStringValueForm(Request.Form["Apellidos"]); 
                dto.Mail = active.GetStringValueForm(Request.Form["Mail"]); 
                dto.IsClaveUnica = active.GetBoolValueForm(Request.Form["IsClaveUnica"]); 
                

                if (dto.UsuarioID >= 0)
                {
                    appCommon.DeletePerfilesUser(dto.UsuarioID);
                    tipoLog = Enums.TipoLog.EditarUsuario;
                    dto.FechaModificacion = ahora;
                    dto.FechaRegistro = DateTime.Parse(Request.Form["FechaRegistro"]);
                }
                else
                {
                    dto.FechaRegistro = ahora;
                    dto.FechaModificacion = ahora;
                }

                dto.UsuarioID = appCommon.SaveUser(dto);

                if (active.IsInputValue(Request.Form["PerfilIDs"]))
                {
                    int[] PerfilIDs = Array.ConvertAll(Request.Form.GetValues("PerfilIDs"), int.Parse);

                    for (int i = 0; i < PerfilIDs.Length; i++)
                    {
                        DTO.Models.AsocUsuarioPerfil asoc = new DTO.Models.AsocUsuarioPerfil();
                        asoc.AsocUsuarioPerfilID = 0;
                        asoc.UsuarioID = dto.UsuarioID;
                        asoc.PerfilID = PerfilIDs[i];

                        appCommon.SaveAsocPerfilUsuario(asoc);
                    }
                }

                dbLog.TipoLog = tipoLog;
                dbLog.Save();

                return Json(new { Update = Update });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        #endregion
        
        #region Mantenedor de MimeTypes

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TipoFormato(int id = 0)
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                var lista = appCommon.GetTipoFormato();

                ViewBag.Items = lista;
                ViewBag.clickTipoFormato = id;

                return View();
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetListaTipoFormato()
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            try
            {
                IList<DTO.Models.TipoFormato> TipoFormatos = appCommon.GetTipoFormato();

                IList<DTO.ListaTipoFormato> list = new List<DTO.ListaTipoFormato>();

                foreach (var item in TipoFormatos)
                {
                    DTO.ListaTipoFormato dto = new DTO.ListaTipoFormato();
                    dto.TipoFormatoID = item.TipoFormatoID;
                    dto.Descripcion = item.Descripcion;
                    dto.ExtraCss = item.ExtraCss;
                    dto.Vigente = item.Vigente;

                    Link a = new Link();

                    #region bt2: Editar
                    a.title = "Editar";
                    a.xicon = "x-icon-edit";
                    a.style = "margin-left:3px";
                    a.href = "javascript:;";
                    a.click = "_NewEditTipoFormato(" + item.TipoFormatoID + ")";
                    string bt2 = a.Generate(false);
                    #endregion

                    #region bt3: MimeTypes
                    a.title = "MimeTypes de " + item.Descripcion;
                    a.xicon = "x-icon x-icon-org";
                    a.href = "javascript:;";
                    a.click = "GetMimeTypesByTipoFormato(" + item.TipoFormatoID + ", this);seleccionarTR(this)";
                    a.data_txt = item.Descripcion.Trim();
                    a.id = "btMimeTypes_TipoFormatoID_" + item.TipoFormatoID;
                    string bt3 = a.Generate(false);
                    a.data_txt = "";
                    #endregion

                    #region bt4: Eliminar

                    #endregion

                    #region StringBuilder

                    StringBuilder str = new StringBuilder();
                    str.Append(bt2); //Editar
                    str.Append(bt3); //MimeTypes
                                     //str.Append(bt4); //Eliminar

                    #endregion

                    dto.Acciones = str.ToString();
                    list.Add(dto);
                }

                return Json(list);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="TipoFormatoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewEditTipoFormato(int TipoFormatoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.TipoFormato tipoFormato = new DTO.Models.TipoFormato();

                if (TipoFormatoID != 0)
                {
                    tipoFormato = appCommon.GetTipoFormato().FirstOrDefault(x => x.TipoFormatoID == TipoFormatoID);
                    tipoFormato.Descripcion = tipoFormato.Descripcion.Trim();
                }
                else
                {
                    tipoFormato.TipoFormatoID = 0;
                }

                return PartialView("_NewEditTipoFormato", tipoFormato);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="tipoFormato"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult SaveTipoFormato(DTO.Models.TipoFormato tipoFormato)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                dbLog.TipoLog = (tipoFormato.TipoFormatoID != 0) ? TipoLog.ActualizarTipoFormato : TipoLog.AgregarTipoFormato;

                if (tipoFormato.TipoFormatoID == 0 && ExistsValidation(0, tipoFormato.Descripcion.Trim(), 0))
                {
                    return Json(new { result = (int)Enums.ReturnJson.YaExisteDescripcion });
                }

                tipoFormato.Descripcion = active.GetStringValueForm(Request.Form["Descripcion"]);
                tipoFormato.ExtraCss = active.GetStringValueForm(Request.Form["ExtraCss"]);

                tipoFormato.TipoFormatoID = appCommon.SaveTipoFormato(tipoFormato);
                dbLog.Save(IncluyeParametros: false);

                return Json(new
                {
                    result = (int)Enums.ReturnJson.ActionSuccess,
                    TipoFormatoID = tipoFormato.TipoFormatoID
                });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="TipoFormatoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteTipoFormato(int TipoFormatoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                appCommon.DeleteTipoFormato(TipoFormatoID);
                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="TipoFormatoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetListaMimeTypes(int? TipoFormatoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.FamiliasMimeType> FamiliasMimeType = appCommon.GetFamiliasMimeType().Where(x => x.TipoFormatoID == TipoFormatoID).ToList();

                IList<DTO.ListaMimeTypes> list = new List<DTO.ListaMimeTypes>();

                foreach (var item in FamiliasMimeType)
                {
                    DTO.ListaMimeTypes dto = new DTO.ListaMimeTypes();
                    dto.FamiliasMimeTypeID = item.FamiliasMimeTypeID;
                    dto.Descripcion = item.Descripcion;
                    dto.Vigente = item.Vigente;

                    Link a = new Link();

                    #region bt2: Editar
                    a.title = "Editar";
                    a.xicon = "x-icon-edit";
                    a.style = "margin-left:3px";
                    a.href = "javascript:;";
                    a.click = "_NewEditMimeType(" + item.TipoFormatoID + "," + item.FamiliasMimeTypeID + ")";
                    string bt2 = a.Generate(false);
                    #endregion

                    #region bt4: Eliminar
                    a.title = "Eliminar";
                    a.xicon = "x-icon x-icon-delete";
                    a.href = "javascript:;";
                    a.click = "_DeleteMimeType(" + item.FamiliasMimeTypeID + ", this)";
                    string bt4 = a.Generate(false);
                    a.data_txt = "";
                    #endregion

                    StringBuilder str = new StringBuilder();
                    str.Append(bt2); //Editar
                    str.Append(bt4); //Eliminar

                    dto.Acciones = str.ToString();
                    list.Add(dto);
                }

                return Json(list);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="TipoFormatoID"></param>
        /// <param name="FamiliasMimeTypeID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewEditMimeType(int TipoFormatoID, int FamiliasMimeTypeID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.FamiliasMimeType mimeType = new DTO.Models.FamiliasMimeType();

                if (FamiliasMimeTypeID != 0)
                {
                    mimeType = appCommon.GetMimeType(FamiliasMimeTypeID);
                    mimeType.Descripcion = mimeType.Descripcion.Trim();
                }
                else
                {
                    mimeType.TipoFormatoID = TipoFormatoID;
                    mimeType.FamiliasMimeTypeID = 0;
                }

                return PartialView("_NewEditMimeType", mimeType);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="familiasMimeType"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult SaveMimetype(DTO.Models.FamiliasMimeType familiasMimeType)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                dbLog.TipoLog = (familiasMimeType.FamiliasMimeTypeID != 0) ? TipoLog.ActualizarFamiliaMimeType : TipoLog.AgregarFamiliaMimeType;

                if (familiasMimeType.FamiliasMimeTypeID == 0 && ExistsValidation(1, familiasMimeType.Descripcion.Trim(), familiasMimeType.TipoFormatoID))
                {
                    return Json(new { result = (int)Enums.ReturnJson.YaExisteDescripcion });
                }

                familiasMimeType.Descripcion = active.GetStringValueForm(Request.Form["Descripcion"]);

                familiasMimeType.FamiliasMimeTypeID = appCommon.SaveMimeType(familiasMimeType);
                dbLog.Save();

                return Json(new
                {
                    result = (int)Enums.ReturnJson.ActionSuccess,
                    FamiliasMimeTypeID = familiasMimeType.FamiliasMimeTypeID
                });


            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }

        /// <summary>
        /// Conjunto de acciones para gestionar los Tipos de Formato (MimeTypes) 
        /// para los los ocumentos adjuntos del sistema.
        /// </summary>
        /// <param name="FamiliasMimeTypeID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteMimeType(int FamiliasMimeTypeID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                appCommon.DeleteMimeType(FamiliasMimeTypeID);
                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }

        private bool ExistsValidation(int TipoValidacion, string Descripcion, int? TipoFormatoID)
        {

            //Tipo Validación: 
            // 0 = TiposFormatos
            // 1 = FamiliasMimeType           
            bool existe;

            if (TipoValidacion == 0)
            {
                DTO.Models.TipoFormato tipoFormato;
                tipoFormato = appCommon.GetTipoFormato().FirstOrDefault(x => x.Descripcion.Trim() == Descripcion.Trim());
                existe = (tipoFormato != null);
            }
            else
            {
                IList<DTO.Models.FamiliasMimeType> FamiliasMimeType = appCommon.GetFamiliasMimeType().Where(x => x.TipoFormatoID == TipoFormatoID).ToList();
                DTO.Models.FamiliasMimeType mimeType = FamiliasMimeType.FirstOrDefault(x => x.Descripcion.Trim() == Descripcion.Trim());
                existe = (mimeType != null);
            }

            return existe;

        }

        #endregion

        #region Mantendor Tipo Documento

        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <returns></returns>
        public ActionResult MantenedorTipoDocumento()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");
            
            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                var listaTipoDocumento = appCommon.GetTipoDocumento();

                int[] permitidos = new int[]
                                    {
                                        (int)Enums.TipoDocumento.Causa,
                                        (int)Enums.TipoDocumento.Escrito,
                                        (int)Enums.TipoDocumento.Expediente
                                    };

                var TipoDocumento = listaTipoDocumento.Where(x => permitidos.Contains(x.TipoDocumentoID) && x.Visible && x.Vigente).ToList();

                ViewBag.TipoDocumento = TipoDocumento;

                return View();
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <param name="TipoDocumentoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleTipoDocumento(int TipoDocumentoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.AsocTipoDocumentoAdjunto> AsocTipoDocumentoAdjunto = appCommon.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);

                ViewBag.AsocTipoDocumentoAdjunto = AsocTipoDocumentoAdjunto;
                ViewBag.TipoDocumentoID = TipoDocumentoID;

                return PartialView("_DetalleTipoDocumento");
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <param name="AsocTipoDocumentoAdjuntoID"></param>
        /// <param name="DocumentoAdjuntoID"></param>
        /// <param name="TipoDocumentoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDocumentoAdjunto(int AsocTipoDocumentoAdjuntoID, int DocumentoAdjuntoID, int TipoDocumentoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.AsocTipoDocumentoAdjunto> AsocTipoDocumentoAdjunto = appCommon.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);

                DTO.Models.AsocTipoDocumentoAdjunto asoc = new DTO.Models.AsocTipoDocumentoAdjunto();
                DTO.Models.DocumentoAdjunto doc = new DTO.Models.DocumentoAdjunto();

                bool IsNew = (AsocTipoDocumentoAdjuntoID == 0);

                if (IsNew)
                {
                    DTO.Models.VersionEncript _enc = appCommon.GetLastVersionEncript();

                    asoc.AsocTipoDocumentoAdjuntoID = 0;
                    asoc.TipoDocumentoID = TipoDocumentoID;
                    asoc.DocumentoAdjuntoID = DocumentoAdjuntoID;

                    doc.DocumentoAdjuntoID = 0;
                    doc.VersionEncriptID = _enc.VersionEncriptID;
                    doc.MaximoTamanoArchivoID = 0;
                    doc.TipoFormatoID = 0;
                    doc.NombreDocumento = "";
                    doc.Descripcion = "";
                    doc.Hash = "";
                    doc.NombreArchivoFisico = "";
                }
                else
                {
                    asoc = AsocTipoDocumentoAdjunto.FirstOrDefault(x => x.AsocTipoDocumentoAdjuntoID == AsocTipoDocumentoAdjuntoID);
                    doc = asoc.DocumentoAdjunto;
                }

                var MaximoTamanoArchivo = appCommon.GetMaximoTamanoArchivo();
                var TipoFormato = appCommon.GetTipoFormato();

                ViewBag.MaximoTamanoArchivo = MaximoTamanoArchivo;
                ViewBag.TipoFormato = TipoFormato;

                ViewBag.AsocTipoDocumentoAdjunto = asoc;

                return PartialView("_DocumentoAdjunto", doc);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveDocumentoAdjunto(DTO.Models.DocumentoAdjunto dto)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                bool IsNew = (dto.DocumentoAdjuntoID == 0);
                                
                int TipoDocumentoID = active.GetIntValueForm(Request.Form["TipoDocumentoID"]);

                dbLog.TipoLog = (!IsNew) ? TipoLog.EditarConfDocumentoAdjunto : TipoLog.AgregarConfDocumentoAdjunto;

                dto.Descripcion = active.GetStringValueForm(Request.Form["Descripcion"]);
                dto.NombreDocumento = dto.Descripcion;
                dto.NombreArchivoFisico = active.GetStringValueForm(Request.Form["NombreArchivoFisico"]);
                dto.Hash = active.GetStringValueForm(Request.Form["Hash"]);


                dto.DocumentoAdjuntoID = appCommon.SaveDocumentoAdjunto(dto);

                if (IsNew)
                {
                    DTO.Models.AsocTipoDocumentoAdjunto asoc = new DTO.Models.AsocTipoDocumentoAdjunto();
                    asoc.AsocTipoDocumentoAdjuntoID = 0;
                    asoc.DocumentoAdjuntoID = dto.DocumentoAdjuntoID;
                    asoc.TipoDocumentoID = TipoDocumentoID;

                    asoc.AsocTipoDocumentoAdjuntoID = appCommon.SaveAsocTipoDocumentoAdjunto(asoc);
                }

                dbLog.Save();

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <param name="AsocTipoDocumentoAdjuntoID"></param>
        /// <param name="DocumentoAdjuntoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteDocumentoAdjuntoConf(int AsocTipoDocumentoAdjuntoID, int DocumentoAdjuntoID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                appCommon.DeleteDocumentoAdjunto(DocumentoAdjuntoID, AsocTipoDocumentoAdjuntoID);

                dbLog.TipoLog = TipoLog.EliminarConfDocumentoAdjunto;
                dbLog.Save();

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        #endregion

        #region Mantenedor Generico

        /// <summary>
        /// Conjunto de acciones para el mantenedor genérico del sistema
        /// </summary>
        /// <returns></returns>
        public ActionResult MantenedorGenerico()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");
            
            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IConfDescApplicationService appConf = new ConfDescApplicationService();
                IList<DTO.Mantenedor.ConfigurarDescripcion> ConfigurarDescripcion = appConf.GetAll();
                ViewBag.ConfigurarDescripcion = ConfigurarDescripcion;

                return View();
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones para el mantenedor genérico del sistema
        /// </summary>
        /// <param name="codigoObj"></param>
        /// <param name="TableName"></param>
        /// <param name="txtDesc"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult BuscarMantenedor(int codigoObj, string TableName, string txtDesc)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                IConfDescApplicationService app = new ConfDescApplicationService();
                var lista = app.GetDetalleTabla(TableName, codigoObj, txtDesc.ReplaceVocals());

                return Json(lista);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }
        
        /// <summary>
        /// Conjunto de acciones para el mantenedor genérico del sistema
        /// </summary>
        /// <param name="codigoObj"></param>
        /// <param name="TableName"></param>
        /// <param name="txtDesc"></param>
        /// <param name="isVigente"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateMantenedor(int codigoObj, string TableName, string txtDesc, bool isVigente)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IConfDescApplicationService app = new ConfDescApplicationService();

                var obj = new DTO.Mantenedor.ObjetoGenerico();
                obj.Codigo = codigoObj;
                obj.Descripcion = txtDesc;
                obj.Vigente = isVigente;
                obj.TableName = TableName;

                app.UpdateDetalleTabla(obj);
                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }
        
        /// <summary>
        /// Conjunto de acciones para el mantenedor genérico del sistema
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="txtDesc"></param>
        /// <param name="isVigente"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateMantenedor(string TableName, string txtDesc, bool isVigente)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IConfDescApplicationService app = new ConfDescApplicationService();

                var obj = new DTO.Mantenedor.ObjetoGenerico();
                obj.Codigo = 0;
                obj.Descripcion = txtDesc;
                obj.Vigente = isVigente;
                obj.TableName = TableName;

                app.CreateDetalleTabla(obj);
                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>        
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleMantenedorGenerico(int codigoObjeto = 0, string itemDescripcion = "", bool vigente = false)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                string Descripcion = string.Empty;
                bool IsVigente = false;

                if (codigoObjeto > 0 || itemDescripcion != string.Empty)
                {
                    Descripcion = itemDescripcion;
                    IsVigente = vigente;
                }

                ViewBag.codigoObjeto = codigoObjeto;
                ViewBag.Descripcion = Descripcion;
                ViewBag.IsVigente = IsVigente;

                return PartialView("_NewEditMantenedorGenerico");
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        #endregion
        
        #region Mantenedor Opciones Tipo Tramite 

        /// <summary>
        /// Conjunto de acciones para definir Opciones Tipo Tramite del sistema.
        /// </summary>
        /// <returns></returns>
        public ActionResult MantenedorOpcionesTipoTramite()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.TipoTramite> TipoTramiteList = appCommon.GetTipoTramite(SoloVigente: true);
                ViewBag.TipoTramiteList = TipoTramiteList;

                return View();
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

           
        }

        /// <summary>
        /// Conjunto de acciones para definir Opciones Tipo Tramite del sistema.
        /// </summary>
        /// <param name="TipoTramiteID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetOpcionesTipoTramite(int TipoTramiteID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.OpcionesTramite> OpcionesTramiteList = appCommon.GetOpcionesTramite(SoloVigente: true);

                IList<DTO.Models.AsocTipoTramiteOpciones> AsocTipoTramiteOpcionesList = appCommon.GetAsocTipoTramiteOpciones(TipoTramiteID);

                ViewBag.OpcionesTramiteList = OpcionesTramiteList;
                ViewBag.AsocTipoTramiteOpcionesList = AsocTipoTramiteOpcionesList;
                ViewBag.TipoTramiteID = TipoTramiteID;

                return PartialView("_OpcionesTipoTramite");
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para definir Opciones Tipo Tramite del sistema.
        /// </summary>
        /// <param name="AsocTipoTramiteOpcionesID"></param>
        /// <param name="OpcionesTramiteID"></param>
        /// <param name="TipoTramiteID"></param>
        /// <param name="IsTabla"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewEditOpcionesTipoTramite(int AsocTipoTramiteOpcionesID, int OpcionesTramiteID, int TipoTramiteID, bool IsTabla = false)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.AsocTipoTramiteOpciones asoc = new DTO.Models.AsocTipoTramiteOpciones();

                bool IsNew = (AsocTipoTramiteOpcionesID == 0);

                if (!IsNew)
                {
                    asoc = appCommon.GetAsocTipoTramiteOpciones(TipoTramiteID).FirstOrDefault(x => x.AsocTipoTramiteOpcionesID == AsocTipoTramiteOpcionesID);
                }
                else
                {
                    asoc.AsocTipoTramiteOpcionesID = AsocTipoTramiteOpcionesID;
                    asoc.TipoTramiteID = TipoTramiteID;
                    asoc.OpcionesTramiteID = OpcionesTramiteID;
                }

                asoc.IsTabla = IsTabla;
                IList<DTO.Models.EstadoCausa> EstadoCausaList = appCommon.GetEstadoCausa(SoloVigente: true);
                IList<DTO.Models.EstadosAplica> EstadosAplicaList = appCommon.GetEstadosAplicaByAsocTipoTramiteOpciones(AsocTipoTramiteOpcionesID);

                ViewBag.EstadoCausaList = EstadoCausaList;
                ViewBag.EstadosAplicaList = EstadosAplicaList;

                return PartialView("_NewEditOpcionesTipoTramite", asoc);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones para definir Opciones Tipo Tramite del sistema.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveOpcionTipoTramite(DTO.Models.AsocTipoTramiteOpciones dto)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                bool IsNew = (dto.AsocTipoTramiteOpcionesID == 0);
                
                dbLog.TipoLog = (!IsNew) ? TipoLog.EditarTipoTramiteOpciones : TipoLog.AgregarTipoTramiteOpciones;

                dto.Vigente = true;                
                dto.AsocTipoTramiteOpcionesID = appCommon.SaveAsocTipoTramiteOpciones(dto);
                               
                dbLog.Save();

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para definir Opciones Tipo Tramite del sistema.
        /// </summary>
        /// <param name="AsocTipoTramiteOpcionesID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteOpcionTipoTramite(int AsocTipoTramiteOpcionesID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                appCommon.DeleteAsocTipoTramiteOpciones(AsocTipoTramiteOpcionesID);

                dbLog.TipoLog = TipoLog.EditarTipoTramiteOpciones;
                dbLog.Save();

                return Json(0);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// SaveEstadosAplica
        /// </summary>
        /// <param name="EstadoCausaID"></param>
        /// <param name="AsocTipoTramiteOpcionesID"></param>
        /// <param name="TipoTramiteID"></param>
        /// <param name="OpcionesTramiteID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult SaveEstadosAplica(int EstadoCausaID, int AsocTipoTramiteOpcionesID, int TipoTramiteID, int OpcionesTramiteID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.EstadosAplica dto = new Application.DTO.Models.EstadosAplica();
                dto.EstadoCausaID= EstadoCausaID;
                dto.IsSiguiente = false;

                #region AsocTipoTramiteOpcionesID
                if (AsocTipoTramiteOpcionesID == 0)
                {
                    DTO.Models.AsocTipoTramiteOpciones _tipo = new DTO.Models.AsocTipoTramiteOpciones();
                    _tipo.TipoTramiteID = TipoTramiteID;
                    _tipo.OpcionesTramiteID = OpcionesTramiteID;
                    dto.AsocTipoTramiteOpcionesID = appCommon.SaveAsocTipoTramiteOpciones(_tipo);
                }
                else
                {
                    dto.AsocTipoTramiteOpcionesID = AsocTipoTramiteOpcionesID;
                }
                #endregion

                dto.EstadosAplicaID = appCommon.SaveEstadosAplica(dto);
                
                dbLog.TipoLog = TipoLog.AgregarEstadosAplica;
                dbLog.Save();

                return Json(new
                {
                    result = (int)Enums.ReturnJson.ActionSuccess,
                    EstadosAplicaID = dto.EstadosAplicaID,
                    AsocTipoTramiteOpcionesID = dto.AsocTipoTramiteOpcionesID
                });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones apra definir los tipos de documentos del sistema.
        /// </summary>
        /// <param name="EstadoAplicaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteEstadosAplica(int EstadoAplicaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                if (EstadoAplicaID > 0)
                {
                    appCommon.DeleteEstadosAplica(EstadoAplicaID);

                    dbLog.TipoLog = TipoLog.EliminarEstadosAplica;
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

        #region Mantenedor de Ayudas

        /// <summary>
        /// Conjunto de acciones para gestionar las Ayudas del sistema
        /// </summary>
        /// <returns></returns>
        public ActionResult Ayudas()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");
            
            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                IList<DTO.Models.TipoVentana> tipoV = appCommon.GetTipoVentana();

                ViewBag.TipoVentana = tipoV.OrderBy(x => x.Descripcion).ToList();

                return View("Ayudas");

            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones para gestionar las Ayudas del sistema
        /// </summary>
        /// <param name="TipoVentanaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetDetalleVentana(int TipoVentanaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;


            try
            {
                DTO.Models.Ventana ventana = appCommon.GetVentanaByTipoVentanaID(TipoVentanaID);

                return Json(ventana);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones para gestionar las Ayudas del sistema
        /// </summary>
        /// <param name="ventana"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveVentana(DTO.Models.Ventana ventana)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();
            DateTime ahora = DateTime.Now;
            
            DBLogger dbLog = new DBLogger();
            dbLog.TipoLog = Enums.TipoLog.ModificarAyudas;
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                ventana.IntroduccionEspanol = active.GetStringValueForm(Request.Form["__IntroduccionEspanol"]);
                ventana.IntroduccionEspanol = ventana.IntroduccionEspanol.Replace("<p>", "").Replace("</p>", "&nbsp;<br />");

                ventana.IntroduccionIngles = active.GetStringValueForm(Request.Form["__IntroduccionIngles"]);
                ventana.IntroduccionIngles = ventana.IntroduccionIngles.Replace("<p>", "").Replace("</p>", "&nbsp;<br />");

                if (ventana.TipoVentanaID == 0)
                {
                    DTO.Models.TipoVentana _dto = new DTO.Models.TipoVentana();
                    _dto.Descripcion = (!string.IsNullOrEmpty(Request.Form["TipoVentanaDescripcion"])) ? Request.Form["TipoVentanaDescripcion"] : " ";

                    int newTipoVentana = appCommon.SaveTipoVentana(_dto);

                    DTO.Models.Ventana _ventana = new DTO.Models.Ventana();
                    _ventana.TipoVentanaID = newTipoVentana;
                    _ventana.IntroduccionEspanol = ventana.IntroduccionEspanol;
                    _ventana.IntroduccionIngles = ventana.IntroduccionIngles;

                    _ventana.TipoVentanaID = appCommon.SaveVentana(_ventana);

                    return Json(new { Redirect = 1 });
                }

                int save = appCommon.SaveVentana(ventana);

                dbLog.Save(IncluyeParametros: false);

                return Json(new { Redirect = 0 });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }


        }

        #endregion

        #region Mantenedor de Plantillas email


        /// <summary>
        /// Conjunto de acciones para administrar las platillas de Email del sistema.
        /// </summary>
        /// <returns></returns>
        public ActionResult TipoNotificacion()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                ViewBag.TipoNotificacion = appCommon.GetTipoNotificacion().OrderBy(x => x.Descripcion).ToList();

                return View("TipoNotificacion");
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }

        }
        
        /// <summary>
        /// Conjunto de acciones para administrar las platillas de Email del sistema.
        /// </summary>
        /// <param name="TipoNotificacionID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult GetDetalleTipoNotificacion(int TipoNotificacionID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                DTO.Models.TipoNotificacion notificacion = appCommon.GetTipoNotificacionByID(TipoNotificacionID);

                return Json(notificacion);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// Conjunto de acciones para administrar las platillas de Email del sistema.
        /// </summary>
        /// <param name="TipoNotificacionID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetPerfilesNotificacion(int TipoNotificacionID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;
            
            try
            {
                DTO.Models.TipoNotificacion notificacion = appCommon.GetTipoNotificacionByID(TipoNotificacionID);
                ViewBag.Notificacion = notificacion;
                ViewBag.Perfiles = appCommon.GetPerfil(true);

                return PartialView("_PerfilesNotificacion");

            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }
        
        /// <summary>
        /// Conjunto de acciones para administrar las platillas de Email del sistema.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveTipoNotificacion(DTO.Models.TipoNotificacion model)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool acceso = (sso.IsAdministrador());
            if (!acceso) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();
            DateTime ahora = DateTime.Now;
            
            DBLogger dbLog = new DBLogger();
            dbLog.TipoLog = Enums.TipoLog.ModificarTipoNotificacion;
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                model.Mensaje = active.GetStringValueForm(Request.Form["__Mensaje"]);
                model.Mensaje = model.Mensaje.Replace("<p>", "").Replace("</p>", "&nbsp;<br />");
                model.ConCopia = active.GetStringValueForm(Request.Form["ConCopia"]); 

                int TipoNotificacionID = appCommon.SaveTipoNotificacion(model);

                dbLog.Save(IncluyeParametros: false);

                if (active.IsInputValue(Request.Form["PerfilIDs"]))
                {
                    appCommon.DeleteAsocTipoNotificacionPerfil(TipoNotificacionID);

                    int[] PerfilIDs = Array.ConvertAll(Request.Form.GetValues("PerfilIDs"), int.Parse);

                    for (int i = 0; i < PerfilIDs.Length; i++)
                    {
                        DTO.Models.AsocTipoNotificacionPerfil asoc = new DTO.Models.AsocTipoNotificacionPerfil();
                        asoc.AsocTipoNotificacionPerfilID = 0;
                        asoc.TipoNotificacionID = TipoNotificacionID;
                        asoc.PerfilID = PerfilIDs[i];

                       appCommon.SaveAsocTipoNotificacionPerfil(asoc);
                    }
                }
                else
                {
                    appCommon.DeleteAsocTipoNotificacionPerfil(TipoNotificacionID);
                }

                return Json(new { result = (int)Enums.ReturnJson.ActionSuccess, TipoNotificacionID = TipoNotificacionID });
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