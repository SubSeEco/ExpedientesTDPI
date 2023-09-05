using Application.DTO.Utils;
using Application.Services;
using Infrastructure.Logging;
using Infrastructure.Utils.Extensions;
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
    public class ExpedientesController : GlobalController
    {

        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IExpedienteAppServices appExpediente = new ExpedienteAppServices();
        private readonly IMailAppServices appMail = new MailAppServices();

        private readonly Commons active = new Commons();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
            filtros.TipoCausa = appCommon.GetTipoCausa(true);
            filtros.EstadoCausa = appCommon.GetEstadoCausa(true);

            ViewBag.FiltrosEscritorio = filtros;

            #region DataForm

            DTO.DataForm DataForm = new DTO.DataForm();
            DataForm.UserActive = sso.UserActive;
            DataForm.PerfilActive.IsTDPI = sso.IsTDPI();
            DataForm.PerfilActive.IsINAPI = sso.IsINAPI();
            DataForm.PerfilActive.IsAdministrador = sso.IsAdministrador();
            DataForm.PerfilActive.IsSAG = sso.IsSAG();
            DataForm.PerfilActive.IsInvitado = sso.IsInvitado();

            ViewBag.DataForm = DataForm;

            #endregion

            return View();
        }


        /// <summary>
        /// Registro
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registro(int id = 0, int id2 = 0)
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool acceso = (sso.IsTDPI() || sso.IsSAG() || sso.IsINAPI() || sso.IsAdministrador());
            if (!acceso) return RedirectToRoute("ActionInitialSystem");

            int CausaID = id;
            int TipoCausaID = id2;

            bool IsNew = (CausaID == 0);
            bool IsView = false;

            if (TipoCausaID == 0 && IsNew)
            {
                return Redirect(WebConfig.Url_MenuSistemas);
            }

            DTO.Models.Causa model = new DTO.Models.Causa();
            model.Anio = DateTime.Now.Year;
            model.TipoCausaID = TipoCausaID;
            model.IsContencioso = true;

            #region Valida Causa y TipoCausa

            if (CausaID != 0)
            {
                try
                {
                    model = appExpediente.GetCausa(CausaID);
                    model.Parte = appExpediente.GetParteByCausa(CausaID);
                    model.DocumentoCausa = appExpediente.GetDocumentoCausa(CausaID, Domain.Infrastructure.TipoDocumento.Causa);
                }
                catch (Exception)
                {
                    Logger.Execute().Error(new System.ArgumentException("No existe Causa", "No existe CausaID " + CausaID));
                    return RedirectToAction("Index");
                }
            }

            var TipoCausa = appCommon.GetTipoCausa(true);
            var tc = TipoCausa.FirstOrDefault(x => x.TipoCausaID == TipoCausaID);
            if (tc == null)
            {
                Logger.Execute().Error(new System.ArgumentException("No existe TipoCausa", "No existe TipoCausaID " + CausaID));
                return RedirectToAction("Index");
            }
            else
            {
                model.TipoCausa = tc;
            }

            #endregion

            DTO.Models.ConfTipoCausa Config = appCommon.GetConfTipoCausa(TipoCausaID);
            ViewBag.ConfTipoCausa = Config;

            #region DataForm

            DTO.DataForm DataForm = new DTO.DataForm();
            DataForm.IsNew = IsNew;
            DataForm.IsView = IsView;
            DataForm.Pais = appCommon.GetPais();
            DataForm.TipoContencioso = appCommon.GetTipoContencioso(true);
            DataForm.TipoParte = appCommon.GetTipoParte(true);
            DataForm.AsocTipoDocumentoAdjunto = appCommon.GetTipoDocumentoAdjuntoByID((int)Enums.TipoDocumento.Causa);

            ViewBag.DataForm = DataForm;

            #endregion


            return View(model);
        }


        #region Parte

        /// <summary>
        /// GetParte
        /// </summary>
        /// <param name="ParteID"></param>
        /// <param name="CausaID"></param>
        /// <param name="TipoParteID"></param>
        /// <param name="TipoCausaID"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetParte(int ParteID, int CausaID, int TipoParteID, int TipoCausaID, int index)
        {
            try
            {
                DTO.Models.Parte parte = new Application.DTO.Models.Parte();
                parte.CausaID = CausaID;
                parte.TipoParteID = TipoParteID;
                parte.TipoParte = appCommon.GetTipoParte().FirstOrDefault(x => x.TipoParteID == TipoParteID);

                if (ParteID > 0)
                {
                    parte = appExpediente.GetParte(ParteID);
                }

                if (ParteID == (int)Enums.GenericJson.TempData)
                {
                    string objTemp = active.GetStringValueForm(Request.Form["objTemp"]);
                    DTO.ParteVue parteVue = JsonConvert.DeserializeObject<DTO.ParteVue>(objTemp);

                    parte.PaisID = parteVue.PaisID;
                    parte.Rut = parteVue.Rut;
                    parte.Nombre = parteVue.Nombre;
                    parte.RutRepresentante = parteVue.RutRepresentante;
                    parte.NombreRepresentante = parteVue.NombreRepresentante;
                    parte.NombreAbogado = parteVue.NombreAbogado;
                    parte.EmailAbogado = parteVue.EmailAbogado;
                    parte.NombreEstudioJuridico = parteVue.NombreEstudioJuridico;
                    parte.FolioConsignacion = parteVue.FolioConsignacion;
                    parte.FechaConsignacion = active.GetDateTimeValueOrNull(parteVue.FechaConsignacion);
                    parte.RutConsignacion = parteVue.RutConsignacion;
                    parte.NombreConsignacion = parteVue.NombreConsignacion;
                    parte.ParteID = ParteID;
                }

                DTO.DataForm DataForm = new DTO.DataForm();
                DataForm.Pais = appCommon.GetPais().Where(x => x.Vigente).ToList();
                ViewBag.DataForm = DataForm;

                DTO.Models.ConfTipoCausa Config = appCommon.GetConfTipoCausa(TipoCausaID);
                ViewBag.ConfTipoCausa = Config;

                ViewBag.Index = index;

                return PartialView("_Parte", parte);
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
        public ActionResult SaveParte(DTO.Models.Parte dto)
        {
            try
            {
                Enums.ReturnJson retorno = Enums.ReturnJson.SinAccion;
                bool IsNew = dto.ParteID == 0;
                bool IsTemp = dto.ParteID == (int)Enums.GenericJson.TempData;

                if (dto.CausaID != 0 && !IsTemp)
                {
                    dto.Nombre = active.GetStringValueForm(Request.Form["Nombre"]);
                    dto.NombreRepresentante = active.GetStringValueForm(Request.Form["NombreRepresentante"]);
                    dto.NombreAbogado = active.GetStringValueForm(Request.Form["NombreAbogado"]);
                    dto.EmailAbogado = active.GetStringValueForm(Request.Form["EmailAbogado"]);
                    dto.NombreEstudioJuridico = active.GetStringValueForm(Request.Form["NombreEstudioJuridico"]);
                    dto.FolioConsignacion = active.GetStringValueForm(Request.Form["FolioConsignacion"]);
                    dto.NombreConsignacion = active.GetStringValueForm(Request.Form["NombreConsignacion"]);

                    dto.ParteID = appExpediente.SaveParte(dto);

                    retorno = Enums.ReturnJson.ActionSuccess;
                }

                return Json(new { result = (int)retorno, ParteID = dto.ParteID, IsNew = IsNew });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

        #endregion


        /// <summary>
        /// SaveCausa
        /// </summary>
        /// <param name="causa"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveCausa(DTO.Models.Causa causa)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool IsInvitado = sso.IsInvitado();
            if (IsInvitado) return Response403();

            int UserActive = sso.GetUsuarioActivoID();

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                //Enums.TipoLog action = Enums.TipoLog.ActionLog;
                Enums.ReturnJson Retorno = Enums.ReturnJson.ActionSuccess;

                bool IsNew = causa.CausaID == 0;

                DTO.Models.ConfTipoCausa config = appCommon.GetConfTipoCausa(causa.TipoCausaID);

                causa.Anio = config.IsAnio ? causa.Anio : 0;
                causa.Observacion = config.IsObservacion ? active.GetStringValueForm(Request.Form["Observacion"]) : "";

                if (config.IsContencioso)
                {
                    causa.TipoContenciosoID = causa.IsContencioso ? causa.TipoContenciosoID : (int)Enums.TipoContencioso.NA;
                }
                else
                {
                    causa.IsContencioso = false;
                    causa.TipoContenciosoID = (int)Enums.TipoContencioso.NA;
                }

                causa.Numero = config.IsNumeroSolicitud ? active.GetStringValueForm(Request.Form["Numero"]) : "";
                causa.NumeroRegistro = config.IsNumeroRegistro ? active.GetStringValueForm(Request.Form["NumeroRegistro"]) : "";

                
                if (IsNew)
                {
                    DTO.Models.Folio folio = appExpediente.GetFolio(ahora.Year);
                    causa.NumeroTicket = string.Format("{0:000000}-{1}", folio.Correlativo, ahora.Year);

                    causa.Denominacion = active.GetStringValueForm(Request.Form["Denominacion"]);
                    causa.Anio = ahora.Year;
                    causa.EstadoCausaID = (int)Enums.EstadoCausa.PreIngresado;
                    causa.FechaIngreso = ahora;
                    causa.TipoCanalID = (int)Enums.TipoCanal.Presencial;
                    causa.UsuarioID = sso.GetUsuarioActivoID();

                    causa.CausaID = appExpediente.SaveCausa(causa);

                    #region Partes

                    if (active.IsInputValue(Request.Form["strPartes"]))
                    {
                        try
                        {
                            string strPartes = active.GetStringValueForm(Request.Form["strPartes"]);
                            List<DTO.ParteVue> listaPartes = JsonConvert.DeserializeObject<List<DTO.ParteVue>>(strPartes);
                            foreach (var item in listaPartes)
                            {
                                DTO.Models.Parte p = new Application.DTO.Models.Parte();
                                p.ParteID = item.ParteID;
                                p.PaisID = item.PaisID;
                                p.CausaID = causa.CausaID;
                                p.TipoParteID = item.TipoParteID;
                                p.Rut = item.Rut;
                                p.Nombre = active.GetStringValueForm(item.Nombre);
                                p.RutRepresentante = item.RutRepresentante;
                                p.NombreRepresentante = active.GetStringValueForm(item.NombreRepresentante);
                                p.NombreAbogado = active.GetStringValueForm(item.NombreAbogado);
                                p.EmailAbogado = active.GetStringValueForm(item.EmailAbogado);
                                p.NombreEstudioJuridico = active.GetStringValueForm(item.NombreEstudioJuridico);
                                p.FolioConsignacion = active.GetStringValueForm(item.FolioConsignacion);
                                p.FechaConsignacion = active.GetDateTimeValueOrNull(item.FechaConsignacion);
                                p.RutConsignacion = item.RutConsignacion;
                                p.NombreConsignacion = active.GetStringValueForm(item.NombreConsignacion);

                                p.ParteID = appExpediente.SaveParte(p);
                            }
                        }
                        catch (Exception ex2)
                        {
                            Logger.Execute().Error(ex2);
                        }
                    }

                    #endregion


                    #region DocumentosAdjuntos

                    int[] DocTmpIDs = { };
                    string[] DocTmpNombres = { };
                    string[] DocTmpDescs = { };

                    string TipoUploadChar = "{#}";

                    if (active.IsInputValue(Request.Form["DocTmpID"]))
                        DocTmpIDs = Array.ConvertAll(Request.Form.GetValues("DocTmpID"), int.Parse);

                    if (active.IsInputValue(Request.Form["DocTmpNombre"]))
                        DocTmpNombres = Request.Form.GetValues("DocTmpNombre");

                    if (active.IsInputValue(Request.Form["DocTmpDesc"]))
                        DocTmpDescs = Request.Form.GetValues("DocTmpDesc");

                    if (DocTmpIDs.Length > 0)
                    {
                        DTO.Models.VersionEncript _enc = appCommon.GetLastVersionEncript();
                        TheHash xEncode = new TheHash("");
                        string BasePath = WebConfig.PathBaseRepository;

                        IList<DTO.Models.DocumentoTmp> docs = new List<DTO.Models.DocumentoTmp>();

                        foreach (var docID in DocTmpIDs)
                        {
                            var doc = appCommon.GetDocumentoTmp(docID);
                            docs.Add(doc);
                        }

                        int contador = 0;

                        foreach (var item in docs)
                        {
                            xEncode = new TheHash(item.VersionEncript.Cadena.Trim());

                            string pathFisicoTemp = xEncode.DecryptData(item.Hash);

                            string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);
                            string pathFisico = HashDecode[1];
                            string EndPathFile = WebConfig.PathBaseRepository + pathFisico;

                            if (System.IO.File.Exists(EndPathFile))
                            {
                                long Ticks = DateTime.Now.TimeOfDay.Ticks;

                                string FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}",
                                causa.Anio, causa.CausaID, Enums.TipoDocumento.Causa, Ticks);

                                string MergePath = BasePath + FolderSave;

                                if (!System.IO.Directory.Exists(MergePath))
                                    System.IO.Directory.CreateDirectory(MergePath);

                                string EndPath = System.IO.Path.Combine(MergePath, DocTmpNombres[contador]);
                                System.IO.File.Copy(EndPathFile, EndPath, true);

                                string PathFinalArchivo = System.IO.Path.Combine(FolderSave, DocTmpNombres[contador]);

                                //Save DDBB
                                xEncode = new TheHash(_enc.Cadena);
                                string _EncodeEnd = string.Format("{0}{1}{2}{3}{4}", true, TipoUploadChar, PathFinalArchivo, TipoUploadChar, 0);
                                string _SHAFile = xEncode.EncryptData(_EncodeEnd);

                                DTO.Models.DocumentoCausa adjunto = new DTO.Models.DocumentoCausa();
                                adjunto.DocumentoCausaID = 0;
                                adjunto.CausaID = causa.CausaID;
                                adjunto.VersionEncriptID = _enc.VersionEncriptID;
                                adjunto.Hash = _SHAFile;
                                adjunto.NombreArchivoFisico = DocTmpNombres[contador];
                                adjunto.Fecha = ahora;
                                adjunto.Descripcion = DocTmpDescs[contador].Left(250);
                                adjunto.DocumentoCausaID = appExpediente.SaveDocumentoCausa(adjunto);

                                var asocs = appCommon.GetTipoDocumentoAdjuntoByID((int)Enums.TipoDocumento.Causa);
                                if (asocs.Count > 0)
                                {
                                    var AsocTipoDocumentoAdjunto = asocs.FirstOrDefault();

                                    DTO.Models.AsocCausaDocumento asoc = new DTO.Models.AsocCausaDocumento();
                                    asoc.AsocCausaDocumentoID = 0;
                                    asoc.DocumentoAdjuntoID = AsocTipoDocumentoAdjunto.DocumentoAdjuntoID;
                                    asoc.DocumentoCausaID = adjunto.DocumentoCausaID;
                                    asoc.CompromisoID = 0;
                                    asoc.AsocCausaDocumentoID = appExpediente.SaveAsocCausaDocumento(asoc);
                                }

                                //Borrar Archivo Temporal
                                string Carpeta = System.IO.Path.GetDirectoryName(EndPathFile);
                                System.IO.File.Delete(EndPathFile);
                                if (active.IsDirectoryEmpty(Carpeta))
                                    System.IO.Directory.Delete(Carpeta);

                                appCommon.DeleteDocumentoTmpByID(item.DocumentoTmpID);
                            }

                            contador++;
                        }
                    }

                    #endregion


                    LogCausa _logC = new LogCausa();
                    _logC.Fecha = ahora;
                    _logC.CausaID = causa.CausaID;
                    _logC.UsuarioID = sso.GetUsuarioActivoID();
                    _logC.EstadoCausa = (Enums.EstadoCausa)causa.EstadoCausaID;
                    _logC.Observaciones = "Crear Causa";
                    _logC.TipoLog = Enums.TipoLog.AgregarCausa;
                    _logC.Save();

                    dbLog.TipoLog = Enums.TipoLog.AgregarCausa;
                    dbLog.Save();
                }
                else
                {
                    appExpediente.UpdateCausa(causa);

                    LogCausa _logC = new LogCausa();
                    _logC.Fecha = ahora;
                    _logC.CausaID = causa.CausaID;
                    _logC.UsuarioID = sso.GetUsuarioActivoID();
                    _logC.EstadoCausa = (Enums.EstadoCausa)causa.EstadoCausaID;
                    _logC.Observaciones = "Crear Causa";
                    _logC.TipoLog = Enums.TipoLog.ActualizarCausa;
                    _logC.Save();

                    dbLog.TipoLog = Enums.TipoLog.ActualizarCausa;
                    dbLog.Save();
                }

                #region Notificacion
                if (IsNew)
                {
                    appMail.IngresoNuevaCausa(causa.CausaID, UserActive);
                }
                #endregion


                return Json(new
                {
                    result = (int)Retorno,
                    NumeroTicket = causa.NumeroTicket,
                    SolicitudID = causa.CausaID,
                    Fecha = ahora.ToString("dd-MM-yyyy"),
                    updated = !IsNew
                });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetEscritorioTDPI
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetEscritorioTDPI(DTO.FiltrosEscritorio filtros)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool IsInvitado = sso.IsInvitado();
            int UserActive = IsInvitado ? 0 : sso.GetUsuarioActivoID();

            try
            {
                filtros.UsuarioID = UserActive;
                filtros.NumeroTicket = active.GetStringValueForm(Request.Form["NumeroTicket"]);
                filtros.NumeroRegistro = active.GetStringValueForm(Request.Form["NumeroRegistro"]);
                filtros.Denominacion = active.GetStringValueForm(Request.Form["Denominacion"]);
                filtros.Apelante = active.GetStringValueForm(Request.Form["Apelante"]);
                filtros.FechaIngreso = active.GetStringValueForm(Request.Form["FechaIngreso"]);
                filtros.Apelado = active.GetStringValueForm(Request.Form["Apelado"]);
                filtros.NumeroSolicitud = active.GetStringValueForm(Request.Form["NumeroSolicitud"]);

                IList<DTO.Models.SP_Causas_Result> lista = appExpediente.GetSP_Causas(filtros);
                int Enable = (int)Enums.GenericOption.Si;
                IList<DTO.EscritorioJson> json = new List<DTO.EscritorioJson>();

                foreach (var item in lista)
                {
                    //Se debe enviar el dato para filtrar en base datos no aca
                    if (WebConfig.IsAccesoPublico && item.EstadoCausaID == (int)Enums.EstadoCausa.PreIngresado)
                    {
                        continue;
                    }

                   // bool PuedeGenerarPDF = sso.IsTDPI() || sso.IsINAPI() || sso.IsSAG();
                    bool PuedeVer = sso.IsTDPI();
                    bool PuedeEditar = sso.IsTDPI();
                    bool PuedeEliminar = sso.IsTDPI();

                    Link a = new Link();

                    #region bt1: PDF
                    a._class = "";
                    a.click = "";
                    a.title = "Generar PDF";
                    a.xicon = "x-icon x-icon-pdf";
                    a.href = "javascript:GenerarExpedientePDF(" + item.CausaID + ");";
                    string bt1 = a.Generate(true);
                    #endregion

                    #region bt2: Ver
                    a.title = "Ver";
                    a.xicon = "x-icon-zoom1";
                    a.style = "margin-left:3px";
                    a.href = Url.Action("Registro", "Expedientes", new { id = item.CausaID, id2 = item.TipoCausaID });
                    a.click = "javascript:void(0);";
                    string bt2 = a.Generate(true);
                    #endregion

                    #region bt3: Editar
                    a.title = "Editar";
                    a.xicon = "x-icon-edit";
                    a.style = "margin-left:3px";
                    a.href = Url.Action("Registro", "Expedientes", new { id = item.CausaID, id2 = item.TipoCausaID });
                    a.click = "javascript:void(0);";
                    string bt3 = a.Generate(true);
                    #endregion

                    #region bt4: Eliminar
                    a.title = "Eliminar";
                    a.xicon = "x-icon x-icon-delete";
                    a.href = "javascript:;";
                    a.click = "";
                    string bt4 = a.Generate(true);
                    #endregion

                    #region bt5: Eventos expediente
                    a.title = "Eventos Expediente";
                    a.xicon = "x-icon x-icon-org";
                    a.href = "javascript:;";
                    a._class = "iconDetalle";
                    a.click = "GetEventosExpediente(" + item.CausaID + ", this);seleccionarTR(\"grilla\",this)";
                    string bt5 = a.Generate(true);
                    a._class = "";
                    #endregion

                    #region bt6: Revisar escrito
                    a.title = "Revisar escrito";
                    a.xicon = "x-icon x-icon-txt";
                    a.href = "javascript:;";
                    a.style = "margin-left:3px";
                    a.click = "";
                    string bt6 = a.Generate(true);
                    #endregion


                    #region bt7: Historial
                    a.title = "Ver Historial";
                    a.xicon = "x-icon x-icon-historial";
                    a.href = "javascript:;";
                    a.click = "VerHistorial(" + item.CausaID + ")";
                    string bt7 = a.Generate(true);
                    #endregion

                    #region StringBuilder
                    List<string> str = new List<string>();

                    if (item.bt1 == Enable) str.Add(bt1); //PDF
                    if (item.bt2 == Enable && PuedeVer) str.Add(bt2); //Ver
                    if (item.bt3 == Enable && PuedeEditar) str.Add(bt3); //Editar
                    if (item.bt4 == Enable && PuedeEliminar) str.Add(bt4); //Eliminar
                    if (item.bt5 == Enable) str.Add(bt5); //Eventos expediente
                    if (item.bt6 == Enable) str.Add(bt6); //Revisar escrito
                    if (PuedeVer) str.Add(bt7); //Historial

                    #endregion

                    item.AccionesList = str;

                    #region Json

                    DTO.EscritorioJson j = new DTO.EscritorioJson();
                    j.cId = item.CausaID;
                    //j.tcId = item.TipoCanalID;
                    //j.tc = item.TipoCanal;
                    //j.ecId = item.EstadoCausaID;
                    j.ec = item.EstadoCausa;
                    //j.usrId = item.UsuarioID;
                    j.us = item.Usuario;
                    //j.respId = item.UsuarioResponsableID;
                    //j.tconId = item.TipoContenciosoID;
                    //j.tcon = item.TipoContencioso;
                    //j.tcauId = item.TipoCausaID;
                    j.tcau = item.TipoCausa;
                    j.date = item.FechaIngreso;
                    j.ticket = item.NumeroTicket;
                    j.num = item.Numero;
                    j.ano = item.Anio;
                    j.denom = item.Denominacion;
                    //j.obs = item.Observacion;
                    //j.reg = item.NumeroRegistro;
                    j.Act = str;
                    json.Add(j);

                    #endregion
                }

                JsonResult jsonResult = Json(json);
                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetEventosExpediente
        /// </summary>
        /// <param name="CausaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetEventosExpediente(int CausaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Causa causa = appExpediente.GetCausa(CausaID);
                causa.Parte = appExpediente.GetParteByCausa(CausaID);
                causa.DocumentoCausa = appExpediente.GetDocumentoCausa(CausaID, Enums.TipoDocumento.Causa);

                if (WebConfig.IsAccesoPublico)
                {
                    causa.Expediente = appExpediente.GetExpedienteByCausa(CausaID).Where(x => x.IsFinalizado).ToList();
                }
                else
                {
                    causa.Expediente = appExpediente.GetExpedienteByCausa(CausaID);
                }

                foreach (var exp in causa.Expediente)
                {
                    //exp.Firma = appExpediente.GetFirmaByExpedienteID(exp.ExpedienteID);
                }

                DTO.DataForm DataForm = new DTO.DataForm();
                DataForm.PerfilActive.IsInvitado = sso.IsInvitado();
                ViewBag.DataForm = DataForm;

                return PartialView("_EventosExpediente", causa);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// AdjuntosExpediente
        /// </summary>
        /// <param name="CausaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetLogCausa(int CausaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            bool IsInvitado = sso.IsInvitado();
            if (IsInvitado) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();
            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UsuarioActive;

            try
            {
                DTO.Models.Causa model = appExpediente.GetCausa(CausaID);

                IList<DTO.Models.LogCausa> Historial = appExpediente.GetLogCausa(CausaID);
                ViewBag.Historial = Historial;

                return PartialView("_HistorialCausa", model);
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }


        #region Evento

        /// <summary>
        /// AdjuntosExpediente
        /// </summary>
        /// <param name="ExpedienteID"></param>
        /// <param name="CausaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetAdjuntosExpediente(int ExpedienteID, int CausaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Expediente model = appExpediente.GetExpediente(ExpedienteID);
                model.Causa = appExpediente.GetCausa(CausaID);
                model.AsocEscritoDocto = appExpediente.GetAsocEscritoDocto(model.ExpedienteID);

                return PartialView("_AdjuntosExpediente", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetEvento
        /// </summary>
        /// <param name="ExpedienteID"></param>
        /// <param name="CausaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetExpediente(int ExpedienteID, int CausaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                DTO.Models.Expediente model = new Application.DTO.Models.Expediente();
                model.ExpedienteID = ExpedienteID;
                model.CausaID = CausaID;
                model.IsFinalizado = false;

                bool IsNew = (model.ExpedienteID == 0);

                if (!IsNew)
                {
                    model = appExpediente.GetExpediente(model.ExpedienteID);
                    //model.Firma = appExpediente.GetFirmaByExpedienteID(model.ExpedienteID);
                }

                model.Causa = appExpediente.GetCausa(CausaID);
                model.AsocEscritoDocto = appExpediente.GetAsocEscritoDocto(model.ExpedienteID);

                #region Tipo Tramite
                var TipoTramite = appCommon.GetTipoTramite(true);
                List<int> filterTipoTramite = new List<int>();

                if (WebConfig.IsAccesoPublico)
                {
                    filterTipoTramite.Add((int)Enums.TipoTramite.Escrito);
                    filterTipoTramite.Add((int)Enums.TipoTramite.Oficio);
                }
                else
                {
                    filterTipoTramite.Add((int)Enums.TipoTramite.Actuacion);
                    filterTipoTramite.Add((int)Enums.TipoTramite.Resolucion);
                }
                #endregion

                DTO.DataForm DataForm = new DTO.DataForm();
                DataForm.TipoTramite = TipoTramite.Where(x => filterTipoTramite.Contains(x.TipoTramiteID)).ToList();
                DataForm.Usuario = appCommon.GetUsuarios().Where(x => x.IsTDPI()).ToList();
                DataForm.AsocTipoDocumentoAdjunto = appCommon.GetTipoDocumentoAdjuntoByID((int)Enums.TipoDocumento.Expediente);
                ViewBag.DataForm = DataForm;

                return PartialView("_Expediente", model);
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
        public ActionResult SaveExpediente(DTO.Models.Expediente dto)
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
                bool IsNew = dto.ExpedienteID == 0;

                DTO.Models.Causa causa = appExpediente.GetCausa(dto.CausaID);

                dto.Observacion = active.GetStringValueForm(Request.Form["Observacion"]);
                dto.Comentario = active.GetStringValueForm(Request.Form["Comentario"]);
                dto.NumeroOficio = active.GetStringValueForm(Request.Form["NumeroOficio"]);

                dto.FechaExpediente = dto.FechaExpediente.Value
                    .Date.AddHours(ahora.Hour)
                    .AddMinutes(ahora.Minute)
                    .AddSeconds(ahora.Second);

                dto.ExpedienteID = appExpediente.SaveExpediente(dto);

                dbLog.ExpedienteID = dto.ExpedienteID;

                bool IsResolucion = dto.TipoTramiteID == (int)Enums.TipoTramite.Resolucion;

                if (IsNew)
                {
                    #region DocumentosAdjuntos

                    int[] DocTmpIDs = { };
                    string[] DocTmpNombres = { };
                    string[] DocTmpDescs = { };

                    string TipoUploadChar = "{#}";

                    if (active.IsInputValue(Request.Form["DocTmpID"]))
                        DocTmpIDs = Array.ConvertAll(Request.Form.GetValues("DocTmpID"), int.Parse);

                    if (active.IsInputValue(Request.Form["DocTmpNombre"]))
                        DocTmpNombres = Request.Form.GetValues("DocTmpNombre");

                    if (active.IsInputValue(Request.Form["DocTmpDesc"]))
                        DocTmpDescs = Request.Form.GetValues("DocTmpDesc");

                    if (DocTmpIDs.Length > 0)
                    {
                        DTO.Models.VersionEncript _enc = appCommon.GetLastVersionEncript();
                        TheHash xEncode = new TheHash("");
                        string BasePath = WebConfig.PathBaseRepository;

                        IList<DTO.Models.DocumentoTmp> docs = new List<DTO.Models.DocumentoTmp>();

                        foreach (var docID in DocTmpIDs)
                        {
                            var doc = appCommon.GetDocumentoTmp(docID);
                            docs.Add(doc);
                        }

                        int contador = 0;

                        foreach (var item in docs)
                        {
                            xEncode = new TheHash(item.VersionEncript.Cadena.Trim());

                            string pathFisicoTemp = xEncode.DecryptData(item.Hash);

                            string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);
                            string pathFisico = HashDecode[1];
                            string EndPathFile = WebConfig.PathBaseRepository + pathFisico;

                            if (System.IO.File.Exists(EndPathFile))
                            {
                                long Ticks = DateTime.Now.TimeOfDay.Ticks;

                                string FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}\\{4}",
                                    causa.Anio, causa.CausaID, Enums.TipoDocumento.Expediente, dto.ExpedienteID, Ticks);

                                string MergePath = BasePath + FolderSave;

                                if (!System.IO.Directory.Exists(MergePath))
                                    System.IO.Directory.CreateDirectory(MergePath);

                                string EndPath = System.IO.Path.Combine(MergePath, DocTmpNombres[contador]);
                                System.IO.File.Copy(EndPathFile, EndPath, true);

                                string PathFinalArchivo = System.IO.Path.Combine(FolderSave, DocTmpNombres[contador]);

                                //Save DDBB
                                xEncode = new TheHash(_enc.Cadena);
                                string _EncodeEnd = string.Format("{0}{1}{2}{3}{4}", true, TipoUploadChar, PathFinalArchivo, TipoUploadChar, 0);
                                string _SHAFile = xEncode.EncryptData(_EncodeEnd);

                                DTO.Models.DocumentoCausa adjunto = new DTO.Models.DocumentoCausa();
                                adjunto.DocumentoCausaID = 0;
                                adjunto.CausaID = causa.CausaID;
                                adjunto.VersionEncriptID = _enc.VersionEncriptID;
                                adjunto.Hash = _SHAFile;
                                adjunto.NombreArchivoFisico = DocTmpNombres[contador];
                                adjunto.Fecha = ahora;
                                adjunto.Descripcion = DocTmpDescs[contador].Left(250);
                                adjunto.DocumentoCausaID = appExpediente.SaveDocumentoCausa(adjunto);

                                var asocs = appCommon.GetTipoDocumentoAdjuntoByID((int)Enums.TipoDocumento.Expediente);
                                if (asocs.Count > 0)
                                {
                                    var AsocTipoDocumentoAdjunto = asocs.FirstOrDefault();

                                    DTO.Models.AsocCausaDocumento asoc = new DTO.Models.AsocCausaDocumento();
                                    asoc.AsocCausaDocumentoID = 0;
                                    asoc.DocumentoAdjuntoID = AsocTipoDocumentoAdjunto.DocumentoAdjuntoID;
                                    asoc.DocumentoCausaID = adjunto.DocumentoCausaID;
                                    asoc.CompromisoID = 0;
                                    asoc.AsocCausaDocumentoID = appExpediente.SaveAsocCausaDocumento(asoc);

                                    DTO.Models.AsocEscritoDocto esc = new DTO.Models.AsocEscritoDocto();
                                    esc.AsocEscritoDoctoID = 0;
                                    esc.AsocCausaDocumentoID = asoc.AsocCausaDocumentoID;
                                    esc.ExpedienteID = dto.ExpedienteID;
                                    esc.AsocEscritoDoctoID = appExpediente.SaveAsocEscritoDocto(esc);
                                }

                                //Borrar Archivo Temporal
                                string Carpeta = System.IO.Path.GetDirectoryName(EndPathFile);
                                System.IO.File.Delete(EndPathFile);
                                if (active.IsDirectoryEmpty(Carpeta))
                                    System.IO.Directory.Delete(Carpeta);

                                appCommon.DeleteDocumentoTmpByID(item.DocumentoTmpID);
                            }

                            contador++;
                        }
                    }

                    #endregion
                }

                int OpcionesTramiteID = active.GetIntValueForm(Request.Form["OpcionesTramiteID"]);
                if (OpcionesTramiteID > 0)
                {
                    DTO.Models.AsocExpedienteOpcion opt = new DTO.Models.AsocExpedienteOpcion();
                    opt.AsocExpedienteOpcionID = 0;
                    opt.ExpedienteID = dto.ExpedienteID;
                    opt.OpcionesTramiteID = OpcionesTramiteID;

                    opt.AsocExpedienteOpcionID = appExpediente.SaveAsocExpedienteOpcion(opt);
                }

                if (IsResolucion)
                {
                    #region Firmas

                    if (!IsNew)
                        appExpediente.BorrarFirmasExpediente(dto.ExpedienteID);

                    
                    if (active.IsInputValue(Request.Form["strFirmas"]))
                    {
                        try
                        {
                            string strFirmas = active.GetStringValueForm(Request.Form["strFirmas"]);
                            List<DTO.Models.Firma> listaFirmas = JsonConvert.DeserializeObject<List<DTO.Models.Firma>>(strFirmas);
                            IList<DTO.Models.AsocEscritoDocto> asocEscrito = appExpediente.GetAsocEscritoDocto(dto.ExpedienteID);
                            foreach (var item in listaFirmas)
                            {
                                //DTO.Models.Firma f = new Application.DTO.Models.Firma();
                                //f.FirmaID = 0;
                                //f.UsuarioID = item.UsuarioFirmaID;
                                //f.ExpedienteID = dto.ExpedienteID;
                                //f.Orden = item.Orden;
                                //f.FirmaID = appExpediente.SaveFirma(f);

                                //foreach (var asEsc in asocEscrito)
                                //{
                                //    DTO.Models.AsocFirmaDocto asoc = new DTO.Models.AsocFirmaDocto();
                                //    asoc.AsocFirmaDoctoID = 0;
                                //    asoc.AsocEscritoDoctoID = asEsc.AsocEscritoDoctoID;
                                //    asoc.FirmaID = f.FirmaID;
                                //    asoc.AsocFirmaDoctoID = appExpediente.SaveAsocFirmaDocto(asoc);
                                //}
                                
                            }
                        }
                        catch (Exception ex2)
                        {
                            Logger.Execute().Error(ex2);
                        }
                    }

                    #endregion
                }

                retorno = Enums.ReturnJson.ActionSuccess;

                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.SaveExpediente;
                dbLog.Save();
                #endregion

                return Json(new { result = (int)retorno, ParteID = dto.ExpedienteID, IsNew = IsNew });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }



        /// <summary>
        /// GetEstadoAplica
        /// </summary>
        /// <param name="TipoTramiteID"></param>
        /// <param name="EstadoCausaID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetEstadoAplica(int TipoTramiteID, int EstadoCausaID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                var EstadosAplica = appExpediente.GetEstadosAplica(TipoTramiteID, EstadoCausaID);

                List<DTO.Models.OpcionesTramite> lista = new List<DTO.Models.OpcionesTramite>();

                foreach (var item in EstadosAplica)
                {
                    lista.Add(new DTO.Models.OpcionesTramite() {
                        OpcionesTramiteID = item.AsocTipoTramiteOpciones.OpcionesTramite.OpcionesTramiteID,
                        Descripcion = item.AsocTipoTramiteOpciones.OpcionesTramite.Descripcion.Trim()
                    });
                }

                return Json(lista);
                 
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        /// <summary>
        /// GetDetalleEvento
        /// </summary>
        /// <param name="TipoTramiteID"></param>
        /// <param name="ExpedienteID"></param>
        /// <param name="EstadoCausaID"></param>
        /// <param name="OpcionesTramiteID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetDetalleEvento(int TipoTramiteID, int ExpedienteID, int EstadoCausaID, int OpcionesTramiteID)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            try
            {
                if (OpcionesTramiteID == 0 || TipoTramiteID == 0)
                {
                    return Content(string.Empty);
                }

                var EstadosLista = appExpediente.GetEstadosAplica(TipoTramiteID, EstadoCausaID);

                DTO.DataForm DataForm = new DTO.DataForm();
                DataForm.ExpedienteID = ExpedienteID;
                DataForm.EstadosAplica = EstadosLista.FirstOrDefault(x => x.AsocTipoTramiteOpciones.OpcionesTramiteID == OpcionesTramiteID);

                ViewBag.DataForm = DataForm;

                DTO.Models.Expediente model = new DTO.Models.Expediente();
                model.ExpedienteID = ExpedienteID;

                if (model.ExpedienteID != 0)
                {
                    model = appExpediente.GetExpediente(ExpedienteID);  
                }

                return PartialView("_DetalleEvento", model);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }


        #endregion



        /// <summary>
        /// ActionExpediente => Generic actions
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Identidad"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ActionExpediente(Enums.ActionSystem action, int Identidad = 0)
        {
            var sso = new SsoActionResult();
            if (!sso.AsyncAuthenticate(ControllerContext)) return Response403();

            DateTime ahora = DateTime.Now;
            int UserActiveID = sso.GetUsuarioActivoID();
            DTO.Models.Usuario UserActive = sso.UserActive;

            //bool IsDev = true;
            //if (IsDev)
            //{
            //    UserActiveID = 2;
            //    UserActive = appCommon.GetUsuarioByID(UserActiveID);
            //}

            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            dbLog.UsuarioID = UserActiveID;

            try
            {
                Enums.ReturnJson result = Enums.ReturnJson.SinAccion;
                string message = "";
                bool activeAction = false;

                if (action == Enums.ActionSystem.EliminarParte)
                {
                    appExpediente.DeleteParteByID(Identidad);

                    dbLog.TipoLog = Enums.TipoLog.DeleteParteByID;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.EliminarTabla)
                {
                    appExpediente.SetEstadoTabla(Identidad, Domain.Infrastructure.EstadoTabla.Eliminado);

                    dbLog.TipoLog = Enums.TipoLog.EliminarTabla;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.QuitarVigenciaDetalleTabla)
                {
                    appExpediente.SetVigenciaDetalleTabla(Identidad, false);

                    dbLog.TipoLog = Enums.TipoLog.QuitarVigenciaDetalleTabla;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.AgregarCausaTabla)
                {
                    int TablaID = active.GetIntValueForm(Request.Form["TablaID"]);
                    int CausaID = Identidad;

                    DTO.Models.DetalleTabla detalle = new DTO.Models.DetalleTabla();
                    detalle.DetalleTablaID = 0;
                    detalle.CausaID = CausaID;
                    detalle.TablaID = TablaID;
                    detalle.Orden = 0;
                    detalle.Vigente = true;

                    detalle.DetalleTablaID = appExpediente.SaveDetalleTabla(detalle, true);

                    dbLog.TipoLog = Enums.TipoLog.AgregarCausaTabla;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.EliminarEstadoDiario)
                {
                    appExpediente.SetTipoEstadoDiario(Identidad, Domain.Infrastructure.TipoEstadoDiario.Eliminado);

                    dbLog.TipoLog = Enums.TipoLog.EliminarEstadoDiario;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.QuitarVigenciaDetalleEstadoDiario)
                {
                    int[] keys = active.GetArrayIntValueForm(Request.Form["Keys"]);

                    for (int i = 0; i < keys.Length; i++)
                    {
                        appExpediente.SetVigenciaDetalleEstadoDiario(keys[i], false);

                        dbLog.TipoLog = Enums.TipoLog.QuitarVigenciaDetalleEstadoDiario;
                        dbLog.Save();
                    }
                }

                if (action == Enums.ActionSystem.AgregarExpedienteEstadoDiario)
                {
                    int EstadoDiarioID = active.GetIntValueForm(Request.Form["EstadoDiarioID"]);
                    int CausaID = Identidad;

                    DTO.Models.EstadoDiario EstadoDiario = appExpediente.GetEstadoDiarioByID(EstadoDiarioID);

                    DTO.Models.Causa model = appExpediente.GetCausa(CausaID);
                    model.Expediente = appExpediente.GetExpedienteByCausa(CausaID);

                    //Buscar que no este antes de agregar

                    foreach (var item in model.Expediente)
                    {
                        if (item.IsDisponibleResolucion(EstadoDiario.Fecha))
                        {
                            DTO.Models.DetalleEstadoDiario ed = new DTO.Models.DetalleEstadoDiario();
                            ed.DetalleEstadoDiarioID = 0;
                            ed.ExpedienteID = item.ExpedienteID;
                            ed.EstadoDiarioID = EstadoDiarioID;
                            ed.Vigente = true;

                            ed.DetalleEstadoDiarioID = appExpediente.SaveDetalleEstadoDiario(ed);

                            dbLog.TipoLog = Enums.TipoLog.AgregarExpedienteEstadoDiario;
                            dbLog.Save();
                        }
                    }
                }

                if (action == Enums.ActionSystem.FinalizarEstadoDiario)
                {
                    appExpediente.SetTipoEstadoDiario(Identidad, Domain.Infrastructure.TipoEstadoDiario.FirmadoPublicado);

                    dbLog.TipoLog = Enums.TipoLog.FinalizarEstadoDiario;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.FinalizarTabla)
                {
                    appExpediente.SetEstadoTabla(Identidad, Domain.Infrastructure.EstadoTabla.FirmadoPublicado);

                    dbLog.TipoLog = Enums.TipoLog.FinalizarTabla;
                    dbLog.Save();
                }

                if (action == Enums.ActionSystem.FinalizarExpediente)
                {
                    int ExpedienteID = Identidad;
                    int CausaID = active.GetIntValueForm(Request.Form["CausaID"]);

                    appExpediente.SetExpedienteFinalizado(ExpedienteID, true);

                    dbLog.ExpedienteID = ExpedienteID;
                    dbLog.TipoLog = Enums.TipoLog.FinalizarExpediente;
                    dbLog.Save();

                    DTO.Models.Expediente expediente = appExpediente.GetExpediente(ExpedienteID);

                    var AsocExpedienteOpcion = expediente.AsocExpedienteOpcion.LastOrDefault();
                    if (AsocExpedienteOpcion != null)
                    {
                        var asoc = appCommon.GetAsocTipoTramiteOpciones(expediente.TipoTramiteID)
                            .FirstOrDefault(x => x.OpcionesTramiteID == AsocExpedienteOpcion.OpcionesTramiteID);

                        if (asoc !=null)
                        {
                            active.SetCambiaEstadoCausa(ahora, dbLog.UsuarioID, CausaID, asoc, dbLog);
                        }
                    }
                }

                if (action == Enums.ActionSystem.GenerarExpedientePDF)
                {
                    int CausaID = Identidad;

                    DTO.Models.Causa causa = appExpediente.GetCausa(CausaID);
                    causa.Expediente = appExpediente.GetExpedienteByCausa(CausaID);
                    causa.Parte = appExpediente.GetParteByCausa(CausaID);
                    causa.DocumentoCausa = appExpediente.GetDocumentoCausa(CausaID, Domain.Infrastructure.TipoDocumento.Causa);

                    #region PDF
                    Enums.TipoDocumento tipoDocumento = Enums.TipoDocumento.ExpedienteElectronicoPDF;

                    string FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}", causa.Anio, causa.CausaID, tipoDocumento, ahora.ToString("HHmmss"));

                    string MergePath = WebConfig.PathBaseRepository + FolderSave;
                    if (!System.IO.Directory.Exists(MergePath)) System.IO.Directory.CreateDirectory(MergePath);

                    DocPdf pdf = new DocPdf();
                    pdf.Causa = causa;
                    pdf.Usuario = UserActive;
                    pdf.IsExpedienteElectronico = true;
                    pdf.Ahora = ahora;
                    pdf.MapPath = Server.MapPath("~");
                    pdf.PathSave = MergePath;
                    pdf.Filename = $"Exp_{causa.NumeroTicket}_{ahora.ToString("HHmmss")}.pdf";
                    pdf.Titulo = "Registro de Expedientes";
                    pdf.CreateDocument("TDPI: Expediente Electrónico");

                    pdf.SetExpedienteElectronico();
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

                    DTO.Models.DocumentoCausa doc = new DTO.Models.DocumentoCausa();
                    doc.DocumentoCausaID = 0;
                    doc.CausaID = CausaID;
                    doc.VersionEncriptID = enc.VersionEncriptID;
                    doc.Hash = SHAFile;
                    doc.NombreArchivoFisico = pdf.Filename;
                    doc.Fecha = ahora;
                    doc.Descripcion = "Expediente Electrónico PDF";

                    doc.DocumentoCausaID = appExpediente.SaveDocumentoCausa(doc);

                    var asocs = appCommon.GetTipoDocumentoAdjuntoByID((int)tipoDocumento);
                    if (asocs.Count > 0)
                    {
                        var AsocTipoDocumentoAdjunto = asocs.FirstOrDefault();

                        DTO.Models.AsocCausaDocumento asoc = new DTO.Models.AsocCausaDocumento();
                        asoc.AsocCausaDocumentoID = 0;
                        asoc.DocumentoAdjuntoID = AsocTipoDocumentoAdjunto.DocumentoAdjuntoID;
                        asoc.DocumentoCausaID = doc.DocumentoCausaID;
                        asoc.CompromisoID = 0;
                        asoc.AsocCausaDocumentoID = appExpediente.SaveAsocCausaDocumento(asoc);
                    }

                    result = Domain.Infrastructure.ReturnJson.ActionSuccess;

                    #endregion

                    return Json(new
                    {
                        result = (int)result,
                        h = doc.Hash,
                        d = doc.DocumentoCausaID,
                        i = CausaID
                    });
                }

                return Json(new { result = (int)result, message = message, active = activeAction });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

    }
}