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
            _mail.Asunto = $"{tm.ModelTipoNotificacion.Asunto} - Rol: {tm.NumeroTicket}";
            _mail.Body = bodyMail;

            #region Envio Externo
            _mail.Destinatario = user.Mail.Trim();

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.ListadoIngresoDiario, _mail, true);
            #endregion

        }

        #region Expediente
        public void NotificacionDerivacion(int CausaID, int UsuarioID, int UsuarioActive, string ComentariosDerivacion)
        {
            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.Derivacion, CausaID);
            List<string> concopia = new List<string>();

            #region Usuarios
            IList<DTO.Models.Usuario> usuarios = appCommon.GetUsuarios();

            DTO.Models.Usuario UsuarioResponsable = usuarios.FirstOrDefault(x => x.UsuarioID == UsuarioID);
            tm.NombreResponsable = UsuarioResponsable.GetFullName();

            DTO.Models.Usuario UsuarioConectado = usuarios.FirstOrDefault(x => x.UsuarioID == UsuarioActive);
            tm.UsuarioConectado = UsuarioConectado.GetFullName();
            #endregion

            tm.ComentariosDerivacion = ComentariosDerivacion;

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);
            concopia.Add(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;
            _mail.Body = bodyMail;

            #region Envio
            _mail.Destinatario = UsuarioResponsable.Mail.Trim();

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.Derivacion, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.Derivacion, _mail, true);
            #endregion
        }

        public void NotificacionAdmisibilidad(DTO.Models.Expediente expediente, int UsuarioID, string Comentarios)
        {
            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.Admisibilidad, expediente.CausaID);
            List<string> concopia = new List<string>();

            #region Usuario
            IList<DTO.Models.Usuario> usuarios = appCommon.GetUsuarios();

            DTO.Models.Usuario UsuarioResponsable = usuarios.FirstOrDefault(x => x.UsuarioID == UsuarioID);
            tm.NombreResponsable = UsuarioResponsable.GetFullName();

            #endregion

            tm.ComentariosDerivacion = Comentarios;

            tm.SetExpediente(expediente);

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);
            concopia.Add(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.Asunto = $"{tm.ModelTipoNotificacion.Asunto} - Rol: {tm.NumeroTicket}";
            _mail.Body = bodyMail;
            _mail.Destinatario = UsuarioResponsable.Mail.Trim();

            #region Envio
            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.Derivacion, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.Derivacion, _mail, true);
            #endregion

        }

        public void IngresoExpediente(int CausaID, int UsuarioID, string strTipoTramite)
        {
            DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioID);

            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.IngresoExpediente, CausaID);
            List<string> concopia = new List<string>();

            tm.NombreCiudadano = user.GetFullName();
            tm.TipoTramite = strTipoTramite.Trim();

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);
            concopia.Add(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.Asunto = $"{tm.ModelTipoNotificacion.Asunto} - Rol: {tm.NumeroTicket}";
            _mail.Body = bodyMail;

            #region Envio
            _mail.Destinatario = user.Mail.Trim();

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.IngresoExpediente, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.IngresoExpediente, _mail, true);
            #endregion
        }
        #endregion
        
        #region Abogado

        public void RegistroAbogado(int UsuarioID)
        {
            DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioID);

            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.RegistroAbogado, 0);

            tm.NombreCiudadano = user.GetFullName();

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;
            _mail.Body = bodyMail;

            #region Envio
            _mail.Destinatario = user.Mail.Trim();

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.IngresoExpediente, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.IngresoExpediente, _mail, true);
            #endregion

        }
        public void SolicitudRegistroAbogado(int UsuarioID, string Hash)
        {
            DTO.Models.Usuario user = appCommon.GetUsuarioByID(UsuarioID);

            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.SolicitudRegistroAbogado, 0);

            tm.NombreCiudadano = user.GetFullName();
            tm.RutCiudadano = user.GetRUT();

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            tm.MailConCopia.Remove(tm.EmailInstitucional);

            MyMail _mail = new MyMail();
            _mail.Asunto = $"{tm.ModelTipoNotificacion.Asunto} - Rut: {tm.RutCiudadano}";
            _mail.Body = bodyMail;
            _mail.Destinatario = String.Join(",", tm.MailConCopia);
            _mail.Attachments = GetAdjuntos(TipoDocumento.CertificadoTituloAbogado, Hash.Trim());

            #region Envio

            if (IsDesarrollo) SaveLogNotifica(TipoNotificacion.SolicitudRegistroAbogado, _mail, true);

            _mail.Enviar();

            if (!IsDesarrollo) SaveLogNotifica(TipoNotificacion.SolicitudRegistroAbogado, _mail, true);
            #endregion


        }

        #endregion

        #region Alarmas
        public void SendAlarmaInterna(DTO.Models.SP_Alarmas_Result alarma)
        {            
            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.RecordatorioAtraso, alarma.CausaID);
            tm.NumeroTicket = alarma.NumeroTicket.Trim();

            tm.NombreResponsable = alarma.NombreResponsable.Trim();
            tm.Fecha = alarma.Fecha;
            tm.FechaVencimiento = alarma.FechaVencimiento;

            DTO.Models.Expediente expediente = appExpediente.GetExpediente(alarma.ExpedienteID);

            tm.SetExpediente(expediente);

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            MyMail _mail = new MyMail();
            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;
            _mail.Asunto += " - " + tm.NumeroTicket;
            _mail.Destinatario = alarma.MailResponsable.Trim();
            _mail.Body = bodyMail;
            _mail.Enviar();

            SaveLogNotifica(TipoNotificacion.RecordatorioAtraso, _mail, true);
        }

        public void SendAlarmaSinAsignar(List<string> Roles)
        {
            MyMail _mail = new MyMail();

            TemplateMail tm = GetTemplate(Enums.TipoNotificacion.ExpedientesSinAsignar, 0);

            string bodyMail = tm.Render(tm.ModelTipoNotificacion.Mensaje);

            #region Expedientes Sin Asignar
            StringBuilder sb = new StringBuilder(bodyMail);

            string busca = "{ExpedientesNoAsignados}";
            string valor = string.Join("<br/>", Roles.ToArray());

            if (valor == null)
                valor = string.Empty;

            sb.Replace(busca, valor);
            #endregion

            bodyMail = sb.ToString();

            _mail.Asunto = tm.ModelTipoNotificacion.Asunto;
            _mail.Destinatario = String.Join(",", tm.MailConCopia);
            _mail.Body = bodyMail;
            //_mail.ForzarEnvioOriginal = true;
            
            _mail.Enviar();

            SaveLogNotifica(TipoNotificacion.ExpedientesSinAsignar, _mail, true);
        }
        #endregion

        #region Helpers
        private TemplateMail GetTemplate(Enums.TipoNotificacion TipoNotificacion, int CausaID = 0)
        {
            DTO.Models.TipoNotificacion notificacion = appCommon.GetTipoNotificacionByID((int)TipoNotificacion);

            TemplateMail tm = new TemplateMail();

            IList<DTO.Models.Usuario> userList = appCommon.GetUsuarios();
            tm.ModelTipoNotificacion = notificacion;
            tm.MailConCopia = new List<string>();

            if (TipoNotificacion == Enums.TipoNotificacion.IngresoNuevaCausa 
                || TipoNotificacion == Enums.TipoNotificacion.Derivacion
                || TipoNotificacion == Enums.TipoNotificacion.IngresoExpediente
                || TipoNotificacion == Enums.TipoNotificacion.Admisibilidad)
            {
                DTO.Models.Causa causa = appExpediente.GetCausa(CausaID);
                tm.SetCausa(causa);
            }
            
            foreach (var item in notificacion.AsocTipoNotificacionPerfil)
            {
                IList<DTO.Models.Usuario> usuarios = userList.Where(x => x.AsocUsuarioPerfil.Any(z => z.PerfilID == item.PerfilID)).ToList();

                foreach (var user in usuarios)
                {
                    if (!tm.MailConCopia.Contains(user.Mail.Trim()))
                    {
                        tm.MailConCopia.Add(user.Mail.Trim());
                    }
                    
                }
            }

            tm.EmailInstitucional = (!string.IsNullOrEmpty(notificacion.ConCopia.Trim())) ? notificacion.ConCopia.Trim() : string.Empty;
            
            return tm;
        }

        private List<string> GetAdjuntos(Enums.TipoDocumento tipoDocumento, string Hash = "")
        {
            List<string> adjuntos = new List<string>();

            #region Ingreso
            if (tipoDocumento == TipoDocumento.Ingreso || 
                tipoDocumento == TipoDocumento.CertificadoTituloAbogado)
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
