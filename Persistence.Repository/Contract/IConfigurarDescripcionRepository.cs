using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface IConfigurarDescripcionRepository
    {
        IList<ConfigurarDescripcion> GetAll();
        List<ObjetoGenerico> GetDetalleTablaGenerica(string tableName, int primaryKey, string like);

        void UpdateDetalleTabla(ObjetoGenerico obj);
        void CreateDetalleTabla(ObjetoGenerico obj);
    }
}
