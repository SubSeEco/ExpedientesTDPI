using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class AsocDocSistemaFirma
    {
        public int AsocDocSistemaFirmaID { get; set; }
        public int DocumentoSistemaID { get; set; }
        public int FirmaID { get; set; }
        public bool IsFirmado { get; set; }

        public virtual Firma Firma { get; set; }
        public virtual DocumentoSistema DocumentoSistema { get; set; }
    }
}
