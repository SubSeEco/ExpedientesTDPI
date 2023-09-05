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
    
    public partial class DocumentoCausa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentoCausa()
        {
            this.AsocCausaDocumento = new HashSet<AsocCausaDocumento>();
        }
    
        public int DocumentoCausaID { get; set; }
        public int CausaID { get; set; }
        public int VersionEncriptID { get; set; }
        public string Hash { get; set; }
        public string NombreArchivoFisico { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Descripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AsocCausaDocumento> AsocCausaDocumento { get; set; }
        public virtual Causa Causa { get; set; }
        public virtual VersionEncript VersionEncript { get; set; }
    }
}