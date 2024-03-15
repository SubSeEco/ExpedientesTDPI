
using System.Collections.Generic;
using Application.DTO.Models;

namespace Application.Services
{
    public interface ICommonAppServices
    {
        VersionEncript GetLastVersionEncript();
        VersionEncript GetFirstVersionEncript();
        VersionEncript GetVersionEncriptById(int id);

        #region Ventana

        Ventana GetVentanaByTipoVentanaID(int TipoVentanaID);
        Ventana GetInfoByTipoVentanaID(int TipoVentanaID);
        IList<TipoVentana> GetTipoVentana();
        int SaveTipoVentana(TipoVentana _dto);
        int SaveVentana(Ventana _ventana);

        #endregion

        #region Mantenedor de MimeTypes
        IList<TipoFormato> GetTipoFormato();
        IList<TipoDocumento> GetTipoDocumento();
        IList<MaximoTamanoArchivo> GetMaximoTamanoArchivo();
        #endregion

        #region Usuario
        IList<Usuario> GetUsuarios();
        IList<DTO.Models.Perfil> GetPerfil(bool SoloVigentes = false);
        int SaveUser(DTO.Models.Usuario dto);
        void SaveAsocPerfilUsuario(DTO.Models.AsocUsuarioPerfil dto);
        void DeletePerfilesUser(int UsuarioID);

        IList<AsocUsuarioPerfil> GetAsocUsuarioPerfil();
        IList<Perfil> PerfilesFuncionario(int FuncionarioID);
        void SaveAsocDocumentoUsuario(DTO.Models.AsocDocumentoUsuario dto);

        IList<AsocDocumentoUsuario> GetAsocDocumentoUsuario(int UsuarioID);
        #endregion

        #region Mantenedor de MimeTypes
        int SaveTipoFormato(DTO.Models.TipoFormato tipoFormato);
        void DeleteTipoFormato(int TipoFormatoID);
        IList<FamiliasMimeType> GetFamiliasMimeType();
        DTO.Models.FamiliasMimeType GetMimeType(int FamiliasMimeTypeID);
        int SaveMimeType(DTO.Models.FamiliasMimeType familiasMimeType);
        void DeleteMimeType(int FamiliasMimeTypeID);

        #endregion

        #region DocumentoAdjunto
        IList<AsocTipoDocumentoAdjunto> GetTipoDocumentoAdjuntoByID(int TipoDocumentoID);
        int SaveDocumentoAdjunto(DocumentoAdjunto dto);
        int SaveAsocTipoDocumentoAdjunto(AsocTipoDocumentoAdjunto dto);
        void DeleteDocumentoAdjunto(int DocumentoAdjuntoID, int AsocTipoDocumentoAdjuntoID);
        #endregion

        #region Tipo Notificacion
        TipoNotificacion GetTipoNotificacionByID(int TipoNotificacionID);
        IList<TipoNotificacion> GetTipoNotificacion();
        int SaveTipoNotificacion(TipoNotificacion dto);
        void SaveAsocTipoNotificacionPerfil(AsocTipoNotificacionPerfil model);
        void DeleteAsocTipoNotificacionPerfil(int TipoNotificacionID);
        #endregion

        #region Feriados
        IList<Feriado> GetAllFeriados();
        int SaveFeriado(Feriado dto);
        void DeleteFeriado(int FeriadoID);
        #endregion

        IList<TipoCausa> GetTipoCausa(bool SoloVigente = false);
        IList<EstadoCausa> GetEstadoCausa(bool SoloVigente = false);
        IList<OpcionesTramite> GetOpcionesTramite(bool SoloVigente = false);
        IList<TipoParte> GetTipoParte(bool SoloVigente = false);
        IList<TipoTramite> GetTipoTramite(bool SoloVigente = false);
        IList<Pais> GetPais();
        IList<EstadoTabla> GetEstadoTabla(bool SoloVigente = false);
        IList<TipoTabla> GetTipoTabla(bool SoloVigente = false);
        IList<Sala> GetSala(bool SoloVigente = false);
        IList<TipoContencioso> GetTipoContencioso(bool SoloVigente = false);
        Usuario GetUsuarioByID(int usuarioID);

        #region Documento Tmp
        void DeleteDocumentosTmp(System.DateTime fecha);
        int SaveDocumentoTmp(DocumentoTmp dto);
        DocumentoTmp GetDocumentoTmp(int DocumentoTmpID);
        void DeleteDocumentoTmpByID(int DocumentoTmpID);
        void DeleteDocumento(Domain.Infrastructure.TipoDocumento tipoDocumento, int DocumentoID);
        #endregion
        ConfTipoCausa GetConfTipoCausa(int tipoCausaID);

        IList<AsocTipoTramiteOpciones> GetAsocTipoTramiteOpciones(int TipoTramiteID);
        int SaveAsocTipoTramiteOpciones(AsocTipoTramiteOpciones dto);
        void DeleteAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID);
        IList<EstadosAplica> GetEstadosAplicaByAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID);
        int SaveEstadosAplica(EstadosAplica  dto);
        void DeleteEstadosAplica(int EstadosAplicaID);
        int SaveLogSistema(LogSistema dto);

        IList<TipoGenero> GetTipoGenero(bool SoloVigente = false);
        IList<Perfil> GetPerfilUsuario(int UsuarioID);
        void SaveSigner(int usuarioActive, string signer);
        Usuario GetFirmanteTable(int tipoFirma);
    }
}
