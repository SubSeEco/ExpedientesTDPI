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
    
    public partial class TipoFormato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoFormato()
        {
            this.DocumentoAdjunto = new HashSet<DocumentoAdjunto>();
            this.FamiliasMimeType = new HashSet<FamiliasMimeType>();
        }
    
        public int TipoFormatoID { get; set; }
        public string Descripcion { get; set; }
        public bool Vigente { get; set; }
        public string ExtraCss { get; set; }
        public Nullable<bool> UsoSolicitud { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoAdjunto> DocumentoAdjunto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamiliasMimeType> FamiliasMimeType { get; set; }
    }
}
