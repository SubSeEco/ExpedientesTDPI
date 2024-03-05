using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using resources = Infrastructure.Resources;
using Domain.Infrastructure;
using System.IO;
using Infrastructure.Logging;
using Application.DTO.Utils;
using System.Collections;
using System.Runtime.InteropServices;
using Infrastructure.Utils.Extensions;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// UploaderController
    /// </summary>
    public class UploaderController : GlobalController
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IExpedienteAppServices appExpediente = new ExpedienteAppServices();

        private readonly Commons active = new Commons();

        /// <summary>
        /// Index desactivado.
        /// </summary>
        /// <returns></returns>
		public ActionResult Index()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            return Redirect(WebConfigValues.Url_MenuSistemas);
        }



        /// <summary>
        /// Carga documentos temporales al sistema.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="TipoDocumentoID"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadFileTemp(HttpPostedFileBase file, int TipoDocumentoID)
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            var frm = Request.Form;

            string Descripcion = active.GetStringValueForm(frm["DescArchivo"]);

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            if (file != null)
            {
                #region Minimo Tamanio

                if (file.ContentLength < 1)
                {
                    return Json(new { Error = 1, Desc = resources.UploadFiles.messageErrorVacio });
                }

                #endregion


                #region Guardar Archivo Temp Dir

                string fileNameTmp, EndPathTmp;

                string BasePath = WebConfigValues.PathBaseRepository;
                string FolderSave = string.Format("\\Temp\\{0}", DateTime.Now.TimeOfDay.Ticks);
                string TmpDir = string.Format("{0}{1}", BasePath, FolderSave);

                try
                {
                    if (!Directory.Exists(TmpDir)) Directory.CreateDirectory(TmpDir);

                    fileNameTmp = Path.GetFileName(CleanFilename(file.FileName));
                    EndPathTmp = Path.Combine(TmpDir, fileNameTmp);

                    file.SaveAs(EndPathTmp);
                }
                catch (Exception ex)
                {
                    Logger.Execute().Error(ex);
                    dbLog.TipoLog = TipoLog.Error;
                    dbLog.Error = "Nombre Archivo: " + file.FileName + " | " + ex.Message.ToString();
                    dbLog.Save();
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_NoSeGuardoElArchivo });
                }

                #endregion


                #region Validaciones MimeType + Tamaño

                IList<DTO.Models.AsocTipoDocumentoAdjunto> FormatosPermitidos = appCommon.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);

                //Mime Type
                List<string> MimeTypePermitidos = new List<string>();
                foreach (var item in FormatosPermitidos)
                {
                    foreach (var str in item.DocumentoAdjunto.TipoFormato.FamiliasMimeType)
                    {
                        MimeTypePermitidos.Add(str.Descripcion.Trim());
                    }
                }

                //Simple Mime
                bool cumpleMEXT = MimeTypePermitidos.Any(i => i.Trim() == file.ContentType);
                if (!cumpleMEXT)
                {
                    System.IO.File.Delete(EndPathTmp);
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_FormatoNoSoportado, Mime = file.ContentType });
                }

                //Complex Mime
                string MimeTypeFile = GetMimeFromFile(EndPathTmp);
                bool cumpleMIME = MimeTypePermitidos.Any(i => i.Trim() == MimeTypeFile);
                if (!cumpleMIME)
                {
                    System.IO.File.Delete(EndPathTmp);
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_FormatoNoSoportado, Mime = MimeTypeFile });
                }

                //Tamaños
                List<int> tamanios = new List<int>();
                foreach (var item in FormatosPermitidos)
                {
                    tamanios.Add(item.DocumentoAdjunto.MaximoTamanoArchivo.Tamano);
                }

                int maximoTamanio = tamanios.Max() * 1048576;
                if (file.ContentLength > maximoTamanio)
                {
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_TamanioMaximo });
                }



                #endregion


                #region Save File ECM or Local Repository

                int FileSharePointID = 0;

                string PathFinalArchivo = string.Empty;
                PathFinalArchivo = Path.Combine(FolderSave, fileNameTmp);

                #endregion

                #region Encrypt File Path

                DTO.Models.VersionEncript enc = appCommon.GetLastVersionEncript();
                TheHash xEncode = new TheHash(enc.Cadena);
                string EncodeEnd = WebConfigValues.IsLocalRepository + TipoUploadChar + PathFinalArchivo + TipoUploadChar + FileSharePointID;
                string SHAFile = xEncode.EncryptData(EncodeEnd);

                #endregion
                try
                {
                    #region Save in Store

                    DTO.Models.DocumentoTmp tmp = new DTO.Models.DocumentoTmp();
                    tmp.DocumentoTmpID = 0;
                    tmp.TipoDocumentoID = TipoDocumentoID;
                    tmp.VersionEncriptID = enc.VersionEncriptID;
                    tmp.Hash = SHAFile;
                    tmp.Fecha = ahora;


                    appCommon.DeleteDocumentosTmp(ahora);

                    tmp.DocumentoTmpID = appCommon.SaveDocumentoTmp(tmp);


                    #region CustomParams
                    var _request = Request;
                    List<DTO.ItemForm> Parametros = new List<DTO.ItemForm>();
                    ArrayList lista = new ArrayList();

                    //keys que no se guardan en el Log
                    String[] Filtro = { "__RequestVerificationToken", "Password", "Clave" };

                    foreach (string item in _request.Form.Keys)
                    {
                        DTO.ItemForm p = new DTO.ItemForm();
                        p.Nombre = item;
                        p.Valor = _request.Form[item];

                        if (!Filtro.Contains(item))
                        {
                            Parametros.Add(p);
                        }
                    }

                    foreach (var item in Parametros)
                    {
                        lista.Add(item.Nombre + "=" + item.Valor);
                    }

                    string customParams = string.Join("#", lista.ToArray());
                    #endregion

                    dbLog.CustomParams = "Params: " + customParams + " | Path: " + PathFinalArchivo;
                    dbLog.Save(true, true);

                    #endregion


                    return Json(new
                    {
                        Error = 0,
                        Desc = "",
                        DocumentoTmpID = tmp.DocumentoTmpID,
                        nombreDoc = fileNameTmp,
                        descDoc = Descripcion,
                        fechaDoc = ahora.ToString("dd-MM-yyyy HH:mm:ss"),
                        cssIcon = System.IO.Path.GetExtension(fileNameTmp).Substring(1),
                        //hash = SHAFile,
                        //encID = enc.VersionEncriptID
                    });
                }
                catch (Exception ex)
                {

                    Logger.Execute().Error(ex);
                    dbLog.TipoLog = TipoLog.Error;
                    dbLog.Error = "Nombre Archivo: " + file.FileName + " | " + ex.Message.ToString();
                    dbLog.Save();
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_NoSePudoProcesarDocumento });
                }

            }

            return Json(new { Error = 0 });
        }



        /// <summary>
        /// Carga documentos al sistema.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
		[HttpPost, ValidateAntiForgeryToken]
        public ActionResult SubirArchivo(HttpPostedFileBase file)
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            DateTime ahora = DateTime.Now;
            DBLogger dbLog = new DBLogger();
            dbLog.Fecha = ahora;
            int UsuarioActive = sso.GetUsuarioActivoID();
            dbLog.UsuarioID = UsuarioActive;

            var frm = Request.Form;
            int CausaID = active.GetIntValueForm(frm["CausaID"]);
            int ExpedienteID = active.GetIntValueForm(frm["ExpedienteID"]);
            int TipoDocumentoID = active.GetIntValueForm(frm["TipoDocumentoID"]);
            string Descripcion = active.GetStringValueForm(frm["Descripcion"]);

            DTO.Models.Causa causa = new Application.DTO.Models.Causa();

            if (TipoDocumentoID != (int)Enums.TipoDocumento.CertificadoTituloAbogado)
            {
                causa = appExpediente.GetCausa(CausaID);
            }

            if (file != null)
            {
                #region Minimo Tamanio

                if (file.ContentLength < 1)
                {
                    return Json(new { Error = 1, Desc = resources.UploadFiles.messageErrorVacio });
                }

                #endregion


                #region Maximo Caractener Nombre

                int maximoCaracteresArchivo = 80;
                int nombreArchivoCaracteres = file.FileName.Length;

                if (nombreArchivoCaracteres > maximoCaracteresArchivo)
                {
                    return Json(new { Error = 1, Desc = string.Format(resources.UploadFiles.msg_MaximoCaracteres, maximoCaracteresArchivo) });
                }

                #endregion


                #region Guardar Archivo Temp Dir

                string fileNameTmp, EndPathTmp;

                string BasePath = WebConfigValues.PathBaseRepository;
                string TmpDir = string.Format("{0}\\Temp", BasePath);

                try
                {
                    if (!Directory.Exists(TmpDir)) Directory.CreateDirectory(TmpDir);

                    fileNameTmp = Path.GetFileName(CleanFilename(file.FileName));
                    EndPathTmp = Path.Combine(TmpDir, fileNameTmp);

                    file.SaveAs(EndPathTmp);
                }
                catch (Exception ex)
                {
                    Logger.Execute().Error(ex);
                    dbLog.TipoLog = TipoLog.Error;
                    dbLog.Error = "Nombre Archivo: " + file.FileName + " | " + ex.Message.ToString();
                    dbLog.Save();
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_NoSeGuardoElArchivo });
                }

                #endregion


                #region Validaciones MimeType + Tamaño

                IList<DTO.Models.AsocTipoDocumentoAdjunto> FormatosPermitidos = appCommon.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);


                //Mime Type
                List<string> MimeTypePermitidos = new List<string>();
                foreach (var item in FormatosPermitidos)
                {
                    foreach (var str in item.DocumentoAdjunto.TipoFormato.FamiliasMimeType)
                    {
                        MimeTypePermitidos.Add(str.Descripcion.Trim());
                    }
                }

                //Simple Mime
                bool cumpleMEXT = MimeTypePermitidos.Any(i => i.Trim() == file.ContentType);
                if (!cumpleMEXT)
                {
                    System.IO.File.Delete(EndPathTmp);
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_FormatoNoSoportado, Mime = file.ContentType });
                }

                //Complex Mime
                string MimeTypeFile = GetMimeFromFile(EndPathTmp);
                bool cumpleMIME = MimeTypePermitidos.Any(i => i.Trim() == MimeTypeFile);
                if (!cumpleMIME)
                {
                    System.IO.File.Delete(EndPathTmp);
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_FormatoNoSoportado, Mime = MimeTypeFile });
                }

                //Tamaños
                List<int> tamanios = new List<int>();
                foreach (var item in FormatosPermitidos)
                {
                    tamanios.Add(item.DocumentoAdjunto.MaximoTamanoArchivo.Tamano);
                }

                int maximoTamanio = tamanios.Max() * 1048576;
                if (file.ContentLength > maximoTamanio)
                {
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_TamanioMaximo });
                }



                #endregion


                #region Local Repository

                string FolderSave = string.Empty;
                string PathFinalArchivo = string.Empty;
                int FileSharePointID = 0;

                if (WebConfigValues.IsLocalRepository)
                {
                    if (TipoDocumentoID == (int)Enums.TipoDocumento.Causa)
                    {
                        FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}",
                            causa.Anio,
                            causa.CausaID,
                            ((TipoDocumento)TipoDocumentoID),
                            DateTime.Now.TimeOfDay.Ticks);
                    }

                    if (TipoDocumentoID == (int)Enums.TipoDocumento.Expediente)
                    {
                        FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}\\{4}",
                            causa.Anio,
                            causa.CausaID,
                            ((TipoDocumento)TipoDocumentoID),
                            ExpedienteID,
                            DateTime.Now.TimeOfDay.Ticks);
                    }

                    if (TipoDocumentoID == (int)Enums.TipoDocumento.CertificadoTituloAbogado)
                    {
                        FolderSave = string.Format("\\{0}\\{1}\\{2}\\{3}",
                          "CertificadoTituloAbogado",
                          ((TipoDocumento)TipoDocumentoID),
                          UsuarioActive,
                          DateTime.Now.TimeOfDay.Ticks);
                    }


                    string MergePath = BasePath + FolderSave;

                    if (!Directory.Exists(MergePath)) Directory.CreateDirectory(MergePath);

                    string EndPath = Path.Combine(MergePath, fileNameTmp);
                    System.IO.File.Move(EndPathTmp, EndPath);

                    PathFinalArchivo = Path.Combine(FolderSave, fileNameTmp);
                }
                else
                {
                    return Json(new { Error = 1, Desc = resources.UploadFiles.msg_NoSeGuardoElArchivo });
                }

                System.IO.File.Delete(EndPathTmp);

                #endregion


                #region Encrypt File Path

                DTO.Models.VersionEncript enc = appCommon.GetLastVersionEncript();
                TheHash xEncode = new TheHash(enc.Cadena);
                string EncodeEnd = WebConfigValues.IsLocalRepository + TipoUploadChar + PathFinalArchivo + TipoUploadChar + FileSharePointID;
                string SHAFile = xEncode.EncryptData(EncodeEnd);

                #endregion


                #region Save in Store

                LogCausa _logC = new LogCausa();
                _logC.Fecha = ahora;
                _logC.CausaID = (TipoDocumentoID != (int)Enums.TipoDocumento.CertificadoTituloAbogado) ? causa.CausaID : 0;
                _logC.UsuarioID = UsuarioActive;
                _logC.EstadoCausa = (TipoDocumentoID != (int)Enums.TipoDocumento.CertificadoTituloAbogado) ? (Enums.EstadoCausa)causa.EstadoCausaID : Enums.EstadoCausa.EventoLog;
                _logC.Observaciones = Descripcion;

                if (TipoDocumentoID == (int)Enums.TipoDocumento.Causa || TipoDocumentoID == (int)Enums.TipoDocumento.Expediente)
                {
                    bool IsExpediente = TipoDocumentoID == (int)Enums.TipoDocumento.Expediente;

                    DTO.Models.DocumentoCausa doc = new DTO.Models.DocumentoCausa();
                    doc.DocumentoCausaID = 0;
                    doc.CausaID = CausaID;
                    doc.VersionEncriptID = enc.VersionEncriptID;
                    doc.Hash = SHAFile;
                    doc.NombreArchivoFisico = fileNameTmp;
                    doc.Fecha = ahora;
                    doc.Descripcion = Descripcion.Left(250);

                    doc.DocumentoCausaID = appExpediente.SaveDocumentoCausa(doc);

                    var asocs = appCommon.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);
                    if (asocs.Count > 0)
                    {
                        var AsocTipoDocumentoAdjunto = asocs.FirstOrDefault();

                        DTO.Models.AsocCausaDocumento asoc = new DTO.Models.AsocCausaDocumento();
                        asoc.AsocCausaDocumentoID = 0;
                        asoc.DocumentoAdjuntoID = AsocTipoDocumentoAdjunto.DocumentoAdjuntoID;
                        asoc.DocumentoCausaID = doc.DocumentoCausaID;
                        asoc.CompromisoID = 0;
                        asoc.AsocCausaDocumentoID = appExpediente.SaveAsocCausaDocumento(asoc);

                        if (IsExpediente)
                        {
                            DTO.Models.AsocEscritoDocto esc = new DTO.Models.AsocEscritoDocto();
                            esc.AsocEscritoDoctoID = 0;
                            esc.AsocCausaDocumentoID = asoc.AsocCausaDocumentoID;
                            esc.ExpedienteID = ExpedienteID;
                            esc.AsocEscritoDoctoID = appExpediente.SaveAsocEscritoDocto(esc);
                        }
                    }

                    _logC.TipoLog = TipoLog.CambiaEstadoCausa;
                    _logC.Save();
                }

                if (TipoDocumentoID == (int)Enums.TipoDocumento.CertificadoTituloAbogado)
                {
                    DTO.Models.DocumentoSistema doc = new DTO.Models.DocumentoSistema();
                    doc.DocumentoSistemaID = 0;
                    doc.VersionEncriptID = enc.VersionEncriptID;
                    doc.TipoDocumentoID = TipoDocumentoID;
                    doc.Hash = SHAFile;
                    doc.NombreArchivoFisico = fileNameTmp;
                    doc.Fecha = ahora;
                    doc.Descripcion = Enums.TipoDocumento.CertificadoTituloAbogado.ToString();

                    doc.DocumentoSistemaID = appExpediente.SaveDocumentoSistema(doc);

                    DTO.Models.AsocDocumentoUsuario asoc = new Application.DTO.Models.AsocDocumentoUsuario();
                    asoc.AsocDocumentoUsuarioID = 0;
                    asoc.DocumentoSistemaID = doc.DocumentoSistemaID;
                    asoc.UsuarioID = UsuarioActive;
                    appCommon.SaveAsocDocumentoUsuario(asoc);
                }

                #endregion
            }


            return Json(new { Error = 0 });
        }




        /// <summary>
        /// Método para eliminación de archivos adjuntos.
        /// </summary>
        /// <param name="ObjID1">Id del docuento (si existe)</param>
        /// <param name="TipoDoc">Tipo de documento</param>
        /// <param name="VersionEnc">Versión de encriptación</param>
        /// <param name="Hash">Hash</param>
        /// <param name="ObjID2">Id alternativo del documento (si existe)</param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult DeleteFile(int ObjID1, int TipoDoc, int VersionEnc, string Hash, int ObjID2)
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
                if (ObjID1 == (int)Enums.TipoDocumento.Temporal)
                {
                    int DocumentoTempID = TipoDoc;

                    DTO.Models.DocumentoTmp doc = appCommon.GetDocumentoTmp(DocumentoTempID);

                    TheHash xEnc0 = new TheHash(doc.VersionEncript.Cadena.Trim());
                    string pathFisicoTemp0 = xEnc0.DecryptData(doc.Hash.Trim());
                    string[] HashDecode0 = pathFisicoTemp0.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                    appCommon.DeleteDocumento(Enums.TipoDocumento.Temporal, DocumentoTempID);
                    dbLog.TipoLog = TipoLog.EliminarArchivoAdjuntoTemporal;

                    string EnPathFile = WebConfigValues.PathBaseRepository + HashDecode0[1];

                    if (System.IO.File.Exists(EnPathFile))
                        System.IO.File.Delete(EnPathFile);

                    try
                    {
                        string Carpeta = Path.GetDirectoryName(EnPathFile);

                        if (System.IO.File.Exists(EnPathFile))
                            System.IO.File.Delete(EnPathFile);

                        bool IsEmpty = active.IsDirectoryEmpty(Carpeta);
                        if (IsEmpty)
                            Directory.Delete(Carpeta);
                    }
                    catch (Exception f)
                    {
                        Logger.Execute().Error(f);
                    }


                    return Json(new { result = (int)ReturnJson.ActionSuccess });
                }


                DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(VersionEnc);

                TheHash xEnc = new TheHash(enc.Cadena.Trim());
                string pathFisicoTemp = xEnc.DecryptData(Hash.Trim());
                string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                DTO.Models.Causa causa = appExpediente.GetCausa(ObjID2);

                LogCausa _logC = new LogCausa();
                _logC.Fecha = ahora;
                _logC.CausaID = causa.CausaID;
                _logC.UsuarioID = UsuarioActive;
                _logC.EstadoCausa = (Enums.EstadoCausa)causa.EstadoCausaID;

                if (TipoDoc == (int)Enums.TipoDocumento.Causa)
                {
                    appCommon.DeleteDocumento(Enums.TipoDocumento.Causa, ObjID1);
                    dbLog.TipoLog = TipoLog.EliminarArchivoAdjunto;

                    _logC.TipoLog = TipoLog.EliminarArchivoAdjunto;
                    _logC.Save();
                }

                if (TipoDoc == (int)Enums.TipoDocumento.Expediente)
                {
                    appCommon.DeleteDocumento(Enums.TipoDocumento.Expediente, ObjID1);
                    dbLog.TipoLog = TipoLog.EliminarArchivoAdjunto;

                    _logC.TipoLog = TipoLog.EliminarArchivoAdjunto;
                    _logC.Save();
                }

                bool IsLocalRepository = bool.Parse(HashDecode[0]);

                if (IsLocalRepository)
                {
                    string EnPathFile = WebConfigValues.PathBaseRepository + HashDecode[1];

                    if (System.IO.File.Exists(EnPathFile))
                        System.IO.File.Delete(EnPathFile);
                }

                dbLog.ExpedienteID = ObjID2;
                dbLog.Error = pathFisicoTemp;
                dbLog.Save();

                return Json(new { result = (int)ReturnJson.ActionSuccess });

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
        /// <param name="hash"></param>
        /// <param name="vs"></param>
        /// <param name="td"></param>
        [HttpGet, AllowAnonymous]
        public void GetFile(string hash, int vs = 0, int td = 0)
        {
            bool Show = true;

            if (string.IsNullOrEmpty(hash)) Show = false;
            if (vs == 0 || td == 0) Show = false;

            if (Show)
            {
                try
                {
                    DTO.Models.VersionEncript enc = appCommon.GetVersionEncriptById(vs);
                    TheHash xEncode = new TheHash(enc.Cadena.Trim());
                    string decode = xEncode.DecryptData(hash);

                    string PathFinal = "";
                    string Filename;

                    Enums.TipoDocumento TipoDocumento = (Enums.TipoDocumento)td;

                    Filename = string.Format("Doc_{0}", TipoDocumento.ToString());

                    PathFinal = decode;

                    if (TipoDocumento == TipoDocumento.Causa || TipoDocumento == TipoDocumento.Expediente)
                    {
                        string[] HashDecode = decode.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                        Filename = "Expediente";
                        PathFinal = HashDecode[1];
                    }

                    string EndPathFile = WebConfigValues.PathBaseRepository + PathFinal;
                    string extension = Path.GetExtension(EndPathFile);
                    Filename = Filename + extension;

                    if (System.IO.File.Exists(EndPathFile))
                    {
                        Response.Buffer = true;
                        Response.Clear();
                        Response.AppendHeader("Cache-Control", "no-cache"); //private
                        Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                        Response.AddHeader("Content-Type", "application/octet-stream ");
                        Response.WriteFile(EndPathFile);
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Execute().Error(ex);
                }
            }

            string template = RenderPartialViewToString("NoExiste", "");

            Response.Buffer = true;
            Response.Clear();
            Response.Write(template);
            Response.End();
        }


        /// <summary>
        /// Método para descarga de archivos adjuntos.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public void DownloadFile()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            var frm = Request.Form;

            int DocumentoID = active.GetIntValueForm(frm["ObjID1"]);
            int CausaID = active.GetIntValueForm(frm["ObjID2"]);
            int TipoDoc = active.GetIntValueForm(frm["TipoDoc"]);
            string Hash = active.GetStringValueForm(frm["Hash"]);

            string CadenaDec = string.Empty;
            string EndFileName = string.Empty;

            TipoDocumento td = TipoDocumento.Causa;

            if (TipoDoc == (int)Enums.TipoDocumento.Temporal)
            {
                var DocumentoTmp = appCommon.GetDocumentoTmp(DocumentoID);
                CadenaDec = DocumentoTmp.VersionEncript.Cadena.Trim();
                EndFileName = Hash;
                Hash = DocumentoTmp.Hash.Trim();
            }
            else if (TipoDoc == (int)Enums.TipoDocumento.Causa || 
                TipoDoc == (int)Enums.TipoDocumento.Ingreso ||
                TipoDoc == (int)Enums.TipoDocumento.Tabla ||
                TipoDoc == (int)Enums.TipoDocumento.ExpedienteElectronicoPDF ||
                TipoDoc == (int)Enums.TipoDocumento.EstadoDiario ||
                TipoDoc == (int)Enums.TipoDocumento.Expediente ||
                TipoDoc == (int)Enums.TipoDocumento.CertificadoTituloAbogado)
            {
                int DocID = DocumentoID;

                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash((TipoDocumento)TipoDoc, Hash, CausaID, 0, DocID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }
            else
            {
                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash(td, Hash, CausaID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }

            TheHash xEncode = new TheHash(CadenaDec.Trim());
            string pathFisico = "";

            string EndPathFile = string.Empty;

            try
            {
                string pathFisicoTemp = xEncode.DecryptData(Hash);

                string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                bool IsLocalRepository = bool.Parse(HashDecode[0]);

                if (IsLocalRepository)
                {
                    pathFisico = HashDecode[1];

                    EndPathFile = WebConfigValues.PathBaseRepository + pathFisico;
                }
                else
                {
                    Logger.Execute().Info("Enable IsLocalRepository");
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }


            if (System.IO.File.Exists(EndPathFile))
            {
                Response.Buffer = true;
                Response.Clear();
                Response.AppendHeader("Cache-Control", "no-cache"); //private
                Response.AddHeader("content-disposition", "attachment; filename=" + EndFileName);
                Response.AddHeader("Content-Type", "application/octet-stream ");
                Response.WriteFile(EndPathFile);
                Response.End();
            }
            else
            {
                string template = RenderPartialViewToString("NoExiste", EndFileName);

                Response.Buffer = true;
                Response.Clear();
                Response.Write(template);
                Response.End();
            }
        }

        /// <summary>
        /// Método para descarga de archivos adjuntos.
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        public void DownloadFilePdf()
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            var frm = Request.Form;

            int DocumentoID = active.GetIntValueForm(frm["ObjID1"]);
            int CausaID = active.GetIntValueForm(frm["ObjID2"]);
            int TipoDoc = active.GetIntValueForm(frm["TipoDoc"]);
            string Hash = active.GetStringValueForm(frm["Hash"]);

            string CadenaDec = string.Empty;
            string EndFileName = string.Empty;

            TipoDocumento td = TipoDocumento.Causa;

            if (TipoDoc == (int)Enums.TipoDocumento.Temporal)
            {
                var DocumentoTmp = appCommon.GetDocumentoTmp(DocumentoID);
                CadenaDec = DocumentoTmp.VersionEncript.Cadena.Trim();
                EndFileName = Hash;
                Hash = DocumentoTmp.Hash.Trim();
            }
            else if (TipoDoc == (int)Enums.TipoDocumento.Causa ||
                TipoDoc == (int)Enums.TipoDocumento.Ingreso ||
                TipoDoc == (int)Enums.TipoDocumento.Tabla ||
                TipoDoc == (int)Enums.TipoDocumento.ExpedienteElectronicoPDF ||
                TipoDoc == (int)Enums.TipoDocumento.EstadoDiario ||
                TipoDoc == (int)Enums.TipoDocumento.Expediente ||
                TipoDoc == (int)Enums.TipoDocumento.CertificadoTituloAbogado)
            {
                int DocID = DocumentoID;

                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash((TipoDocumento)TipoDoc, Hash, CausaID, 0, DocID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }
            else
            {
                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash(td, Hash, CausaID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }

            TheHash xEncode = new TheHash(CadenaDec.Trim());
            string pathFisico = "";

            string EndPathFile = string.Empty;

            try
            {
                string pathFisicoTemp = xEncode.DecryptData(Hash);

                string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                bool IsLocalRepository = bool.Parse(HashDecode[0]);

                if (IsLocalRepository)
                {
                    pathFisico = HashDecode[1];

                    EndPathFile = WebConfigValues.PathBaseRepository + pathFisico;
                }
                else
                {
                    Logger.Execute().Info("Enable IsLocalRepository");
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }


            if (System.IO.File.Exists(EndPathFile))
            {
                Response.Buffer = true;
                Response.Clear();
                Response.AppendHeader("Cache-Control", "no-cache"); //private
                Response.AddHeader("content-disposition", "attachment; filename=" + EndFileName);
                Response.AddHeader("Content-Type", "application/pdf ");
                Response.WriteFile(EndPathFile);
                Response.End();
            }
            else
            {
                string template = RenderPartialViewToString("NoExiste", EndFileName);

                Response.Buffer = true;
                Response.Clear();
                Response.Write(template);
                Response.End();
            }
        }

        /// <summary>
        /// Método para traer url de archivos adjuntos.
        /// </summary>
        [HttpGet]
        public string TraeUrlDocumentoFirma(int DocumentoID, int CausaID, int TipoDoc, string Hash)
        {
            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);
            string CadenaDec = string.Empty;
            string EndFileName = string.Empty;

            TipoDocumento td = TipoDocumento.Causa;

            if (TipoDoc == (int)Enums.TipoDocumento.Temporal)
            {
                var DocumentoTmp = appCommon.GetDocumentoTmp(DocumentoID);
                CadenaDec = DocumentoTmp.VersionEncript.Cadena.Trim();
                EndFileName = Hash;
                Hash = DocumentoTmp.Hash.Trim();
            }
            else if (TipoDoc == (int)Enums.TipoDocumento.Causa ||
                TipoDoc == (int)Enums.TipoDocumento.Ingreso ||
                TipoDoc == (int)Enums.TipoDocumento.Tabla ||
                TipoDoc == (int)Enums.TipoDocumento.ExpedienteElectronicoPDF ||
                TipoDoc == (int)Enums.TipoDocumento.EstadoDiario ||
                TipoDoc == (int)Enums.TipoDocumento.Expediente ||
                TipoDoc == (int)Enums.TipoDocumento.CertificadoTituloAbogado)
            {
                int DocID = DocumentoID;

                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash((TipoDocumento)TipoDoc, Hash, CausaID, 0, DocID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }
            else
            {
                DTO.DownloadFile file = appExpediente.GetDownloadFileByHash(td, Hash, CausaID);
                CadenaDec = file.CadenaVersionEncript;
                EndFileName = file.NombreArchivoFisico;
            }

            TheHash xEncode = new TheHash(CadenaDec.Trim());
            string pathFisico = "";

            string EndPathFile = string.Empty;

            try
            {
                string pathFisicoTemp = xEncode.DecryptData(Hash);

                string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                bool IsLocalRepository = bool.Parse(HashDecode[0]);

                if (IsLocalRepository)
                {
                    pathFisico = HashDecode[1];

                    EndPathFile = WebConfigValues.PathBaseRepository + pathFisico;
                }
                else
                {
                    Logger.Execute().Info("Enable IsLocalRepository");
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }

            var oUrlRetorna = string.Empty;

            if (System.IO.File.Exists(EndPathFile))
            {
                byte[] bytes22 = System.IO.File.ReadAllBytes(EndPathFile);
                var rutaTemporal = Server.MapPath("~/TempPdf/" + EndFileName);
                if (System.IO.File.Exists(rutaTemporal)) { System.IO.File.Delete(rutaTemporal); }
                System.IO.File.WriteAllBytes(rutaTemporal, bytes22);

                var oUrl = HttpContext.Request.Url.AbsoluteUri.Split('E')[0];
                oUrlRetorna = string.Format("{0}{1}", oUrl, "TempPdf/" + EndFileName);
            }

            return oUrlRetorna;
        }

        /// <summary>
        /// Método para Validar si el usuario tiene tomado el documento para firmar
        /// </summary>
        [HttpGet]
        public string ValidaTomaFirma(int l_expedienteID, int l_usuarioID, int l_iTipoTramite)
        {
            string sRetorna = "0|El documento esta siendo firmado por otro usuario.";

            if (l_iTipoTramite != (int)Enums.TipoTramite.Resolucion)
            {
                sRetorna = "1|El documento está listo para firmar.";
                return sRetorna;
            }

            SsoActionResult sso = new SsoActionResult();
            sso.ExecuteResult(ControllerContext);

            int UsuarioActive = sso.GetUsuarioActivoID();
            if (l_usuarioID != UsuarioActive) { return sRetorna; }

            //Entity Firmante
            var entityActualFirmanteTemp = appExpediente.GetAsocFirmaDoctoGS(l_expedienteID, l_usuarioID);
            if (entityActualFirmanteTemp == null) { return sRetorna; }


            //Validar si esta tomado hasta  5 min mas de la fecha marcada
            var oListFirmantes = appExpediente.GetListAsocFirmaDoctoGS(l_expedienteID);
            if (oListFirmantes.Count > 0)
            {
                #region [Validacion para cuarto firmante]

                if (entityActualFirmanteTemp.Firma.Orden == 4)
                {
                    var iCantidadNoFirmas = oListFirmantes.Where(q => q.IsFirmado != true).Count();
                    var iCantidadFirmas = oListFirmantes.Where(q => q.IsFirmado == true).Count();
                    if (iCantidadFirmas == 3)
                    {
                        sRetorna = "1|El documento está listo para firmar.";
                        return sRetorna;
                    }
                    else
                    {

                        sRetorna = string.Format("0|Está pendiente la firma de {0} usuario(s).", iCantidadNoFirmas - 1);
                        return sRetorna;
                    }
                }

                #endregion


                #region [Validacion para usuarios que no sea el ultimo firmante]
                //Traer el registros tomado con la fecha mas reciente
                var oListTomadoMasReciente = oListFirmantes.Where(x => x.IsTomado == true && x.FechaTomado != null).ToList(); //.OderByDescending(q => q.fecha).FirstOrDefault();
                if (oListTomadoMasReciente.Count > 0)
                {
                    var entityTomado = oListTomadoMasReciente.OrderByDescending(q => q.FechaTomado).FirstOrDefault();
                    if (entityTomado != null)
                    {
                        var dtNow = DateTime.Now;
                        DateTime dtFechaIngreso = Convert.ToDateTime(entityTomado.FechaTomado);
                        DateTime dtingresoMas = dtFechaIngreso.AddMinutes(5);

                        int result = DateTime.Compare(dtNow, dtingresoMas);
                        if (result < 0)
                        {
                            var entityUsuario = entityTomado.Firma != null && entityTomado.Firma.Usuario != null ? entityTomado.Firma.Usuario : null;

                            var idUserrrrr = entityUsuario != null ? entityUsuario.UsuarioID : 0;

                            //Aun estaActivo
                            sRetorna = idUserrrrr == l_usuarioID ? "1|El documento está listo para firmar." : string.Format("0|El documento esta siendo firmado por {0}.", entityUsuario != null ? entityUsuario.Nombres + " " + entityUsuario.Apellidos : "usuario no encontrado");
                            return sRetorna;
                        }
                        else
                        {
                            #region [Actualiza Registros anteriores]
                            foreach (var item in oListTomadoMasReciente)
                            {
                                if (item.AsocFirmaDoctoID != entityActualFirmanteTemp.AsocFirmaDoctoID)
                                {

                                    var entityActiva = new DTO.Models.AsocFirmaDocto
                                    {
                                        AsocFirmaDoctoID = item.AsocFirmaDoctoID,
                                        FirmaID = item.FirmaID,
                                        AsocEscritoDoctoID = item.AsocEscritoDoctoID,
                                        IsFirmado = item.IsFirmado,
                                        IsTomado = false,
                                        FechaTomado = null
                                    };
                                    appExpediente.SaveAsocFirmaDocto(entityActiva);
                                }
                            }
                            #endregion


                            #region[Registrar nuevo registro de toma]
                            //iRetorna = 1;
                            sRetorna = "1|El documento está listo para firmar.";

                            if (entityActualFirmanteTemp != null)
                            {
                                var entityActiva = new DTO.Models.AsocFirmaDocto
                                {
                                    AsocFirmaDoctoID = entityActualFirmanteTemp.AsocFirmaDoctoID,
                                    FirmaID = entityActualFirmanteTemp.FirmaID,
                                    AsocEscritoDoctoID = entityActualFirmanteTemp.AsocEscritoDoctoID,
                                    IsFirmado = entityActualFirmanteTemp.IsFirmado,
                                    IsTomado = true,
                                    FechaTomado = DateTime.Now
                                };
                                appExpediente.SaveAsocFirmaDocto(entityActiva);
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    #region[Registrar nuevo registro de toma]
                    //iRetorna = 1;
                    sRetorna = "1|El documento está listo para firmar.";

                    if (entityActualFirmanteTemp != null)
                    {
                        var entityActiva = new DTO.Models.AsocFirmaDocto
                        {
                            AsocFirmaDoctoID = entityActualFirmanteTemp.AsocFirmaDoctoID,
                            FirmaID = entityActualFirmanteTemp.FirmaID,
                            AsocEscritoDoctoID = entityActualFirmanteTemp.AsocEscritoDoctoID,
                            IsFirmado = entityActualFirmanteTemp.IsFirmado,
                            IsTomado = true,
                            FechaTomado = DateTime.Now
                        };
                        appExpediente.SaveAsocFirmaDocto(entityActiva);
                    }
                    #endregion
                }
                #endregion
            }

            return sRetorna;

        }

        /// <summary>
        /// Devuelve una vista parcial cuando no se puede descargar el archivo.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        protected string RenderPartialViewToString(string viewName, string filename)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewBag.Archivo = filename;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        private string CleanFilename(string fileName)
        {
            string replaceChar = "_";
            string retorno = fileName.Replace(" ", replaceChar);

            return retorno.Replace(TipoUploadChar, replaceChar).ReplaceNameFileChars();
        }


        #region Get Mime Type from File

        private static string GetMimeFromFile(string file)
        {
            IntPtr mimeout;
            if (!System.IO.File.Exists(file))
                return "";
            //throw new FileNotFoundException(file + " not found");

            int MaxContent = (int)new FileInfo(file).Length;
            if (MaxContent > 4096) MaxContent = 4096;
            FileStream fs = new FileStream(file, FileMode.Open); //File.OpenRead(file);

            byte[] buf = new byte[MaxContent];
            fs.Read(buf, 0, MaxContent);
            fs.Close();
            int result = FindMimeFromData(IntPtr.Zero, file, buf, MaxContent, null, 0, out mimeout, 0);

            if (result != 0)
                throw Marshal.GetExceptionForHR(result);
            string mime = Marshal.PtrToStringUni(mimeout);
            Marshal.FreeCoTaskMem(mimeout);
            return mime;
        }

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
              [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
             [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)]
            byte[] pBuffer,
              int cbSize,
                 [MarshalAs(UnmanagedType.LPWStr)]  string pwzMimeProposed,
              int dwMimeFlags,
              out IntPtr ppwzMimeOut,
              int dwReserved);

        #endregion

    }
}