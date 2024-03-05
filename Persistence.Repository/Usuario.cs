
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
    
public partial class Usuario
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Usuario()
    {

        this.AsocDocumentoUsuario = new HashSet<AsocDocumentoUsuario>();

        this.AsocUsuarioPerfil = new HashSet<AsocUsuarioPerfil>();

        this.Causa = new HashSet<Causa>();

        this.Derivacion = new HashSet<Derivacion>();

        this.Expediente = new HashSet<Expediente>();

        this.Firma = new HashSet<Firma>();

    }


    public int UsuarioID { get; set; }

    public int TipoGeneroID { get; set; }

    public string AdID { get; set; }

    public Nullable<int> Rut { get; set; }

    public string Nombres { get; set; }

    public string Apellidos { get; set; }

    public string Mail { get; set; }

    public string Telefono { get; set; }

    public bool IsClaveUnica { get; set; }

    public System.DateTime FechaRegistro { get; set; }

    public System.DateTime FechaModificacion { get; set; }

    public string Signer { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocDocumentoUsuario> AsocDocumentoUsuario { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<AsocUsuarioPerfil> AsocUsuarioPerfil { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Causa> Causa { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Derivacion> Derivacion { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Expediente> Expediente { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Firma> Firma { get; set; }

    public virtual TipoGenero TipoGenero { get; set; }

}

}
