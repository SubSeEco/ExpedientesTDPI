using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ConfigurarDescripcionRepository : IConfigurarDescripcionRepository
    {
        private readonly SGDE2Entities _context = null;
                
        public ConfigurarDescripcionRepository()
        {
            _context = new SGDE2Entities();
        }

        public IList<ConfigurarDescripcion> GetAll()
        {
            return _context.ConfigurarDescripcion.Where(x => x.Vigente).OrderBy(x => x.NombreTabla).ToList();
        }

        public List<ObjetoGenerico> GetDetalleTablaGenerica(string tableName, int primaryKey, string like)
        {
            string query = string.Format("exec SP_ObtieneRegistroMantenedorGenerico '{0}', {1}, '{2}'", tableName, primaryKey, like);
            var result = _context.Database.SqlQuery<ObjetoGenerico>(query);

            return result.ToList();
        }

        public void UpdateDetalleTabla(ObjetoGenerico obj)
        {
            string query = string.Format("exec SP_ActualizaRegistroMantenedorGenerico '{0}', {1}, {2}, '{3}'",
                obj.TableName, obj.Codigo, (obj.Vigente) ? 1 : 0, obj.Descripcion);

            _context.Database.ExecuteSqlCommand(query);
        }

        public void CreateDetalleTabla(ObjetoGenerico obj)
        {
            string query = string.Format("exec SP_AgregaRegistroMantenedorGenerico '{0}', '{1}', {2}",
                obj.TableName, obj.Descripcion, (obj.Vigente) ? 1 : 0);

            _context.Database.ExecuteSqlCommand(query);
        }

    }
}
