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
    
    public partial class Firma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Firma()
        {
            this.AsocDocSistemaFirma = new HashSet<AsocDocSistemaFirma>();
            this.AsocExpeFirma = new HashSet<AsocExpeFirma>();
            this.AsocFirmaDocto = new HashSet<AsocFirmaDocto>();
        }
    
        public int FirmaID { get; set; }
        public int UsuarioID { get; set; }
        public int Orden { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AsocDocSistemaFirma> AsocDocSistemaFirma { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AsocExpeFirma> AsocExpeFirma { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AsocFirmaDocto> AsocFirmaDocto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}