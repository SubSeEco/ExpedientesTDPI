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
    
    public partial class MaximoTamanoArchivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaximoTamanoArchivo()
        {
            this.DocumentoAdjunto = new HashSet<DocumentoAdjunto>();
        }
    
        public int MaximoTamanoArchivoID { get; set; }
        public int Tamano { get; set; }
        public bool Vigente { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentoAdjunto> DocumentoAdjunto { get; set; }
    }
}
