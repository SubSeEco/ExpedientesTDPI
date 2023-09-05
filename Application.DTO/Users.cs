
using System.Collections.Generic;
namespace Application.DTO
{
    class Users
    {
    }
    
    public class UserAD
    {
        public string AdID { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }

    }

    public class UserAndRoles
    {
        public UserAndRoles()
        {
           // this.Roles = new List<Models.IA_ROLES_USUARIO>();
        }

        public int UsuarioID { get; set; }
        public string AdID { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }
        //public int RolID { get; set; }
        //
        
        public string RolesDescripcion { get; set; }
        public int NumRoles { get; set; }
    }
}
