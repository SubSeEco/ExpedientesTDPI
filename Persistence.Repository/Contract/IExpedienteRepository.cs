using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;

namespace Persistence.Repository
{
    public interface IExpedienteRepository
    {
        IList<SP_Causas_Result> GetSP_Causas(FiltrosEscritorio filtros);
        Causa GetCausa(int causaID);
        IList<Expediente> GetExpedienteByCausa(int causaID);
        IList<Parte> GetParteByCausa(int causaID);
        Parte GetParte(int parteID);
        int SaveParte(Parte model);
        int SaveLogCausa(LogCausa model);
        IList<LogCausa> GetLogCausa(int CausaID);
        int SaveDocumentoCausa(DocumentoCausa model);
        int SaveAsocCausaDocumento(AsocCausaDocumento model);
        IList<DocumentoCausa> GetDocumentoCausa(int causaID, Domain.Infrastructure.TipoDocumento tipoDoc);
        void UpdateCausa(Causa model);
        Folio GetFolio(int anio);
        IList<Causa> GetCausasByFilter(FiltrosEscritorio filtros, TipoGrid tipo);
        int SaveDocumentoSistema(DocumentoSistema model);
        DocumentoSistema GetDocumentoSistemaByHash(string hash);
        void CambiarEstadoCausa(int causaID, Domain.Infrastructure.EstadoCausa estado);
        int SaveExpediente(Expediente model);
        Expediente GetExpediente(int expedienteID);
        IList<Firma> GetFirmaByExpedienteID(int expedienteID);
        IList<Firma> GetFirmasByUsuarioID(int UsuarioID);
        IList<Firma> GetEscritorioFirmas(FiltrosEscritorio filtros);
        IList<ItemDocumentoMigracion> GetItemsMigracion(string query);
        void UpdateRegistroMigrado(int TempID, bool IsMigrado, string Comentario);
        IList<SP_FlujoEstado_Result> GetFlujoEstado(int tipoTramiteID);
        IList<EstadosAplica> GetEstadosAplica(int tipoTramiteID, int estadoCausaID);
        int SaveAsocEscritoDocto(AsocEscritoDocto model);
        int SaveAsocExpedienteOpcion(AsocExpedienteOpcion model);
        IList<AsocEscritoDocto> GetAsocEscritoDocto(int expedienteID);
        void BorrarFirmasExpediente(int expedienteID);
        void BorrarFirmaByFirmaID(int FirmaID);
        void BorrarFirmaByAsocDocSistema(int AsocDocSistemaFirmaID);
        int SaveFirma(Firma model);
        //int SaveAsocFirmaDocto(AsocFirmaDocto model);
        int SaveAsocFirmaDocto(AsocFirmaDocto model);
        int SaveAsocFirmaDoctoMarcarToma(AsocFirmaDocto model);
        AsocFirmaDocto GetAsocFirmaDoctoGS(int expedienteID, int UsuarioID);
        IList<AsocFirmaDocto> GetListAsocFirmaDoctoGS(int expedienteID);
        AsocDocSistemaFirma GetAsocDocSistemaFirma(int FirmaID, int DocumentoSistemaID);
        IList<AsocDocSistemaFirma> GetAsocDocSistemaFirmaByDocto(int DocumentoSistemaID);
        int SaveAsocDocSistemaFirma(AsocDocSistemaFirma model);
        int SaveCausa(Causa model);
        void DeleteParteByID(int parteID);

        void SetEstadoTabla(int tablaID, Domain.Infrastructure.EstadoTabla estado);
        void SetVigenciaDetalleTabla(int detalleTablaID, bool vigencia);
        int SaveTabla(Tabla model);
        Tabla GetTablaByID(int tablaID);
        IList<Tabla> GetTabla(FiltrosEscritorio filtros);
        bool GetExisteTabla(DateTime fecha);
        IList<DetalleTabla> GetDetalleTablaByCausa(int causaID);
        int SaveDetalleTabla(DetalleTabla model, bool SetLastOrder);
        int SaveAsocDocumentoSistemaTabla(AsocDocumentoSistemaTabla model);
        IList<AsocDocumentoSistemaTabla> GetAsocDocumentoSistemaTabla(int TablaID);
        IList<AsocDocumentoSistemaTabla> GetAsocDocumentoSistemaTablaByDocumentoSitemaID(int DocumentoSistemaID);
        int SaveAsocDocumentoSistemaEstadoDiario(AsocDocumentoSistemaEstadoDiario model);
        IList<AsocDocumentoSistemaEstadoDiario> GetAsocDocumentoSistemaEstadoDiario(int EstadoDiarioID);
        IList<AsocDocumentoSistemaEstadoDiario> GetAsocDocumentoSistemaEstadoDiarioByDocumentoSitemaID(int DocumentoSistemaID);
        IList<DocumentoSistema> GetAsocDocumentoSistema(int identidad, Domain.Infrastructure.TipoDocumento tipoDoc);

        DocumentoSistema GetDocumentoSistema(int DocumentoSistemaID);

        int SaveEstadoDiario(EstadoDiario model);
        void SetTipoEstadoDiario(int estadoDiarioID, Domain.Infrastructure.TipoEstadoDiario estado);
        void SetVigenciaDetalleEstadoDiario(int detalleEstadoDiarioID, bool vigencia);
        EstadoDiario GetEstadoDiarioByID(int estadoDiarioID);
        IList<EstadoDiario> GetEstadoDiario(FiltrosEscritorio filtros);
        int SaveDetalleEstadoDiario(DetalleEstadoDiario model);
        IList<DetalleEstadoDiario> GetDetalleEstadoDiarioByExpediente(int expedienteID);
        void SetExpedienteFinalizado(int expedienteID, bool finalizar);
        void SetExpedienteInadmisible(int expedienteID);
        IList<AsocExpeFirma> GetAsocExpeFirmaByExpedienteID(int expedienteID);
        int SaveAsocExpeFirma(AsocExpeFirma model);
        void UpdateResponsable(int expedienteID, int usuarioID);
        int SaveDerivacion(Derivacion model);
        IList<SP_Alarmas_Result> GetAlarmasInternasService(int UsuarioID);


        IList<Causa> WCF_ObtenerExpediente(int expediente_tipo, string fecha_ingreso, int rolCorr, 
            int rolAnio, string numero_solicitud, string numero_registro, string individualizacion, 
            string nombre_solicitante, string nombre_apelante, string nombre_apelado, int paginaActual, int registrosPorPagina);

        IList<Expediente> WCF_ObtenerEventosExpediente(int rol, int anio);
    }
}
