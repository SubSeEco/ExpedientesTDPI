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
    
    public partial class DetalleTabla
    {
        public int DetalleTablaID { get; set; }
        public int CausaID { get; set; }
        public int TablaID { get; set; }
        public int Orden { get; set; }
        public bool Vigente { get; set; }
    
        public virtual Causa Causa { get; set; }
        public virtual Tabla Tabla { get; set; }
    }
}