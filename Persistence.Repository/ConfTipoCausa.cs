
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
    
public partial class ConfTipoCausa
{

    public int ConfTipoCausaID { get; set; }

    public int TipoCausaID { get; set; }

    public bool IsAnio { get; set; }

    public bool IsObservacion { get; set; }

    public bool IsNumeroRegistro { get; set; }

    public bool IsContencioso { get; set; }

    public int TipoParteID1 { get; set; }

    public int TipoParteID2 { get; set; }

    public bool isNumeroSolicitud { get; set; }

    public bool IsConsignacion { get; set; }



    public virtual TipoCausa TipoCausa { get; set; }

}

}
