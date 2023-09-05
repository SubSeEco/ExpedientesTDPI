using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IConfDescApplicationService
    {
        IList<DTO.Mantenedor.ConfigurarDescripcion> GetAll();

        IList<DTO.Mantenedor.ObjetoGenerico> GetDetalleTabla(string tableName, int primaryKey, string like);

        void UpdateDetalleTabla(DTO.Mantenedor.ObjetoGenerico obj);
        void CreateDetalleTabla(DTO.Mantenedor.ObjetoGenerico obj);
    }
}
