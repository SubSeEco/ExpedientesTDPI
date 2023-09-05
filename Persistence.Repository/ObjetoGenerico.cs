using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ObjetoGenerico
    {
        public string TableName { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
    }

    public class CompletitudRegistroConsultor
    {
        public string Descripcion { get; set; }
        public string DesCampo { get; set; }
    }
}
