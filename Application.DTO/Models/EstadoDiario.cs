using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class EstadoDiario
    {
        public EstadoDiario()
        {
            this.DetalleEstadoDiario = new HashSet<DetalleEstadoDiario>();
            this.AsocDocumentoSistemaEstadoDiario = new HashSet<AsocDocumentoSistemaEstadoDiario>();
        }

        public int EstadoDiarioID { get; set; }
        public int TipoEstadoDiarioID { get; set; }
        public System.DateTime Fecha { get; set; }

        public virtual ICollection<DetalleEstadoDiario> DetalleEstadoDiario { get; set; }
        public virtual TipoEstadoDiario TipoEstadoDiario { get; set; }
        public virtual ICollection<AsocDocumentoSistemaEstadoDiario> AsocDocumentoSistemaEstadoDiario { get; set; }

        //Custom
        public virtual ICollection<DocumentoSistema> DocumentoSistemaTabla { get; set; }

        public bool IsBorrador()
        {
            return TipoEstadoDiarioID == (int)Domain.Infrastructure.EstadoTabla.Borrador;
        }
        public bool IsGenerado()
        {
            return TipoEstadoDiarioID == (int)Domain.Infrastructure.EstadoTabla.Generado;
        }
        public bool IsPublicado()
        {
            return TipoEstadoDiarioID == (int)Domain.Infrastructure.EstadoTabla.FirmadoPublicado;
        }

        public bool IsEliminado()
        {
            return TipoEstadoDiarioID == (int)Domain.Infrastructure.EstadoTabla.Eliminado;
        }

        public string GetFirmas()
        {
            if (DocumentoSistemaTabla != null && DocumentoSistemaTabla.Count > 0 && IsPublicado())
            {
                int total = 0;
                int ok = 0;

                foreach (var item in DocumentoSistemaTabla)
                {
                    if (item.AsocDocSistemaFirma != null && item.AsocDocSistemaFirma.Count > 0)
                    {
                        foreach (var asocFirma in item.AsocDocSistemaFirma)
                        {
                            total++;
                            if (asocFirma.IsFirmado)
                            {
                                ok++;
                            }
                        }
                    }                    
                }

                return (total == 0) ? "N/A" : $"{ok} de {total}";
            }
            else
            {
                return "N/A";
            }
        }

    }
}
