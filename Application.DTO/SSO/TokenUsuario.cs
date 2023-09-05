using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.SSO
{
    public class TokenUsuario
    {
        public string usuarioID { get; set; }
        public int SistemaID { get; set; }
        public int TokenUsuarioID { get; set; }
        public int VersionEncripID { get; set; }
        public System.DateTime Caducidad { get; set; }
        public string Token { get; set; }
        public string IP { get; set; }
        public string HashVerificacion { get; set; }

    }
}
