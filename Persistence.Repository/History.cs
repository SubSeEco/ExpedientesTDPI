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
    
    public partial class History
    {
        public int HistoryID { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaHasta { get; set; }
        public string Proceso { get; set; }
        public Nullable<int> CodigoError { get; set; }
        public string Descripcion { get; set; }
        public string Parametros { get; set; }
        public int filas { get; set; }
        public int spid { get; set; }
        public Nullable<System.DateTime> FechaIniServer { get; set; }
        public Nullable<System.DateTime> FechaTerServer { get; set; }
        public int Tamanio { get; set; }
    }
}
