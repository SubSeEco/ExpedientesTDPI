using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class AsocTipoCausaPerfil
    {
        public int AsocTipoCausaPerfilID { get; set; }
        public int PerfilID { get; set; }
        public int TipoCausaID { get; set; }

        public virtual TipoCausa TipoCausa { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
