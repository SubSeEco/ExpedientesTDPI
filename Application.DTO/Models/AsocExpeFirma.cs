using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class AsocExpeFirma
    {
        public int AsocExpeFirmaID { get; set; }
        public int FirmaID { get; set; }
        public int ExpedienteID { get; set; }

        public virtual Expediente Expediente { get; set; }
        public virtual Firma Firma { get; set; }
    }
}
