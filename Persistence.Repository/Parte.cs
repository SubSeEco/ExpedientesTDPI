
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
    
public partial class Parte
{

    public int ParteID { get; set; }

    public int PaisID { get; set; }

    public int CausaID { get; set; }

    public int TipoParteID { get; set; }

    public int Rut { get; set; }

    public string Nombre { get; set; }

    public int RutRepresentante { get; set; }

    public string NombreRepresentante { get; set; }

    public string NombreAbogado { get; set; }

    public string EmailAbogado { get; set; }

    public string NombreEstudioJuridico { get; set; }

    public string FolioConsignacion { get; set; }

    public Nullable<System.DateTime> FechaConsignacion { get; set; }

    public int RutConsignacion { get; set; }

    public string NombreConsignacion { get; set; }



    public virtual Causa Causa { get; set; }

    public virtual Pais Pais { get; set; }

    public virtual TipoParte TipoParte { get; set; }

}

}
