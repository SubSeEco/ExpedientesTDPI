using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class TipoGenero
    {
        public TipoGenero()
        {
            this.Usuario = new HashSet<Usuario>();
        }

        public int TipoGeneroID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
