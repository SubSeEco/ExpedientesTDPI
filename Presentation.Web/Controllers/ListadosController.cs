using Application.DTO.Utils;
using Application.Services;
using Infrastructure.Logging;
using Newtonsoft.Json;
using Presentation.Web.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using WebConfig = Domain.Infrastructure.WebConfigValues;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [SiteContext]
    public class ListadosController : GlobalController
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IExpedienteAppServices appExpediente = new ExpedienteAppServices();
        private readonly IMailAppServices appMail = new MailAppServices();
        private readonly Commons active = new Commons();
        

        #region Ingreso

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = sso.IsTDPI();
            if (!acceso) return Redirect(WebConfig.LogOffAuthenticationSystem);

            DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
            ViewBag.FiltrosEscritorio = filtros;

            return View("Ingreso/Index", filtros); //Ingreso\Index
        }


        /// <summary>
        /// GetListadoIngresos
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetListadoIngresos(DTO.FiltrosEscritorio filtros)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                int UsuarioActive = sso.GetUsuarioActivoID();

                filtros.UsuarioID = UsuarioActive;
                IList<DTO.Models.Causa> lista = appExpediente.GetCausasByFilter(filtros, Enums.TipoGrid.ListadoIngresos);

                ViewBag.Listado = lista;

                return PartialView("Ingreso/_ListadoIngresos");
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GenerarListadoIngresoPDF(string lista)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            DateTime ahora = DateTime.Now;

            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                Enums.ReturnJson retorno = Enums.ReturnJson.SinAccion;

                var listaArray = active.GetStringValueForm(Request.Form["lista"]);
                int[] listaID = JsonConvert.DeserializeObject<int[]>(listaArray);

                DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
                filtros.ListaID = listaID;

                IList<DTO.Models.Causa> causas = appExpediente.GetCausasByFilter(filtros, Enums.TipoGrid.PDFListadoIngresos);

                #region PDF
                Enums.TipoDocumento tipoDocumento = Enums.TipoDocumento.Ingreso;

                string FolderSave = string.Format("\\Listados\\{0}\\{1}\\{2}\\{3}", ahora.Year, ahora.Month, tipoDocumento, ahora.ToString("HHmmss"));
                string MergePath = WebConfig.PathBaseRepository + FolderSave;
                if (!Directory.Exists(MergePath)) Directory.CreateDirectory(MergePath);

                DocPdf pdf = new DocPdf();
                pdf.Usuario = sso.UserActive;
                pdf.MapPath = Server.MapPath("~");
                pdf.PathSave = MergePath;
                pdf.Filename = $"IN-{ahora.ToString("dd-MM-yyyy")}.pdf";
                pdf.Titulo = "LISTADO DE INGRESOS";
                pdf.SubTitulo = $"Ingresos de fecha {ahora.ToString("dddd, dd MMMM \\de yyyy")}";
                pdf.CreateDocument("TDPI: Listado de Ingreso");

                pdf.Causas = causas;
                pdf.SetListadoIngreso();
                pdf.Render();

                List<string> emails = new List<string>();
                //emails.Add("test@test.cl");
                //emails.Add("test2@test2.cl");
                #endregion


                #region TheHash

                string PathFinalArchivo = Path.Combine(FolderSave, pdf.Filename);

                DTO.Models.VersionEncript enc = appCommon.GetLastVersionEncript();
                TheHash xEncode = new TheHash(enc.Cadena);
                string EncodeEnd = WebConfig.IsLocalRepository + TipoUploadChar + PathFinalArchivo + TipoUploadChar + 0;
                string SHAFile = xEncode.EncryptData(EncodeEnd);
                #endregion


                #region Save in Store

                DTO.Models.DocumentoSistema doc = new DTO.Models.DocumentoSistema();
                doc.DocumentoSistemaID = 0;
                doc.VersionEncriptID = enc.VersionEncriptID;
                doc.TipoDocumentoID = (int)tipoDocumento;
                doc.Hash = SHAFile;
                doc.NombreArchivoFisico = pdf.Filename;
                doc.Fecha = ahora;
                doc.Descripcion = pdf.SubTitulo;

                doc.DocumentoSistemaID = appExpediente.SaveDocumentoSistema(doc);

                #endregion

                #region Notificacion
                //Notificacion Funcionario
                List<string> destinatarios = appMail.ListadoIngresoDiario(doc.Hash);
                foreach (var item in destinatarios)
                {
                    emails.Add(item);
                }
                #endregion


                int TipoTramiteID = (int)Enums.TipoTramite.Actuacion;
                int OpcionesTramiteID = (int)Enums.OpcionesTramite.Tramite;

                for (int i = 0; i < listaID.Length; i++)
                {
                    int CausaID = listaID[i];

                    active.CreateEventoExpediente(CausaID, TipoTramiteID, OpcionesTramiteID, ahora, UsuarioActive);
                }

                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.GenerarIngresoPDF;
                dbLog.Save();
                #endregion

                return Json(new
                {
                    result = (int)retorno,
                    emails = string.Join(", ", emails),
                    vs = doc.VersionEncriptID,
                    hash = doc.Hash,
                    documento = doc.DocumentoSistemaID,
                    lista = listaID
                });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

        #endregion



        #region Tablas


        /// <summary>
        /// Tablas
        /// </summary>
        /// <returns></returns>
        public ActionResult Tablas()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = sso.IsTDPI();
            if (!acceso) return Redirect(WebConfig.LogOffAuthenticationSystem);

            DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
            filtros.Sala = appCommon.GetSala(true);
            filtros.Usuario = appCommon.GetUsuarios();
            filtros.EstadoTabla = appCommon.GetEstadoTabla();

            return View("Tablas/Index", filtros);
        }


        /// <summary>
        /// GetListadoTablas
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetListadoTablas(DTO.FiltrosEscritorio filtros)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                filtros.NumeroTicket = active.GetStringValueForm(filtros.NumeroTicket);

                IList<DTO.Models.Tabla> Tablas = appExpediente.GetTabla(filtros);
                IList<DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                foreach (var item in Tablas)
                {
                    var user = Usuarios.FirstOrDefault(x => x.UsuarioID == item.UsuarioRelatorID);
                    item.NombreRelator = user.GetFullName();

                    if (item.IsGenerado() || item.IsPublicado())
                    {
                        item.DocumentoSistemaTabla = appExpediente.GetAsocDocumentoSistema(item.TablaID, Enums.TipoDocumento.Tabla);
                    }
                }

                ViewBag.Listado = Tablas;

                return PartialView("Tablas/_ListadoTablas");
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetTabla
        /// </summary>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetTabla(int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Tabla model = new Application.DTO.Models.Tabla();
                model.TablaID = TablaID;
                model.Fecha = DateTime.Now;
                model.EstadoTablaID = (int)Enums.EstadoTabla.Borrador;

                if (model.TablaID > 0)
                {
                    model = appExpediente.GetTablaByID(TablaID);
                }

                DTO.DataForm DataForm = new DTO.DataForm();
                DataForm.Sala = appCommon.GetSala(true);
                DataForm.TipoTabla = appCommon.GetTipoTabla(true);
                DataForm.Usuario = appCommon.GetUsuarios();
                ViewBag.DataForm = DataForm;

                return PartialView("Tablas/_Tabla", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// SaveParte
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveTabla(DTO.Models.Tabla dto)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                Enums.ReturnJson retorno;

                dto.TablaID = appExpediente.SaveTabla(dto);
                retorno = Enums.ReturnJson.ActionSuccess;

                return Json(new { result = (int)retorno, TablaID = dto.TablaID});
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetDetalleCausasTabla
        /// </summary>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleCausasTabla(int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                IList<DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                DTO.Models.Tabla tabla = appExpediente.GetTablaByID(TablaID);
                tabla.NombreRelator = Usuarios.FirstOrDefault(x => x.UsuarioID == tabla.UsuarioRelatorID).GetFullName();

                foreach (var item in tabla.DetalleTabla)
                {
                    item.Causa = appExpediente.GetCausa(item.CausaID);
                }

                return PartialView("Tablas/_DetalleCausasTabla", tabla);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetAgregarCausaTabla
        /// </summary>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetAgregarCausaTabla(int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Tabla tabla = appExpediente.GetTablaByID(TablaID);

                return PartialView("Tablas/_AgregarCausaTabla", tabla);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetBuscarCausa
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetBuscarCausa(DTO.FiltrosEscritorio filtros, int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                filtros.NumeroTicket = active.GetStringValueForm(Request.Form["buscaRol"]);
                filtros.Anio = active.GetIntValueForm(Request.Form["buscaAnio"]);

                IList<DTO.Models.Causa> causas = appExpediente.GetCausasByFilter(filtros, Enums.TipoGrid.ListadoTablas);

                foreach (var item in causas)
                {
                    item.DetalleTabla = appExpediente.GetDetalleTablaByCausa(item.CausaID);
                }

                ViewBag.Causas = causas;
                ViewBag.TablaID = TablaID;

                return PartialView("Tablas/_BuscarCausa");

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetDetalleTablaByCausa
        /// </summary>
        /// <param name="CausaID"></param>
        /// <param name="Rol"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleTablaByCausa(int CausaID, string Rol)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                IList<DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                IList<DTO.Models.DetalleTabla> listado = appExpediente.GetDetalleTablaByCausa(CausaID);

                foreach (var item in listado)
                {
                    item.Tabla = appExpediente.GetTablaByID(item.TablaID);
                    item.Tabla.NombreRelator = Usuarios.FirstOrDefault(x => x.UsuarioID == item.Tabla.UsuarioRelatorID).GetFullName();
                }

                ViewBag.DetalleTabla = listado;
                ViewBag.Rol = Rol;

                return PartialView("Tablas/_DetalleTablaByCausa");
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveAgregarCausaTabla(int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = DateTime.Now;
            dbLog.UsuarioID = sso.GetUsuarioActivoID();

            try
            {
                return Json(0);


                //dbLog.TipoLog = Enums.TipoLog.AgregarCausaTabla;
                //dbLog.Save();
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="TablaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GenerarListadoTablaPDF(int TablaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            DateTime ahora = DateTime.Now;

            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                Enums.ReturnJson retorno;
                string message = "";

                IList<DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                DTO.Models.Tabla tabla = appExpediente.GetTablaByID(TablaID);
                tabla.UsuarioRelator = Usuarios.FirstOrDefault(x => x.UsuarioID == tabla.UsuarioRelatorID);

                if (tabla.DetalleTabla.Count == 0)
                {
                    retorno = Enums.ReturnJson.DatoNoEncontrado;
                }
                else
                {
                    foreach (var item in tabla.DetalleTabla)
                    {
                        item.Causa = appExpediente.GetCausa(item.CausaID);
                        item.Causa.Parte = appExpediente.GetParteByCausa(item.CausaID);
                    }

                    #region PDF
                    Enums.TipoDocumento tipoDocumento = Enums.TipoDocumento.Tabla;

                    string FolderSave = string.Format("\\Listados\\{0}\\{1}\\{2}\\{3}", ahora.Year, ahora.Month, tipoDocumento, ahora.ToString("HHmmss"));
                    string MergePath = WebConfig.PathBaseRepository + FolderSave;
                    if (!Directory.Exists(MergePath)) Directory.CreateDirectory(MergePath);


                    DocPdf pdf = new DocPdf();
                    pdf.Usuario = sso.UserActive;
                    pdf.IsTabla = true;
                    pdf.Tabla = tabla;
                    pdf.MapPath = Server.MapPath("~");
                    pdf.PathSave = MergePath;
                    pdf.Filename = $"Tabla-{tabla.Fecha.ToString("dd-MM-yyyy")}.pdf";
                    pdf.Titulo = "TABLA DE CAUSAS CON ALEGATOS";
                    pdf.SubTitulo = $"{tabla.Fecha.ToString("dddd, dd MMMM \\de yyyy")}";
                    pdf.SubTitulo2 = $"{tabla.UsuarioRelator.GetTextoRelator()} {tabla.UsuarioRelator.GetFullName()}";
                    pdf.CreateDocument("TDPI: Tabla de Causas");

                    pdf.SetListadoTabla();
                    pdf.Render();

                    #endregion

                    #region TheHash

                    string PathFinalArchivo = Path.Combine(FolderSave, pdf.Filename);

                    DTO.Models.VersionEncript enc = appCommon.GetLastVersionEncript();
                    TheHash xEncode = new TheHash(enc.Cadena);
                    string EncodeEnd = WebConfig.IsLocalRepository + TipoUploadChar + PathFinalArchivo + TipoUploadChar + 0;
                    string SHAFile = xEncode.EncryptData(EncodeEnd);
                    #endregion

                    #region Save in Store

                    DTO.Models.DocumentoSistema doc = new DTO.Models.DocumentoSistema();
                    doc.DocumentoSistemaID = 0;
                    doc.VersionEncriptID = enc.VersionEncriptID;
                    doc.TipoDocumentoID = (int)tipoDocumento;
                    doc.Hash = SHAFile;
                    doc.NombreArchivoFisico = pdf.Filename;
                    doc.Fecha = ahora;
                    doc.Descripcion = pdf.SubTitulo;

                    doc.DocumentoSistemaID = appExpediente.SaveDocumentoSistema(doc);

                    DTO.Models.AsocDocumentoSistemaTabla asoc = new DTO.Models.AsocDocumentoSistemaTabla();
                    asoc.AsocDocumentoSistemaTablaID = 0;
                    asoc.TablaID = tabla.TablaID;
                    asoc.DocumentoSistemaID = doc.DocumentoSistemaID;
                    asoc.AsocDocumentoSistemaTablaID = appExpediente.SaveAsocDocumentoSistemaTabla(asoc);

                    #endregion

                    LogCausa _logC = new LogCausa();
                    _logC.Fecha = ahora;
                    _logC.UsuarioID = UsuarioActive;
                    _logC.TipoLog = Enums.TipoLog.CambiaEstadoCausa;


                    Enums.EstadoCausa estadoNew = Enums.EstadoCausa.EnTabla;

                    foreach (var c in tabla.DetalleTabla)
                    {
                        Enums.EstadoCausa estadoActual = (Enums.EstadoCausa)c.Causa.EstadoCausaID;

                        appExpediente.CambiarEstadoCausa(c.CausaID, Enums.EstadoCausa.EnTabla);

                        _logC.CausaID = c.CausaID;
                        _logC.EstadoCausa = estadoNew;
                        _logC.Observaciones = $"{estadoActual} ==> {estadoNew}";
                        _logC.Save();
                    }

                    appExpediente.SetEstadoTabla(tabla.TablaID, Enums.EstadoTabla.Generado);

                    retorno = Enums.ReturnJson.ActionSuccess;

                    #region LogSistema
                    dbLog.TipoLog = Enums.TipoLog.GenerarTablaPDF;
                    dbLog.Save();
                    #endregion
                }

                return Json(new
                {
                    result = (int)retorno,
                    TablaID = TablaID,
                    message = message
                });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }
        #endregion



        #region Estados Diarios EstadosDiarios

        /// <summary>
        /// GenerarListadoEstadoDiarioPDF
        /// </summary>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GenerarListadoEstadoDiarioPDF(int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            DateTime ahora = DateTime.Now;

            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                Enums.ReturnJson retorno;
                string message = "";


                DTO.Models.EstadoDiario model = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);
                model.DetalleEstadoDiario = model.DetalleEstadoDiario.Where(x => x.Vigente).ToList();

                if (model.DetalleEstadoDiario.Count == 0)
                {
                    retorno = Enums.ReturnJson.DatoNoEncontrado;
                }
                else
                {
                    List<DTO.Models.Causa> causas = new List<DTO.Models.Causa>();
                    List<DTO.Models.Expediente> expedientes = new List<DTO.Models.Expediente>();

                    foreach (var item in model.DetalleEstadoDiario)
                    {
                        item.Expediente = appExpediente.GetExpediente(item.ExpedienteID);

                        if (!causas.Any(x => x.CausaID == item.Expediente.CausaID))
                        {
                            var causa = appExpediente.GetCausa(item.Expediente.CausaID);
                            causa.Parte = appExpediente.GetParteByCausa(item.Expediente.CausaID);
                            causas.Add(causa);
                        }

                        expedientes.Add(item.Expediente);
                    }
                    
                    foreach (var item in causas)
                    {
                        item.Expediente = expedientes.Where(x => x.CausaID == item.CausaID).ToList();
                    }

                    #region PDF
                    Enums.TipoDocumento tipoDocumento = Enums.TipoDocumento.EstadoDiario;

                    string FolderSave = string.Format("\\Listados\\{0}\\{1}\\{2}\\{3}", ahora.Year, ahora.Month, tipoDocumento, ahora.ToString("HHmmss"));
                    string MergePath = WebConfig.PathBaseRepository + FolderSave;
                    if (!Directory.Exists(MergePath)) Directory.CreateDirectory(MergePath);

                    DocPdf pdf = new DocPdf();
                    pdf.Usuario = sso.UserActive;
                    pdf.IsEstadoDiario = true;
                    pdf.Causas = causas;
                    pdf.Ahora = ahora;
                    pdf.MapPath = Server.MapPath("~");
                    pdf.PathSave = MergePath;
                    pdf.Filename = $"ED-{model.Fecha.ToString("dd-MM-yyyy")}.pdf";
                    pdf.Titulo = $"Estado Diario De Fecha {string.Format("{0:D}", ahora)}";
                    pdf.CreateDocument("TDPI: Estado Diario");

                    pdf.SetListadoEstadoDiario();
                    pdf.Render();

                    #endregion

                    #region TheHash

                    string PathFinalArchivo = Path.Combine(FolderSave, pdf.Filename);
                    message = PathFinalArchivo;

                    DTO.Models.VersionEncript enc = appCommon.GetLastVersionEncript();
                    TheHash xEncode = new TheHash(enc.Cadena);
                    string EncodeEnd = WebConfig.IsLocalRepository + TipoUploadChar + PathFinalArchivo + TipoUploadChar + 0;
                    string SHAFile = xEncode.EncryptData(EncodeEnd);
                    #endregion

                    #region Save in Store

                    DTO.Models.DocumentoSistema doc = new DTO.Models.DocumentoSistema();
                    doc.DocumentoSistemaID = 0;
                    doc.VersionEncriptID = enc.VersionEncriptID;
                    doc.TipoDocumentoID = (int)tipoDocumento;
                    doc.Hash = SHAFile;
                    doc.NombreArchivoFisico = pdf.Filename;
                    doc.Fecha = ahora;
                    doc.Descripcion = pdf.Titulo;

                    doc.DocumentoSistemaID = appExpediente.SaveDocumentoSistema(doc);

                    DTO.Models.AsocDocumentoSistemaEstadoDiario asoc = new DTO.Models.AsocDocumentoSistemaEstadoDiario();
                    asoc.AsocDocumentoSistemaEstadoDiarioID = 0;
                    asoc.EstadoDiarioID = model.EstadoDiarioID;
                    asoc.DocumentoSistemaID = doc.DocumentoSistemaID;
                    asoc.AsocDocumentoSistemaEstadoDiarioID = appExpediente.SaveAsocDocumentoSistemaEstadoDiario(asoc);

                    #endregion

                    appExpediente.SetTipoEstadoDiario(model.EstadoDiarioID, Enums.TipoEstadoDiario.Generado);

                    retorno = Enums.ReturnJson.ActionSuccess;
                }

                return Json(new
                {
                    result = (int)retorno,
                    EstadoDiarioID = EstadoDiarioID,
                    message = message
                });

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

        /// <summary>
        /// EstadosDiarios
        /// </summary>
        /// <returns></returns>
        public ActionResult EstadosDiarios()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = sso.IsTDPI();
            if (!acceso) return Redirect(WebConfig.LogOffAuthenticationSystem);

            DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
            filtros.Sala = appCommon.GetSala(true);
            filtros.Usuario = appCommon.GetUsuarios();
            filtros.EstadoTabla = appCommon.GetEstadoTabla();

            return View("EstadosDiarios/Index", filtros);
        }


        /// <summary>
        /// GetListadoEstadoDiario
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetListadoEstadoDiario(DTO.FiltrosEscritorio filtros)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                filtros.NumeroTicket = active.GetStringValueForm(filtros.NumeroTicket);

                IList<DTO.Models.EstadoDiario> Tablas = appExpediente.GetEstadoDiario(filtros);

                foreach (var item in Tablas)
                {
                    if (item.IsGenerado() || item.IsPublicado())
                    {
                        item.DocumentoSistemaTabla = appExpediente.GetAsocDocumentoSistema(item.EstadoDiarioID, Enums.TipoDocumento.EstadoDiario);
                    }
                }

                ViewBag.Listado = Tablas;

                return PartialView("EstadosDiarios/_Listado");
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }



        /// <summary>
        /// GetEstadoDiario
        /// </summary>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        public ActionResult GetEstadoDiario(int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.EstadoDiario model = new Application.DTO.Models.EstadoDiario();
                model.EstadoDiarioID = EstadoDiarioID;
                model.Fecha = DateTime.Now;
                model.TipoEstadoDiarioID = (int)Enums.TipoEstadoDiario.Borrador;

                if (model.EstadoDiarioID > 0)
                {
                    model = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);
                }

                return PartialView("EstadosDiarios/_EstadoDiario", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// SaveEstadoDiario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveEstadoDiario(DTO.Models.EstadoDiario dto)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                Enums.ReturnJson retorno;

                dto.EstadoDiarioID = appExpediente.SaveEstadoDiario(dto);
                retorno = Enums.ReturnJson.ActionSuccess;

                return Json(new { result = (int)retorno, EstadoDiarioID = dto.EstadoDiarioID });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

        /// <summary>
        /// GetDetalleExpedientes
        /// </summary>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleExpedientes(int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.EstadoDiario model = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);
                model.DetalleEstadoDiario = model.DetalleEstadoDiario.Where(x => x.Vigente).ToList();

                List<DTO.Models.Causa> causas = new List<DTO.Models.Causa>();
                List<DTO.Models.Expediente> expedientes = new List<DTO.Models.Expediente>();

                foreach (var item in model.DetalleEstadoDiario)
                {
                    item.Expediente = appExpediente.GetExpediente(item.ExpedienteID);

                    if (!causas.Any(x => x.CausaID == item.Expediente.CausaID))
                    {
                        var causa = appExpediente.GetCausa(item.Expediente.CausaID);
                        causa.Parte = appExpediente.GetParteByCausa(item.Expediente.CausaID);
                        causas.Add(causa);
                    }

                    expedientes.Add(item.Expediente);
                }

                foreach (var item in causas)
                {
                    item.Expediente = expedientes.Where(x => x.CausaID == item.CausaID).ToList();
                }

                ViewBag.Causas = causas;

                return PartialView("EstadosDiarios/_DetalleExpedientes", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetAgregarDetalleEstadoDiario
        /// </summary>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetAgregarDetalleEstadoDiario(int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.EstadoDiario model = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);

                return PartialView("EstadosDiarios/_AgregarDetalle", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetBuscarExpediente
        /// </summary>
        /// <param name="filtros"></param>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetBuscarExpediente(DTO.FiltrosEscritorio filtros, int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.EstadoDiario EstadoDiario = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);

                filtros.NumeroTicket = active.GetStringValueForm(Request.Form["buscaRol"]);
                filtros.Anio = active.GetIntValueForm(Request.Form["buscaAnio"]);
                filtros.FechaDesde = EstadoDiario.Fecha.ToString("dd-MM-yyyy");

                IList<DTO.Models.Causa> causas = appExpediente.GetCausasByFilter(filtros, Enums.TipoGrid.ListadoEstadoDiario);

                List<int> EstadosResolucion = new List<int>();
                EstadosResolucion.Add((int)Enums.TipoTramite.Resolucion);

                foreach (var item in causas)
                {
                    item.Expediente = appExpediente.GetExpedienteByCausa(item.CausaID);
                    foreach (var e in item.Expediente)
                    {
                        if (EstadosResolucion.Contains(e.TipoTramiteID))
                        {
                            e.DetalleEstadoDiario = appExpediente.GetDetalleEstadoDiarioByExpediente(e.ExpedienteID);
                        }
                    }
                }

                ViewBag.Causas = causas;
                ViewBag.EstadoDiario = EstadoDiario;

                return PartialView("EstadosDiarios/_BuscarExpediente");

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetExpedientesByCausa
        /// </summary>
        /// <param name="CausaID"></param>
        /// <param name="EstadoDiarioID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetExpedientesByCausa(int CausaID, int EstadoDiarioID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Causa model = appExpediente.GetCausa(CausaID);
                model.Expediente = appExpediente.GetExpedienteByCausa(CausaID);

                DTO.Models.EstadoDiario EstadoDiario = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);
                ViewBag.EstadoDiario = EstadoDiario;

                return PartialView("EstadosDiarios/_ExpedientesByCausa", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

        #endregion

    }
}