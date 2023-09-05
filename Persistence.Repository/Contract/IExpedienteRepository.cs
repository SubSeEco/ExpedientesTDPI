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
        IList<SP_FlujoEstado_Result> GetFlujoEstado(int tipoTramiteID);
        IList<EstadosAplica> GetEstadosAplica(int tipoTramiteID, int estadoCausaID);
        int SaveAsocEscritoDocto(AsocEscritoDocto model);
        int SaveAsocExpedienteOpcion(AsocExpedienteOpcion model);
        IList<AsocEscritoDocto> GetAsocEscritoDocto(int expedienteID);
        void BorrarFirmasExpediente(int expedienteID);
        int SaveFirma(Firma model);
        int SaveAsocFirmaDocto(AsocFirmaDocto model);
        AsocDocSistemaFirma GetAsocDocSistemaFirma(int FirmaID, int DocumentoSistemaID);
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
        int SaveAsocDocumentoSistemaEstadoDiario(AsocDocumentoSistemaEstadoDiario model);
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
    }
}
