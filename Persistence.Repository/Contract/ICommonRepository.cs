using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public interface ICommonRepository
    {
        VersionEncript GetLastVersion();
        VersionEncript GetFirstVersion();
        VersionEncript GetVersionEncriptById(int id);

        #region MimeType
        IList<TipoFormato> GetTipoFormato();
        IList<MaximoTamanoArchivo> GetMaximoTamanoArchivo();
        IList<FamiliasMimeType> GetFamiliasMimeType();
        IList<TipoDocumento> GetTipoDocumento();
        #endregion

        #region Ventana
        Ventana GetVentanaByTipoVentanaID(int TipoVentanaID);
        IList<TipoVentana> GetTipoVentana();
        int SaveTipoVentana(TipoVentana model);
        int SaveVentana(Ventana model);
        #endregion
        
        #region Usuario
        IList<Usuario> GetUsuarios();
        IList<Perfil> GetPerfil();
        int SaveUser(Usuario model);
        void SaveAsocPerfilUsuario(AsocUsuarioPerfil model);
        void DeletePerfilesUser(int UsuarioID);
        void DeleteUser(int UsuarioID);

        IList<AsocUsuarioPerfil> GetAsocUsuarioPerfil();        
        IList<Perfil> PerfilesFuncionario(int FuncionarioID);

        void SaveAsocDocumentoUsuario(AsocDocumentoUsuario model);
        IList<AsocDocumentoUsuario> GetAsocDocumentoUsuario(int UsuarioID);
        #endregion

        #region Mantenedor de MimeTypes
        int SaveTipoFormato(TipoFormato model);
        void DeleteTipoFormato(int TipoFormatoID);
        FamiliasMimeType GetMimeType(int FamiliasMimeTypeID);
        int SaveMimeType(FamiliasMimeType model);
        void DeleteMimeType(int FamiliasMimeTypeID);

        #endregion

        #region DocumentoAdjunto
        IList<AsocTipoDocumentoAdjunto> GetTipoDocumentoAdjuntoByID(int TipoDocumentoID);
        int SaveDocumentoAdjunto(DocumentoAdjunto model);
        int SaveAsocTipoDocumentoAdjunto(AsocTipoDocumentoAdjunto model);
        void DeleteDocumentoAdjunto(int documentoAdjuntoID, int asocTipoDocumentoAdjuntoID);
        #endregion

        #region Tipo Notificacion
        TipoNotificacion GetTipoNotificacionByID(int TipoNotificacionID);
        IList<TipoNotificacion> GetTipoNotificacion();
        int SaveTipoNotificacion(TipoNotificacion model);
        void SaveAsocTipoNotificacionPerfil(AsocTipoNotificacionPerfil model);
        void DeleteAsocTipoNotificacionPerfil(int TipoNotificacionID);
        #endregion

        #region Feriados
        IList<Feriado> GetAllFeriados();
        int SaveFeriado(Feriado model);
        void DeleteFeriado(int FeriadoID);
        #endregion

        IList<TipoCausa> GetTipoCausa();
        IList<EstadoCausa> GetEstadoCausa();
        IList<OpcionesTramite> GetOpcionesTramite();
        IList<TipoParte> GetTipoParte();
        IList<TipoTramite> GetTipoTramite();
        IList<Pais> GetPais();

        IList<EstadoTabla> GetEstadoTabla();
        IList<TipoTabla> GetTipoTabla();
        IList<Sala> GetSala();

        #region Documento Tmp
        void DeleteDocumentosTmp(DateTime fecha);
        int SaveDocumentoTmp(DocumentoTmp model);
        DocumentoTmp GetDocumentoTmp(int documentoTmpID);
        void DeleteDocumentoTmpByID(int documentoTmpID);
        void DeleteDocumentoTmp(int documentoID);
        void DeleteDocumentoCausa(int documentoID);
        void DeleteDocumentoExpediente(int documentoID);
        #endregion
        ConfTipoCausa GetConfTipoCausa(int tipoCausaID);
        IList<TipoContencioso> GetTipoContencioso();

        IList<AsocTipoTramiteOpciones> GetAsocTipoTramiteOpciones(int TipoTramiteID);
        int SaveAsocTipoTramiteOpciones(AsocTipoTramiteOpciones model);
        void DeleteAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID);
        IList<EstadosAplica> GetEstadosAplicaByAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID);
        int SaveEstadosAplica(EstadosAplica model);
        void DeleteEstadosAplica(int EstadosAplicaID);
        int SaveLogSistema(LogSistema model);

        IList<TipoGenero> GetTipoGenero();
        Usuario GetUsuarioByID(int usuarioID);
        IList<Perfil> GetPerfilUsuario(int usuarioID);
        void SaveSigner(int usuarioActive, string signer);
    }
}
