using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;
using System.Globalization;

namespace Application.Services
{
    public class TemplateMail
    {
        public DTO.Models.Causa ModelCausa { get; set; }
        public DTO.Models.TipoNotificacion ModelTipoNotificacion { get; set; }
        public List<string> MailConCopia { get; set; }
        public string EmailInstitucional { get; set; }

        public string NombreCiudadano { get; set; }
        public string RutCiudadano { get; set; }
        public string NumeroTicket { get; set; }
        public string DetalleSolicitud { get; set; }
        public string CodigoVerificacion { get; set; }
        public string NombreResponsable { get; set; }
        public string CargoResponsable { get; set; }
        public string EmailResponsable { get; set; }
        public string Anio { get; set; }
        public string DiasAtraso { get; set; }
        public string ComentariosDerivacion { get; set; }
        public string UsuarioConectado { get; set; }
        public string TipoTramite { get; set; }
        public string OpcionTramite { get; set; }

        public string FechaHoraActual { get; set; }
        public string SenorSra { get; set; }
        public string FechaIngreso { get; set; }
        public string DiaHoy { get; set; }
        public string MesHoy { get; set; }
        public string ComentarioRechazo { get; set; }
        public string SistemaFuncionarioURL { get; set; }
        public string Fecha { get; set; }
        public string FechaVencimiento { get; set; }



        public void SetCausa(DTO.Models.Causa Causa)
        {
            ModelCausa = Causa;
            NumeroTicket = Causa.NumeroTicket;
           
            SistemaFuncionarioURL = WebConfigValues.Url_MenuSistemas;

        }

        public void SetExpediente(DTO.Models.Expediente Expediente)
        {
            FechaIngreso = Expediente.FechaExpediente.ToString();
            DetalleSolicitud = Expediente.Observacion.Trim();
            TipoTramite = Expediente.TipoTramite.Descripcion.Trim();
            OpcionTramite = Expediente.AsocExpedienteOpcion.FirstOrDefault().OpcionesTramite.Descripcion.Trim();            
        }

        private void SetFechaHoraActual()
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            DateTime fecha = DateTime.Now;
            string mes = dtinfo.GetMonthName(fecha.Month);

            //13-08-2019 10:00 horas
            //FechaHoraActual = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            //13 de agosto de 2019 10:00 horas
            this.FechaHoraActual = string.Format("{0} de {1} de {2} {3:HH:mm}", fecha.Day, mes, fecha.Year, fecha);
        }

        public string Render(string Mensaje)
        {

            StringBuilder sb = new StringBuilder(Mensaje);

            PropertyInfo[] myPropertyInfo = typeof(TemplateMail).GetProperties(); //BindingFlags.Public | BindingFlags.Static

            List<string> notParse = new List<string>();
            notParse.Add("ModelCausa");
            notParse.Add("ModelTipoNotificacion");
            notParse.Add("EmailInstitucional");
            notParse.Add("MailConCopia");

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                bool existe = notParse.Any(x => x == myPropertyInfo[i].Name);

                if (!existe)
                {
                    string busca = "{" + myPropertyInfo[i].Name + "}";
                    string valor = (string)GetType().GetProperty(myPropertyInfo[i].Name).GetValue(this, null);

                    if (valor == null)
                        valor = string.Empty;

                    sb.Replace(busca, valor);
                }
            }

            return sb.ToString();
        }


    }
}

