using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{
    public partial class SP_Causas_Result
    {
        public int CausaID { get; set; }
        public int TipoCanalID { get; set; }
        public string TipoCanal { get; set; }
        public int EstadoCausaID { get; set; }
        public string EstadoCausa { get; set; }
        public int UsuarioID { get; set; }
        public string Usuario { get; set; }
        public int UsuarioResponsableID { get; set; }
        public int TipoContenciosoID { get; set; }
        public string TipoContencioso { get; set; }
        public int TipoCausaID { get; set; }
        public string TipoCausa { get; set; }
        public string FechaIngreso { get; set; }
        public string NumeroTicket { get; set; }
        public string Numero { get; set; }
        public int Anio { get; set; }
        public string Denominacion { get; set; }
        public string Observacion { get; set; }
        public bool IsContencioso { get; set; }
        public string NumeroRegistro { get; set; }
        public int bt1 { get; set; }
        public int bt2 { get; set; }
        public int bt3 { get; set; }
        public int bt4 { get; set; }
        public int bt5 { get; set; }
        public int bt6 { get; set; }

        //Custom
        public List<string> AccionesList { get; set; }
    }
}