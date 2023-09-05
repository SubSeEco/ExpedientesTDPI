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
    
    public partial class TipoCausa
    {
        public TipoCausa()
        {
            this.AsocTipoCausaPerfil = new HashSet<AsocTipoCausaPerfil>();
            this.Causa = new HashSet<Causa>();
            this.ConfTipoCausa = new HashSet<ConfTipoCausa>();
        }

        public int TipoCausaID { get; set; }
        public string Descripcion { get; set; }
        public bool IsPublico { get; set; }
        public bool IsInterno { get; set; }
        public string DescripcionLarga { get; set; }
        public bool Vigente { get; set; }

        public virtual ICollection<AsocTipoCausaPerfil> AsocTipoCausaPerfil { get; set; }
        public virtual ICollection<Causa> Causa { get; set; }
        public virtual ICollection<ConfTipoCausa> ConfTipoCausa { get; set; }

        //Custom

        public string DescripcionHTML { get; set; }

        public bool IsRecurdoDeHecho()
        {
            return (TipoCausaID == (int)Domain.Infrastructure.TipoCausa.RecursoHechoMarca) ||
                (TipoCausaID == (int)Domain.Infrastructure.TipoCausa.RecursoHechoPatente);
        }
        public string Icon { get; set; }

        public void SetIcon()
        {
            string icono = "";
            Domain.Infrastructure.TipoCausa tipo = (Domain.Infrastructure.TipoCausa)TipoCausaID;

            switch (tipo)
            {
                case Domain.Infrastructure.TipoCausa.Marca:
                    icono = "fa-registered";
                    break;
                case Domain.Infrastructure.TipoCausa.Patente:
                    icono = "fa-lightbulb";
                    break;
                case Domain.Infrastructure.TipoCausa.VariedadVegetal:
                    icono = "fa-leaf";
                    break;
                case Domain.Infrastructure.TipoCausa.ProteccionSuplementaria:
                    icono = "fa-lock";
                    break;
                case Domain.Infrastructure.TipoCausa.RecursoHechoMarca:
                    icono = "fa-balance-scale";
                    break;
                case Domain.Infrastructure.TipoCausa.RecursoHechoPatente:
                    icono = "fa-balance-scale";
                    break;
                default:
                    icono = "fa-square";
                    break;
            }

            Icon = icono;
        }
    }
}