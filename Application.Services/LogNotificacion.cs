using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Persistence.Repository;
using Enums = Domain.Infrastructure;

namespace Application.Services
{
    public class LogNotificacion
    {
        public Enums.TipoNotificacion TipoNotificacion { get; set; }
        public string EmailDestinatario { get; set; }
        public string EmailContenido { get; set; }
        public List<string> MailConCopia { get; set; }
        public List<string> Adjuntos { get; set; }

        public LogNotificacion(Enums.TipoNotificacion _TipoNotificacion)
        {
            this.MailConCopia = new List<string>();
            this.Adjuntos = new List<string>();
            this.TipoNotificacion = _TipoNotificacion;
        }

        public void SaveLog()
        {
            DTO.Models.LogSistema log = new DTO.Models.LogSistema();
            log.LogSistemaID = 0;
            log.Fecha = DateTime.Now;
            log.Pagina = "Envío Notificación";
            log.Accion = TipoNotificacion.ToString();            
            log.UsuarioID = 0;
            log.Parametros = GetDestinatarios();
            log.IpUsuario = string.Empty;
            log.Tipo = TipoNotificacion.ToString();
            log.Descripcion = EmailContenido;

            ICommonAppServices app = new CommonAppServices();
            app.SaveLogSistema(log);
        }

        private string GetDestinatarios()
        {
            string copia = String.Join(", ", MailConCopia.ToArray());
            string dest = "To: " + EmailDestinatario + " | CC: " + copia;

            if (Adjuntos.Count > 0)
            {
                dest += " | Adjuntos: " + String.Join(", ", Adjuntos.ToArray());
            }

            return dest;
        }

        public void UpdateTabla()
        {
            string MailCopia = String.Join(", ", MailConCopia.ToArray());

            if (string.IsNullOrEmpty(EmailDestinatario))
                EmailDestinatario = "";

            if (string.IsNullOrEmpty(MailCopia))
                MailCopia = "";

            //ISolicitudRepository repo = new SolicitudRepository();
            //repo.UpdateDatosNotificacion(
            //    TipoNotificacion,
            //    SolicitudID,
            //    EmailDestinatario,
            //    MailCopia);
        }
    }
}
