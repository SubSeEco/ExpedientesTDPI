using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    using System;
    using System.Collections.Generic;

    public partial class AsocDocumentoUsuario
    {
        public int AsocDocumentoUsuarioID { get; set; }
        public int UsuarioID { get; set; }
        public int DocumentoSistemaID { get; set; }

        public virtual DocumentoSistema DocumentoSistema { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
