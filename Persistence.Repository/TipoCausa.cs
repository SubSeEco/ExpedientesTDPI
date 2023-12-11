
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
    
public partial class TipoCausa
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocTipoCausaPerfil> AsocTipoCausaPerfil { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Causa> Causa { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ConfTipoCausa> ConfTipoCausa { get; set; }

}

}
