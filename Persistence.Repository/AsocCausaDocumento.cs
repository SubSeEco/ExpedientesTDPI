
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
    
public partial class AsocCausaDocumento
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocEscritoDocto> AsocEscritoDocto { get; set; }

}

}
