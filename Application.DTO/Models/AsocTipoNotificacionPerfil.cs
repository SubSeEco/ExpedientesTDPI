﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.DTO.Models
{
    using System;
    using System.Collections.Generic;

    public partial class AsocTipoNotificacionPerfil
    {
        public int AsocTipoNotificacionPerfilID { get; set; }
        public int PerfilID { get; set; }
        public int TipoNotificacionID { get; set; }

        public virtual TipoNotificacion TipoNotificacion { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}

