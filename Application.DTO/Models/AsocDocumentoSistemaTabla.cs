using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class AsocDocumentoSistemaTabla
    {
        public int AsocDocumentoSistemaTablaID { get; set; }
        public int TablaID { get; set; }
        public int DocumentoSistemaID { get; set; }

        public virtual DocumentoSistema DocumentoSistema { get; set; }
        public virtual Tabla Tabla { get; set; }
    }
}
