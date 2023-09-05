using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class EstadoTabla
    {
        public EstadoTabla()
        {
            this.Tabla = new HashSet<Tabla>();
        }

        public int EstadoTablaID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }

        public virtual ICollection<Tabla> Tabla { get; set; }
    }
}
