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
    
    public partial class DocumentoTmp
    {
        public int DocumentoTmpID { get; set; }
        public int TipoDocumentoID { get; set; }
        public int VersionEncriptID { get; set; }
        public string Hash { get; set; }
        public System.DateTime Fecha { get; set; }
    
        public virtual VersionEncript VersionEncript { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
    }
}
