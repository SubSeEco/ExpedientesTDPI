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
    
    public partial class AsocCausaDocumento
    {
        public AsocCausaDocumento()
        {
            this.AsocEscritoDocto = new HashSet<AsocEscritoDocto>();
        }
    
        public int AsocCausaDocumentoID { get; set; }
        public int DocumentoAdjuntoID { get; set; }
        public int DocumentoCausaID { get; set; }
        public int CompromisoID { get; set; }
    
        public virtual DocumentoAdjunto DocumentoAdjunto { get; set; }
        public virtual DocumentoCausa DocumentoCausa { get; set; }
        public virtual ICollection<AsocEscritoDocto> AsocEscritoDocto { get; set; }
    }
}
