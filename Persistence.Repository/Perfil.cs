
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
    
public partial class Perfil
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Perfil()
    {

        this.AsocTipoCausaPerfil = new HashSet<AsocTipoCausaPerfil>();

        this.AsocTipoNotificacionPerfil = new HashSet<AsocTipoNotificacionPerfil>();

        this.AsocUsuarioPerfil = new HashSet<AsocUsuarioPerfil>();

    }


    public int PerfilID { get; set; }

    public string Descripcion { get; set; }

    public bool Vigente { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocTipoCausaPerfil> AsocTipoCausaPerfil { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocTipoNotificacionPerfil> AsocTipoNotificacionPerfil { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocUsuarioPerfil> AsocUsuarioPerfil { get; set; }

}

}
