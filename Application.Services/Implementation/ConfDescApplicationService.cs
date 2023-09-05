using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Persistence.Repository;
using DTO = Application.DTO;


namespace Application.Services
{
    public class ConfDescApplicationService : IConfDescApplicationService
    {
        public IList<DTO.Mantenedor.ConfigurarDescripcion> GetAll()
        {
            IConfigurarDescripcionRepository conf = new ConfigurarDescripcionRepository();

            IList<ConfigurarDescripcion> tablasRepo = conf.GetAll();
            IList<DTO.Mantenedor.ConfigurarDescripcion> listaDTO = new List<DTO.Mantenedor.ConfigurarDescripcion>();

            foreach (var item in tablasRepo)
            {
                var m = new DTO.Mantenedor.ConfigurarDescripcion();
                m.ConfigurarDescripcionID = item.ConfigurarDescripcionID;
                m.NombreTabla = item.NombreTabla.Trim();
                m.Vigente = item.Vigente;

                listaDTO.Add(m);
            }

            return listaDTO;
        }

        public IList<DTO.Mantenedor.ObjetoGenerico> GetDetalleTabla(string tableName, int primaryKey, string like)
        {
            IConfigurarDescripcionRepository conf = new ConfigurarDescripcionRepository();

            IList<ObjetoGenerico> tablasRepo = conf.GetDetalleTablaGenerica(tableName, primaryKey, like);

            IList<DTO.Mantenedor.ObjetoGenerico> listaDTO = new List<DTO.Mantenedor.ObjetoGenerico>();

            foreach (var item in tablasRepo)
            {
                var obj = new DTO.Mantenedor.ObjetoGenerico();
                obj.Codigo = item.Codigo;
                obj.Descripcion = item.Descripcion.Trim();
                obj.Vigente = item.Vigente;

                listaDTO.Add(obj);
            }

            return listaDTO;
        }

        public void UpdateDetalleTabla(DTO.Mantenedor.ObjetoGenerico obj)
        {
            IConfigurarDescripcionRepository conf = new ConfigurarDescripcionRepository();

            ObjetoGenerico objRepo = new ObjetoGenerico();
            objRepo.Codigo = obj.Codigo;
            objRepo.Descripcion = obj.Descripcion;
            objRepo.TableName = obj.TableName;
            objRepo.Vigente = obj.Vigente;

            conf.UpdateDetalleTabla(objRepo);
        }

        public void CreateDetalleTabla(DTO.Mantenedor.ObjetoGenerico obj)
        {
            IConfigurarDescripcionRepository conf = new ConfigurarDescripcionRepository();

            ObjetoGenerico objRepo = new ObjetoGenerico();
            objRepo.Codigo = obj.Codigo;
            objRepo.Descripcion = obj.Descripcion;
            objRepo.TableName = obj.TableName;
            objRepo.Vigente = obj.Vigente;

            conf.CreateDetalleTabla(objRepo);
        }
    }
}
