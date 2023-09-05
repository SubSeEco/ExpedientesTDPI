using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Models
{

    public class FiltrosEscritorio
    {
        public int UsuarioID { get; set; }
        public string NumeroTicket { get; set; }
        public int Anio { get; set; }
        public int TipoCausaID { get; set; }
        public string NumeroRegistro { get; set; }
        public string Denominacion { get; set; }
        public string Apelante { get; set; }
        public string FechaIngreso { get; set; }
        public string Apelado { get; set; }
        public int EstadoCausaID { get; set; }
    }

    public class FuncionarioInterno
    {
        public string Nombre { get; set; }
        public int Id_Funcionario { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
    }

    public class DatoPagoTasaManual
    {
        public int SolicitudID { get; set; }
        public string NumeroPCT { get; set; }
        public Nullable<System.DateTime> FechaLiberacion { get; set; }
        public string Solicitante { get; set; }
        public string EmailComunicacion { get; set; }
    }

    public class ItemPagoUpdate
    {
        public decimal USD { get; set; }
        public string fecha { get; set; }
        public int id { get; set; }
        public int TasaCobradaID { get; set; }
        public int Codigo { get; set; }
        public string nombre { get; set; }
        public string organismo { get; set; }
        public decimal pagoCLP { get; set; }
        public decimal pagoUSD { get; set; }
    }
}
