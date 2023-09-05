using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Domain.Infrastructure;
using Infrastructure.Logging;
using Infrastructure.Utils;
using Application.DTO.Utils;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using System.Threading;

namespace Application.Services
{
    public class MailAppServices : IMailAppServices
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();
        private readonly IExpedienteAppServices appExpediente = new ExpedienteAppServices();

        private readonly bool IsDesarrollo = WebConfigValues.isEmailDesarrollo;
        private static string TipoUploadChar = "{#}";
        
        //Notificacion ListadoIngresoDiario
        public List<string> ListadoIngresoDiario(string Hash)
        {
            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.ListadoIngresoDiario);
            List<string> concopia = new List<string>();

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);
            concopia.Add(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.ConCopia = concopia;
            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;
            _mail.Destinatario = String.Join(",", tm.MailConCopia);
            _mail.Body = bodyMail;
            _mail.Attachments = GetAdjuntos(TipoDocumento.Ingreso, Hash.Trim());

            #region Enviar
            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);
            #endregion

            tm.MailConCopia.Add(tm.EmailInstitucional);
            return tm.MailConCopia;
        }

        public void IngresoNuevaCausa(int CausaID, int UsuarioID) {

            DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioID);

            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.IngresoNuevaCausa, CausaID);
            List<string> concopia = new List<string>();

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);
            concopia.Add(tm.EmailInstitucional);

            MyMail _mail = new MyMail();            
            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;            
            _mail.Body = bodyMail;

            #region Envio Externo
            _mail.Destinatario = user.Mail.Trim();

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);
            #endregion

        }

        #region Helpers
        private TemplateMail GetTemplate(Enums.TipoNotificacion TipoNotificacion, int CausaID = 0)
        {
            DTO.Models.TipoNotificacion notificacion = appCommon.GetTipoNotificacionByID((int)TipoNotificacion);

            TemplateMail tm = new TemplateMail();

            IList<DTO.Models.Usuario> userList = appCommon.GetUsuarios();
            tm.ModelTipoNotificacion = notificacion;
            tm.MailConCopia = new List<string>();

            if (TipoNotificacion == Enums.TipoNotificacion.IngresoNuevaCausa)
            {
                DTO.Models.Causa causa = appExpediente.GetCausa(CausaID);
                tm.SetCausa(causa);
            }

            foreach (var item in notificacion.AsocTipoNotificacionPerfil)
            {
                IList<DTO.Models.Usuario> usuarios = userList.Where(x => x.AsocUsuarioPerfil.Any(z => z.PerfilID == item.PerfilID)).ToList();

                foreach (var user in usuarios)
                {
                    tm.MailConCopia.Add(user.Mail);
                }
            }

            tm.EmailInstitucional = (notificacion.ConCopia.Trim() != "") ? notificacion.ConCopia.Trim() : string.Empty;

            return tm;
        }

        private List<string> GetAdjuntos(Enums.TipoDocumento tipoDocumento, string Hash = "")
        {
            List<string> adjuntos = new List<string>();

            #region Ingreso
            if (tipoDocumento == TipoDocumento.Ingreso)
            {
                DTO.DownloadFile archivo = appExpediente.GetDownloadFileByHash(tipoDocumento, Hash.Trim(), CausaID: 0);

                if (archivo.NombreArchivoFisico != "")
                {
                    archivo.HashFile = Hash.Trim();
                    DTO.Utils.TheHash xEncode = new DTO.Utils.TheHash(archivo.CadenaVersionEncript.Trim());

                    string EndPathFile = string.Empty;

                    string pathFisicoTemp = xEncode.DecryptData(archivo.HashFile.Trim());

                    string[] HashDecode = pathFisicoTemp.Split(new string[] { TipoUploadChar }, StringSplitOptions.None);

                    bool IsLocalRepository = bool.Parse(HashDecode[0]);

                    if (IsLocalRepository)
                    {
                        string pathFisico = HashDecode[1];

                        EndPathFile = WebConfigValues.PathBaseRepository + pathFisico;
                    }

                    adjuntos.Add(EndPathFile);
                }
            }
            #endregion

            return adjuntos;
        }

        private void SaveLogNotifica(Enums.TipoNotificacion tipo, MyMail _mail, bool UpdateTable = false)
        {
            try
            {
                LogNotificacion log = new LogNotificacion(tipo);
                log.EmailDestinatario = _mail.Destinatario;
                log.EmailContenido = _mail.Body;
                log.MailConCopia = _mail.ConCopia;
                log.Adjuntos = _mail.Attachments;
                log.SaveLog();

                if (UpdateTable)
                {
                    log.UpdateTabla();
                }

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }
        }
        #endregion

    }
}
