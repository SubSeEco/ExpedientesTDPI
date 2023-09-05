using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class DetalleEstadoDiario
    {
        public int DetalleEstadoDiarioID { get; set; }
        public int ExpedienteID { get; set; }
        public int EstadoDiarioID { get; set; }
        public bool Vigente { get; set; }

        public virtual EstadoDiario EstadoDiario { get; set; }
        public virtual Expediente Expediente { get; set; }

    }
}
