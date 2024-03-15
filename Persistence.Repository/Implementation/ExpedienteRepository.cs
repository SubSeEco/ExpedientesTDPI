using Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ExpedienteRepository : IExpedienteRepository
    {
        private readonly SGDE2Entities _context = null;

        public ExpedienteRepository()
        {
            _context = new SGDE2Entities();
        }

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


        public IList<SP_Causas_Result> GetSP_Causas(FiltrosEscritorio filtros)
        {
            return _context.SP_Causas(
                filtros.UsuarioID,
                filtros.NumeroTicket,
                filtros.Anio,
                filtros.TipoCausaID,
                filtros.NumeroRegistro,
                filtros.Denominacion,
                filtros.Apelante,
                filtros.FechaIngreso,
                filtros.Apelado,
                filtros.EstadoCausaID,
                filtros.NumeroSolicitud, 
                filtros.IsSoloEscritos, 
                filtros.IsSoloMisEscritos).ToList();
        }

        public Causa GetCausa(int causaID)
        {
            return _context
                .Causa
                .Include("TipoCausa")
                .Include("TipoContencioso")
                .Include("Usuario")
                .Include("EstadoCausa")
                .Include("TipoCanal")
                .FirstOrDefault(x => x.CausaID == causaID);
        }

        public IList<Expediente> GetExpedienteByCausa(int causaID)
        {
            return _context
                .Expediente
                .Include("TipoTramite.AsocTipoTramiteOpciones")
                .Include("Usuario.AsocUsuarioPerfil.Perfil")
                .Include("AsocEscritoDocto")
                .Include("AsocExpedienteOpcion.OpcionesTramite")
                .Include("AsocExpeFirma.Firma.AsocFirmaDocto")  //econtreras - 0000316: 05/01 - Estado Diario
                .Where(x => x.CausaID == causaID).ToList();
        }

        public IList<Parte> GetParteByCausa(int causaID)
        {
            return _context
                .Parte
                .Include("Pais")
                .Include("TipoParte")
                .Where(x => x.CausaID == causaID).ToList();
        }

        public Parte GetParte(int parteID)
        {
            return _context
                .Parte
                .Include("Pais")
                .Include("TipoParte")
                .FirstOrDefault(x => x.ParteID == parteID);
        }

        public int SaveParte(Parte model)
        {
            if (model.ParteID == 0)
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
                return model.ParteID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        public int SaveLogCausa(LogCausa model)
        {
            if (model.LogCausaID == 0)
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
                return model.LogCausaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<LogCausa> GetLogCausa(int CausaID)
        {
            return _context.LogCausa.AsNoTracking().Where(x => x.CausaID == CausaID).ToList();
        }

        public int SaveDocumentoCausa(DocumentoCausa model)
        {
            if (model.DocumentoCausaID == 0)
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
                return model.DocumentoCausaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocCausaDocumento(AsocCausaDocumento model)
        {
            if (model.AsocCausaDocumentoID == 0)
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
                return model.AsocCausaDocumentoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocEscritoDocto(AsocEscritoDocto model)
        {
            if (model.AsocEscritoDoctoID == 0)
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
                return model.AsocEscritoDoctoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocExpedienteOpcion(AsocExpedienteOpcion model)
        {
            if (model.AsocExpedienteOpcionID == 0)
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
                return model.AsocExpedienteOpcionID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<DocumentoCausa> GetDocumentoCausa(int causaID, Domain.Infrastructure.TipoDocumento tipoDoc)
        {
            if (tipoDoc == Domain.Infrastructure.TipoDocumento.Causa || 
                tipoDoc == Domain.Infrastructure.TipoDocumento.ExpedienteElectronicoPDF)
            {
                IList<DocumentoCausa> lista = new List<DocumentoCausa>();

                var q = _context.DocumentoCausa.Include("VersionEncript").Where(x => x.CausaID == causaID).ToList();
                foreach (var item in q)
                {
                    var asoc = _context
                        .AsocCausaDocumento
                        .Include("DocumentoAdjunto.AsocTipoDocumentoAdjunto.TipoDocumento")
                        .FirstOrDefault(x => x.DocumentoCausaID == item.DocumentoCausaID);

                    if (asoc != null)
                    {
                        var asocTD = asoc.DocumentoAdjunto.AsocTipoDocumentoAdjunto.FirstOrDefault();
                        if (asocTD != null)
                        {
                            if (asocTD.TipoDocumentoID == (int)tipoDoc)
                            {
                                lista.Add(item);
                            }
                        }
                    }
                }

                return lista;
            }
            else
            {
                return _context
                    .DocumentoCausa
                    .Include("VersionEncript")
                    .Where(x => x.CausaID == causaID).ToList();
            }


        }

        public void UpdateCausa(Causa model)
        {
            Causa causa = _context.Causa.Find(model.CausaID);
            causa.Denominacion = model.Denominacion;
            causa.Observacion = model.Observacion;
            causa.IsContencioso = model.IsContencioso;
            causa.NumeroRegistro = model.NumeroRegistro;

            causa.TipoContenciosoID = model.TipoContenciosoID;
            causa.Numero = model.Numero;

            _context.Entry(causa).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Folio GetFolio(int anio)
        {
            bool existe = _context.Folio.Any(x => x.Anio == anio);
            if (existe)
            {
                Folio folio = _context.Folio.FirstOrDefault(x => x.Anio == anio);
                folio.Correlativo = folio.Correlativo + 1;

                _context.Entry(folio).State = EntityState.Modified;
                _context.SaveChanges();

                return folio;
            }
            else
            {
                Folio folio = new Folio();
                folio.FolioID = 0;
                folio.Anio = anio;
                folio.Correlativo = 1;

                _context.Entry(folio).State = EntityState.Added;
                _context.SaveChanges();

                return folio;
            }


        }

        public IList<Causa> GetCausasByFilter(FiltrosEscritorio filtros, Domain.Infrastructure.TipoGrid tipo)
        {
            List<Causa> lista = new List<Causa>();

            if (tipo == Domain.Infrastructure.TipoGrid.ListadoIngresos)
            {
                DateTime Desde = Convert.ToDateTime(filtros.FechaDesde).Date;
                DateTime Hasta = Convert.ToDateTime(filtros.FechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);

                lista = _context.Causa.Where(x =>
                        x.EstadoCausaID == (int)Domain.Infrastructure.EstadoCausa.PreIngresado &&
                        x.FechaIngreso >= Desde && x.FechaIngreso <= Hasta)
                        .Include("EstadoCausa")
                        .ToList();
            }

            if (tipo == Domain.Infrastructure.TipoGrid.PDFListadoIngresos)
            {
                lista = _context.Causa.Where(x => filtros.ListaID.Contains(x.CausaID)).Include("EstadoCausa").ToList();
            }

            if (tipo == Domain.Infrastructure.TipoGrid.ListadoTablas)
            {
                List<int> estadosFilter = new List<int>();
                estadosFilter.Add((int)Domain.Infrastructure.EstadoCausa.AutosEnRelacion);
                estadosFilter.Add((int)Domain.Infrastructure.EstadoCausa.DeseCuenta);
                estadosFilter.Add((int)Domain.Infrastructure.EstadoCausa.EnTabla);

                lista = (from a in _context.Causa
                           where a.CausaID > 0
                           && (estadosFilter.Contains(a.EstadoCausaID))
                           && ((string.IsNullOrEmpty(filtros.NumeroTicket)) || (!string.IsNullOrEmpty(filtros.NumeroTicket) && a.NumeroTicket == filtros.NumeroTicket))
                           && ((filtros.Anio == 0) || (filtros.Anio != 0 && a.Anio == filtros.Anio))
                           select a)
                           .Include("EstadoCausa")
                           .ToList();
            }

            if (tipo == Domain.Infrastructure.TipoGrid.ListadoEstadoDiario)
            {
                DateTime fecha = Convert.ToDateTime(filtros.FechaDesde).Date;

                var q = (from c in _context.Causa
                         where (c.EstadoCausaID > 0)
                         //&& (c.FechaIngreso.Year >= fecha.Year && c.FechaIngreso.Month >= fecha.Month && c.FechaIngreso.Day >= fecha.Day)
                         && (c.Expediente.Any(x => x.TipoTramiteID == (int)Domain.Infrastructure.TipoTramite.Resolucion
                                                && x.FechaIngreso.Year >= fecha.Year && x.FechaIngreso.Month >= fecha.Month && x.FechaIngreso.Day >= fecha.Day
                                                && x.IsAdmisible && x.IsFinalizado))
                         select c
                         ).Include("Expediente").Include("EstadoCausa").ToList();

                lista = (from a in q
                         where (a.CausaID > 0)
                         && ((string.IsNullOrEmpty(filtros.NumeroTicket)) || (!string.IsNullOrEmpty(filtros.NumeroTicket) && a.NumeroTicket == filtros.NumeroTicket))
                         && ((filtros.Anio == 0) || (filtros.Anio != 0 && a.Anio == filtros.Anio))
                         select a).ToList();

                //lista = (from a in _context.Causa
                //         where a.CausaID > 0
                //         //&& (estadosFilter.Contains(a.EstadoCausaID))
                //         && ((string.IsNullOrEmpty(filtros.NumeroTicket)) || (!string.IsNullOrEmpty(filtros.NumeroTicket) && a.NumeroTicket == filtros.NumeroTicket))
                //         && ((filtros.Anio == 0) || (filtros.Anio != 0 && a.Anio == filtros.Anio))
                //         select a)
                //           .Include("EstadoCausa")
                //           .ToList();
            }

            return lista;
        }

        public int SaveDocumentoSistema(DocumentoSistema model)
        {
            if (model.DocumentoSistemaID == 0)
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
                return model.DocumentoSistemaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public DocumentoSistema GetDocumentoSistemaByHash(string hash)
        {
            return _context.DocumentoSistema.Include("VersionEncript").FirstOrDefault(x => x.Hash.Trim() == hash.Trim());
        }

        public void CambiarEstadoCausa(int causaID, Domain.Infrastructure.EstadoCausa estado)
        {
            Causa model = _context.Causa.Find(causaID);
            model.EstadoCausaID = (int)estado;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int SaveExpediente(Expediente model)
        {
            if (model.ExpedienteID == 0)
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
                return model.ExpedienteID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public Expediente GetExpediente(int expedienteID)
        {
            return _context
                .Expediente
                .Include("AsocExpedienteOpcion.OpcionesTramite")
                .Include("TipoTramite.AsocTipoTramiteOpciones")
                .Include("AsocExpeFirma.Firma.AsocFirmaDocto").AsNoTracking()
                .FirstOrDefault(x => x.ExpedienteID == expedienteID);
        }

        public IList<Firma> GetFirmaByExpedienteID(int expedienteID)
        {
            Expediente _exp = _context.Expediente.Include("AsocExpeFirma").AsNoTracking().FirstOrDefault(x=> x.ExpedienteID == expedienteID);

            List<int> asocs = new List<int>();
            foreach (var item in _exp.AsocExpeFirma)
            {
                asocs.Add(item.FirmaID);
            }

            return _context
                .Firma
                .Include("Usuario")
                .Include("AsocExpeFirma.Expediente")
                .Include("AsocFirmaDocto").AsNoTracking()
                .Where(x => asocs.Contains(x.FirmaID))
                .ToList();
        }

        public IList<Firma> GetFirmasByUsuarioID(int UsuarioID)
        {
            return _context
                .Firma
                .Include("Usuario")
                .Include("AsocExpeFirma.Expediente")
                .Include("AsocFirmaDocto.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa")
                .Include("AsocDocSistemaFirma.DocumentoSistema.TipoDocumento")
                .AsNoTracking()
                .Where(x => x.UsuarioID == UsuarioID).ToList();
        }

        public IList<Firma> GetEscritorioFirmas(FiltrosEscritorio filtros)
        {
            DateTime Desde = Convert.ToDateTime(filtros.FechaDesde).Date;
            DateTime Hasta = Convert.ToDateTime(filtros.FechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            var q = (from f in _context.Firma
                       where f.UsuarioID == filtros.UsuarioID
                        && (f.AsocExpeFirma.Any(z => z.Expediente.FechaIngreso >= Desde && z.Expediente.FechaIngreso <= Hasta)
                            || f.AsocDocSistemaFirma.Any(z => z.DocumentoSistema.Fecha >= Desde && z.DocumentoSistema.Fecha <= Hasta))
                         && ((filtros.TipoDocumentoID == 0)
                              || (filtros.TipoDocumentoID == (int)Domain.Infrastructure.TipoDocumento.Expediente && f.AsocFirmaDocto.Count > 0)
                              || (filtros.TipoDocumentoID != (int)Domain.Infrastructure.TipoDocumento.Expediente && f.AsocDocSistemaFirma.Any(z => z.DocumentoSistema.TipoDocumentoID == filtros.TipoDocumentoID)))
                         //&& ((filtros.EstadoFirma == (int)Domain.Infrastructure.EstadosDoctoFirma.VerTodas) // Revisar Filtro, no está aplicando para los documentos de expediente
                         //     || (filtros.EstadoFirma == (int)Domain.Infrastructure.EstadosDoctoFirma.SoloPendientes && (f.AsocFirmaDocto.Any(z => !z.IsFirmado && z.Firma.UsuarioID == filtros.UsuarioID) || f.AsocDocSistemaFirma.Any(z => !z.IsFirmado && z.Firma.UsuarioID == filtros.UsuarioID)))
                         //     || (filtros.EstadoFirma == (int)Domain.Infrastructure.EstadosDoctoFirma.Firmadas && (f.AsocFirmaDocto.Any(z => z.IsFirmado && z.Firma.UsuarioID == filtros.UsuarioID) || f.AsocDocSistemaFirma.Any(z => z.IsFirmado && z.Firma.UsuarioID == filtros.UsuarioID))))
                         && ((string.IsNullOrEmpty(filtros.NumeroTicket)) || (!string.IsNullOrEmpty(filtros.NumeroTicket) && f.AsocFirmaDocto.Any(z => z.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.NumeroTicket.Trim() == filtros.NumeroTicket)))
                     select f)
                        .Include("Usuario")
                        .Include("AsocExpeFirma.Expediente")
                         .Include("AsocExpeFirma.Expediente.AsocExpedienteOpcion")
                        .Include("AsocFirmaDocto.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa")
                        .Include("AsocDocSistemaFirma.DocumentoSistema.TipoDocumento")
                        .ToList();

            return q;
        }

        public IList<SP_FlujoEstado_Result> GetFlujoEstado(int tipoTramiteID)
        {
            return _context.SP_FlujoEstado(tipoTramiteID).ToList();
        }

        public IList<EstadosAplica> GetEstadosAplica(int tipoTramiteID, int estadoCausaID)
        {
            return _context.EstadosAplica
                .Include("AsocTipoTramiteOpciones.OpcionesTramite")
                .Where(x => 
                x.EstadoCausaID == estadoCausaID && 
                x.AsocTipoTramiteOpciones.Vigente && 
                x.AsocTipoTramiteOpciones.TipoTramiteID == tipoTramiteID).ToList();
        }

        public IList<AsocEscritoDocto> GetAsocEscritoDocto(int expedienteID)
        {
            return _context
                .AsocEscritoDocto
                .Include("AsocCausaDocumento.DocumentoCausa.VersionEncript")
                .Where(x => x.ExpedienteID == expedienteID).ToList();
        }

        public void BorrarFirmasExpediente(int expedienteID)
        {
            var lista = _context.AsocExpeFirma.AsNoTracking().Where(x => x.ExpedienteID == expedienteID).ToList();
            if (lista.Count > 0)
            {
                List<int> d = lista.Select(s => s.FirmaID).ToList();
                foreach (var item in d)
                {
                    IList<AsocFirmaDocto> asocFirmaList = _context.AsocFirmaDocto.AsNoTracking().Where(x => x.FirmaID == item).ToList();
                    foreach (var asoc in asocFirmaList)
                    {
                        _context.Entry(asoc).State = EntityState.Deleted;
                    }

                    IList<AsocExpeFirma> asocExpeList = _context.AsocExpeFirma.AsNoTracking().Where(x => x.FirmaID == item).ToList();
                    foreach (var asoc in asocExpeList)
                    {
                        _context.Entry(asoc).State = EntityState.Deleted;
                    }

                    Firma firma = _context.Firma.Find(item);
                    _context.Entry(firma).State = EntityState.Deleted;
                    _context.SaveChanges();
                }
            }
        }

        public void BorrarFirmaByFirmaID(int FirmaID)
        {
            var lista = _context.AsocExpeFirma.AsNoTracking().Where(x => x.FirmaID == FirmaID).ToList();
            if (lista.Count > 0)
            {
                List<int> d = lista.Select(s => s.FirmaID).ToList();
                foreach (var item in d)
                {
                    IList<AsocFirmaDocto> asocFirmaList = _context.AsocFirmaDocto.AsNoTracking().Where(x => x.FirmaID == item).ToList();
                    foreach (var asoc in asocFirmaList)
                    {
                        _context.Entry(asoc).State = EntityState.Deleted;
                    }

                    IList<AsocExpeFirma> asocExpeList = _context.AsocExpeFirma.AsNoTracking().Where(x => x.FirmaID == item).ToList();
                    foreach (var asoc in asocExpeList)
                    {
                        _context.Entry(asoc).State = EntityState.Deleted;
                    }

                    Firma firma = _context.Firma.Find(item);
                    _context.Entry(firma).State = EntityState.Deleted;
                    _context.SaveChanges();
                }
            }
        }

        public void BorrarFirmaByAsocDocSistema(int AsocDocSistemaFirmaID)
        {
            var model = _context.AsocDocSistemaFirma.AsNoTracking().FirstOrDefault(x => x.AsocDocSistemaFirmaID == AsocDocSistemaFirmaID);
            if (model != null)
            {
                _context.Entry(model).State = EntityState.Deleted;                    

                Firma firma = _context.Firma.Find(model.FirmaID);
                _context.Entry(firma).State = EntityState.Deleted;
                _context.SaveChanges();                
            }
        }

        public int SaveFirma(Firma model)
        {
            if (model.FirmaID == 0)
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
                return model.FirmaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocFirmaDocto(AsocFirmaDocto model)
        {
            if (model.AsocFirmaDoctoID == 0)
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
                return model.AsocFirmaDoctoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocFirmaDoctoMarcarToma(AsocFirmaDocto model)
        {
            if (model.AsocFirmaDoctoID == 0)
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
                return model.AsocFirmaDoctoID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public AsocFirmaDocto GetAsocFirmaDoctoGS(int expedienteID, int UsuarioID)
        {
            return _context.AsocFirmaDocto
                  .Include("AsocEscritoDocto")
                 .Include("Firma.Usuario")
                 //  .Include("AsocEscritoDocto")
                 .AsNoTracking()
                 .FirstOrDefault(x => x.Firma.UsuarioID == UsuarioID && x.AsocEscritoDocto.ExpedienteID == expedienteID);
        }

        public IList<AsocFirmaDocto> GetListAsocFirmaDoctoGS(int expedienteID)
        {
            return _context.AsocFirmaDocto
                 .Include("AsocEscritoDocto")
                 .Include("Firma.Usuario")
                 // .Include("AsocEscritoDocto")
                 .AsNoTracking()
                 .Where(x => x.AsocEscritoDocto.ExpedienteID == expedienteID).ToList();
        }

        public AsocDocSistemaFirma GetAsocDocSistemaFirma(int FirmaID, int DocumentoSistemaID)
        {
            return _context.AsocDocSistemaFirma
                .Include("Firma")
                .Include("DocumentoSistema.VersionEncript")
                .AsNoTracking()
                .FirstOrDefault(x=> x.FirmaID == FirmaID && x.DocumentoSistemaID == DocumentoSistemaID);
        }

        public IList<AsocDocSistemaFirma> GetAsocDocSistemaFirmaByDocto(int DocumentoSistemaID)
        {
            return _context.AsocDocSistemaFirma.Include("Firma").AsNoTracking().Where(x => x.DocumentoSistemaID == DocumentoSistemaID).ToList();
        }

        public int SaveAsocDocSistemaFirma(AsocDocSistemaFirma model)
        {
            if (model.AsocDocSistemaFirmaID == 0)
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
                return model.AsocDocSistemaFirmaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveCausa(Causa model)
        {
            _context.Entry(model).State = EntityState.Added;

            try
            {
                _context.SaveChanges();
                return model.CausaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void DeleteParteByID(int parteID)
        {
            Parte model = _context.Parte.Find(parteID);
            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void SetEstadoTabla(int tablaID, Domain.Infrastructure.EstadoTabla estado)
        {
            Tabla model = _context.Tabla.Find(tablaID);
            model.EstadoTablaID = (int)estado;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SetVigenciaDetalleTabla(int detalleTablaID, bool vigencia)
        {
            DetalleTabla model = _context.DetalleTabla.Find(detalleTablaID);
            model.Vigente = vigencia;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int SaveTabla(Tabla model)
        {
            if (model.TablaID == 0)
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
                return model.TablaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public Tabla GetTablaByID(int tablaID)
        {
            return _context
                .Tabla
                .Include("Sala")
                .Include("TipoTabla")
                .Include("EstadoTabla")
                .Include("DetalleTabla").AsNoTracking()
                .FirstOrDefault(x => x.TablaID == tablaID);
        }

        public IList<Tabla> GetTabla(FiltrosEscritorio filtros)
        {
            DateTime Desde = Convert.ToDateTime(filtros.FechaDesde).Date;
            DateTime Hasta = Convert.ToDateTime(filtros.FechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);

            int CausaID = 0;

            if (!string.IsNullOrEmpty(filtros.NumeroTicket))
            {
                Desde = new DateTime(Domain.Infrastructure.WebConfigValues.AnioInicial, 1, 1).Date;
                var Causa = _context.Causa.FirstOrDefault(x => x.NumeroTicket == filtros.NumeroTicket);
                if (Causa != null)
                {
                    CausaID = Causa.CausaID;
                }
            }

            var qry = (from a in _context.Tabla
                       where a.TablaID > 0
                       && ((filtros.SalaID == 0) || (filtros.SalaID != 0 && a.SalaID == filtros.SalaID))
                       && ((filtros.EstadoTablaID == 0) || (filtros.EstadoTablaID != 0 && a.EstadoTablaID == filtros.EstadoTablaID))
                       && ((filtros.UsuarioRelatorID <= 0) || (filtros.UsuarioRelatorID > 0 && a.UsuarioRelatorID == filtros.UsuarioRelatorID))
                       && (a.Fecha >= Desde && a.Fecha <= Hasta)
                       && ((CausaID == 0) || (CausaID != 0 && a.DetalleTabla.Any(x => x.CausaID == CausaID)))
                       select a)
                       .Include("Sala")
                       .Include("TipoTabla")
                       .Include("EstadoTabla")
                       .ToList();

            return qry;
        }

        public bool GetExisteTabla(DateTime fecha)
        {
            return _context.Tabla.Any(x =>
            x.Fecha.Year == fecha.Year &&
            x.Fecha.Month == fecha.Month &&
            x.Fecha.Day == fecha.Day &&
            x.EstadoTablaID != (int)Domain.Infrastructure.EstadoTabla.Eliminado);
        }

        public IList<DetalleTabla> GetDetalleTablaByCausa(int causaID)
        {
            return _context.DetalleTabla.Where(x => x.CausaID == causaID && x.Vigente).ToList();
        }

        public int SaveDetalleTabla(DetalleTabla model, bool SetLastOrder)
        {
            if (model.DetalleTablaID == 0)
            {
                if (SetLastOrder)
                {
                    int num = _context.DetalleTabla.Count(x => x.TablaID == model.TablaID);
                    model.Orden = num + 1;
                }

                _context.Entry(model).State = EntityState.Added;
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                _context.SaveChanges();
                return model.DetalleTablaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int SaveAsocDocumentoSistemaTabla(AsocDocumentoSistemaTabla model)
        {
            if (model.AsocDocumentoSistemaTablaID == 0)
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
                return model.AsocDocumentoSistemaTablaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<AsocDocumentoSistemaTabla> GetAsocDocumentoSistemaTabla(int TablaID)
        {
            return _context.AsocDocumentoSistemaTabla.Where(x => x.TablaID == TablaID).ToList();
        }

        public IList<AsocDocumentoSistemaTabla> GetAsocDocumentoSistemaTablaByDocumentoSitemaID(int DocumentoSistemaID)
        {
            return _context.AsocDocumentoSistemaTabla.Where(x => x.DocumentoSistemaID == DocumentoSistemaID).ToList();
        }

        public IList<AsocDocumentoSistemaEstadoDiario> GetAsocDocumentoSistemaEstadoDiario(int EstadoDiarioID)
        {
            return _context.AsocDocumentoSistemaEstadoDiario.Where(x => x.EstadoDiarioID == EstadoDiarioID).ToList();
        }

        public IList<AsocDocumentoSistemaEstadoDiario> GetAsocDocumentoSistemaEstadoDiarioByDocumentoSitemaID(int DocumentoSistemaID)
        {
            return _context.AsocDocumentoSistemaEstadoDiario.Where(x => x.DocumentoSistemaID == DocumentoSistemaID).ToList();
        }

        public int SaveAsocDocumentoSistemaEstadoDiario(AsocDocumentoSistemaEstadoDiario model)
        {
            if (model.AsocDocumentoSistemaEstadoDiarioID == 0)
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
                return model.AsocDocumentoSistemaEstadoDiarioID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<DocumentoSistema> GetAsocDocumentoSistema(int identidad, Domain.Infrastructure.TipoDocumento tipoDoc)
        {
            List<DocumentoSistema> lista = new List<DocumentoSistema>();

            if (tipoDoc == Domain.Infrastructure.TipoDocumento.Tabla)
            {
                var asoc = _context.AsocDocumentoSistemaTabla.Include("DocumentoSistema.VersionEncript").Include("DocumentoSistema.AsocDocSistemaFirma.Firma")
                    .Where(x => x.TablaID == identidad).ToList();
                foreach (var item in asoc)
                {
                    lista.Add(item.DocumentoSistema);
                }
            }

            if (tipoDoc == Domain.Infrastructure.TipoDocumento.EstadoDiario)
            {
                var asoc = _context.AsocDocumentoSistemaEstadoDiario.Include("DocumentoSistema.VersionEncript").Include("DocumentoSistema.AsocDocSistemaFirma.Firma")
                    .Where(x => x.EstadoDiarioID == identidad).ToList();

                foreach (var item in asoc)
                {
                    lista.Add(item.DocumentoSistema);
                }
            }

            return lista;
        }

        public DocumentoSistema GetDocumentoSistema(int DocumentoSistemaID)
        {
            return _context.DocumentoSistema.Include("AsocDocSistemaFirma.Firma").AsNoTracking().FirstOrDefault(x=> x.DocumentoSistemaID == DocumentoSistemaID);
        }


        public int SaveEstadoDiario(EstadoDiario model)
        {
            if (model.EstadoDiarioID == 0)
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
                return model.EstadoDiarioID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public EstadoDiario GetEstadoDiarioByID(int estadoDiarioID)
        {
            return _context
                .EstadoDiario
                .Include("TipoEstadoDiario")
                .Include("DetalleEstadoDiario")
                .FirstOrDefault(x => x.EstadoDiarioID == estadoDiarioID);
        }

        public IList<EstadoDiario> GetEstadoDiario(FiltrosEscritorio filtros)
        {
            DateTime Desde = Convert.ToDateTime(filtros.FechaDesde).Date;
            DateTime Hasta = Convert.ToDateTime(filtros.FechaHasta).AddHours(23).AddMinutes(59).AddSeconds(59);


            var qry = (from a in _context.EstadoDiario
                       where a.EstadoDiarioID > 0
                       && (a.Fecha >= Desde && a.Fecha <= Hasta)
                       select a)
                       .AsNoTracking()
                       .Include("TipoEstadoDiario")
                       .ToList();

            return qry;
        }

        public void SetTipoEstadoDiario(int estadoDiarioID, Domain.Infrastructure.TipoEstadoDiario estado)
        {
            EstadoDiario model = _context.EstadoDiario.Find(estadoDiarioID);
            model.TipoEstadoDiarioID = (int)estado;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SetVigenciaDetalleEstadoDiario(int detalleEstadoDiarioID, bool vigencia)
        {
            DetalleEstadoDiario model = _context.DetalleEstadoDiario.Find(detalleEstadoDiarioID);
            model.Vigente = vigencia;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int SaveDetalleEstadoDiario(DetalleEstadoDiario model)
        {
            if (model.DetalleEstadoDiarioID == 0)
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
                return model.DetalleEstadoDiarioID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public IList<DetalleEstadoDiario> GetDetalleEstadoDiarioByExpediente(int expedienteID)
        {
            return _context.DetalleEstadoDiario.Where(x => x.ExpedienteID == expedienteID && x.Vigente).ToList();
        }

        public void SetExpedienteFinalizado(int expedienteID, bool finalizar)
        {
            Expediente model = _context.Expediente.Find(expedienteID);
            model.IsFinalizado = finalizar;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void SetExpedienteInadmisible(int expedienteID)
        {
            Expediente model = _context.Expediente.Find(expedienteID);
            model.IsFinalizado = true;
            model.IsAdmisible = false;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IList<AsocExpeFirma> GetAsocExpeFirmaByExpedienteID(int expedienteID)
        {
            return _context.AsocExpeFirma
                .Include("Firma.Usuario")
                .Include("Firma.AsocFirmaDocto")
                .Where(x => x.ExpedienteID == expedienteID).ToList();
        }

        public int SaveAsocExpeFirma(AsocExpeFirma model)
        {
            if (model.AsocExpeFirmaID == 0)
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
                return model.AsocExpeFirmaID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void UpdateResponsable(int expedienteID, int usuarioID)
        {
            Expediente model = _context.Expediente.Find(expedienteID);
            model.UsuarioResponsableID = usuarioID;

            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int SaveDerivacion(Derivacion model)
        {
            if (model.DerivacionID == 0)
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
                return model.DerivacionID;
            }
            catch (DbEntityValidationException ex)
            {
                string exceptionMessage = EscribirLog(ex);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }

        public IList<SP_Alarmas_Result> GetAlarmasInternasService(int UsuarioID)
        {
            return _context.SP_Alarmas(UsuarioID).ToList();
        }



        private bool IsFecha(string texto)
        {
            DateTime valor;
            return (DateTime.TryParse(texto, out valor));
        }


        public IList<Causa> WCF_ObtenerExpediente(int expediente_tipo, string fecha_ingreso, int rolCorr,
            int rolAnio, string numero_solicitud, string numero_registro, string individualizacion,
            string nombre_solicitante, string nombre_apelante, string nombre_apelado, int paginaActual, int registrosPorPagina)
        {

            List<Causa> lista;

            string numeroTicket = "";
            if (rolCorr > 0)
            {
                if (rolAnio > 0)
                {
                    numeroTicket = string.Format("{0:000000}-{1}", rolCorr, rolAnio);
                }
                else
                {
                    numeroTicket = string.Format("{0:000000}", rolCorr);
                }
            }

            lista = (from c in _context.Causa
                     where c.CausaID > 0
                     && (c.EstadoCausaID != (int)Domain.Infrastructure.EstadoCausa.PreIngresado)
                     && ((expediente_tipo == 0) || (expediente_tipo != 0 && c.TipoCausaID == expediente_tipo))
                     && ((rolAnio == 0) || (rolAnio != 0 && c.Anio == rolAnio))
                     && ((string.IsNullOrEmpty(numeroTicket)) || (!string.IsNullOrEmpty(numeroTicket) && c.NumeroTicket.Contains(numeroTicket)))
                     && ((string.IsNullOrEmpty(numero_solicitud)) || (!string.IsNullOrEmpty(numero_solicitud) && c.Numero == numero_solicitud))
                     && ((string.IsNullOrEmpty(numero_registro)) || (!string.IsNullOrEmpty(numero_registro) && c.NumeroRegistro == numero_registro))
                     && ((string.IsNullOrEmpty(individualizacion)) || (!string.IsNullOrEmpty(individualizacion) && c.Denominacion.ToLower().Contains(individualizacion.ToLower())))
                     select c)
                     .Include("EstadoCausa")
                     .Include("TipoCausa")
                     .Include("Parte")
                     .Include("TipoContencioso")
                     .OrderByDescending(a => a.CausaID).ToList();

            if (!string.IsNullOrWhiteSpace(fecha_ingreso))
            {
                DateTime? fcing = null;
                if (!IsFecha(fcing.ToString()))
                {
                    fcing = Convert.ToDateTime(fecha_ingreso).Date;
                }

                if (fcing.HasValue)
                {
                    lista = lista.Where(e => e.FechaIngreso.Date == fcing.Value.Date).ToList();
                }
            }

            if (!string.IsNullOrEmpty(nombre_solicitante))
            {
                IList<Causa> filter = new List<Causa>();

                foreach (var item in lista)
                {
                    if (item.Parte.Any(x => x.TipoParteID == (int)Domain.Infrastructure.TipoParte.Solicitante 
                        && x.Nombre.ToLower().Contains(nombre_solicitante.ToLower())))
                    {
                        filter.Add(item);
                    }
                }

                lista = filter.ToList();
            }

            if (!string.IsNullOrEmpty(nombre_apelante))
            {
                IList<Causa> filter = new List<Causa>();

                foreach (var item in lista)
                {
                    if (item.Parte.Any(x => x.TipoParteID == (int)Domain.Infrastructure.TipoParte.Apelante
                        && x.Nombre.ToLower().Contains(nombre_apelante.ToLower())))
                    {
                        filter.Add(item);
                    }
                }

                lista = filter.ToList();
            }


            if (!string.IsNullOrEmpty(nombre_apelado))
            {
                IList<Causa> filter = new List<Causa>();

                foreach (var item in lista)
                {
                    if (item.Parte.Any(x => x.TipoParteID == (int)Domain.Infrastructure.TipoParte.Apelado
                        && x.Nombre.ToLower().Contains(nombre_apelado.ToLower())))
                    {
                        filter.Add(item);
                    }
                }

                lista = filter.ToList();
            }

            if (paginaActual != 0 && registrosPorPagina > 0)
            {
                return lista.Skip((paginaActual - 1) * registrosPorPagina).Take(registrosPorPagina).ToList();
            }
            else
            {
                return lista;
            }

            
        }

        public IList<Expediente> WCF_ObtenerEventosExpediente(int rol, int anio)
        {
            string numeroTicket = string.Format("{0:000000}-{1}", rol, anio);

            Causa causa = _context
                .Causa
                .Include("Expediente.TipoTramite")
                .Include("Expediente.AsocExpedienteOpcion.OpcionesTramite")
                .FirstOrDefault(x => x.Anio == anio && x.NumeroTicket == numeroTicket);

            if (causa != null)
            {
                return causa.Expediente.Where(x => x.IsFinalizado && x.IsAdmisible).OrderBy(i => i.FechaIngreso).ToList();
            }
            else
            {
                IList<Expediente> lista = new List<Expediente>();
                return lista;
            }
        }

        public IList<ItemDocumentoMigracion> GetItemsMigracion(string query)
        {
            var result = _context.Database.SqlQuery<ItemDocumentoMigracion>(query).ToList();

            return result;
        }

        public void UpdateRegistroMigrado(int TempID, bool IsMigrado, string Comentario)
        {
            int _migrado = IsMigrado ? 1 : 0;
            string query = string.Format("UPDATE TempMigraAdjuntos SET Migrado = {0}, Comentario = '{1}' where TempID = '{2}' ", _migrado, Comentario, TempID);
            _context.Database.ExecuteSqlCommand(query);
        }
    }
}
