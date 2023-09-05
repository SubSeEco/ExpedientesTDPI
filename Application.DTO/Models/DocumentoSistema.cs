using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class DocumentoSistema
    {
        public DocumentoSistema()
        {
            this.AsocDocumentoSistemaEstadoDiario = new HashSet<AsocDocumentoSistemaEstadoDiario>();
            this.AsocDocumentoSistemaTabla = new HashSet<AsocDocumentoSistemaTabla>();
        }

        public int DocumentoSistemaID { get; set; }
        public int VersionEncriptID { get; set; }
        public int TipoDocumentoID { get; set; }
        public string Hash { get; set; }
        public string NombreArchivoFisico { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Descripcion { get; set; }

        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual VersionEncript VersionEncript { get; set; }
        public virtual ICollection<AsocDocSistemaFirma> AsocDocSistemaFirma { get; set; }
        public virtual ICollection<AsocDocumentoSistemaEstadoDiario> AsocDocumentoSistemaEstadoDiario { get; set; }
        public virtual ICollection<AsocDocumentoSistemaTabla> AsocDocumentoSistemaTabla { get; set; }

        public string GetDiasTranscurridos()
        {
            TimeSpan difference = DateTime.Now - Fecha.Value;

            return $"{difference.Days}";
        }
    }
}