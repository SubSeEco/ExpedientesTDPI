using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class Tabla
    {
        public Tabla()
        {
            this.DetalleTabla = new HashSet<DetalleTabla>();
            this.AsocDocumentoSistemaTabla = new HashSet<AsocDocumentoSistemaTabla>();

            //Custom
            this.DocumentoSistemaTabla = new HashSet<DocumentoSistema>();
        }

        public int TablaID { get; set; }
        public int EstadoTablaID { get; set; }
        public int TipoTablaID { get; set; }
        public int SalaID { get; set; }
        public System.DateTime Fecha { get; set; }
        public int UsuarioRelatorID { get; set; }
        public int UsuarioSubroganteID { get; set; }

        public virtual ICollection<DetalleTabla> DetalleTabla { get; set; }
        public virtual EstadoTabla EstadoTabla { get; set; }
        public virtual Sala Sala { get; set; }
        public virtual TipoTabla TipoTabla { get; set; }
        public virtual ICollection<AsocDocumentoSistemaTabla> AsocDocumentoSistemaTabla { get; set; }

        //Custom
        public virtual ICollection<DocumentoSistema> DocumentoSistemaTabla { get; set; }

        public virtual Usuario UsuarioRelator { get; set; }
        public string NombreRelator { get; set; }

        public bool IsBorrador()
        {
            return EstadoTablaID == (int)Domain.Infrastructure.EstadoTabla.Borrador;
        }
        public bool IsGenerado()
        {
            return EstadoTablaID == (int)Domain.Infrastructure.EstadoTabla.Generado;
        }
        public bool IsPublicado()
        {
            return EstadoTablaID == (int)Domain.Infrastructure.EstadoTabla.FirmadoPublicado;
        }

        public bool IsEliminado()
        {
            return EstadoTablaID == (int)Domain.Infrastructure.EstadoTabla.Eliminado;
        }
    }
}

