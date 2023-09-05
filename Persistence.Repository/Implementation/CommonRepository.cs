using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;

using Infrastructure.Logging;
using Enums = Domain.Infrastructure;

namespace Persistence.Repository
{
    public class CommonRepository : ICommonRepository
    {

        private readonly SGDE2Entities _context = null;

        public CommonRepository()
        {
            _context = new SGDE2Entities();
        }


        #region VersionEncript

        public VersionEncript GetLastVersion()
        {
            return _context.VersionEncript.OrderByDescending(u => u.VersionEncriptID).FirstOrDefault();
        }

        public VersionEncript GetFirstVersion()
        {
            return _context.VersionEncript.OrderBy(u => u.VersionEncriptID).FirstOrDefault();
        }

        public VersionEncript GetVersionEncriptById(int id)
        {
            return _context.VersionEncript.Where(a => a.VersionEncriptID == id).FirstOrDefault();
        }

        #endregion

        #region Archivos Adjuntos

        public IList<TipoFormato> GetTipoFormato()
        {
            return _context.TipoFormato.ToList();
        }

        public IList<TipoDocumento> GetTipoDocumento()
        {
            return _context.TipoDocumento.ToList();
        }

        public IList<MaximoTamanoArchivo> GetMaximoTamanoArchivo()
        {
            return _context.MaximoTamanoArchivo.ToList();
        }

        public IList<FamiliasMimeType> GetFamiliasMimeType()
        {
            return _context.FamiliasMimeType.ToList();
        }

        #endregion

        #region Mantenedor de MimeTypes

        public int SaveTipoFormato(TipoFormato model)
        {
            if (model.TipoFormatoID > 0)
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(model).State = EntityState.Added;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return model.TipoFormatoID;

        }

        public void DeleteTipoFormato(int TipoFormatoID)
        {
            TipoFormato model = _context.TipoFormato.Where(x => x.TipoFormatoID == TipoFormatoID).FirstOrDefault();

            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public FamiliasMimeType GetMimeType(int FamiliasMimeTypeID)
        {
            return _context.FamiliasMimeType.Where(x => x.FamiliasMimeTypeID == FamiliasMimeTypeID).FirstOrDefault();
        }

        public int SaveMimeType(FamiliasMimeType model)
        {
            if (model.FamiliasMimeTypeID > 0)
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(model).State = EntityState.Added;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

            return model.FamiliasMimeTypeID;

        }

        public void DeleteMimeType(int FamiliasMimeTypeID)
        {
            FamiliasMimeType model = _context.FamiliasMimeType.Where(x => x.FamiliasMimeTypeID == FamiliasMimeTypeID).FirstOrDefault();

            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        #endregion

        #region Documento Adjunto

        public IList<AsocTipoDocumentoAdjunto> GetTipoDocumentoAdjuntoByID(int TipoDocumentoID)
        {
            return _context
            .AsocTipoDocumentoAdjunto
            .Include("TipoDocumento")
            .Include("DocumentoAdjunto.TipoFormato.FamiliasMimeType")
            .Include("DocumentoAdjunto.MaximoTamanoArchivo")
            .Include("DocumentoAdjunto.VersionEncript")
            .AsNoTracking()
            .Where(x => x.TipoDocumentoID == TipoDocumentoID).ToList();
        }

        public int SaveDocumentoAdjunto(DocumentoAdjunto model)
        {
            if (model.DocumentoAdjuntoID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
                return model.DocumentoAdjuntoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public int SaveAsocTipoDocumentoAdjunto(AsocTipoDocumentoAdjunto model)
        {
            if (model.AsocTipoDocumentoAdjuntoID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();

                return model.AsocTipoDocumentoAdjuntoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public void DeleteDocumentoAdjunto(int documentoAdjuntoID, int asocTipoDocumentoAdjuntoID)
        {
            AsocTipoDocumentoAdjunto asoc = _context.AsocTipoDocumentoAdjunto.Find(asocTipoDocumentoAdjuntoID);
            _context.Entry(asoc).State = EntityState.Deleted;

            DocumentoAdjunto doc = _context.DocumentoAdjunto.Find(documentoAdjuntoID);
            _context.Entry(doc).State = EntityState.Deleted;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Infrastructure.Logging.Logger.Execute().Error(ex);
            }
        }

        #endregion

        #region Ventana

        public Ventana GetVentanaByTipoVentanaID(int TipoVentanaID)
        {
            return _context.Ventana.Where(i => i.TipoVentanaID == TipoVentanaID).FirstOrDefault();
        }

        public IList<TipoVentana> GetTipoVentana()
        {
            return _context.TipoVentana.ToList();
        }

        public int SaveTipoVentana(TipoVentana model)
        {
            _context.Entry(model).State = EntityState.Added;

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }

            return model.TipoVentanaID;
        }

        public int SaveVentana(Ventana model)
        {

            if (model.VentanaID != 0)
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(model).State = EntityState.Added;
            }

            try
            {
                _context.SaveChanges();
                return model.VentanaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        #endregion

        #region Usuario
        public IList<Usuario> GetUsuarios()
        {
            return _context.Usuario.AsNoTracking().Include("TipoGenero").Include("AsocUsuarioPerfil.Perfil").OrderBy(x => x.Nombres).ToList();
        }

        public IList<Perfil> GetPerfil()
        {
            return _context.Perfil.ToList();
        }

        public int SaveUser(Usuario model)
        {
            if (model.UsuarioID >= 0)
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(model).State = EntityState.Added;
            }

            try
            {
                _context.SaveChanges();
                return model.UsuarioID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void SaveAsocPerfilUsuario(AsocUsuarioPerfil model)
        {
            _context.Entry(model).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void DeletePerfilesUser(int UsuarioID)
        {
            List<int> lista = _context.AsocUsuarioPerfil.AsNoTracking()
                .Where(x => x.UsuarioID == UsuarioID)
                .Select(x => x.AsocUsuarioPerfilID).ToList();

            foreach (var item in lista)
            {
                var x = _context.AsocUsuarioPerfil.Find(item);
                _context.Entry(x).State = EntityState.Deleted;
            }

            _context.SaveChanges();
        }

        public void DeleteUser(int UsuarioID)
        {
            var x = _context.Usuario.Find(UsuarioID);

            _context.Entry(x).State = EntityState.Modified;
            _context.SaveChanges();
        }

        //OLD

        public IList<AsocUsuarioPerfil> GetAsocUsuarioPerfil()
        {
            return _context.AsocUsuarioPerfil.ToList();
        }
        
        public IList<Perfil> PerfilesFuncionario(int FuncionarioID)
        {
            IList<AsocUsuarioPerfil> asoc = _context
                .AsocUsuarioPerfil
                .Include("Perfil")
                .Where(x => x.UsuarioID == FuncionarioID).ToList();

            //AsocUnidadTipoSolicitud

            IList<Perfil> lista = new List<Perfil>();

            foreach (var item in asoc)
            {
                bool existe = lista.Any(x => x.PerfilID == item.PerfilID);
                if (!existe)
                {
                    item.Perfil.AsocUsuarioPerfil = _context
                        .AsocUsuarioPerfil
                        .Where(x => x.PerfilID == item.PerfilID && x.UsuarioID == item.UsuarioID).ToList();

                    lista.Add(item.Perfil);
                }
            }

            return lista;
        }
        #endregion

        #region Tipo Notificacion

        public TipoNotificacion GetTipoNotificacionByID(int TipoNotificacionID)
        {
            return _context.TipoNotificacion.Include("AsocTipoNotificacionPerfil").AsNoTracking().SingleOrDefault(i => i.TipoNotificacionID == TipoNotificacionID);
        }

        public IList<TipoNotificacion> GetTipoNotificacion()
        {
            return _context.TipoNotificacion.ToList();
        }

        public int SaveTipoNotificacion(TipoNotificacion model)
        {
            if (model.TipoNotificacionID != 0)
            {
                _context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(model).State = EntityState.Added;
            }

            try
            {
                _context.SaveChanges();

                return model.TipoNotificacionID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }

        public void SaveAsocTipoNotificacionPerfil(AsocTipoNotificacionPerfil model)
        {
            _context.Entry(model).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void DeleteAsocTipoNotificacionPerfil(int TipoNotificacionID)
        {
            List<int> lista = _context.AsocTipoNotificacionPerfil.AsNoTracking()
                .Where(x => x.TipoNotificacionID == TipoNotificacionID)
                .Select(x => x.AsocTipoNotificacionPerfilID).ToList();

            foreach (var item in lista)
            {
                var x = _context.AsocTipoNotificacionPerfil.Find(item);
                _context.Entry(x).State = EntityState.Deleted;
            }

            _context.SaveChanges();
        }

        #endregion

        #region Feriados
        public IList<Feriado> GetAllFeriados()
        {
            return _context.Feriado.AsNoTracking().ToList();
        }
        public int SaveFeriado(Feriado model)
        {
            if (model.FeriadoID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();

                return model.FeriadoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void DeleteFeriado(int FeriadoID)
        {
            Feriado model = _context.Feriado.Where(x => x.FeriadoID == FeriadoID).FirstOrDefault();
            _context.Entry(model).State = EntityState.Deleted;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Infrastructure.Logging.Logger.Execute().Error(ex);
            }
        }

        #endregion


        private static string EscribirLog(DbEntityValidationException ex)
        {

            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


            StringBuilder sb = new StringBuilder();

            foreach (var eve in ex.EntityValidationErrors)
            {
                sb.Append(string.Format("Entidad {0} en estado {1} tiene errores de validacion:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                sb.AppendLine();
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.Append(string.Format("--> Propiedad: {0}, Error: {1}", ve.PropertyName, ve.ErrorMessage));
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            Logger.Execute().Error(ex);
            Logger.Execute().Info(sb.ToString());

            return exceptionMessage;

        }



        public IList<TipoContencioso> GetTipoContencioso()
        {
            return _context.TipoContencioso.ToList();
        }

        public IList<TipoCausa> GetTipoCausa()
        {
            return _context.TipoCausa.ToList();
        }

        public IList<EstadoCausa> GetEstadoCausa()
        {
            return _context.EstadoCausa.ToList();
        }

        public IList<OpcionesTramite> GetOpcionesTramite()
        {
            return _context.OpcionesTramite.ToList();
        }

        public IList<TipoParte> GetTipoParte()
        {
            return _context.TipoParte.ToList();
        }

        public IList<TipoTramite> GetTipoTramite()
        {
            return _context.TipoTramite.ToList();
        }
        public IList<Pais> GetPais()
        {
            return _context.Pais.OrderBy(x => x.Descripcion).ToList();
        }

        public IList<EstadoTabla> GetEstadoTabla()
        {
            return _context.EstadoTabla.OrderBy(x => x.Descripcion).ToList();
        }
        public IList<TipoTabla> GetTipoTabla()
        {
            return _context.TipoTabla.OrderBy(x => x.Descripcion).ToList();
        }
        public IList<Sala> GetSala()
        {
            return _context.Sala.OrderBy(x => x.Descripcion).ToList();

        }

        public void DeleteDocumentosTmp(DateTime fecha)
        {
            DateTime fechaBorrar = fecha.AddDays(-1);

            List<int> lista = _context.DocumentoTmp.AsNoTracking()
                .Where(x => x.Fecha < fechaBorrar)
                .Select(x => x.DocumentoTmpID).ToList();

            foreach (var item in lista)
            {
                var x = _context.DocumentoTmp.Find(item);
                _context.Entry(x).State = EntityState.Deleted;
            }

            _context.SaveChanges();
        }

        public int SaveDocumentoTmp(DocumentoTmp model)
        {
            if (model.DocumentoTmpID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();

                return model.DocumentoTmpID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public DocumentoTmp GetDocumentoTmp(int documentoTmpID)
        {
            return _context.DocumentoTmp.Include("VersionEncript").AsNoTracking().FirstOrDefault(x => x.DocumentoTmpID == documentoTmpID);
        }

        public void DeleteDocumentoTmpByID(int documentoTmpID)
        {
            DocumentoTmp model = _context.DocumentoTmp.Find(documentoTmpID);
            _context.Entry(model).State = EntityState.Deleted;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }
        }

        public void DeleteDocumentoTmp(int documentoID)
        {
            DocumentoTmp model = _context.DocumentoTmp.Find(documentoID);
            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void DeleteDocumentoCausa(int documentoID)
        {
            DocumentoCausa model = _context
                .DocumentoCausa
                .Include("AsocCausaDocumento")
                .SingleOrDefault(x => x.DocumentoCausaID == documentoID);

            if (model.AsocCausaDocumento.Count > 0)
            {
                List<int> asocs = model.AsocCausaDocumento.Select(x => x.AsocCausaDocumentoID).ToList();

                foreach (var x in asocs)
                {
                    AsocCausaDocumento d = _context.AsocCausaDocumento.Find(x);
                    _context.Entry(d).State = EntityState.Deleted;
                }
            }

            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public ConfTipoCausa GetConfTipoCausa(int tipoCausaID)
        {
            return _context.ConfTipoCausa.FirstOrDefault(x => x.TipoCausaID == tipoCausaID);
        }


        public IList<AsocTipoTramiteOpciones> GetAsocTipoTramiteOpciones(int TipoTramiteID)
        {
            return _context.AsocTipoTramiteOpciones.Where(x=> x.TipoTramiteID == TipoTramiteID).ToList();
        }

        public int SaveAsocTipoTramiteOpciones(AsocTipoTramiteOpciones model)
        {
            if (model.AsocTipoTramiteOpcionesID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
                return model.AsocTipoTramiteOpcionesID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void DeleteAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID)
        {
            AsocTipoTramiteOpciones model = _context
                .AsocTipoTramiteOpciones.FirstOrDefault(x => x.AsocTipoTramiteOpcionesID == AsocTipoTramiteOpcionesID);

            model.Vigente = false;
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public IList<EstadosAplica> GetEstadosAplicaByAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID)
        {
            return _context.EstadosAplica.Where(x => x.AsocTipoTramiteOpcionesID == AsocTipoTramiteOpcionesID).ToList();
        }

        public int SaveEstadosAplica(EstadosAplica model)
        {
            if (model.EstadosAplicaID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
                return model.EstadosAplicaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void DeleteEstadosAplica(int EstadosAplicaID)
        {
            EstadosAplica model = _context
                .EstadosAplica.FirstOrDefault(x => x.EstadosAplicaID == EstadosAplicaID);
            
            _context.Entry(model).State = EntityState.Deleted;

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveLogSistema(LogSistema model)
        {
            if (model.LogSistemaID == 0)
            {
                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
                return model.LogSistemaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<TipoGenero> GetTipoGenero()
        {
            return _context.TipoGenero.ToList();
        }

        public Usuario GetUsuarioByID(int usuarioID)
        {
            return _context.Usuario
                .Include("AsocUsuarioPerfil")
                .Include("TipoGenero")
                .FirstOrDefault(x => x.UsuarioID == usuarioID);
        }

        public IList<Perfil> GetPerfilUsuario(int usuarioID)
        {
            IList<Perfil> lista = new List<Perfil>();

            var asoc = _context.AsocUsuarioPerfil.Include("Perfil").Where(x => x.UsuarioID == usuarioID).ToList();
            if (asoc.Count > 0)
            {
                lista = asoc.Select(x => x.Perfil).ToList();
            }

            return lista;
        }
    }
}
        
