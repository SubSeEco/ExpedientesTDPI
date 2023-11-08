using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    using System;

    public partial class SP_Alarmas_Result
    {
        public int CausaID { get; set; }
        public int ExpedienteID { get; set; }
        public string NumeroTicket { get; set; }
        public string Fecha { get; set; }
        public string FechaVencimiento { get; set; }
        public int UsuarioResponsableID { get; set; }
        public string MailResponsable { get; set; }
        public string NombreResponsable { get; set; }
    }
}
