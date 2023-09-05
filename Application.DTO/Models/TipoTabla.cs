using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class TipoTabla
    {
        public TipoTabla()
        {
            this.Tabla = new HashSet<Tabla>();
        }

        public int TipoTablaID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }

        public virtual ICollection<Tabla> Tabla { get; set; }
    }
}

