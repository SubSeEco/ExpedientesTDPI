using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    class Grids
    {
    }

    public class ListaTipoFormato
    {
        public int TipoFormatoID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
        public string ExtraCss { get; set; }
        public string Acciones { get; set; }

    }

    public class ListaMimeTypes
    {
        public int FamiliasMimeTypeID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
        public string Acciones { get; set; }
    }
}
