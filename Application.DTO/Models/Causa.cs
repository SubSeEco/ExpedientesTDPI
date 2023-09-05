//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.DTO.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class Causa
    {
        public Causa()
        {
            this.DocumentoCausa = new HashSet<DocumentoCausa>();
            this.LogCausa = new HashSet<LogCausa>();
            this.Parte = new HashSet<Parte>();
            this.Expediente = new HashSet<Expediente>();
            this.DetalleTabla = new HashSet<DetalleTabla>();
        }

        public int CausaID { get; set; }
        public int TipoCanalID { get; set; }
        public int EstadoCausaID { get; set; }
        public int UsuarioID { get; set; }
        public int UsuarioResponsableID { get; set; }
        public int TipoContenciosoID { get; set; }
        public int TipoCausaID { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string NumeroTicket { get; set; }
        public string Numero { get; set; }
        public int Anio { get; set; }
        public string Denominacion { get; set; }
        public string Observacion { get; set; }
        public bool IsContencioso { get; set; }
        public string NumeroRegistro { get; set; }
    
        public virtual TipoCausa TipoCausa { get; set; }
        public virtual TipoContencioso TipoContencioso { get; set; }
        public virtual ICollection<LogCausa> LogCausa { get; set; }
        public virtual ICollection<Parte> Parte { get; set; }
        public virtual EstadoCausa EstadoCausa { get; set; }
        public virtual ICollection<Expediente> Expediente { get; set; }
        public virtual TipoCanal TipoCanal { get; set; }
        public virtual ICollection<DocumentoCausa> DocumentoCausa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<DetalleTabla> DetalleTabla { get; set; }

        //CUstom

        public string GetDiasTranscurridos()
        {
            TimeSpan difference = DateTime.Now - FechaIngreso;
            return $"{difference.Days}";
        }

        public string GetParteCausa(Domain.Infrastructure.TipoParte tipoParte)
        {
            List<int> Parte1 = new List<int>();
            Parte1.Add((int)Domain.Infrastructure.TipoParte.Apelante);
            Parte1.Add((int)Domain.Infrastructure.TipoParte.Recurrente);
            Parte1.Add((int)Domain.Infrastructure.TipoParte.Solicitante);

            List<int> Parte2 = new List<int>();
            Parte2.Add((int)Domain.Infrastructure.TipoParte.Apelado);
            Parte2.Add((int)Domain.Infrastructure.TipoParte.Recurrido);

            List<int> listaFilter = (tipoParte == Domain.Infrastructure.TipoParte.Apelante) ? Parte1 : Parte2;

            IList<Parte> lista = Parte.Where(x => listaFilter.Contains(x.TipoParteID)).ToList();
            List<string> nombres = new List<string>();

            foreach (var item in lista)
            {
                nombres.Add(item.Nombre.Trim());
            }

            return string.Join(", ", nombres);
        }

    }
}
