
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
    
public partial class EstadoCausa
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EstadoCausa()
    {

        this.AsocTipoTramiteOpciones = new HashSet<AsocTipoTramiteOpciones>();

        this.Causa = new HashSet<Causa>();

        this.LogCausa = new HashSet<LogCausa>();

        this.EstadosAplica = new HashSet<EstadosAplica>();

    }


    public int EstadoCausaID { get; set; }

    public string Descripcion { get; set; }

    public bool Vigente { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocTipoTramiteOpciones> AsocTipoTramiteOpciones { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Causa> Causa { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<LogCausa> LogCausa { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<EstadosAplica> EstadosAplica { get; set; }

}

}
