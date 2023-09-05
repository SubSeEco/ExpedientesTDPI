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
        public string NumeroTicket { get; set; }
        public string DetalleSolicitud { get; set; }
        public string CodigoVerificacion { get; set; }
        public string NombreResponsable { get; set; }
        public string CargoResponsable { get; set; }
        public string EmailResponsable { get; set; }
        public string PlazoRespuesta { get; set; }
        public string Unidad { get; set; }
        public string DerivacionInstitucionNombre { get; set; }
        public string DerivacionInstitucionEmail { get; set; }
        public string Anio { get; set; }
        public string RespuestaSolicitud { get; set; }
        public string DiasAtraso { get; set; }
        public string MontoCobro { get; set; }
        public string ComentarioContactoCiudadano { get; set; }
        public string ComentarioCobro { get; set; }
        public string ComentariosProrroga { get; set; }
        public string ComentariosDerivacion { get; set; }
        public string ComentariosAdmisibilidad { get; set; }
        public string UsuarioConectado { get; set; }
        public string MotivoDesestimiento { get; set; }
        public string DescripcionDesestimiento { get; set; }

        public string DiasRecordatorioOIRS { get; set; }
        public string DiasRecordatorioTransparencia { get; set; }
        public string FechaHoraActual { get; set; }
        public string SenorSra { get; set; }
        public string FechaIngreso { get; set; }
        public string DiaHoy { get; set; }
        public string MesHoy { get; set; }
        public string TipoSolicitud { get; set; }
        public string Categoria { get; set; }
        public string SubCategoria { get; set; }
        public string RutDenunciado { get; set; }
        public string PersonaInstitucionRespuesta { get; set; }
        public string EncuestaURL { get; set; }
        public string DiasCompromiso { get; set; }
        public string DetalleCompromiso { get; set; }
        public string FechaVenCompromiso { get; set; }
        public string ContactoCiudadanoURL { get; set; }
        public string ComentarioRechazo { get; set; }
        public string InsatisfaccionAmparo { get; set; }
        public string ComentarioInsatisfaccionAmparo { get; set; }
        public string VerEstadoSolicitudURL { get; set; }
        public string BuzonCiudadanoURL { get; set; }
        public string SistemaFuncionarioURL { get; set; }


        public void SetCausa(DTO.Models.Causa Causa)
        {
            ModelCausa = Causa;
            NumeroTicket = Causa.NumeroTicket;
           
            //BuzonCiudadanoURL = WebConfigValues.BuzonCiudadanoURL;
            SistemaFuncionarioURL = WebConfigValues.Url_MenuSistemas;

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

