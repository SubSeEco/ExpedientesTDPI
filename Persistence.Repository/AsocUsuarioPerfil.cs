
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
    
public partial class AsocUsuarioPerfil
{

    public int AsocUsuarioPerfilID { get; set; }

    public int UsuarioID { get; set; }

    public int PerfilID { get; set; }



    public virtual Perfil Perfil { get; set; }

    public virtual Usuario Usuario { get; set; }

}

}
