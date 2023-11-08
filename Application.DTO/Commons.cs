using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class Commons
    {
    }

    public enum StatusLogin
    {
        Success = 0,
        UserNotExist = 1,
        PasswordNotCorrect = 2,
        UserNotAvailable = 3,
        ServiceNotAvailable = 4,
        Fail = 5
    }
    public class FiltrosEscritorio
    {
        public FiltrosEscritorio()
        {
            this.TipoCausa = new List<DTO.Models.TipoCausa>();
            this.EstadoCausa = new List<DTO.Models.EstadoCausa>();
            this.Sala = new List<DTO.Models.Sala>();
            this.Usuario = new List<DTO.Models.Usuario>();
            this.EstadoTabla = new List<DTO.Models.EstadoTabla>();
            this.ListaID = new int[] { };
        }

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
        public int EstadoTablaID { get; set; }
        public string NumeroSolicitud { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public int[] ListaID { get; set; }
        public int UsuarioRelatorID { get; set; }
        public int SalaID { get; set; } 
        public int TablaID { get; set; }
        public bool IsSoloEscritos { get; set; }
        public bool IsSoloMisEscritos { get; set; }
        public int EstadoFirma { get; set; } // Firmas
        public int TipoDocumentoID { get; set; } // Firmas

        public IList<DTO.Models.TipoCausa> TipoCausa { get; set; }
        public IList<DTO.Models.EstadoCausa> EstadoCausa { get; set; }
        public IList<DTO.Models.EstadoTabla> EstadoTabla { get; set; }
        public IList<DTO.Models.Sala> Sala { get; set; }
        public IList<DTO.Models.Usuario> Usuario { get; set; }
    }

    public partial class EscritorioJson
    {
        public int cId { get; set; }
        //public int tcId { get; set; }
        //public string tc { get; set; }
        //public int ecId { get; set; }
        public string ec { get; set; }
        //public int usrId { get; set; }
        public string us { get; set; }
        //public int respId { get; set; }
        //public int tconId { get; set; }
        //public string tcon { get; set; }
        //public int tcauId { get; set; }
        public string tcau { get; set; }
        public string date { get; set; }
        public string ticket { get; set; }
        public string num { get; set; }
        public int ano { get; set; }
        public string denom { get; set; }
        //public string obs { get; set; }
        //public bool IsContencioso { get; set; }
        //public string reg { get; set; }

        //Custom
        public List<string> Act { get; set; }
    }

    public partial class EscritorioFirmaJson
    {
        public string diasT { get; set; }
        public int St1 { get; set; }
        public int St2 { get; set; }
        public string us { get; set; }
        public string date { get; set; }
        public string ticket { get; set; }
        public string resp { get; set; }

        //Custom
        public string AccionesList { get; set; }
    }

    public class ItemForm
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }

    public class DataForm
    {
        public DataForm()
        {
            this.Pais = new List<Models.Pais>();
            this.AsocTipoDocumentoAdjunto = new List<Models.AsocTipoDocumentoAdjunto>();
            this.TipoParte = new List<Models.TipoParte>();
            this.TipoTramite = new List<Models.TipoTramite>();
            this.TipoContencioso = new List<Models.TipoContencioso>();
            this.Usuario = new List<Models.Usuario>();
            this.Sala = new List<Models.Sala>();
            this.TipoTabla = new List<Models.TipoTabla>();
            this.PerfilActive = new PerfilActive();
            this.UserActive = new Models.Usuario();
        }

        public int CausaID { get; set; }
        public int ExpedienteID { get; set; }
        public bool IsNew { get; set; }
        public bool IsView { get; set; }

        public PerfilActive PerfilActive { get; set; }
        public Models.Usuario UserActive { get; set; }

        public IList<Models.Pais> Pais { get; set; }
        public IList<Models.TipoParte> TipoParte { get; set; }
        public IList<Models.TipoTramite> TipoTramite { get; set; }
        public IList<Models.TipoContencioso> TipoContencioso { get; set; }
        public IList<Models.AsocTipoDocumentoAdjunto> AsocTipoDocumentoAdjunto { get; set; }
        public IList<Models.Usuario> Usuario { get; set; }
        public IList<Models.Sala> Sala { get; set; }
        public IList<Models.TipoTabla> TipoTabla { get; set; }
        public Models.EstadosAplica EstadosAplica { get; set; }
    }

    public class PerfilActive
    {
        public bool IsTDPI { get; set; }
        public bool IsINAPI { get; set; }
        public bool IsAdministrador { get; set; }
        public bool IsSAG { get; set; }
        public bool IsInvitado { get; set; }
    }


    public class FuncionarioInterno
    {
        public string Nombre { get; set; }
        public int Id_Funcionario { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public string ActiveDirectoryID { get; set; }
        //public List<string> Grupos { get; set; }
        public int RegionID { get; set; }
        public bool IsResponsable { get; set; }
    }

    public class UserGrid
    {
        public string User { get; set; }
        public int UsuarioID { get; set; }
        public string AdID { get; set; }
        public Nullable<int> Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Mail { get; set; }
        public bool IsClaveUnica { get; set; }
        public string Perfiles { get; set; }        
        public string Actions { get; set; }

        public string GetRUT() {
            int rut = Convert.ToInt32(this.Rut);
            if (rut > 0)
            {
                Infrastructure.Utils.Mod11Validator mod11 = new Infrastructure.Utils.Mod11Validator(rut, "");
                return rut.ToString().Trim() + "-" + mod11.CalcularDigitoVerificador(rut);
            }
            else
            {
                return "";
            }
        }

    }

    public class DownloadFile
    {
        public string CadenaVersionEncript { get; set; }
        public string NombreArchivoFisico { get; set; }
        public string HashFile { get; set; }

        public DownloadFile()
        {
            this.CadenaVersionEncript = string.Empty;
            this.NombreArchivoFisico = string.Empty;
            this.HashFile = string.Empty;
        }
    }

    public class ValidateFile
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; }

        public ValidateFile()
        {
            this.Mensaje = string.Empty;
        }
    }

    public class Feriados
    {
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
    }

    public class ParteVue
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
        public string FechaConsignacion { get; set; }
        public int RutConsignacion { get; set; }
        public string NombreConsignacion { get; set; }
    }
    public class ParteVueSinConsignacion
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
    }

    public class ItemSession
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class BoxHome
    {
        public int TipoCausaID { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionHTML { get; set; }
        public string DescripcionLarga { get; set; }
        public string icon { get; set; }
        public bool IsSearch { get; set; }

        //Custom
        public bool IsRecurdoDeHecho()
        {
            return (TipoCausaID == (int)Domain.Infrastructure.TipoCausa.RecursoHechoMarca) ||
                (TipoCausaID == (int)Domain.Infrastructure.TipoCausa.RecursoHechoPatente);
        }
    }
}
