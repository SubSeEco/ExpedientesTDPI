using Application.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using Enums = Domain.Infrastructure;

namespace Application.Services
{
    public interface IExpedienteAppServices
    {
        IList<SP_Causas_Result> GetSP_Causas(DTO.FiltrosEscritorio filtros);
        Causa GetCausa(int causaID);
        IList<Expediente> GetExpedienteByCausa(int causaID);
        IList<Parte> GetParteByCausa(int causaID);
        DTO.DownloadFile GetDownloadFileByHash(Enums.TipoDocumento tipoDocumento, string Hash, int CausaID, int DoctoID = 0, int Docto2ID = 0);
        Parte GetParte(int parteID);
        int SaveParte(Parte dto);
        int SaveCausa(Causa dto);
        void UpdateCausa(Causa dto);
        int SaveLogCausa(LogCausa dto);
        IList<LogCausa> GetLogCausa(int CausaID);
        int SaveDocumentoCausa(DocumentoCausa dto);
        int SaveAsocCausaDocumento(AsocCausaDocumento dto);
        int SaveAsocEscritoDocto(AsocEscritoDocto dto);
        IList<DocumentoCausa> GetDocumentoCausa(int causaID, Domain.Infrastructure.TipoDocumento tipoDoc);
        Folio GetFolio(int Anio);
        IList<Causa> GetCausasByFilter(DTO.FiltrosEscritorio filtros, Enums.TipoGrid listadoIngresos = Enums.TipoGrid.None);
        int SaveDocumentoSistema(DocumentoSistema dto);
        void CambiarEstadoCausa(int causaID, Domain.Infrastructure.EstadoCausa estado);
        int SaveExpediente(Expediente dto);
        Expediente GetExpediente(int expedienteID);
        IList<Firma> GetFirmaByExpedienteID(int expedienteID);
        IList<Firma> GetEscritorioFirmas(DTO.FiltrosEscritorio filtros);
        IList<EstadoCausa> GetFlujoEstado(int tipoTramiteID);
        IList<EstadosAplica> GetEstadosAplica(int tipoTramiteID, int estadoCausaID);
        int SaveAsocExpedienteOpcion(AsocExpedienteOpcion dto);
        IList<AsocEscritoDocto> GetAsocEscritoDocto(int expedienteID);
        void BorrarFirmasExpediente(int expedienteID);
        int SaveFirma(Firma dto);
        int SaveAsocFirmaDocto(AsocFirmaDocto dto);
        AsocDocSistemaFirma GetAsocDocSistemaFirmaByFirmaDocto(int FirmaID, int DocumentoSistemaID);
        int SaveAsocDocSistemaFirma(AsocDocSistemaFirma dto);
        void DeleteParteByID(int parteID);
        void SetVigenciaDetalleTabla(int detalleTablaID, bool vigencia);
        void SetEstadoTabla(int tablaID, Domain.Infrastructure.EstadoTabla estado);
        int SaveTabla(Tabla dto);
        Tabla GetTablaByID(int tablaID);
        IList<Tabla> GetTabla(DTO.FiltrosEscritorio filtros);
        bool GetExisteTabla(DateTime fecha);
        IList<DetalleTabla> GetDetalleTablaByCausa(int causaID);
        int SaveDetalleTabla(DetalleTabla dto, bool SetLastOrder);
        int SaveAsocDocumentoSistemaTabla(AsocDocumentoSistemaTabla dto);
        int SaveAsocDocumentoSistemaEstadoDiario(AsocDocumentoSistemaEstadoDiario dto);
        IList<DocumentoSistema> GetAsocDocumentoSistema(int Identidad, Enums.TipoDocumento tipoDoc);

        int SaveEstadoDiario(EstadoDiario dto);
        void SetTipoEstadoDiario(int estadoDiarioID, Domain.Infrastructure.TipoEstadoDiario estado);
        void SetVigenciaDetalleEstadoDiario(int detalleEstadoDiarioID, bool vigencia);
        EstadoDiario GetEstadoDiarioByID(int estadoDiarioID);
        IList<EstadoDiario> GetEstadoDiario(DTO.FiltrosEscritorio filtros);
        int SaveDetalleEstadoDiario(DetalleEstadoDiario dto);
        IList<DetalleEstadoDiario> GetDetalleEstadoDiarioByExpediente(int expedienteID);
        void SetExpedienteFinalizado(int expedienteID, bool finalizar);
    }
}
