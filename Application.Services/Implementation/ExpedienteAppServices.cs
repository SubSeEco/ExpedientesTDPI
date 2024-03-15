using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExpedienteAppServices : IExpedienteAppServices
    {
        private readonly IExpedienteRepository repo = new ExpedienteRepository();
        private readonly ICommonAppServices appCommon = new CommonAppServices();

        #region Private

        private string SiNoOrNull(bool? valor)
        {
            if (valor == null)
            {
                return "";
            }
            else
            {
                return (valor ?? false) ? "SI" : "NO";
            }
        }

        private string SiNo(int valor)
        {
            return (valor == 1) ? "SI" : "NO";
        }

        private FiltrosEscritorio MapFilter(DTO.FiltrosEscritorio filtros)
        {
            FiltrosEscritorio dto = new FiltrosEscritorio();
            dto.UsuarioID = filtros.UsuarioID;
            dto.NumeroTicket = filtros.NumeroTicket;
            dto.Anio = filtros.Anio;
            dto.TipoCausaID = filtros.TipoCausaID;
            dto.NumeroRegistro = filtros.NumeroRegistro;
            dto.Denominacion = filtros.Denominacion;
            dto.Apelante = filtros.Apelante;
            dto.FechaIngreso = filtros.FechaIngreso;
            dto.Apelado = filtros.Apelado;
            dto.EstadoCausaID = filtros.EstadoCausaID;
            dto.EstadoTablaID = filtros.EstadoTablaID;
            dto.NumeroSolicitud = filtros.NumeroSolicitud;
            dto.FechaDesde = filtros.FechaDesde;
            dto.FechaHasta = filtros.FechaHasta;
            dto.ListaID = filtros.ListaID;
            dto.UsuarioRelatorID = filtros.UsuarioRelatorID;
            dto.SalaID = filtros.SalaID;
            dto.TablaID = filtros.TablaID;
            dto.IsSoloEscritos = filtros.IsSoloEscritos;
            dto.IsSoloMisEscritos = filtros.IsSoloMisEscritos;

            //Firmas
            dto.TipoDocumentoID = filtros.TipoDocumentoID;
            dto.EstadoFirma = filtros.EstadoFirma;

            return dto;
        }

        #endregion

        public IList<DTO.Models.SP_Causas_Result> GetSP_Causas(DTO.FiltrosEscritorio filtros)
        {
            IList<SP_Causas_Result> repoList = repo.GetSP_Causas(MapFilter(filtros));

            IList<DTO.Models.SP_Causas_Result> listDTO = new List<DTO.Models.SP_Causas_Result>();

            foreach (var item in repoList)
            {
                DTO.Models.SP_Causas_Result dto = new DTO.Models.SP_Causas_Result();
                dto.CausaID = item.CausaID;
                dto.TipoCanalID = item.TipoCanalID;
                dto.TipoCanal = item.TipoCanal;
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.EstadoCausa = item.EstadoCausa.Trim();
                dto.UsuarioID = item.UsuarioID;
                dto.Usuario = item.Usuario;
                dto.UsuarioResponsableID = item.UsuarioResponsableID;
                dto.TipoContenciosoID = item.TipoContenciosoID;
                dto.TipoContencioso = item.TipoContencioso.Trim();
                dto.TipoCausaID = item.TipoCausaID;
                dto.TipoCausa = item.TipoCausa.Trim();
                dto.FechaIngreso = item.FechaIngreso.Trim();
                dto.NumeroTicket = item.NumeroTicket.Trim();
                dto.Numero = item.Numero.Trim();
                dto.Anio = item.Anio;
                dto.Denominacion = item.Denominacion.Trim();
                dto.Observacion = item.Observacion.Trim();
                dto.IsContencioso = item.IsContencioso;
                dto.NumeroRegistro = item.NumeroRegistro.Trim();
                dto.bt1 = item.bt1;
                dto.bt2 = item.bt2;
                dto.bt3 = item.bt3;
                dto.bt4 = item.bt4;
                dto.bt5 = item.bt5;
                dto.bt6 = item.bt6;

                listDTO.Add(dto);
            }

            return listDTO;
        }


        public DTO.Models.Causa GetCausa(int causaID)
        {
            Causa model = repo.GetCausa(causaID);

            DTO.Models.Causa dto = new DTO.Models.Causa();
            dto.CausaID = model.CausaID;
            dto.TipoCanalID = model.TipoCanalID;
            dto.EstadoCausaID = model.EstadoCausaID;
            dto.UsuarioID = model.UsuarioID;
            dto.UsuarioResponsableID = model.UsuarioResponsableID;
            dto.TipoContenciosoID = model.TipoContenciosoID;
            dto.TipoCausaID = model.TipoCausaID;
            dto.FechaIngreso = model.FechaIngreso;
            dto.NumeroTicket = model.NumeroTicket.Trim();
            dto.Numero = model.Numero.Trim();
            dto.Anio = model.Anio;
            dto.Denominacion = model.Denominacion.Trim();
            dto.Observacion = model.Observacion.Trim();
            dto.IsContencioso = model.IsContencioso;
            dto.NumeroRegistro = model.NumeroRegistro.Trim();

            dto.TipoCausa = new DTO.Models.TipoCausa();
            dto.TipoCausa.TipoCausaID = model.TipoCausa.TipoCausaID;
            dto.TipoCausa.Descripcion = model.TipoCausa.Descripcion.Trim();
            dto.TipoCausa.DescripcionLarga = model.TipoCausa.DescripcionLarga.Trim();
            dto.TipoCausa.Vigente = model.TipoCausa.Vigente;
            dto.TipoCausa.IsPublico = model.TipoCausa.IsPublico;
            dto.TipoCausa.IsInterno = model.TipoCausa.IsInterno;

            dto.TipoContencioso = new DTO.Models.TipoContencioso();
            dto.TipoContencioso.TipoContenciosoID = model.TipoContencioso.TipoContenciosoID;
            dto.TipoContencioso.Descripcion = model.TipoContencioso.Descripcion;
            dto.TipoContencioso.Vigente = model.TipoContencioso.Vigente;

            dto.TipoCanal = new DTO.Models.TipoCanal();
            dto.TipoCanal.TipoCanalID = model.TipoCanal.TipoCanalID;
            dto.TipoCanal.Descripcion = model.TipoCanal.Descripcion;
            dto.TipoCanal.Vigente = model.TipoCanal.Vigente;

            dto.EstadoCausa = new DTO.Models.EstadoCausa();
            dto.EstadoCausa.EstadoCausaID = model.EstadoCausa.EstadoCausaID;
            dto.EstadoCausa.Descripcion = model.EstadoCausa.Descripcion;
            dto.EstadoCausa.Vigente = model.EstadoCausa.Vigente;

            dto.Usuario = new DTO.Models.Usuario();
            dto.Usuario.UsuarioID = model.Usuario.UsuarioID;
            dto.Usuario.AdID = model.Usuario.AdID;
            dto.Usuario.Rut = model.Usuario.Rut;
            dto.Usuario.Nombres = model.Usuario.Nombres;
            dto.Usuario.Apellidos = model.Usuario.Apellidos;
            dto.Usuario.Mail = model.Usuario.Mail;
            dto.Usuario.Telefono = model.Usuario.Telefono;
            dto.Usuario.IsClaveUnica = model.Usuario.IsClaveUnica;
            dto.Usuario.FechaRegistro = model.Usuario.FechaRegistro;
            dto.Usuario.FechaModificacion = model.Usuario.FechaModificacion;

            return dto;
        }

        public IList<DTO.Models.Expediente> GetExpedienteByCausa(int causaID)
        {
            IList<Expediente> repoList = repo.GetExpedienteByCausa(causaID);
            IList<DTO.Models.Expediente> listDTO = new List<DTO.Models.Expediente>();

            foreach (var item in repoList)
            {
                DTO.Models.Expediente dto = new DTO.Models.Expediente();
                dto.ExpedienteID = item.ExpedienteID;
                dto.TipoCanalID = item.TipoCanalID;
                dto.TipoTramiteID = item.TipoTramiteID;
                dto.CausaID = item.CausaID;
                dto.UsuarioID = item.UsuarioID;
                dto.UsuarioResponsableID = item.UsuarioResponsableID;
                dto.FechaExpediente = item.FechaIngreso;
                dto.IsAdmisible = item.IsAdmisible;
                dto.Observacion = item.Observacion;
                dto.Comentario = item.Comentario;
                dto.NumeroOficio = item.NumeroOficio;
                dto.PlazoDias = item.PlazoDias;
                dto.IsHabil = item.IsHabil;
                dto.IsTabla = item.IsTabla;
                dto.IsFinalizado = item.IsFinalizado;
                dto.TablaID = item.TablaID;
                dto.NumeroTicket = item.NumeroTicket;


                dto.TipoTramite = new DTO.Models.TipoTramite();
                dto.TipoTramite.TipoTramiteID = item.TipoTramite.TipoTramiteID;
                dto.TipoTramite.Descripcion = item.TipoTramite.Descripcion;
                dto.TipoTramite.Vigente = item.TipoTramite.Vigente;

                dto.TipoTramite.AsocTipoTramiteOpciones = new List<DTO.Models.AsocTipoTramiteOpciones>();
                foreach (var tt in item.TipoTramite.AsocTipoTramiteOpciones)
                {
                    if (tt.Vigente)
                    {
                        DTO.Models.AsocTipoTramiteOpciones asoc = new DTO.Models.AsocTipoTramiteOpciones();
                        asoc.AsocTipoTramiteOpcionesID = tt.AsocTipoTramiteOpcionesID;
                        asoc.TipoTramiteID = tt.TipoTramiteID;
                        asoc.OpcionesTramiteID = tt.OpcionesTramiteID;
                        asoc.IsTabla = tt.IsTabla;
                        asoc.PlazoDias = tt.PlazoDias;
                        asoc.IsDiasHabiles = tt.IsDiasHabiles;
                        asoc.IsPermiteFechaAnterior = tt.IsPermiteFechaAnterior;
                        asoc.IsInformaAtraso = tt.IsInformaAtraso;
                        asoc.NumeroFirmas = tt.NumeroFirmas;
                        asoc.Status1 = tt.Status1;
                        asoc.Status2 = tt.Status2;
                        asoc.Vigente = tt.Vigente;
                        asoc.IsFinalizaIngreso = tt.IsFinalizaIngreso;
                        asoc.EstadoCausaID = tt.EstadoCausaID;
                        dto.TipoTramite.AsocTipoTramiteOpciones.Add(asoc);
                    }
                }

                dto.Usuario = new DTO.Models.Usuario();
                dto.Usuario.UsuarioID = item.Usuario.UsuarioID;
                dto.Usuario.AdID = item.Usuario.AdID;
                dto.Usuario.Rut = item.Usuario.Rut;
                dto.Usuario.Nombres = item.Usuario.Nombres;
                dto.Usuario.Apellidos = item.Usuario.Apellidos;
                dto.Usuario.Mail = item.Usuario.Mail;
                dto.Usuario.Telefono = item.Usuario.Telefono;
                dto.Usuario.IsClaveUnica = item.Usuario.IsClaveUnica;
                dto.Usuario.FechaRegistro = item.Usuario.FechaRegistro;
                dto.Usuario.FechaModificacion = item.Usuario.FechaModificacion;

                dto.Usuario.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                foreach (var asoc in item.Usuario.AsocUsuarioPerfil)
                {
                    DTO.Models.AsocUsuarioPerfil a = new DTO.Models.AsocUsuarioPerfil();
                    a.AsocUsuarioPerfilID = asoc.AsocUsuarioPerfilID;
                    a.PerfilID = asoc.PerfilID;
                    a.UsuarioID = asoc.UsuarioID;

                    a.Perfil = new DTO.Models.Perfil();
                    a.Perfil.PerfilID = asoc.Perfil.PerfilID;
                    a.Perfil.Descripcion = asoc.Perfil.Descripcion.Trim();


                    dto.Usuario.AsocUsuarioPerfil.Add(a);
                }

                dto.AsocExpedienteOpcion = new List<DTO.Models.AsocExpedienteOpcion>();
                foreach (var asoc in item.AsocExpedienteOpcion)
                {
                    DTO.Models.AsocExpedienteOpcion add = new DTO.Models.AsocExpedienteOpcion();
                    add.AsocExpedienteOpcionID = asoc.AsocExpedienteOpcionID;
                    add.ExpedienteID = asoc.ExpedienteID;
                    add.OpcionesTramiteID = asoc.OpcionesTramiteID;

                    add.OpcionesTramite = new DTO.Models.OpcionesTramite();
                    add.OpcionesTramite.OpcionesTramiteID = asoc.OpcionesTramite.OpcionesTramiteID;
                    add.OpcionesTramite.Descripcion = asoc.OpcionesTramite.Descripcion;
                    add.OpcionesTramite.Vigente = asoc.OpcionesTramite.Vigente;

                    dto.AsocExpedienteOpcion.Add(add);

                }

                dto.AsocEscritoDocto = new List<DTO.Models.AsocEscritoDocto>();
                foreach (var asoc in item.AsocEscritoDocto)
                {
                    DTO.Models.AsocEscritoDocto add = new DTO.Models.AsocEscritoDocto();
                    add.AsocEscritoDoctoID = asoc.AsocEscritoDoctoID;
                    add.AsocCausaDocumentoID = asoc.AsocCausaDocumentoID;
                    add.ExpedienteID = asoc.ExpedienteID;
                    add.OpcionesTramiteID = asoc.OpcionesTramiteID;

                    dto.AsocEscritoDocto.Add(add);
                }

                dto.AsocExpeFirma = new List<DTO.Models.AsocExpeFirma>();
                foreach (var asoc in item.AsocExpeFirma)
                {
                    DTO.Models.AsocExpeFirma add = new DTO.Models.AsocExpeFirma();
                    add.AsocExpeFirmaID = asoc.AsocExpeFirmaID;
                    add.FirmaID = asoc.FirmaID;
                    add.ExpedienteID = asoc.ExpedienteID;

                    add.Firma = new DTO.Models.Firma();
                    add.Firma.FirmaID = asoc.Firma.FirmaID;
                    add.Firma.UsuarioID = asoc.Firma.UsuarioID;
                    add.Firma.Orden = asoc.Firma.Orden;

                    foreach (var _asocFirmaDoctoAsocExpeFirma in asoc.Firma.AsocFirmaDocto)
                    {
                        DTO.Models.AsocFirmaDocto _asocFirmaDoctoExp = new DTO.Models.AsocFirmaDocto();
                        _asocFirmaDoctoExp.AsocFirmaDoctoID = _asocFirmaDoctoAsocExpeFirma.AsocFirmaDoctoID;
                        _asocFirmaDoctoExp.IsFirmado = _asocFirmaDoctoAsocExpeFirma.IsFirmado;
                        _asocFirmaDoctoExp.AsocEscritoDoctoID = _asocFirmaDoctoAsocExpeFirma.AsocEscritoDoctoID;

                        _asocFirmaDoctoExp.Firma = new DTO.Models.Firma();
                        _asocFirmaDoctoExp.Firma.FirmaID = _asocFirmaDoctoAsocExpeFirma.Firma.FirmaID;
                        _asocFirmaDoctoExp.Firma.UsuarioID = _asocFirmaDoctoAsocExpeFirma.Firma.UsuarioID;
                        _asocFirmaDoctoExp.Firma.Orden = _asocFirmaDoctoAsocExpeFirma.Firma.Orden;

                        add.Firma.AsocFirmaDocto.Add(_asocFirmaDoctoExp);
                    }
                    
                    dto.AsocExpeFirma.Add(add);
                }

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.Parte> GetParteByCausa(int causaID)
        {
            IList<Parte> repoList = repo.GetParteByCausa(causaID);
            IList<DTO.Models.Parte> listDTO = new List<DTO.Models.Parte>();

            foreach (var item in repoList)
            {
                DTO.Models.Parte dto = new DTO.Models.Parte();
                dto.ParteID = item.ParteID;
                dto.PaisID = item.PaisID;
                dto.CausaID = item.CausaID;
                dto.TipoParteID = item.TipoParteID;
                dto.Rut = item.Rut;
                dto.Nombre = item.Nombre;
                dto.RutRepresentante = item.RutRepresentante;
                dto.NombreRepresentante = item.NombreRepresentante;
                dto.NombreAbogado = item.NombreAbogado;
                dto.EmailAbogado = item.EmailAbogado;
                dto.NombreEstudioJuridico = item.NombreEstudioJuridico;
                dto.FolioConsignacion = item.FolioConsignacion;
                dto.FechaConsignacion = item.FechaConsignacion;
                dto.RutConsignacion = item.RutConsignacion;
                dto.NombreConsignacion = item.NombreConsignacion;

                dto.Pais = new DTO.Models.Pais();
                dto.Pais.PaisID = item.Pais.PaisID;
                dto.Pais.Descripcion = item.Pais.Descripcion;
                dto.Pais.CodigoArea = item.Pais.CodigoArea;
                dto.Pais.Vigente = item.Pais.Vigente;

                dto.TipoParte = new DTO.Models.TipoParte();
                dto.TipoParte.TipoParteID = item.TipoParte.TipoParteID;
                dto.TipoParte.Descripcion = item.TipoParte.Descripcion;
                dto.TipoParte.Vigente = item.TipoParte.Vigente;


                listDTO.Add(dto);
            }

            return listDTO;
        }

        public DTO.DownloadFile GetDownloadFileByHash(Domain.Infrastructure.TipoDocumento tipoDocumento, string Hash, int CausaID, int DoctoID = 0, int Docto2ID = 0)
        {
            DTO.DownloadFile file = new DTO.DownloadFile();

            if (tipoDocumento == Domain.Infrastructure.TipoDocumento.Causa ||
                tipoDocumento == Domain.Infrastructure.TipoDocumento.ExpedienteElectronicoPDF ||
                tipoDocumento == Domain.Infrastructure.TipoDocumento.Expediente)
            {
                Causa causa = repo.GetCausa(CausaID);
                causa.DocumentoCausa = repo.GetDocumentoCausa(CausaID, tipoDocumento);

                DocumentoCausa _doc = causa.DocumentoCausa.FirstOrDefault(x => x.Hash.Trim() == Hash.Trim());
                if (_doc != null)
                {
                    file.NombreArchivoFisico = _doc.NombreArchivoFisico.Trim();
                    file.CadenaVersionEncript = _doc.VersionEncript.Cadena.Trim();
                }
            }


            if (tipoDocumento == Domain.Infrastructure.TipoDocumento.Ingreso ||
                tipoDocumento == Domain.Infrastructure.TipoDocumento.Tabla ||
                tipoDocumento == Domain.Infrastructure.TipoDocumento.EstadoDiario ||
                tipoDocumento == Domain.Infrastructure.TipoDocumento.CertificadoTituloAbogado)
            {
                DocumentoSistema _doc = repo.GetDocumentoSistemaByHash(Hash);
                if (_doc != null)
                {
                    file.NombreArchivoFisico = _doc.NombreArchivoFisico.Trim();
                    file.CadenaVersionEncript = _doc.VersionEncript.Cadena.Trim();
                }
            }

            return file;
        }


        public DTO.Models.Parte GetParte(int parteID)
        {
            Parte model = repo.GetParte(parteID);

            DTO.Models.Parte dto = new DTO.Models.Parte();
            dto.ParteID = model.ParteID;
            dto.PaisID = model.PaisID;
            dto.CausaID = model.CausaID;
            dto.TipoParteID = model.TipoParteID;
            dto.Rut = model.Rut;
            dto.Nombre = model.Nombre;
            dto.RutRepresentante = model.RutRepresentante;
            dto.NombreRepresentante = model.NombreRepresentante;
            dto.NombreAbogado = model.NombreAbogado;
            dto.EmailAbogado = model.EmailAbogado;
            dto.NombreEstudioJuridico = model.NombreEstudioJuridico;
            dto.FolioConsignacion = model.FolioConsignacion;
            dto.FechaConsignacion = model.FechaConsignacion;
            dto.RutConsignacion = model.RutConsignacion;
            dto.NombreConsignacion = model.NombreConsignacion;

            dto.Pais = new DTO.Models.Pais();
            dto.Pais.PaisID = model.Pais.PaisID;
            dto.Pais.Descripcion = model.Pais.Descripcion;
            dto.Pais.Vigente = model.Pais.Vigente;

            dto.TipoParte = new DTO.Models.TipoParte();
            dto.TipoParte.TipoParteID = model.TipoParte.TipoParteID;
            dto.TipoParte.Descripcion = model.TipoParte.Descripcion;
            dto.TipoParte.Vigente = model.TipoParte.Vigente;

            return dto;
        }

        public int SaveParte(DTO.Models.Parte dto)
        {
            Parte model = new Parte();
            model.ParteID = dto.ParteID;
            model.PaisID = dto.PaisID;
            model.CausaID = dto.CausaID;
            model.TipoParteID = dto.TipoParteID;
            model.Rut = dto.Rut;
            model.Nombre = dto.Nombre;
            model.RutRepresentante = dto.RutRepresentante;
            model.NombreRepresentante = dto.NombreRepresentante;
            model.NombreAbogado = dto.NombreAbogado;
            model.EmailAbogado = dto.EmailAbogado;
            model.NombreEstudioJuridico = dto.NombreEstudioJuridico;
            model.FolioConsignacion = dto.FolioConsignacion;
            model.FechaConsignacion = dto.FechaConsignacion;
            model.RutConsignacion = dto.RutConsignacion;
            model.NombreConsignacion = dto.NombreConsignacion;

            return repo.SaveParte(model);
        }


        public int SaveCausa(DTO.Models.Causa dto)
        {
            Causa model = new Causa();
            model.CausaID = dto.CausaID;
            model.TipoCanalID = dto.TipoCanalID;
            model.EstadoCausaID = dto.EstadoCausaID;
            model.UsuarioID = dto.UsuarioID;
            model.UsuarioResponsableID = dto.UsuarioResponsableID;
            model.TipoContenciosoID = dto.TipoContenciosoID;
            model.TipoCausaID = dto.TipoCausaID;
            model.FechaIngreso = dto.FechaIngreso;
            model.NumeroTicket = dto.NumeroTicket;
            model.Numero = dto.Numero;
            model.Anio = dto.Anio;
            model.Denominacion = dto.Denominacion;
            model.Observacion = dto.Observacion;
            model.IsContencioso = dto.IsContencioso;
            model.NumeroRegistro = dto.NumeroRegistro;

            return repo.SaveCausa(model);
        }


        public void UpdateCausa(DTO.Models.Causa dto)
        {
            Causa model = new Causa();
            model.CausaID = dto.CausaID;
            model.Denominacion = dto.Denominacion;
            model.Observacion = dto.Observacion;
            model.IsContencioso = dto.IsContencioso;

            model.TipoContenciosoID = dto.TipoContenciosoID;
            model.NumeroRegistro = dto.NumeroRegistro;
            model.Numero = dto.Numero;

            repo.UpdateCausa(model);
        }

        public int SaveLogCausa(DTO.Models.LogCausa dto)
        {
            LogCausa model = new LogCausa();
            model.LogCausaID = dto.LogCausaID;
            model.EstadoCausaID = dto.EstadoCausaID;
            model.CausaID = dto.CausaID;
            model.Fecha = dto.Fecha;
            model.UsuarioID = dto.UsuarioID;
            model.Descripcion = dto.Descripcion;
            model.Observaciones = dto.Observaciones;

            return repo.SaveLogCausa(model);
        }

        public IList<DTO.Models.LogCausa> GetLogCausa(int CausaID)
        {
            IList<LogCausa> repoList = repo.GetLogCausa(CausaID);
            IList<DTO.Models.LogCausa> listDTO = new List<DTO.Models.LogCausa>();
            IList<DTO.Models.Usuario> userList = appCommon.GetUsuarios();

            foreach (var item in repoList)
            {
                DTO.Models.LogCausa dto = new DTO.Models.LogCausa();
                dto.LogCausaID = item.LogCausaID;
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.CausaID = item.CausaID;
                dto.Fecha = item.Fecha;
                dto.UsuarioID = item.UsuarioID;
                dto.Descripcion = item.Descripcion;
                dto.Observaciones = item.Observaciones;
                dto.Responsable = string.Empty;

                try
                {
                    dto.Responsable = userList.FirstOrDefault(x=> x.UsuarioID == item.UsuarioID).GetFullName();
                }
                catch (Exception ex)
                {
                    Infrastructure.Logging.Logger.Execute().Error(ex);
                }

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveDocumentoCausa(DTO.Models.DocumentoCausa dto)
        {
            DocumentoCausa model = new DocumentoCausa();
            model.DocumentoCausaID = dto.DocumentoCausaID;
            model.CausaID = dto.CausaID;
            model.VersionEncriptID = dto.VersionEncriptID;
            model.Hash = dto.Hash;
            model.NombreArchivoFisico = dto.NombreArchivoFisico;
            model.Fecha = dto.Fecha;
            model.Descripcion = dto.Descripcion;

            return repo.SaveDocumentoCausa(model);
        }
        public int SaveAsocCausaDocumento(DTO.Models.AsocCausaDocumento dto)
        {
            AsocCausaDocumento model = new AsocCausaDocumento();
            model.AsocCausaDocumentoID = dto.AsocCausaDocumentoID;
            model.DocumentoAdjuntoID = dto.DocumentoAdjuntoID;
            model.DocumentoCausaID = dto.DocumentoCausaID;
            model.CompromisoID = dto.CompromisoID;

            return repo.SaveAsocCausaDocumento(model);

        }

        public int SaveAsocEscritoDocto(DTO.Models.AsocEscritoDocto dto)
        {
            AsocEscritoDocto model = new AsocEscritoDocto();
            model.AsocEscritoDoctoID = dto.AsocEscritoDoctoID;
            model.AsocCausaDocumentoID = dto.AsocCausaDocumentoID;
            model.ExpedienteID = dto.ExpedienteID;
            model.OpcionesTramiteID = dto.OpcionesTramiteID;

            return repo.SaveAsocEscritoDocto(model);
        }

        public IList<DTO.Models.DocumentoCausa> GetDocumentoCausa(int causaID, Domain.Infrastructure.TipoDocumento tipoDoc)
        {
            IList<DocumentoCausa> repoList = repo.GetDocumentoCausa(causaID, tipoDoc);
            IList<DTO.Models.DocumentoCausa> listDTO = new List<DTO.Models.DocumentoCausa>();

            foreach (var item in repoList)
            {
                DTO.Models.DocumentoCausa dto = new DTO.Models.DocumentoCausa();
                dto.DocumentoCausaID = item.DocumentoCausaID;
                dto.CausaID = item.CausaID;
                dto.VersionEncriptID = item.VersionEncriptID;
                dto.Hash = item.Hash.Trim();
                dto.NombreArchivoFisico = item.NombreArchivoFisico.Trim();
                dto.Fecha = item.Fecha;
                dto.Descripcion = item.Descripcion.Trim();

                dto.VersionEncript = new DTO.Models.VersionEncript();
                dto.VersionEncript.VersionEncriptID = item.VersionEncript.VersionEncriptID;
                dto.VersionEncript.Cadena = item.VersionEncript.Cadena.Trim();

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public DTO.Models.Folio GetFolio(int Anio)
        {
            Folio model = repo.GetFolio(Anio);

            DTO.Models.Folio dto = new DTO.Models.Folio();
            dto.FolioID = model.FolioID;
            dto.Anio = model.Anio;
            dto.Correlativo = model.Correlativo;

            return dto;
        }

        public IList<DTO.Models.Causa> GetCausasByFilter(DTO.FiltrosEscritorio filtros, Domain.Infrastructure.TipoGrid listadoIngresos = Domain.Infrastructure.TipoGrid.None)
        {
            IList<Causa> repoList = repo.GetCausasByFilter(MapFilter(filtros), listadoIngresos);

            IList<DTO.Models.Causa> listDTO = new List<DTO.Models.Causa>();

            foreach (var item in repoList)
            {
                DTO.Models.Causa dto = new DTO.Models.Causa();
                dto.CausaID = item.CausaID;
                dto.TipoCanalID = item.TipoCanalID;
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.UsuarioID = item.UsuarioID;
                dto.UsuarioResponsableID = item.UsuarioResponsableID;
                dto.TipoContenciosoID = item.TipoContenciosoID;
                dto.TipoCausaID = item.TipoCausaID;
                dto.FechaIngreso = item.FechaIngreso;
                dto.NumeroTicket = item.NumeroTicket.Trim();
                dto.Numero = item.Numero.Trim();
                dto.Anio = item.Anio;
                dto.Denominacion = item.Denominacion.Trim();
                dto.Observacion = item.Observacion.Trim();
                dto.IsContencioso = item.IsContencioso;
                dto.NumeroRegistro = item.NumeroRegistro.Trim();

                dto.EstadoCausa = new DTO.Models.EstadoCausa();
                dto.EstadoCausa.EstadoCausaID = item.EstadoCausa.EstadoCausaID;
                dto.EstadoCausa.Descripcion = item.EstadoCausa.Descripcion.Trim();
                dto.EstadoCausa.Vigente = item.EstadoCausa.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveDocumentoSistema(DTO.Models.DocumentoSistema dto)
        {
            DocumentoSistema model = new DocumentoSistema();
            model.DocumentoSistemaID = dto.DocumentoSistemaID;
            model.VersionEncriptID = dto.VersionEncriptID;
            model.TipoDocumentoID = dto.TipoDocumentoID;
            model.Hash = dto.Hash;
            model.NombreArchivoFisico = dto.NombreArchivoFisico;
            model.Fecha = dto.Fecha;
            model.Descripcion = dto.Descripcion;

            return repo.SaveDocumentoSistema(model);
        }

        public int SaveAsocExpedienteOpcion(DTO.Models.AsocExpedienteOpcion dto)
        {
            AsocExpedienteOpcion model = new AsocExpedienteOpcion();
            model.AsocExpedienteOpcionID = dto.AsocExpedienteOpcionID;
            model.ExpedienteID = dto.ExpedienteID;
            model.OpcionesTramiteID = dto.OpcionesTramiteID;

            return repo.SaveAsocExpedienteOpcion(model);
        }

        public void CambiarEstadoCausa(int causaID, Domain.Infrastructure.EstadoCausa estado)
        {
            repo.CambiarEstadoCausa(causaID, estado);
        }

        public int SaveExpediente(DTO.Models.Expediente dto)
        {
            Expediente model = new Expediente();
            model.ExpedienteID = dto.ExpedienteID;
            model.TipoCanalID = dto.TipoCanalID;
            model.TipoTramiteID = dto.TipoTramiteID;
            model.CausaID = dto.CausaID;
            model.UsuarioID = dto.UsuarioID;
            model.UsuarioResponsableID = dto.UsuarioResponsableID;
            model.FechaIngreso = dto.FechaExpediente;
            model.IsAdmisible = dto.IsAdmisible;
            model.Observacion = dto.Observacion;
            model.Comentario = dto.Comentario;
            model.NumeroOficio = dto.NumeroOficio;
            model.PlazoDias = dto.PlazoDias;
            model.IsHabil = dto.IsHabil;
            model.IsTabla = dto.IsTabla;
            model.IsFinalizado = dto.IsFinalizado;
            model.TablaID = dto.TablaID;
            model.NumeroTicket = dto.NumeroTicket;

            return repo.SaveExpediente(model);
        }
        public DTO.Models.Expediente GetExpediente(int expedienteID)
        {
            Expediente model = repo.GetExpediente(expedienteID);

            DTO.Models.Expediente dto = new DTO.Models.Expediente();
            dto.ExpedienteID = model.ExpedienteID;
            dto.TipoCanalID = model.TipoCanalID;
            dto.TipoTramiteID = model.TipoTramiteID;
            dto.CausaID = model.CausaID;
            dto.UsuarioID = model.UsuarioID;
            dto.UsuarioResponsableID = model.UsuarioResponsableID;
            dto.FechaExpediente = model.FechaIngreso;
            dto.IsAdmisible = model.IsAdmisible;
            dto.Observacion = model.Observacion;
            dto.Comentario = model.Comentario;
            dto.NumeroOficio = model.NumeroOficio;
            dto.PlazoDias = model.PlazoDias;
            dto.IsHabil = model.IsHabil;
            dto.IsTabla = model.IsTabla;
            dto.IsFinalizado = model.IsFinalizado;
            dto.TablaID = model.TablaID;
            dto.NumeroTicket = model.NumeroTicket;

            dto.AsocExpedienteOpcion = new List<DTO.Models.AsocExpedienteOpcion>();
            foreach (var asoc in model.AsocExpedienteOpcion)
            {
                DTO.Models.AsocExpedienteOpcion add = new DTO.Models.AsocExpedienteOpcion();
                add.AsocExpedienteOpcionID = asoc.AsocExpedienteOpcionID;
                add.ExpedienteID = asoc.ExpedienteID;
                add.OpcionesTramiteID = asoc.OpcionesTramiteID;

                add.OpcionesTramite = new DTO.Models.OpcionesTramite();
                add.OpcionesTramite.OpcionesTramiteID = asoc.OpcionesTramite.OpcionesTramiteID;
                add.OpcionesTramite.Descripcion = asoc.OpcionesTramite.Descripcion;
                add.OpcionesTramite.Vigente = asoc.OpcionesTramite.Vigente;

                dto.AsocExpedienteOpcion.Add(add);
            }

            dto.TipoTramite = new DTO.Models.TipoTramite();
            dto.TipoTramite.TipoTramiteID = model.TipoTramite.TipoTramiteID;
            dto.TipoTramite.Descripcion = model.TipoTramite.Descripcion;
            dto.TipoTramite.Vigente = model.TipoTramite.Vigente;

            dto.TipoTramite.AsocTipoTramiteOpciones = new List<DTO.Models.AsocTipoTramiteOpciones>();
            foreach (var tipo in model.TipoTramite.AsocTipoTramiteOpciones)
            {
                DTO.Models.AsocTipoTramiteOpciones add = new DTO.Models.AsocTipoTramiteOpciones();
                add.AsocTipoTramiteOpcionesID = tipo.AsocTipoTramiteOpcionesID;
                add.TipoTramiteID = tipo.TipoTramiteID;
                add.OpcionesTramiteID = tipo.OpcionesTramiteID;
                add.IsTabla = tipo.IsTabla;
                add.PlazoDias = tipo.PlazoDias;
                add.IsDiasHabiles = tipo.IsDiasHabiles;
                add.IsPermiteFechaAnterior = tipo.IsPermiteFechaAnterior;
                add.IsInformaAtraso = tipo.IsInformaAtraso;
                add.NumeroFirmas = tipo.NumeroFirmas;
                add.Status1 = tipo.Status1;
                add.Status2 = tipo.Status2;
                add.IsFinalizaIngreso = tipo.IsFinalizaIngreso;
                add.EstadoCausaID = tipo.EstadoCausaID;
                add.Vigente = tipo.Vigente;

                dto.TipoTramite.AsocTipoTramiteOpciones.Add(add);
            }

            return dto;
        }

        public IList<DTO.Models.Firma> GetFirmaByExpedienteID(int expedienteID)
        {
            IList<Firma> repoList = repo.GetFirmaByExpedienteID(expedienteID);
            IList<DTO.Models.Firma> listDTO = new List<DTO.Models.Firma>();

            foreach (var item in repoList)
            {
                DTO.Models.Firma dto = new DTO.Models.Firma();
                dto.FirmaID = item.FirmaID;
                dto.UsuarioID = item.UsuarioID;
                //dto.ExpedienteID = item.ExpedienteID;
                dto.Orden = item.Orden;

                dto.Usuario = new DTO.Models.Usuario();
                dto.Usuario.UsuarioID = item.Usuario.UsuarioID;
                dto.Usuario.AdID = item.Usuario.AdID;
                dto.Usuario.Rut = item.Usuario.Rut;
                dto.Usuario.Nombres = item.Usuario.Nombres;
                dto.Usuario.Apellidos = item.Usuario.Apellidos;
                dto.Usuario.Mail = item.Usuario.Mail;
                dto.Usuario.Telefono = item.Usuario.Telefono;
                dto.Usuario.IsClaveUnica = item.Usuario.IsClaveUnica;
                dto.Usuario.FechaRegistro = item.Usuario.FechaRegistro;
                dto.Usuario.FechaModificacion = item.Usuario.FechaModificacion;

                dto.AsocFirmaDocto = new List<DTO.Models.AsocFirmaDocto>();
                foreach (var asoc in item.AsocFirmaDocto)
                {
                    DTO.Models.AsocFirmaDocto add = new DTO.Models.AsocFirmaDocto();
                    add.AsocFirmaDoctoID = asoc.AsocFirmaDoctoID;
                    add.FirmaID = asoc.FirmaID;
                    add.AsocEscritoDoctoID = asoc.AsocEscritoDoctoID;
                    add.IsFirmado = asoc.IsFirmado;

                    dto.AsocFirmaDocto.Add(add);
                }

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.Firma> GetEscritorioFirmas(DTO.FiltrosEscritorio filtros)
        {
            IList<Firma> repoList = repo.GetEscritorioFirmas(MapFilter(filtros));
             
            IList<DTO.Models.Firma> listDTO = new List<DTO.Models.Firma>();

            foreach (var item in repoList)
            {
                DTO.Models.Firma dto = new DTO.Models.Firma();
                dto.FirmaID = item.FirmaID;
                dto.UsuarioID = item.UsuarioID;
                dto.Orden = item.Orden;

                #region Usuario
                dto.Usuario = new DTO.Models.Usuario();
                dto.Usuario.UsuarioID = item.Usuario.UsuarioID;
                dto.Usuario.AdID = item.Usuario.AdID;
                dto.Usuario.Rut = item.Usuario.Rut;
                dto.Usuario.Nombres = item.Usuario.Nombres;
                dto.Usuario.Apellidos = item.Usuario.Apellidos;
                dto.Usuario.Mail = item.Usuario.Mail;
                dto.Usuario.Telefono = item.Usuario.Telefono;
                dto.Usuario.IsClaveUnica = item.Usuario.IsClaveUnica;
                dto.Usuario.FechaRegistro = item.Usuario.FechaRegistro;
                dto.Usuario.FechaModificacion = item.Usuario.FechaModificacion;
                #endregion
                
                #region AsocFirmaDocto
                dto.AsocFirmaDocto = new List<DTO.Models.AsocFirmaDocto>();
                foreach (var asoc in item.AsocFirmaDocto)
                {
                    #region AsocFirmaDocto
                    DTO.Models.AsocFirmaDocto add = new DTO.Models.AsocFirmaDocto();
                    add.AsocFirmaDoctoID = asoc.AsocFirmaDoctoID;
                    add.FirmaID = asoc.FirmaID;
                    add.AsocEscritoDoctoID = asoc.AsocEscritoDoctoID;
                    add.IsFirmado = asoc.IsFirmado;
                    #endregion

                    #region AsocEscritoDocto
                    add.AsocEscritoDocto = new DTO.Models.AsocEscritoDocto();
                    if (asoc.AsocEscritoDocto != null)
                    {
                        add.AsocEscritoDocto.AsocEscritoDoctoID = asoc.AsocEscritoDocto.AsocEscritoDoctoID;
                        add.AsocEscritoDocto.ExpedienteID = asoc.AsocEscritoDocto.ExpedienteID;
                        add.AsocEscritoDocto.OpcionesTramiteID = asoc.AsocEscritoDocto.OpcionesTramiteID;
                    }
                    #endregion
                    
                    #region Expediente
                    add.AsocEscritoDocto.Expediente = new DTO.Models.Expediente();
                    Expediente _expediente = repo.GetExpediente(add.AsocEscritoDocto.ExpedienteID);
                    
                    #region Expediente
                    add.AsocEscritoDocto.Expediente.ExpedienteID = _expediente.ExpedienteID;
                    add.AsocEscritoDocto.Expediente.CausaID = _expediente.CausaID;
                    add.AsocEscritoDocto.Expediente.TipoCanalID = _expediente.TipoCanalID;
                    add.AsocEscritoDocto.Expediente.UsuarioID = _expediente.UsuarioID;
                    add.AsocEscritoDocto.Expediente.UsuarioResponsableID = _expediente.UsuarioResponsableID;
                    add.AsocEscritoDocto.Expediente.FechaExpediente = _expediente.FechaIngreso;
                    add.AsocEscritoDocto.Expediente.IsAdmisible = _expediente.IsAdmisible;
                    add.AsocEscritoDocto.Expediente.Observacion = _expediente.Observacion;
                    add.AsocEscritoDocto.Expediente.NumeroOficio = _expediente.NumeroOficio;
                    add.AsocEscritoDocto.Expediente.PlazoDias = _expediente.PlazoDias;
                    add.AsocEscritoDocto.Expediente.IsHabil = _expediente.IsHabil;
                    add.AsocEscritoDocto.Expediente.IsTabla = _expediente.IsTabla;
                    add.AsocEscritoDocto.Expediente.IsFinalizado = _expediente.IsFinalizado;
                    add.AsocEscritoDocto.Expediente.NumeroTicket = _expediente.NumeroTicket;
                    #endregion

                    #region Firmas Expediente
                    foreach (var _firmas in _expediente.AsocExpeFirma)
                    {
                        DTO.Models.AsocExpeFirma _asocExpFirma = new DTO.Models.AsocExpeFirma();
                        _asocExpFirma.AsocExpeFirmaID = _firmas.AsocExpeFirmaID;
                        _asocExpFirma.FirmaID = _firmas.FirmaID;
                        _asocExpFirma.ExpedienteID = _firmas.ExpedienteID;

                        _asocExpFirma.Firma = new DTO.Models.Firma();
                        _asocExpFirma.Firma.FirmaID = _firmas.Firma.FirmaID;
                        _asocExpFirma.Firma.UsuarioID = _firmas.Firma.UsuarioID;
                        _asocExpFirma.Firma.Orden = _firmas.Firma.Orden;

                        foreach (var _asocFirmaDoctoAsocExpeFirma in _firmas.Firma.AsocFirmaDocto)
                        {
                            DTO.Models.AsocFirmaDocto _asocFirmaDoctoExp = new DTO.Models.AsocFirmaDocto();
                            _asocFirmaDoctoExp.AsocFirmaDoctoID = _asocFirmaDoctoAsocExpeFirma.AsocFirmaDoctoID;
                            _asocFirmaDoctoExp.IsFirmado = _asocFirmaDoctoAsocExpeFirma.IsFirmado;
                            _asocFirmaDoctoExp.AsocEscritoDoctoID = _asocFirmaDoctoAsocExpeFirma.AsocEscritoDoctoID;

                            _asocFirmaDoctoExp.Firma = new DTO.Models.Firma();
                            _asocFirmaDoctoExp.Firma.FirmaID = _asocFirmaDoctoAsocExpeFirma.Firma.FirmaID;
                            _asocFirmaDoctoExp.Firma.UsuarioID = _asocFirmaDoctoAsocExpeFirma.Firma.UsuarioID;
                            _asocFirmaDoctoExp.Firma.Orden = _asocFirmaDoctoAsocExpeFirma.Firma.Orden;

                            _asocExpFirma.Firma.AsocFirmaDocto.Add(_asocFirmaDoctoExp);
                        }

                        add.AsocEscritoDocto.Expediente.AsocExpeFirma.Add(_asocExpFirma);
                    }
                    #endregion

                    #region AsocExpedienteOpcion
                    foreach (var asocExpediente in _expediente.AsocExpedienteOpcion)
                    {
                        DTO.Models.AsocExpedienteOpcion _asocExp = new DTO.Models.AsocExpedienteOpcion();
                        _asocExp.AsocExpedienteOpcionID = asocExpediente.AsocExpedienteOpcionID;
                        _asocExp.ExpedienteID = asocExpediente.ExpedienteID;
                        _asocExp.OpcionesTramiteID = asocExpediente.OpcionesTramiteID;
                        _asocExp.OpcionesTramite = new DTO.Models.OpcionesTramite();
                        _asocExp.OpcionesTramite.OpcionesTramiteID = asocExpediente.OpcionesTramite.OpcionesTramiteID;
                        _asocExp.OpcionesTramite.Descripcion = asocExpediente.OpcionesTramite.Descripcion;
                        _asocExp.OpcionesTramite.Vigente = asocExpediente.OpcionesTramite.Vigente;

                        add.AsocEscritoDocto.Expediente.AsocExpedienteOpcion.Add(_asocExp);
                    }

                    #endregion

                    #region TipoTramite
                    add.AsocEscritoDocto.Expediente.TipoTramite = new DTO.Models.TipoTramite();
                    add.AsocEscritoDocto.Expediente.TipoTramite.TipoTramiteID = _expediente.TipoTramite.TipoTramiteID;
                    add.AsocEscritoDocto.Expediente.TipoTramite.Descripcion = _expediente.TipoTramite.Descripcion;
                    add.AsocEscritoDocto.Expediente.TipoTramite.Vigente = _expediente.TipoTramite.Vigente;
                    #endregion

                    #region AsocTipoTramiteOpciones
                    foreach (var asocTipoTramite in _expediente.TipoTramite.AsocTipoTramiteOpciones)
                    {
                        DTO.Models.AsocTipoTramiteOpciones _asocTipoTramite = new DTO.Models.AsocTipoTramiteOpciones();
                        _asocTipoTramite.AsocTipoTramiteOpcionesID = asocTipoTramite.AsocTipoTramiteOpcionesID;
                        _asocTipoTramite.TipoTramiteID = asocTipoTramite.TipoTramiteID;
                        _asocTipoTramite.OpcionesTramiteID = asocTipoTramite.OpcionesTramiteID;
                        _asocTipoTramite.IsTabla = asocTipoTramite.IsTabla;
                        _asocTipoTramite.PlazoDias = asocTipoTramite.PlazoDias;
                        _asocTipoTramite.IsDiasHabiles = asocTipoTramite.IsDiasHabiles;
                        _asocTipoTramite.IsPermiteFechaAnterior = asocTipoTramite.IsPermiteFechaAnterior;
                        _asocTipoTramite.IsInformaAtraso = asocTipoTramite.IsInformaAtraso;
                        _asocTipoTramite.NumeroFirmas = asocTipoTramite.NumeroFirmas;
                        _asocTipoTramite.Status1 = asocTipoTramite.Status1;
                        _asocTipoTramite.Status2 = asocTipoTramite.Status2;
                        _asocTipoTramite.IsFinalizaIngreso = asocTipoTramite.IsFinalizaIngreso;
                        _asocTipoTramite.EstadoCausaID = asocTipoTramite.EstadoCausaID;
                        _asocTipoTramite.Vigente = asocTipoTramite.Vigente;

                        add.AsocEscritoDocto.Expediente.TipoTramite.AsocTipoTramiteOpciones.Add(_asocTipoTramite);
                    }
                    #endregion

                    #endregion

                    #region AsocCausaDocumento
                    add.AsocEscritoDocto.AsocCausaDocumento = new DTO.Models.AsocCausaDocumento();

                    if (asoc.AsocEscritoDocto.AsocCausaDocumento != null)
                    {
                        add.AsocEscritoDocto.AsocCausaDocumento.AsocCausaDocumentoID = asoc.AsocEscritoDocto.AsocCausaDocumento.AsocCausaDocumentoID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoAdjuntoID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoAdjuntoID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausaID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausaID;
                        add.AsocEscritoDocto.AsocCausaDocumento.CompromisoID = asoc.AsocEscritoDocto.AsocCausaDocumento.CompromisoID;
                    }
                    #endregion

                    #region DocumentoCausa

                    if (asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa != null)
                    {
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa = new DTO.Models.DocumentoCausa();
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.DocumentoCausaID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.DocumentoCausaID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.VersionEncriptID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.VersionEncriptID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.CausaID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.CausaID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Hash = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Hash;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.NombreArchivoFisico = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.NombreArchivoFisico;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Fecha = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Fecha;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Descripcion = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Descripcion;
                    }
                    #endregion

                    #region Causa
                    add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa = new DTO.Models.Causa();
                    
                    if (asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa != null)
                    {
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.CausaID = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.CausaID;
                        add.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.NumeroTicket = asoc.AsocEscritoDocto.AsocCausaDocumento.DocumentoCausa.Causa.NumeroTicket;
                    }
                    
                    #endregion

                    dto.AsocFirmaDocto.Add(add);
                }

                if (dto.AsocFirmaDocto.Count > 0) listDTO.Add(dto);

                #endregion

                #region AsocDocSistemaFirma

                dto.AsocDocSistemaFirma = new List<DTO.Models.AsocDocSistemaFirma>();

                foreach (var asocDocSistema in item.AsocDocSistemaFirma)
                {
                    bool Agregar = false;

                    #region AsocDocSistemaFirma
                    DTO.Models.AsocDocSistemaFirma add = new DTO.Models.AsocDocSistemaFirma();
                    add.AsocDocSistemaFirmaID = asocDocSistema.AsocDocSistemaFirmaID;
                    add.DocumentoSistemaID = asocDocSistema.DocumentoSistemaID;
                    add.FirmaID = asocDocSistema.FirmaID;
                    add.IsFirmado = asocDocSistema.IsFirmado;

                    #endregion

                    #region DocumentoSistema
                    if (asocDocSistema.DocumentoSistema != null)
                    {
                        add.DocumentoSistema = new DTO.Models.DocumentoSistema();
                        add.DocumentoSistema.DocumentoSistemaID = asocDocSistema.DocumentoSistema.DocumentoSistemaID;
                        add.DocumentoSistema.VersionEncriptID = asocDocSistema.DocumentoSistema.VersionEncriptID;
                        add.DocumentoSistema.TipoDocumentoID = asocDocSistema.DocumentoSistema.TipoDocumentoID;
                        add.DocumentoSistema.Hash = asocDocSistema.DocumentoSistema.Hash;
                        add.DocumentoSistema.Fecha = asocDocSistema.DocumentoSistema.Fecha;
                        add.DocumentoSistema.Descripcion = asocDocSistema.DocumentoSistema.Descripcion;

                        add.DocumentoSistema.TipoDocumento = new DTO.Models.TipoDocumento();
                        add.DocumentoSistema.TipoDocumento.TipoDocumentoID = asocDocSistema.DocumentoSistema.TipoDocumento.TipoDocumentoID;
                        add.DocumentoSistema.TipoDocumento.Descripcion = asocDocSistema.DocumentoSistema.TipoDocumento.Descripcion;

                        #region DocumentoSistema.AsocDocSistemaFirma
                        add.DocumentoSistema.AsocDocSistemaFirma = new List<DTO.Models.AsocDocSistemaFirma>();
                        DocumentoSistema _dctoSistema = repo.GetDocumentoSistema(add.DocumentoSistema.DocumentoSistemaID);
                        foreach (var asocDocSistemaDocumentoSistema in _dctoSistema.AsocDocSistemaFirma)
                        {
                            //if (asocDocSistemaDocumentoSistema.AsocDocSistemaFirmaID == add.AsocDocSistemaFirmaID) continue;
                            DTO.Models.AsocDocSistemaFirma _AsocDocSistemaFirma = new DTO.Models.AsocDocSistemaFirma();
                            _AsocDocSistemaFirma.AsocDocSistemaFirmaID = asocDocSistemaDocumentoSistema.AsocDocSistemaFirmaID;
                            _AsocDocSistemaFirma.DocumentoSistemaID = asocDocSistemaDocumentoSistema.DocumentoSistemaID;
                            _AsocDocSistemaFirma.FirmaID = asocDocSistemaDocumentoSistema.FirmaID;
                            _AsocDocSistemaFirma.IsFirmado = asocDocSistemaDocumentoSistema.IsFirmado;

                            _AsocDocSistemaFirma.Firma = new DTO.Models.Firma();
                            _AsocDocSistemaFirma.Firma.FirmaID = asocDocSistemaDocumentoSistema.Firma.FirmaID;
                            _AsocDocSistemaFirma.Firma.UsuarioID = asocDocSistemaDocumentoSistema.Firma.UsuarioID;
                            _AsocDocSistemaFirma.Firma.Orden = asocDocSistemaDocumentoSistema.Firma.Orden;
                            
                            add.DocumentoSistema.AsocDocSistemaFirma.Add(_AsocDocSistemaFirma);
                        }
                        #endregion

                        #region AsocDocumentoSistemaTabla
                        var AsocDocumentoSistemaTabla = repo.GetAsocDocumentoSistemaTablaByDocumentoSitemaID(asocDocSistema.DocumentoSistemaID);
                        foreach (var _asocDoctoSistemaTabla in AsocDocumentoSistemaTabla)
                        {
                            DTO.Models.AsocDocumentoSistemaTabla _addAsoc = new DTO.Models.AsocDocumentoSistemaTabla();
                            _addAsoc.AsocDocumentoSistemaTablaID = _asocDoctoSistemaTabla.AsocDocumentoSistemaTablaID;
                            _addAsoc.TablaID = _asocDoctoSistemaTabla.TablaID;
                            _addAsoc.DocumentoSistemaID = _asocDoctoSistemaTabla.DocumentoSistemaID;

                            _addAsoc.Tabla = new DTO.Models.Tabla();
                            var repoTabla = repo.GetTablaByID(_addAsoc.TablaID);

                            _addAsoc.Tabla.TablaID = repoTabla.TablaID;
                            _addAsoc.Tabla.EstadoTablaID = repoTabla.EstadoTablaID;
                            _addAsoc.Tabla.SalaID = repoTabla.SalaID;
                            _addAsoc.Tabla.Fecha = repoTabla.Fecha;
                            _addAsoc.Tabla.UsuarioRelatorID = repoTabla.UsuarioSubroganteID;

                            add.DocumentoSistema.AsocDocumentoSistemaTabla.Add(_addAsoc);

                            Agregar = (_addAsoc.Tabla.EstadoTablaID == (int)Domain.Infrastructure.EstadoTabla.FirmadoPublicado);
                        }
                        #endregion

                        #region AsocDocumentoSistemaEstadoDiario
                        var AsocDocumentoSistemaEstadoDiario = repo.GetAsocDocumentoSistemaEstadoDiarioByDocumentoSitemaID(asocDocSistema.DocumentoSistemaID);
                        foreach (var _asocDoctoSistemaEstadoDiario in AsocDocumentoSistemaEstadoDiario)
                        {
                            DTO.Models.AsocDocumentoSistemaEstadoDiario _addAsoc = new DTO.Models.AsocDocumentoSistemaEstadoDiario();
                            _addAsoc.AsocDocumentoSistemaEstadoDiarioID = _asocDoctoSistemaEstadoDiario.AsocDocumentoSistemaEstadoDiarioID;
                            _addAsoc.EstadoDiarioID = _asocDoctoSistemaEstadoDiario.EstadoDiarioID;
                            _addAsoc.DocumentoSistemaID = _asocDoctoSistemaEstadoDiario.DocumentoSistemaID;

                            _addAsoc.EstadoDiario = new DTO.Models.EstadoDiario();
                            var repoEstadoDiario = repo.GetEstadoDiarioByID(_addAsoc.EstadoDiarioID);

                            _addAsoc.EstadoDiario.EstadoDiarioID = repoEstadoDiario.EstadoDiarioID;
                            _addAsoc.EstadoDiario.TipoEstadoDiarioID = repoEstadoDiario.TipoEstadoDiarioID;
                            _addAsoc.EstadoDiario.Fecha = repoEstadoDiario.Fecha;

                            add.DocumentoSistema.AsocDocumentoSistemaEstadoDiario.Add(_addAsoc);

                            Agregar = (_addAsoc.EstadoDiario.TipoEstadoDiarioID == (int)Domain.Infrastructure.TipoEstadoDiario.FirmadoPublicado);
                        }
                        #endregion
                        
                    }
                    #endregion
                    if (Agregar)
                    {
                        dto.AsocDocSistemaFirma.Add(add);
                    }
                }
                if (dto.AsocDocSistemaFirma.Count > 0) listDTO.Add(dto);
                #endregion
            }

            return listDTO;
        }


        public IList<DTO.Models.EstadoCausa> GetFlujoEstado(int tipoTramiteID)
        {
            IList<SP_FlujoEstado_Result> repoList = repo.GetFlujoEstado(tipoTramiteID);
            IList<DTO.Models.EstadoCausa> listDTO = new List<DTO.Models.EstadoCausa>();

            foreach (var item in repoList)
            {
                DTO.Models.EstadoCausa dto = new DTO.Models.EstadoCausa();
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.EstadosAplica> GetEstadosAplica(int tipoTramiteID, int estadoCausaID)
        {

            IList<EstadosAplica> repoList = repo.GetEstadosAplica(tipoTramiteID, estadoCausaID);
            IList<DTO.Models.EstadosAplica> listDTO = new List<DTO.Models.EstadosAplica>();

            foreach (var item in repoList)
            {
                DTO.Models.EstadosAplica dto = new DTO.Models.EstadosAplica();
                dto.EstadosAplicaID = item.EstadosAplicaID;
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.AsocTipoTramiteOpcionesID = item.AsocTipoTramiteOpcionesID;
                dto.IsSiguiente = item.IsSiguiente;

                dto.AsocTipoTramiteOpciones = new DTO.Models.AsocTipoTramiteOpciones();
                dto.AsocTipoTramiteOpciones.AsocTipoTramiteOpcionesID = item.AsocTipoTramiteOpciones.AsocTipoTramiteOpcionesID;
                dto.AsocTipoTramiteOpciones.TipoTramiteID = item.AsocTipoTramiteOpciones.TipoTramiteID;
                dto.AsocTipoTramiteOpciones.OpcionesTramiteID = item.AsocTipoTramiteOpciones.OpcionesTramiteID;
                dto.AsocTipoTramiteOpciones.IsTabla = item.AsocTipoTramiteOpciones.IsTabla;
                dto.AsocTipoTramiteOpciones.PlazoDias = item.AsocTipoTramiteOpciones.PlazoDias;
                dto.AsocTipoTramiteOpciones.IsDiasHabiles = item.AsocTipoTramiteOpciones.IsDiasHabiles;
                dto.AsocTipoTramiteOpciones.IsPermiteFechaAnterior = item.AsocTipoTramiteOpciones.IsPermiteFechaAnterior;
                dto.AsocTipoTramiteOpciones.IsInformaAtraso = item.AsocTipoTramiteOpciones.IsInformaAtraso;
                dto.AsocTipoTramiteOpciones.NumeroFirmas = item.AsocTipoTramiteOpciones.NumeroFirmas;
                dto.AsocTipoTramiteOpciones.Status1 = item.AsocTipoTramiteOpciones.Status1;
                dto.AsocTipoTramiteOpciones.Status2 = item.AsocTipoTramiteOpciones.Status2;
                dto.AsocTipoTramiteOpciones.Vigente = item.AsocTipoTramiteOpciones.Vigente;
                dto.AsocTipoTramiteOpciones.IsFinalizaIngreso = item.AsocTipoTramiteOpciones.IsFinalizaIngreso;
                dto.AsocTipoTramiteOpciones.EstadoCausaID = item.AsocTipoTramiteOpciones.EstadoCausaID;

                dto.AsocTipoTramiteOpciones.OpcionesTramite = new DTO.Models.OpcionesTramite();
                dto.AsocTipoTramiteOpciones.OpcionesTramite.OpcionesTramiteID = item.AsocTipoTramiteOpciones.OpcionesTramite.OpcionesTramiteID;
                dto.AsocTipoTramiteOpciones.OpcionesTramite.Descripcion = item.AsocTipoTramiteOpciones.OpcionesTramite.Descripcion;
                dto.AsocTipoTramiteOpciones.OpcionesTramite.Vigente = item.AsocTipoTramiteOpciones.OpcionesTramite.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;

        }

        public IList<DTO.Models.AsocEscritoDocto> GetAsocEscritoDocto(int expedienteID)
        {
            IList<AsocEscritoDocto> repoList = repo.GetAsocEscritoDocto(expedienteID);
            IList<DTO.Models.AsocEscritoDocto> listDTO = new List<DTO.Models.AsocEscritoDocto>();

            foreach (var item in repoList)
            {
                DTO.Models.AsocEscritoDocto dto = new DTO.Models.AsocEscritoDocto();
                dto.AsocEscritoDoctoID = item.AsocEscritoDoctoID;
                dto.AsocCausaDocumentoID = item.AsocCausaDocumentoID;
                dto.ExpedienteID = item.ExpedienteID;
                dto.OpcionesTramiteID = item.OpcionesTramiteID;

                dto.AsocCausaDocumento = new DTO.Models.AsocCausaDocumento();
                dto.AsocCausaDocumento.AsocCausaDocumentoID = item.AsocCausaDocumento.AsocCausaDocumentoID;
                dto.AsocCausaDocumento.DocumentoAdjuntoID = item.AsocCausaDocumento.DocumentoAdjuntoID;
                dto.AsocCausaDocumento.DocumentoCausaID = item.AsocCausaDocumento.DocumentoCausaID;
                dto.AsocCausaDocumento.CompromisoID = item.AsocCausaDocumento.CompromisoID;

                dto.AsocCausaDocumento.DocumentoCausa = new DTO.Models.DocumentoCausa();
                dto.AsocCausaDocumento.DocumentoCausa.DocumentoCausaID = item.AsocCausaDocumento.DocumentoCausa.DocumentoCausaID;
                dto.AsocCausaDocumento.DocumentoCausa.CausaID = item.AsocCausaDocumento.DocumentoCausa.CausaID;
                dto.AsocCausaDocumento.DocumentoCausa.VersionEncriptID = item.AsocCausaDocumento.DocumentoCausa.VersionEncriptID;
                dto.AsocCausaDocumento.DocumentoCausa.Hash = item.AsocCausaDocumento.DocumentoCausa.Hash.Trim();
                dto.AsocCausaDocumento.DocumentoCausa.NombreArchivoFisico = item.AsocCausaDocumento.DocumentoCausa.NombreArchivoFisico.Trim();
                dto.AsocCausaDocumento.DocumentoCausa.Fecha = item.AsocCausaDocumento.DocumentoCausa.Fecha;
                dto.AsocCausaDocumento.DocumentoCausa.Descripcion = item.AsocCausaDocumento.DocumentoCausa.Descripcion.Trim();

                dto.AsocCausaDocumento.DocumentoCausa.VersionEncript = new DTO.Models.VersionEncript();
                dto.AsocCausaDocumento.DocumentoCausa.VersionEncript.VersionEncriptID = item.AsocCausaDocumento.DocumentoCausa.VersionEncript.VersionEncriptID;
                dto.AsocCausaDocumento.DocumentoCausa.VersionEncript.Cadena = item.AsocCausaDocumento.DocumentoCausa.VersionEncript.Cadena.Trim();

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public void BorrarFirmasExpediente(int expedienteID)
        {
            repo.BorrarFirmasExpediente(expedienteID);
        }
        public void BorrarFirmaByFirmaID(int FirmaID)
        {
            repo.BorrarFirmaByFirmaID(FirmaID);
        }

        public void BorrarFirmaByAsocDocSistema(int AsocDocSistemaFirmaID)
        {
            repo.BorrarFirmaByAsocDocSistema(AsocDocSistemaFirmaID);
        }

        public int SaveFirma(DTO.Models.Firma dto)
        {
            Firma model = new Firma();
            model.FirmaID = dto.FirmaID;
            model.UsuarioID = dto.UsuarioID;
            model.Orden = dto.Orden;

            return repo.SaveFirma(model);
        }

        public int SaveAsocFirmaDocto(DTO.Models.AsocFirmaDocto dto)
        {
            AsocFirmaDocto model = new AsocFirmaDocto();
            model.AsocFirmaDoctoID = dto.AsocFirmaDoctoID;
            model.FirmaID = dto.FirmaID;
            model.AsocEscritoDoctoID = dto.AsocEscritoDoctoID;
            model.IsFirmado = dto.IsFirmado;
            model.IsTomado = dto.IsTomado;
            model.FechaTomado = dto.FechaTomado;

            return repo.SaveAsocFirmaDocto(model);
        }
        public int SaveAsocFirmaDoctoMarcarToma(DTO.Models.AsocFirmaDocto dto)
        {
            AsocFirmaDocto model = new AsocFirmaDocto();
            model.AsocFirmaDoctoID = dto.AsocFirmaDoctoID;
            model.IsTomado = dto.IsTomado;
            model.FechaTomado = dto.FechaTomado;

            return repo.SaveAsocFirmaDoctoMarcarToma(model);
        }


        public DTO.Models.AsocFirmaDocto GetAsocFirmaDoctoGS(int expedienteID, int UsuarioID)
        {
            AsocFirmaDocto model = repo.GetAsocFirmaDoctoGS(expedienteID, UsuarioID);
            DTO.Models.AsocFirmaDocto dto = new DTO.Models.AsocFirmaDocto();

            if (model.AsocFirmaDoctoID > 0)
            {
                dto.AsocFirmaDoctoID = model.AsocFirmaDoctoID;
                dto.FirmaID = model.FirmaID;
                dto.AsocEscritoDoctoID = model.AsocEscritoDoctoID;
                dto.IsFirmado = model.IsFirmado;
                dto.IsTomado = model.IsTomado;
                dto.FechaTomado = model.FechaTomado;


                #region AsocEscritoDocto
                dto.AsocEscritoDocto = new DTO.Models.AsocEscritoDocto();
                dto.AsocEscritoDocto.AsocEscritoDoctoID = model.AsocEscritoDocto.AsocEscritoDoctoID;
                dto.AsocEscritoDocto.AsocCausaDocumentoID = model.AsocEscritoDocto.AsocCausaDocumentoID;
                dto.AsocEscritoDocto.ExpedienteID = model.AsocEscritoDocto.ExpedienteID;
                dto.AsocEscritoDocto.OpcionesTramiteID = model.AsocEscritoDocto.OpcionesTramiteID;

                #endregion

                dto.Firma = new DTO.Models.Firma();
                dto.Firma.FirmaID = model.Firma.FirmaID;
                dto.Firma.UsuarioID = model.Firma.UsuarioID;
                dto.Firma.Orden = model.Firma.Orden;

                #region Usuario
                dto.Firma.Usuario = new DTO.Models.Usuario();
                dto.Firma.Usuario.UsuarioID = model.Firma.Usuario.UsuarioID;
                dto.Firma.Usuario.AdID = model.Firma.Usuario.AdID;
                dto.Firma.Usuario.Rut = model.Firma.Usuario.Rut;
                dto.Firma.Usuario.Nombres = model.Firma.Usuario.Nombres;
                dto.Firma.Usuario.Apellidos = model.Firma.Usuario.Apellidos;
                dto.Firma.Usuario.Mail = model.Firma.Usuario.Mail;
                dto.Firma.Usuario.Telefono = model.Firma.Usuario.Telefono;
                dto.Firma.Usuario.IsClaveUnica = model.Firma.Usuario.IsClaveUnica;
                dto.Firma.Usuario.FechaRegistro = model.Firma.Usuario.FechaRegistro;
                dto.Firma.Usuario.FechaModificacion = model.Firma.Usuario.FechaModificacion;
                #endregion
            }

            return dto;
        }

        public IList<DTO.Models.AsocFirmaDocto> GetListAsocFirmaDoctoGS(int expedienteID)
        {
            IList<AsocFirmaDocto> repoList = repo.GetListAsocFirmaDoctoGS(expedienteID);
            IList<DTO.Models.AsocFirmaDocto> listDTO = new List<DTO.Models.AsocFirmaDocto>();

            foreach (var item in repoList)
            {
                DTO.Models.AsocFirmaDocto dto = new DTO.Models.AsocFirmaDocto();

                dto.AsocFirmaDoctoID = item.AsocFirmaDoctoID;
                dto.FirmaID = item.FirmaID;
                dto.AsocEscritoDoctoID = item.AsocEscritoDoctoID;
                dto.IsFirmado = item.IsFirmado;
                dto.IsTomado = item.IsTomado;
                dto.FechaTomado = item.FechaTomado;

                #region AsocEscritoDocto
                dto.AsocEscritoDocto = new DTO.Models.AsocEscritoDocto();
                dto.AsocEscritoDocto.AsocEscritoDoctoID = item.AsocEscritoDocto.AsocEscritoDoctoID;
                dto.AsocEscritoDocto.AsocCausaDocumentoID = item.AsocEscritoDocto.AsocCausaDocumentoID;
                dto.AsocEscritoDocto.ExpedienteID = item.AsocEscritoDocto.ExpedienteID;
                dto.AsocEscritoDocto.OpcionesTramiteID = item.AsocEscritoDocto.OpcionesTramiteID;
                #endregion

                dto.Firma = new DTO.Models.Firma();
                dto.Firma.FirmaID = item.Firma.FirmaID;
                dto.Firma.UsuarioID = item.Firma.UsuarioID;
                dto.Firma.Orden = item.Firma.Orden;

                #region Usuario
                dto.Firma.Usuario = new DTO.Models.Usuario();
                dto.Firma.Usuario.UsuarioID = item.Firma.Usuario.UsuarioID;
                dto.Firma.Usuario.AdID = item.Firma.Usuario.AdID;
                dto.Firma.Usuario.Rut = item.Firma.Usuario.Rut;
                dto.Firma.Usuario.Nombres = item.Firma.Usuario.Nombres;
                dto.Firma.Usuario.Apellidos = item.Firma.Usuario.Apellidos;
                dto.Firma.Usuario.Mail = item.Firma.Usuario.Mail;
                dto.Firma.Usuario.Telefono = item.Firma.Usuario.Telefono;
                dto.Firma.Usuario.IsClaveUnica = item.Firma.Usuario.IsClaveUnica;
                dto.Firma.Usuario.FechaRegistro = item.Firma.Usuario.FechaRegistro;
                dto.Firma.Usuario.FechaModificacion = item.Firma.Usuario.FechaModificacion;
                #endregion


                listDTO.Add(dto);
            }

            return listDTO;
        }

        public DTO.Models.AsocDocSistemaFirma GetAsocDocSistemaFirmaByFirmaDocto(int FirmaID, int DocumentoSistemaID)
        {
            AsocDocSistemaFirma model = repo.GetAsocDocSistemaFirma(FirmaID, DocumentoSistemaID);
            DTO.Models.AsocDocSistemaFirma dto = new DTO.Models.AsocDocSistemaFirma();

            if (model.AsocDocSistemaFirmaID > 0)
            {
                dto.AsocDocSistemaFirmaID = model.AsocDocSistemaFirmaID;
                dto.DocumentoSistemaID = model.DocumentoSistemaID;
                dto.FirmaID = model.FirmaID;
                dto.IsFirmado = model.IsFirmado;

                dto.Firma = new DTO.Models.Firma();
                dto.Firma.FirmaID = model.Firma.FirmaID;
                dto.Firma.UsuarioID = model.Firma.UsuarioID;
                dto.Firma.Orden = model.Firma.Orden;

                dto.DocumentoSistema = new DTO.Models.DocumentoSistema();
                dto.DocumentoSistema.DocumentoSistemaID = model.DocumentoSistema.DocumentoSistemaID;
                dto.DocumentoSistema.VersionEncriptID = model.DocumentoSistema.VersionEncriptID;
                dto.DocumentoSistema.TipoDocumentoID = model.DocumentoSistema.TipoDocumentoID;
                dto.DocumentoSistema.Hash = model.DocumentoSistema.Hash.Trim();
                dto.DocumentoSistema.NombreArchivoFisico = model.DocumentoSistema.NombreArchivoFisico.Trim();
                dto.DocumentoSistema.Fecha = model.DocumentoSistema.Fecha;
                dto.DocumentoSistema.Descripcion = model.DocumentoSistema.Descripcion.Trim();

                dto.DocumentoSistema.VersionEncript = new DTO.Models.VersionEncript();
                dto.DocumentoSistema.VersionEncript.VersionEncriptID = model.DocumentoSistema.VersionEncript.VersionEncriptID;
                dto.DocumentoSistema.VersionEncript.Cadena = model.DocumentoSistema.VersionEncript.Cadena.Trim();

            }

            return dto;
        }

        public int SaveAsocDocSistemaFirma(DTO.Models.AsocDocSistemaFirma dto)
        {
            AsocDocSistemaFirma model = new AsocDocSistemaFirma();
            model.AsocDocSistemaFirmaID = dto.AsocDocSistemaFirmaID;
            model.FirmaID = dto.FirmaID;
            model.DocumentoSistemaID = dto.DocumentoSistemaID;
            model.IsFirmado = dto.IsFirmado;

            return repo.SaveAsocDocSistemaFirma(model);
        }

        public void DeleteParteByID(int parteID)
        {
            repo.DeleteParteByID(parteID);
        }

        public void SetVigenciaDetalleTabla(int detalleTablaID, bool vigencia)
        {
            repo.SetVigenciaDetalleTabla(detalleTablaID, vigencia);
        }

        public void SetEstadoTabla(int tablaID, Domain.Infrastructure.EstadoTabla estado)
        {
            repo.SetEstadoTabla(tablaID, estado);
        }

        public int SaveTabla(DTO.Models.Tabla dto)
        {
            Tabla model = new Tabla();
            model.TablaID = dto.TablaID;
            model.EstadoTablaID = dto.EstadoTablaID;
            model.TipoTablaID = dto.TipoTablaID;
            model.SalaID = dto.SalaID;
            model.Fecha = dto.Fecha;
            model.UsuarioRelatorID = dto.UsuarioRelatorID;
            model.UsuarioSubroganteID = dto.UsuarioSubroganteID;

            return repo.SaveTabla(model);
        }

        public DTO.Models.Tabla GetTablaByID(int tablaID)
        {
            Tabla model = repo.GetTablaByID(tablaID);

            DTO.Models.Tabla dto = new DTO.Models.Tabla();
            dto.TablaID = model.TablaID;
            dto.EstadoTablaID = model.EstadoTablaID;
            dto.TipoTablaID = model.TipoTablaID;
            dto.SalaID = model.SalaID;
            dto.Fecha = model.Fecha;
            dto.UsuarioRelatorID = model.UsuarioRelatorID;
            dto.UsuarioSubroganteID = model.UsuarioSubroganteID;

            dto.Sala = new DTO.Models.Sala();
            dto.Sala.SalaID = model.Sala.SalaID;
            dto.Sala.Descripcion = model.Sala.Descripcion.Trim();
            dto.Sala.Vigente = model.Sala.Vigente;

            dto.TipoTabla = new DTO.Models.TipoTabla();
            dto.TipoTabla.TipoTablaID = model.TipoTabla.TipoTablaID;
            dto.TipoTabla.Descripcion = model.TipoTabla.Descripcion.Trim();
            dto.TipoTabla.Vigente = model.TipoTabla.Vigente;

            dto.EstadoTabla = new DTO.Models.EstadoTabla();
            dto.EstadoTabla.EstadoTablaID = model.EstadoTabla.EstadoTablaID;
            dto.EstadoTabla.Descripcion = model.EstadoTabla.Descripcion.Trim();
            dto.EstadoTabla.Vigente = model.EstadoTabla.Vigente;

            dto.DetalleTabla = new List<DTO.Models.DetalleTabla>();
            foreach (var item in model.DetalleTabla)
            {
                DTO.Models.DetalleTabla add = new DTO.Models.DetalleTabla();
                add.DetalleTablaID = item.DetalleTablaID;
                add.CausaID = item.CausaID;
                add.TablaID = item.TablaID;
                add.Orden = item.Orden;
                add.Vigente = item.Vigente;

                dto.DetalleTabla.Add(add);
            }

            return dto;
        }

        public IList<DTO.Models.Tabla> GetTabla(DTO.FiltrosEscritorio filtros)
        {
            IList<Tabla> repoList = repo.GetTabla(MapFilter(filtros));

            IList<DTO.Models.Tabla> listDTO = new List<DTO.Models.Tabla>();

            foreach (var item in repoList)
            {
                DTO.Models.Tabla dto = new DTO.Models.Tabla();
                dto.TablaID = item.TablaID;
                dto.EstadoTablaID = item.EstadoTablaID;
                dto.TipoTablaID = item.TipoTablaID;
                dto.SalaID = item.SalaID;
                dto.Fecha = item.Fecha;
                dto.UsuarioRelatorID = item.UsuarioRelatorID;
                dto.UsuarioSubroganteID = item.UsuarioSubroganteID;

                dto.Sala = new DTO.Models.Sala();
                dto.Sala.SalaID = item.Sala.SalaID;
                dto.Sala.Descripcion = item.Sala.Descripcion.Trim();
                dto.Sala.Vigente = item.Sala.Vigente;

                dto.TipoTabla = new DTO.Models.TipoTabla();
                dto.TipoTabla.TipoTablaID = item.TipoTabla.TipoTablaID;
                dto.TipoTabla.Descripcion = item.TipoTabla.Descripcion.Trim();
                dto.TipoTabla.Vigente = item.TipoTabla.Vigente;

                dto.EstadoTabla = new DTO.Models.EstadoTabla();
                dto.EstadoTabla.EstadoTablaID = item.EstadoTabla.EstadoTablaID;
                dto.EstadoTabla.Descripcion = item.EstadoTabla.Descripcion.Trim();
                dto.EstadoTabla.Vigente = item.EstadoTabla.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public bool GetExisteTabla(DateTime fecha)
        {
            return repo.GetExisteTabla(fecha);
        }

        public IList<DTO.Models.DetalleTabla> GetDetalleTablaByCausa(int causaID)
        {
            IList<DetalleTabla> repoList = repo.GetDetalleTablaByCausa(causaID);

            IList<DTO.Models.DetalleTabla> listDTO = new List<DTO.Models.DetalleTabla>();

            foreach (var item in repoList)
            {
                DTO.Models.DetalleTabla dto = new DTO.Models.DetalleTabla();
                dto.DetalleTablaID = item.DetalleTablaID;
                dto.CausaID = item.CausaID;
                dto.TablaID = item.TablaID;
                dto.Orden = item.Orden;
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveDetalleTabla(DTO.Models.DetalleTabla dto, bool SetLastOrder)
        {
            DetalleTabla model = new DetalleTabla();
            model.DetalleTablaID = dto.DetalleTablaID;
            model.CausaID = dto.CausaID;
            model.TablaID = dto.TablaID;
            model.Orden = dto.Orden;
            model.Vigente = dto.Vigente;

            return repo.SaveDetalleTabla(model, SetLastOrder);
        }

        public int SaveAsocDocumentoSistemaTabla(DTO.Models.AsocDocumentoSistemaTabla dto)
        {
            AsocDocumentoSistemaTabla model = new AsocDocumentoSistemaTabla();
            model.AsocDocumentoSistemaTablaID = dto.AsocDocumentoSistemaTablaID;
            model.TablaID = dto.TablaID;
            model.DocumentoSistemaID = dto.DocumentoSistemaID;

            return repo.SaveAsocDocumentoSistemaTabla(model);
        }

        public IList<DTO.Models.AsocDocumentoSistemaTabla> GetAsocDocumentoSistemaTabla(int TablaID) {

            IList<DTO.Models.AsocDocumentoSistemaTabla> listDTO = new List<DTO.Models.AsocDocumentoSistemaTabla>();
            IList<AsocDocumentoSistemaTabla> repoList = repo.GetAsocDocumentoSistemaTabla(TablaID);

            foreach (var item in repoList)
            {
                DTO.Models.AsocDocumentoSistemaTabla dto = new DTO.Models.AsocDocumentoSistemaTabla();
                dto.AsocDocumentoSistemaTablaID = item.AsocDocumentoSistemaTablaID;
                dto.TablaID = item.TablaID;
                dto.DocumentoSistemaID= item.DocumentoSistemaID;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.AsocDocumentoSistemaEstadoDiario> GetAsocDocumentoSistemaEstadoDiario(int EstadoDiarioID)
        {
            IList<DTO.Models.AsocDocumentoSistemaEstadoDiario> listDTO = new List<DTO.Models.AsocDocumentoSistemaEstadoDiario>();
            IList<AsocDocumentoSistemaEstadoDiario> repoList = repo.GetAsocDocumentoSistemaEstadoDiario(EstadoDiarioID);

            foreach (var item in repoList)
            {
                DTO.Models.AsocDocumentoSistemaEstadoDiario dto = new DTO.Models.AsocDocumentoSistemaEstadoDiario();
                dto.AsocDocumentoSistemaEstadoDiarioID = item.AsocDocumentoSistemaEstadoDiarioID;
                dto.DocumentoSistemaID = item.DocumentoSistemaID;
                dto.EstadoDiarioID = item.EstadoDiarioID;

                listDTO.Add(dto);
            }
            return listDTO;
        }


        public IList<DTO.Models.AsocDocSistemaFirma> GetAsocDocSistemaFirmaByDocto(int DocumentoSistemaID)
        {
            IList<DTO.Models.AsocDocSistemaFirma> listDTO = new List<DTO.Models.AsocDocSistemaFirma>();
            IList<AsocDocSistemaFirma> repoList = repo.GetAsocDocSistemaFirmaByDocto(DocumentoSistemaID);

            foreach (var item in repoList)
            {
                DTO.Models.AsocDocSistemaFirma dto = new DTO.Models.AsocDocSistemaFirma();
                dto.AsocDocSistemaFirmaID = item.AsocDocSistemaFirmaID;
                dto.DocumentoSistemaID = item.DocumentoSistemaID;
                dto.FirmaID = item.FirmaID;
                dto.IsFirmado = item.IsFirmado;

                dto.Firma = new DTO.Models.Firma();
                dto.Firma.FirmaID = item.Firma.FirmaID;
                dto.Firma.UsuarioID = item.Firma.UsuarioID;
                dto.Firma.UsuarioFirmaID = item.Firma.UsuarioID;
                dto.Firma.Orden= item.Firma.Orden;

                dto.Firma.Usuario = new DTO.Models.Usuario();
                dto.Firma.Usuario = appCommon.GetUsuarioByID(item.Firma.UsuarioID);

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveAsocDocumentoSistemaEstadoDiario(DTO.Models.AsocDocumentoSistemaEstadoDiario dto)
        {
            AsocDocumentoSistemaEstadoDiario model = new AsocDocumentoSistemaEstadoDiario();
            model.AsocDocumentoSistemaEstadoDiarioID = dto.AsocDocumentoSistemaEstadoDiarioID;
            model.EstadoDiarioID = dto.EstadoDiarioID;
            model.DocumentoSistemaID = dto.DocumentoSistemaID;

            return repo.SaveAsocDocumentoSistemaEstadoDiario(model);
        }

        public IList<DTO.Models.DocumentoSistema> GetAsocDocumentoSistema(int Identidad, Domain.Infrastructure.TipoDocumento tipoDoc)
        {
            IList<DocumentoSistema> repoList = repo.GetAsocDocumentoSistema(Identidad, tipoDoc);

            IList<DTO.Models.DocumentoSistema> listDTO = new List<DTO.Models.DocumentoSistema>();

            foreach (var item in repoList)
            {
                DTO.Models.DocumentoSistema dto = new DTO.Models.DocumentoSistema();
                dto.DocumentoSistemaID = item.DocumentoSistemaID;
                dto.VersionEncriptID = item.VersionEncriptID;
                dto.TipoDocumentoID = item.TipoDocumentoID;
                dto.Hash = item.Hash.Trim();
                dto.NombreArchivoFisico = item.NombreArchivoFisico;
                dto.Fecha = item.Fecha;
                dto.Descripcion = item.Descripcion.Trim();

                dto.VersionEncript = new DTO.Models.VersionEncript();
                dto.VersionEncript.VersionEncriptID = item.VersionEncript.VersionEncriptID;
                dto.VersionEncript.Cadena = item.VersionEncript.Cadena.Trim();

                dto.AsocDocSistemaFirma = new List<DTO.Models.AsocDocSistemaFirma>();
                if (item.AsocDocSistemaFirma != null && item.AsocDocSistemaFirma.Count > 0 )
                {
                    foreach (var asoc in item.AsocDocSistemaFirma)
                    {
                        DTO.Models.AsocDocSistemaFirma _addAsoc = new DTO.Models.AsocDocSistemaFirma();
                        _addAsoc.AsocDocSistemaFirmaID = asoc.AsocDocSistemaFirmaID;
                        _addAsoc.DocumentoSistemaID = asoc.DocumentoSistemaID;
                        _addAsoc.FirmaID = asoc.FirmaID;
                        _addAsoc.IsFirmado = asoc.IsFirmado;

                        _addAsoc.Firma = new DTO.Models.Firma();
                        _addAsoc.Firma.FirmaID = asoc.Firma.FirmaID;
                        _addAsoc.Firma.UsuarioID = asoc.Firma.UsuarioID;
                        _addAsoc.Firma.Orden = asoc.Firma.Orden;

                        dto.AsocDocSistemaFirma.Add(_addAsoc);
                    }
                }


                listDTO.Add(dto);
            }

            return listDTO;
        }



        public int SaveEstadoDiario(DTO.Models.EstadoDiario dto)
        {
            EstadoDiario model = new EstadoDiario();
            model.EstadoDiarioID = dto.EstadoDiarioID;
            model.TipoEstadoDiarioID = dto.TipoEstadoDiarioID;
            model.Fecha = dto.Fecha;

            return repo.SaveEstadoDiario(model);
        }

        public void SetTipoEstadoDiario(int estadoDiarioID, Domain.Infrastructure.TipoEstadoDiario estado)
        {
            repo.SetTipoEstadoDiario(estadoDiarioID, estado);
        }

        public void SetVigenciaDetalleEstadoDiario(int detalleEstadoDiarioID, bool vigencia)
        {
            repo.SetVigenciaDetalleEstadoDiario(detalleEstadoDiarioID, vigencia);
        }

        public DTO.Models.EstadoDiario GetEstadoDiarioByID(int estadoDiarioID)
        {
            EstadoDiario model = repo.GetEstadoDiarioByID(estadoDiarioID);

            DTO.Models.EstadoDiario dto = new DTO.Models.EstadoDiario();
            dto.EstadoDiarioID = model.EstadoDiarioID;
            dto.Fecha = model.Fecha;
            dto.TipoEstadoDiarioID = model.TipoEstadoDiarioID;

            dto.TipoEstadoDiario = new DTO.Models.TipoEstadoDiario();
            dto.TipoEstadoDiario.TipoEstadoDiarioID = model.TipoEstadoDiario.TipoEstadoDiarioID;
            dto.TipoEstadoDiario.Descripcion = model.TipoEstadoDiario.Descripcion.Trim();
            dto.TipoEstadoDiario.Vigente = model.TipoEstadoDiario.Vigente;

            dto.DetalleEstadoDiario = new List<DTO.Models.DetalleEstadoDiario>();
            foreach (var item in model.DetalleEstadoDiario)
            {
                DTO.Models.DetalleEstadoDiario add = new DTO.Models.DetalleEstadoDiario();
                add.DetalleEstadoDiarioID = item.DetalleEstadoDiarioID;
                add.ExpedienteID = item.ExpedienteID;
                add.EstadoDiarioID = item.EstadoDiarioID;
                add.Vigente = item.Vigente;

                dto.DetalleEstadoDiario.Add(add);
            }

            return dto;
        }

        public IList<DTO.Models.EstadoDiario> GetEstadoDiario(DTO.FiltrosEscritorio filtros)
        {
            IList<EstadoDiario> repoList = repo.GetEstadoDiario(MapFilter(filtros));

            IList<DTO.Models.EstadoDiario> listDTO = new List<DTO.Models.EstadoDiario>();

            foreach (var item in repoList)
            {
                DTO.Models.EstadoDiario dto = new DTO.Models.EstadoDiario();
                dto.EstadoDiarioID = item.EstadoDiarioID;
                dto.Fecha = item.Fecha;
                dto.TipoEstadoDiarioID = item.TipoEstadoDiarioID;

                dto.TipoEstadoDiario = new DTO.Models.TipoEstadoDiario();
                dto.TipoEstadoDiario.TipoEstadoDiarioID = item.TipoEstadoDiario.TipoEstadoDiarioID;
                dto.TipoEstadoDiario.Descripcion = item.TipoEstadoDiario.Descripcion.Trim();
                dto.TipoEstadoDiario.Vigente = item.TipoEstadoDiario.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveDetalleEstadoDiario(DTO.Models.DetalleEstadoDiario dto)
        {
            DetalleEstadoDiario model = new DetalleEstadoDiario();
            model.DetalleEstadoDiarioID = dto.DetalleEstadoDiarioID;
            model.ExpedienteID = dto.ExpedienteID;
            model.EstadoDiarioID = dto.EstadoDiarioID;
            model.Vigente = dto.Vigente;

            return repo.SaveDetalleEstadoDiario(model);
        }

        public int SaveAsocExpeFirma(DTO.Models.AsocExpeFirma dto)
        {
            AsocExpeFirma model = new AsocExpeFirma();
            model.AsocExpeFirmaID = dto.AsocExpeFirmaID;
            model.FirmaID = dto.FirmaID;
            model.ExpedienteID = dto.ExpedienteID;

            return repo.SaveAsocExpeFirma(model);
        }

        public void UpdateResponsable(int expedienteID, int usuarioID)
        {
            repo.UpdateResponsable(expedienteID, usuarioID);
        }

        public IList<DTO.Models.DetalleEstadoDiario> GetDetalleEstadoDiarioByExpediente(int expedienteID)
        {
            IList<DetalleEstadoDiario> repoList = repo.GetDetalleEstadoDiarioByExpediente(expedienteID);

            IList<DTO.Models.DetalleEstadoDiario> listDTO = new List<DTO.Models.DetalleEstadoDiario>();

            foreach (var item in repoList)
            {
                DTO.Models.DetalleEstadoDiario dto = new DTO.Models.DetalleEstadoDiario();
                dto.DetalleEstadoDiarioID = item.DetalleEstadoDiarioID;
                dto.ExpedienteID = item.ExpedienteID;
                dto.EstadoDiarioID = item.EstadoDiarioID;
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public void SetExpedienteFinalizado(int expedienteID, bool finalizar)
        {
            repo.SetExpedienteFinalizado(expedienteID, finalizar);
        }

        public void SetExpedienteInadmisible(int expedienteID)
        {
            repo.SetExpedienteInadmisible(expedienteID);
        }

        public IList<DTO.Models.AsocExpeFirma> GetAsocExpeFirmaByExpedienteID(int expedienteID)
        {
            IList<AsocExpeFirma> repoList = repo.GetAsocExpeFirmaByExpedienteID(expedienteID);

            IList<DTO.Models.AsocExpeFirma> listDTO = new List<DTO.Models.AsocExpeFirma>();

            foreach (var item in repoList)
            {
                DTO.Models.AsocExpeFirma dto = new DTO.Models.AsocExpeFirma();
                dto.AsocExpeFirmaID = item.AsocExpeFirmaID;
                dto.FirmaID = item.FirmaID;
                dto.AsocExpeFirmaID = item.ExpedienteID;

                dto.Firma = new DTO.Models.Firma();
                dto.Firma.FirmaID = item.Firma.FirmaID;
                dto.Firma.UsuarioID = item.Firma.UsuarioID;
                dto.Firma.Orden = item.Firma.Orden;

                dto.Firma.Usuario = new DTO.Models.Usuario();
                dto.Firma.Usuario.UsuarioID = item.Firma.Usuario.UsuarioID;
                dto.Firma.Usuario.AdID = item.Firma.Usuario.AdID;
                dto.Firma.Usuario.Rut = item.Firma.Usuario.Rut;
                dto.Firma.Usuario.Nombres = item.Firma.Usuario.Nombres;
                dto.Firma.Usuario.Apellidos = item.Firma.Usuario.Apellidos;
                dto.Firma.Usuario.Mail = item.Firma.Usuario.Mail;
                dto.Firma.Usuario.Telefono = item.Firma.Usuario.Telefono;
                dto.Firma.Usuario.IsClaveUnica = item.Firma.Usuario.IsClaveUnica;
                dto.Firma.Usuario.FechaRegistro = item.Firma.Usuario.FechaRegistro;
                dto.Firma.Usuario.FechaModificacion = item.Firma.Usuario.FechaModificacion;

                dto.Firma.AsocFirmaDocto = new List<DTO.Models.AsocFirmaDocto>();
                foreach (var asoc in item.Firma.AsocFirmaDocto)
                {
                    DTO.Models.AsocFirmaDocto add = new DTO.Models.AsocFirmaDocto();
                    add.AsocFirmaDoctoID = asoc.AsocFirmaDoctoID;
                    add.FirmaID = asoc.FirmaID;
                    add.AsocEscritoDoctoID = asoc.AsocEscritoDoctoID;
                    add.IsFirmado = asoc.IsFirmado;                 

                    dto.Firma.AsocFirmaDocto.Add(add);
                }

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveDerivacion(DTO.Models.Derivacion dto)
        {
            Derivacion model = new Derivacion();
            model.DerivacionID = dto.DerivacionID;
            model.ExpedienteID = dto.ExpedienteID;
            model.UsuarioID = dto.UsuarioID;
            model.UsuarioResponsableID = dto.UsuarioResponsableID;
            model.Observacion = dto.Observacion;
            model.PlazoDias = dto.PlazoDias;
            model.Fecha = dto.Fecha;

            return repo.SaveDerivacion(model);
        }
}
}
