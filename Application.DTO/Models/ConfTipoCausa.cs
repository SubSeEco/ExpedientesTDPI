using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class ConfTipoCausa
    {
        public int ConfTipoCausaID { get; set; }
        public int TipoCausaID { get; set; }
        public bool IsAnio { get; set; }
        public bool IsObservacion { get; set; }
        public bool IsNumeroRegistro { get; set; }
        public bool IsContencioso { get; set; }
        public int TipoParteID1 { get; set; }
        public int TipoParteID2 { get; set; }
        public bool IsNumeroSolicitud { get; set; }
        public bool IsConsignacion { get; set; }

        public virtual TipoCausa TipoCausa { get; set; }
    }
}