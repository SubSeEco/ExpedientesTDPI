using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class AsocDocumentoSistemaEstadoDiario
    {
        public int AsocDocumentoSistemaEstadoDiarioID { get; set; }
        public int DocumentoSistemaID { get; set; }
        public int EstadoDiarioID { get; set; }

        public virtual EstadoDiario EstadoDiario { get; set; }
        public virtual DocumentoSistema DocumentoSistema { get; set; }
    }
}
