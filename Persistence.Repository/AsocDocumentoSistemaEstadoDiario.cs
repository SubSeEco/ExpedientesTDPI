//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Persistence.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class AsocDocumentoSistemaEstadoDiario
    {
        public int AsocDocumentoSistemaEstadoDiarioID { get; set; }
        public int DocumentoSistemaID { get; set; }
        public int EstadoDiarioID { get; set; }
    
        public virtual EstadoDiario EstadoDiario { get; set; }
        public virtual DocumentoSistema DocumentoSistema { get; set; }
    }
}
