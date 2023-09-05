using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class TipoEstadoDiario
    {
        public TipoEstadoDiario()
        {
            this.EstadoDiario = new HashSet<EstadoDiario>();
        }

        public int TipoEstadoDiarioID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }

        public virtual ICollection<EstadoDiario> EstadoDiario { get; set; }
    }
}
