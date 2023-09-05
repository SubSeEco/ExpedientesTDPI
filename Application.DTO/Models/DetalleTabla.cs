using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class DetalleTabla
    {
        public int DetalleTablaID { get; set; }
        public int CausaID { get; set; }
        public int TablaID { get; set; }
        public int Orden { get; set; }
        public bool Vigente { get; set; }

        public virtual Causa Causa { get; set; }
        public virtual Tabla Tabla { get; set; }
    }
}
