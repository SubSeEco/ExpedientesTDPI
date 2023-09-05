using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.AD
{
    public class UserAD
    {
        public UserAD()
        {
            this.Grupos = new List<string>();
        }


        public string ActiveDirectoryID { get; set; }
        public string Nombre { get; set; }
        public string Mail { get; set; }

        public List<string> Grupos { get; set; }
        public int RegionID { get; set; }

    }
}
