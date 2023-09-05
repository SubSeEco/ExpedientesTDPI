using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Mantenedor
{
    public class ConfigurarDescripcion
    {
        public int ConfigurarDescripcionID { get; set; }
        public string NombreTabla { get; set; }
        public bool Vigente { get; set; }
    }

    public class ObjetoGenerico
    {
        public string TableName { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
    }
}
