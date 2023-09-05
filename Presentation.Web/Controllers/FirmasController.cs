using Application.DTO;
using Application.DTO.Models;
using Application.Services;
using Presentation.Web.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;

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

                int EstadoDoctoFirma = active.GetIntValueForm(Request.Form["EstadoDoctoFirma"]);

                IList<Firma> lista = appExpediente.GetEscritorioFirmas(filtros);

                IList<EscritorioFirmaJson> json = new List<EscritorioFirmaJson>();

                var usuarios = appCommon.GetUsuarios();
                string strResponsableActual = string.Empty;

                foreach (var item in lista)
                {
                    #region AsocFirmaDocto

                    foreach (var asoc in item.AsocFirmaDocto)
                    {
                        #region Responsable Actual
                        DTO.Models.AsocFirmaDocto asocSiguiente = new DTO.Models.AsocFirmaDocto();

                        var firmas = asoc.AsocEscritoDocto.Expediente.AsocExpeFirma;
                        foreach (var f in firmas.OrderBy(x => x.Firma.Orden))
                        {
                            if (asocSiguiente.AsocFirmaDoctoID > 0) continue;

                            foreach (var _asoc in f.Firma.AsocFirmaDocto)
                            {
                                if (!_asoc.IsFirmado)
                                {
                                    asocSiguiente.AsocFirmaDoctoID = _asoc.AsocFirmaDoctoID;
                                    asocSiguiente.Firma = new DTO.Models.Firma();
                                    asocSiguiente.Firma.UsuarioID = _asoc.Firma.UsuarioID;
                                }
                            }
                        }

                        strResponsableActual = (asocSiguiente.AsocFirmaDoctoID > 0) ? usuarios.FirstOrDefault(x => x.UsuarioID == asocSiguiente.Firma.UsuarioID).GetFullName() : string.Empty;
                        #endregion

                        Link a = new Link();

                        #region Filtros Estado Firma
                        if (EstadoDoctoFirma == (int)Enums.EstadosDoctoFirma.Firmadas && !asoc.IsFirmado)
                        {
                            continue;
                        }

                        if (EstadoDoctoFirma == (int)Enums.EstadosDoctoFirma.SoloPendientes && asoc.IsFirmado)
                        {
                            continue;
                        }

                        #endregion

                        bool PuedeFirmar = (asocSiguiente.Firma != null && asocSiguiente.Firma.UsuarioID == UsuarioActive);

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
                        a.click = "FirmarDocumento(" + asoc.AsocEscritoDocto.ExpedienteID
                                                    + "," + asoc.AsocFirmaDoctoID
                                                    + "," + UsuarioActive + ")";
                        a.href = "javascript:void(0);";
                        string bt2 = a.Generate(false);
                        #endregion

                        #region StringBuilder
                        string str = string.Empty;

                        str += bt1; //Documento
                        if (PuedeFirmar) str += bt2; //Firmar           
                        #endregion

                        #region Json

                        DTO.EscritorioFirmaJson j = new DTO.EscritorioFirmaJson();
                        j.diasT = asoc.AsocEscritoDocto.Expediente.GetDiasTranscurridos();
                        j.St1 = asoc.AsocEscritoDocto.Expediente.TipoTramite.AsocTipoTramiteOpciones.FirstOrDefault().Status1;
                        j.St2 = asoc.AsocEscritoDocto.Expediente.TipoTramite.AsocTipoTramiteOpciones.FirstOrDefault().Status2;
                        j.us = item.UsuarioID.ToString();
                        j.ticket = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.NumeroTicket;
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
                        if (EstadoDoctoFirma == (int)Enums.EstadosDoctoFirma.Firmadas && !asoc.IsFirmado)
                        {
                            continue;
                        }

                        if (EstadoDoctoFirma == (int)Enums.EstadosDoctoFirma.SoloPendientes && asoc.IsFirmado)
                        {
                            continue;
                        }

                        #endregion

                        bool PuedeFirmar = (asocSiguiente.Firma != null && asocSiguiente.Firma.UsuarioID == UsuarioActive);

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
                        j.St1 = int.MaxValue; //Status 1
                        j.St2 = int.MaxValue; //Status 2
                        j.us = item.UsuarioID.ToString();
                        j.ticket = asoc.DocumentoSistema.TipoDocumento.Descripcion.Trim();
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
        public ActionResult FirmarDocumento(int ExpedienteID, int AsocFirmaDoctoID, int UsuarioID)
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
                        return Json(new { result = (int)retorno });
                    }
                    else
                    {
                        asocFirma.AsocFirmaDoctoID = appExpediente.SaveAsocFirmaDocto(asocFirma);
                    }
                }

                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.FirmarDocumento;
                dbLog.Save();
                #endregion

                return Json(new
                {
                    result = (int)retorno,

                });
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

                if (asocFirma != null)
                {
                    if (asocFirma.Firma.UsuarioID != UsuarioID)
                    {
                        return Json(new { result = (int)retorno });
                    }
                    else
                    {
                        asocFirma.IsFirmado = true;
                        asocFirma.AsocDocSistemaFirmaID = appExpediente.SaveAsocDocSistemaFirma(asocFirma);
                    }
                }

                #region LogSistema
                dbLog.TipoLog = Enums.TipoLog.FirmarDocumento;
                dbLog.Save();
                #endregion

                return Json(new
                {
                    result = (int)retorno,

                });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                throw;
            }
        }


    }
}