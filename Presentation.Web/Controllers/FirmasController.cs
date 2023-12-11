using Application.DTO.Utils;
using Application.Services;
using Infrastructure.Logging;
using Infrastructure.Utils.Extensions;
using Newtonsoft.Json;
using Presentation.Web.Context;
using System;
using System.Collections.Generic;
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
    public class FirmasController : GlobalController
    {

        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IExpedienteAppServices appExpediente = new ExpedienteAppServices();
        private readonly Commons active = new Commons();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            bool IsTDPI = sso.IsTDPI();
            if (!IsTDPI) return Response403();

            int UsuarioActive = sso.GetUsuarioActivoID();

            DTO.FiltrosEscritorio filtros = new DTO.FiltrosEscritorio();
            filtros.TipoCausa = appCommon.GetTipoCausa(true);
            filtros.EstadoCausa = appCommon.GetEstadoCausa(true);
            filtros.UsuarioID = UsuarioActive;

            ViewBag.FiltrosEscritorio = filtros;

            #region Filtro Tipo Documento
            List<int> TipoDocumentoFirma = new List<int>();
            TipoDocumentoFirma.Add((int)Enums.TipoDocumento.Expediente);
            TipoDocumentoFirma.Add((int)Enums.TipoDocumento.Tabla);
            TipoDocumentoFirma.Add((int)Enums.TipoDocumento.EstadoDiario);

            var TipoDocumentoList = appCommon.GetTipoDocumento().Where(x => TipoDocumentoFirma.Contains(x.TipoDocumentoID)).ToList();
            ViewBag.TipoDocumentoList = TipoDocumentoList;
            #endregion


            return View();
        }

        /// <summary>
        /// GetEscritorioTDPI
        /// </summary>
        /// <param name="filtros"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetEscritorioFirma(DTO.FiltrosEscritorio filtros)
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
                filtros.UsuarioID = UsuarioActive;
                filtros.NumeroTicket = active.GetStringValueForm(Request.Form["NumeroTicket"]);
                filtros.FechaDesde = active.GetStringValueForm(Request.Form["FechaDesde"]);
                filtros.FechaHasta = active.GetStringValueForm(Request.Form["FechaHasta"]);

                filtros.EstadoFirma = active.GetIntValueForm(Request.Form["EstadoDoctoFirma"]);

                IList<DTO.Models.Firma> lista = appExpediente.GetEscritorioFirmas(filtros);
                
                IList<DTO.EscritorioFirmaJson> json = new List<DTO.EscritorioFirmaJson>();

                var usuarios = appCommon.GetUsuarios();
                string strResponsableActual = string.Empty;

                foreach (var item in lista)
                {
                    #region AsocFirmaDocto

                    foreach (var asoc in item.AsocFirmaDocto)
                    {
                        var iTipoTramite = asoc.AsocEscritoDocto.Expediente.TipoTramite.TipoTramiteID;
                        var oEsResolucion = iTipoTramite == (int)Enums.TipoTramite.Resolucion;
                        #region Responsable Actual
                        DTO.Models.AsocFirmaDocto asocSiguiente = new DTO.Models.AsocFirmaDocto();
                        int DoctoFirma = asoc.AsocEscritoDoctoID;
                        var firmas = asoc.AsocEscritoDocto.Expediente.AsocExpeFirma;
                        foreach (var f in firmas.OrderBy(x => x.Firma.Orden))
                        {
                            //if (asocSiguiente.AsocFirmaDoctoID > 0) continue; GS:28112023
                            if (!oEsResolucion)
                            {
                                if (asocSiguiente.AsocFirmaDoctoID > 0) continue;
                            }
                            foreach (var _asoc in f.Firma.AsocFirmaDocto.Where(x=> x.AsocEscritoDoctoID == DoctoFirma))
                            {
                                if (!_asoc.IsFirmado)
                                {
                                    asocSiguiente.AsocFirmaDoctoID = _asoc.AsocFirmaDoctoID;
                                    asocSiguiente.Firma = new DTO.Models.Firma();
                                    asocSiguiente.Firma.UsuarioID = _asoc.Firma.UsuarioID;
                                    //GS:28112023
                                    strResponsableActual = (asocSiguiente.AsocFirmaDoctoID > 0) ? strResponsableActual + usuarios.FirstOrDefault(x => x.UsuarioID == asocSiguiente.Firma.UsuarioID).GetFullName()+"<br>" : strResponsableActual + string.Empty;
                                }
                            }
                        }
                        //GS:28112023
                        //strResponsableActual = (asocSiguiente.AsocFirmaDoctoID > 0) ? usuarios.FirstOrDefault(x => x.UsuarioID == asocSiguiente.Firma.UsuarioID).GetFullName() : string.Empty;
                        #endregion

                        Link a = new Link();

                        #region Filtros Estado Firma
                        if (filtros.EstadoFirma == (int)Enums.EstadosDoctoFirma.Firmadas && !asoc.IsFirmado)
                        {
                            continue;
                        }

                        if (filtros.EstadoFirma == (int)Enums.EstadosDoctoFirma.SoloPendientes && asoc.IsFirmado)
                        {
                            continue;
                        }

                        #endregion

                        //bool PuedeFirmar = true; //GS: (asocSiguiente.Firma != null && asocSiguiente.Firma.UsuarioID == UsuarioActive);
                        bool PuedeFirmar = oEsResolucion ? true : (asocSiguiente.Firma != null && asocSiguiente.Firma.UsuarioID == UsuarioActive);

                        #region bt1: Descargar Documento
                        a._class = "";
                        a.click = "";
                        a.title = "Descargar Documento";
                        a.xicon = "x-icon x-icon-pdf";
                        a.href = "javascript:void(0);";
                        a.click = "getDownloadFile(" + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausaID
                                                    + "," + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.CausaID
                                                    + ",\"" + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Hash.Trim() + "\","
                                                    + (int)Enums.TipoDocumento.Expediente + ")";
                        string bt1 = a.Generate(false);
                        #endregion

                        #region bt2: Firmar
                        a.title = "Firmar";
                        a.xicon = "x-icon-vcard";
                        a.style = "margin-left:5px";
                        a.click = "FirmarDocumentoShowPdf(" + asoc.AsocEscritoDocto.ExpedienteID
                                                    + "," + asoc.AsocFirmaDoctoID
                                                    + "," + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.CausaID
                                                    + ",\"" + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Hash.Trim() + "\""
                                                    + "," + asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausaID
                                                    + "," + (int)Enums.TipoDocumento.Expediente
                                                    + "," + UsuarioActive
                                                    + "," + iTipoTramite + ")";
                        a.href = "javascript:void(0);";
                        string bt2 = a.Generate(false);
                        #endregion

                        #region StringBuilder
                        string str = string.Empty;

                        str += bt1; //Documento
                        if (PuedeFirmar) str += bt2; //Firmar           
                        #endregion

                        #region Json
                        string strTicket = $"{asoc.AsocEscritoDocto.Expediente.TipoTramite.Descripcion.Trim()} - Rol: {asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.NumeroTicket.Trim()}";

                        DTO.EscritorioFirmaJson j = new DTO.EscritorioFirmaJson();
                        j.diasT = asoc.AsocEscritoDocto.Expediente.GetDiasTranscurridos();
                        j.St1 = asoc.AsocEscritoDocto.Expediente.TipoTramite.AsocTipoTramiteOpciones.FirstOrDefault().Status1;
                        j.St2 = asoc.AsocEscritoDocto.Expediente.TipoTramite.AsocTipoTramiteOpciones.FirstOrDefault().Status2;
                        j.us = item.UsuarioID.ToString();
                        j.ticket = strTicket;
                        j.date = asoc.AsocEscritoDocto.Expediente.FechaExpediente.ToString();
                        j.resp = strResponsableActual;
                        j.AccionesList = String.Join(",", str);
                        json.Add(j);

                        #endregion
                    }
                    #endregion

                    #region AsocDocSistemaFirma
                    foreach (var asoc in item.AsocDocSistemaFirma)
                    {
                        #region Responsable Actual
                        DTO.Models.AsocDocSistemaFirma asocSiguiente = new DTO.Models.AsocDocSistemaFirma();

                        var firmas = asoc.DocumentoSistema.AsocDocSistemaFirma;
                        foreach (var f in firmas.OrderBy(x => x.Firma.Orden))
                        {
                            if (asocSiguiente.AsocDocSistemaFirmaID > 0) continue;
                                                        
                            if (!f.IsFirmado)
                            {
                                asocSiguiente.AsocDocSistemaFirmaID = f.AsocDocSistemaFirmaID;
                                asocSiguiente.Firma = new DTO.Models.Firma();
                                asocSiguiente.Firma.UsuarioID = f.Firma.UsuarioID;
                            }
                            
                        }

                        strResponsableActual = (asocSiguiente.AsocDocSistemaFirmaID > 0) ? usuarios.FirstOrDefault(x => x.UsuarioID == asocSiguiente.Firma.UsuarioID).GetFullName() : string.Empty;
                        #endregion

                        Link a = new Link();

                        #region Filtros Estado Firma
                        if (filtros.EstadoFirma == (int)Enums.EstadosDoctoFirma.Firmadas && !asoc.IsFirmado)
                        {
                            continue;
                        }

                        if (filtros.EstadoFirma == (int)Enums.EstadosDoctoFirma.SoloPendientes && asoc.IsFirmado)
                        {
                            continue;
                        }

                        #endregion

                        bool PuedeFirmar = (asocSiguiente.Firma != null && asocSiguiente.Firma.UsuarioID == UsuarioActive);

                        bool IsTabla = asoc.DocumentoSistema.AsocDocumentoSistemaTabla.Count > 0;

                        string Fecha = IsTabla ? asoc.DocumentoSistema.AsocDocumentoSistemaTabla.FirstOrDefault().Tabla.Fecha.ToString("dd-MM-yyyy") : asoc.DocumentoSistema.AsocDocumentoSistemaEstadoDiario.FirstOrDefault().EstadoDiario.Fecha.ToString("dd-MM-yyyy");

                        #region bt1: Descargar Documento
                        a._class = "";
                        a.click = "";
                        a.title = "Descargar Documento";
                        a.xicon = "x-icon x-icon-pdf";
                        a.href = "javascript:void(0);";
                        a.click = "getDownloadFile(" + asoc.DocumentoSistemaID
                                                    + "," + 0
                                                    + ",\"" + asoc.DocumentoSistema.Hash.Trim() + "\","
                                                    + asoc.DocumentoSistema.TipoDocumentoID + ")";
                        string bt1 = a.Generate(false);
                        #endregion

                        #region bt2: Firmar
                        a.title = "Firmar";
                        a.xicon = "x-icon-vcard";
                        a.style = "margin-left:5px";
                        a.click = "alert('plop!')";
                        a.click = "FirmarDocumentoSistema(" + asoc.DocumentoSistemaID
                                                    + "," + item.FirmaID
                                                    + "," + UsuarioActive + ")";
                        a.href = "javascript:void(0);";
                        string bt2 = a.Generate(false);
                        #endregion

                        #region StringBuilder
                        string str = string.Empty;

                        str += bt1;//str.Add(bt1); //Documento
                        if (PuedeFirmar) str += bt2; //str.Add(bt2); //Firmar           
                        #endregion

                        #region Json

                        DTO.EscritorioFirmaJson j = new DTO.EscritorioFirmaJson();
                        j.diasT = asoc.DocumentoSistema.GetDiasTranscurridos();
                        j.St1 = 0; //Status 1
                        j.St2 = 0; //Status 2
                        j.us = item.UsuarioID.ToString();
                        j.ticket = $"{asoc.DocumentoSistema.TipoDocumento.Descripcion.Trim()}: {Fecha}";
                        j.date = asoc.DocumentoSistema.Fecha.ToString();
                        j.resp = strResponsableActual;
                        j.AccionesList = String.Join(",", str);
                        json.Add(j);

                        #endregion
                    }
                    #endregion               
                }

                JsonResult jsonResult = Json(json);
                jsonResult.MaxJsonLength = int.MaxValue;

                return jsonResult;
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult FirmarDocumentoExpediente(int ExpedienteID, int AsocFirmaDoctoID, int DocumentoCausaID, int UsuarioID)
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
                DTO.Models.Expediente expediente = appExpediente.GetExpediente(ExpedienteID);
                DTO.Models.Causa causa = appExpediente.GetCausa(expediente.CausaID);
                DTO.Models.DocumentoCausa DocPorFimar = appExpediente.GetDocumentoCausa(expediente.CausaID, Enums.TipoDocumento.Expediente)
                    .FirstOrDefault(x => x.DocumentoCausaID == DocumentoCausaID);


                Enums.ReturnJson retorno = Enums.ReturnJson.SinAccion;

                if (UsuarioActive != UsuarioID)
                {
                    return Json(new { result = (int)retorno, message = "" });
                }

                IList<DTO.Models.Firma> firmas = appExpediente.GetFirmaByExpedienteID(ExpedienteID);

                DTO.Models.AsocFirmaDocto asocFirma = new DTO.Models.AsocFirmaDocto();     
                
                foreach (var f in firmas)
                {
                    if (f.UsuarioID != UsuarioID) continue;

                    foreach (var asoc in f.AsocFirmaDocto)
                    {
                        if (asoc.AsocFirmaDoctoID == AsocFirmaDoctoID)
                        {
                            asocFirma.AsocFirmaDoctoID = asoc.AsocFirmaDoctoID;
                            asocFirma.FirmaID = asoc.FirmaID;
                            asocFirma.AsocEscritoDoctoID = asoc.AsocEscritoDoctoID;
                            asocFirma.IsFirmado = true;
                            asocFirma.Firma = new DTO.Models.Firma();
                            asocFirma.Firma.FirmaID = f.FirmaID;
                            asocFirma.Firma.UsuarioID = f.UsuarioID;
                        }
                    }
                }

                if (asocFirma.AsocFirmaDoctoID > 0)
                {
                    if (asocFirma.Firma.UsuarioID != UsuarioID)
                    {
                        return Json(new { result = (int)retorno, message = "" });
                    }
                    else
                    {
                        DTO.Models.Usuario usuario = sso.UserActive;
                        usuario.SignerEncrypted = string.Empty;

                        DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(1);

                        string signer = usuario.Signer.Trim();

                        if (string.IsNullOrWhiteSpace(signer))
                        {
                            retorno = Enums.ReturnJson.ErrorCodigoVerificacion;
                            return Json(new { result = (int)retorno, message = "" });
                        }
                        else
                        {
                            bool IsWsDisable = WebConfig.SignService_Identity == (int)Enums.SignServiceIdentity.Disable;

                            if (!IsWsDisable)
                            {
                                bool IsWsEconomia = WebConfig.SignService_Identity == (int)Enums.SignServiceIdentity.Economia;

                                if (IsWsEconomia)
                                {
                                    try
                                    {
                                        Infrastructure.Utils.TheHash xEncodeUser = new Infrastructure.Utils.TheHash(enc.Cadena.Trim());
                                        usuario.SignerEncrypted = xEncodeUser.DecryptData(signer);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Execute().Error(ex);
                                        retorno = Enums.ReturnJson.ErrorCodigoVerificacion;
                                        return Json(new { result = (int)retorno, message = ex.Message });
                                    }

                                    TheHash xEncodeFile = new TheHash(DocPorFimar.VersionEncript.Cadena.Trim());

                                    try
                                    {
                                        string pathFisicoTemp = xEncodeFile.DecryptData(DocPorFimar.Hash.Trim());
                                        string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);
                                        string pathFisico = HashDecode[1];
                                        string EndPathFile = WebConfig.PathBaseRepository + pathFisico;

                                        Enums.SignPosition signPosition = GetSignPosition(firmas);

                                        SignFile sign = new SignFile(EndPathFile,
                                            DocPorFimar.NombreArchivoFisico.Trim(),
                                            usuario.SignerEncrypted,
                                            Enums.SignPageNumber.LastPage,
                                            signPosition);

                                        ResponseSign ResponseSign = sign.Sign();

                                        if (ResponseSign.Status == Enums.SignStatus.Success.ToString())
                                        {
                                            sign.FileSignedBase64 = ResponseSign.SignedBase64EncodedString;
                                            sign.ReplaceFile(ahora);

                                            asocFirma.AsocFirmaDoctoID = appExpediente.SaveAsocFirmaDocto(asocFirma);
                                            retorno = Enums.ReturnJson.ActionSuccess;
                                        }
                                        else
                                        {
                                            Logger.Execute().Info(JsonConvert.SerializeObject(ResponseSign));
                                            retorno = Enums.ReturnJson.ErrorModelo;
                                            return Json(new { result = (int)retorno, message = ResponseSign.Message });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Execute().Error(ex);
                                        retorno = Enums.ReturnJson.ErrorModelo;

                                        return Json(new { result = (int)retorno, message = ex.Message });
                                    }
                                }
                            }
                            else
                            {
                                Logger.Execute().Info("INFO Firma => SignService Disable");
                                return Json(new { result = (int)retorno, message = "" });
                            }
                        }
                    }
                }

                
                #region Log Causa

                LogCausa _logC = new LogCausa();
                _logC.Fecha = ahora;
                _logC.CausaID = causa.CausaID;
                _logC.UsuarioID = UsuarioActive;
                _logC.EstadoCausa = (Enums.EstadoCausa)causa.EstadoCausaID;
                _logC.Observaciones = $"Documento: {DocPorFimar.NombreArchivoFisico.Trim()}";
                _logC.TipoLog = Enums.TipoLog.FirmarDocumento;
                _logC.Save();
                #endregion

                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.FirmarDocumento;
                dbLog.Save();
                #endregion
                
                return Json(new { result = (int)retorno, message = "" });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }

        private Enums.SignPosition GetSignPosition(IList<DTO.Models.Firma> firmas)
        {
            Enums.SignPosition retorno = Enums.SignPosition.BOTTOM_LEFT;
            int count = 1;
            foreach (var item in firmas)
            {
                foreach (var asoc in item.AsocFirmaDocto)
                {
                    if (asoc.IsFirmado)
                    {
                        count++;
                    }
                }
            }

            if (count == 1) retorno = Enums.SignPosition.BOTTOM_LEFT;
            if (count == 2) retorno = Enums.SignPosition.BOTTOM_EDGE_CENTER;
            if (count == 3) retorno = Enums.SignPosition.BOTTOM_RIGHT;
            if (count == 4) retorno = Enums.SignPosition.LEFT_TOP;
            if (count == 5) retorno = Enums.SignPosition.TOP_EDGE_CENTER;
            if (count > 5) retorno = Enums.SignPosition.TOP_RIGHT;

            return retorno;
        }

        private Enums.SignPosition GetSignPosition(IList<DTO.Models.AsocDocSistemaFirma> firmas)
        {
            Enums.SignPosition retorno = Enums.SignPosition.BOTTOM_LEFT;
            int count = 1;
            foreach (var item in firmas)
            {
                if (item.IsFirmado)
                {
                    count++;
                }
            }

            if (count == 1) retorno = Enums.SignPosition.BOTTOM_LEFT;
            if (count == 2) retorno = Enums.SignPosition.BOTTOM_EDGE_CENTER;
            if (count == 3) retorno = Enums.SignPosition.BOTTOM_RIGHT;
            if (count == 4) retorno = Enums.SignPosition.LEFT_TOP;
            if (count == 5) retorno = Enums.SignPosition.TOP_EDGE_CENTER;
            if (count > 5) retorno = Enums.SignPosition.TOP_RIGHT;

            return retorno;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult FirmarDocumentoSistema(int DocumentoSistemaID, int FirmaID, int UsuarioID)
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
                Enums.ReturnJson retorno = Enums.ReturnJson.SinAccion;

                if (UsuarioActive != UsuarioID)
                {
                    return Json(new { result = (int)retorno });
                }

                DTO.Models.AsocDocSistemaFirma asocFirma = appExpediente.GetAsocDocSistemaFirmaByFirmaDocto(FirmaID, DocumentoSistemaID);

                IList<DTO.Models.AsocDocSistemaFirma> firmas = appExpediente.GetAsocDocSistemaFirmaByDocto(DocumentoSistemaID);

                if (asocFirma.AsocDocSistemaFirmaID > 0)
                {
 
                    if (asocFirma.Firma.UsuarioID != UsuarioID)
                    {
                        return Json(new { result = (int)retorno, message = "" });
                    }                   
                    else
                    {
                        DTO.Models.DocumentoSistema DocPorFimar = asocFirma.DocumentoSistema;

                        DTO.Models.Usuario usuario = sso.UserActive;
                        usuario.SignerEncrypted = string.Empty;

                        DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(1);

                        string signer = usuario.Signer.Trim();

                        if (string.IsNullOrWhiteSpace(signer))
                        {
                            retorno = Enums.ReturnJson.ErrorCodigoVerificacion;
                            return Json(new { result = (int)retorno, message = "" });
                        }
                        else
                        {
                            bool IsWsDisable = WebConfig.SignService_Identity == (int)Enums.SignServiceIdentity.Disable;

                            if (!IsWsDisable)
                            {
                                bool IsWsEconomia = WebConfig.SignService_Identity == (int)Enums.SignServiceIdentity.Economia;

                                if (IsWsEconomia)
                                {
                                    try
                                    {
                                        Infrastructure.Utils.TheHash xEncodeUser = new Infrastructure.Utils.TheHash(enc.Cadena.Trim());
                                        usuario.SignerEncrypted = xEncodeUser.DecryptData(signer);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Execute().Error(ex);
                                        retorno = Enums.ReturnJson.ErrorCodigoVerificacion;
                                        return Json(new { result = (int)retorno, message = ex.Message });
                                    }

                                    TheHash xEncodeFile = new TheHash(DocPorFimar.VersionEncript.Cadena.Trim());

                                    try
                                    {
                                        string pathFisicoTemp = xEncodeFile.DecryptData(DocPorFimar.Hash.Trim());
                                        string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);
                                        string pathFisico = HashDecode[1];
                                        string EndPathFile = WebConfig.PathBaseRepository + pathFisico;

                                        Enums.SignPosition signPosition = GetSignPosition(firmas);

                                        SignFile sign = new SignFile(EndPathFile,
                                            DocPorFimar.NombreArchivoFisico.Trim(),
                                            usuario.SignerEncrypted,
                                            Enums.SignPageNumber.LastPage,
                                            signPosition);

                                        ResponseSign ResponseSign = sign.Sign();

                                        if (ResponseSign.Status == Enums.SignStatus.Success.ToString())
                                        {
                                            sign.FileSignedBase64 = ResponseSign.SignedBase64EncodedString;
                                            sign.ReplaceFile(ahora);

                                            asocFirma.IsFirmado = true;
                                            asocFirma.AsocDocSistemaFirmaID = appExpediente.SaveAsocDocSistemaFirma(asocFirma);

                                            retorno = Enums.ReturnJson.ActionSuccess;
                                        }
                                        else
                                        {
                                            Logger.Execute().Info(JsonConvert.SerializeObject(ResponseSign));
                                            retorno = Enums.ReturnJson.ErrorModelo;
                                            return Json(new { result = (int)retorno, message = ResponseSign.Message });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Execute().Error(ex);
                                        retorno = Enums.ReturnJson.ErrorModelo;

                                        return Json(new { result = (int)retorno, message = ex.Message });
                                    }
                                }
                            }
                            else
                            {
                                Logger.Execute().Info("INFO Firma => SignService Disable");
                                return Json(new { result = (int)retorno, message = "" });
                            }
                        }
                    }
                }


                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.FirmarDocumento;
                dbLog.Save();
                #endregion

                return Json(new { result = (int)retorno, message = "" });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }


    }
}