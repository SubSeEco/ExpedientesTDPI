using System;
using System.Collections.Generic;
using System.Linq;
using Persistence.Repository;
using AutoMapper;
using LanguageEnum = Domain.Infrastructure.LanguageEnum;



namespace Application.Services
{
    public class CommonAppServices : ICommonAppServices
    {
        private readonly ICommonRepository repo = new CommonRepository();

        #region VersionEncript

        public DTO.Models.VersionEncript GetLastVersionEncript()
        {
            VersionEncript enc = repo.GetLastVersion();

            DTO.Models.VersionEncript version = new DTO.Models.VersionEncript();

            version.VersionEncriptID = enc.VersionEncriptID;
            version.Cadena = enc.Cadena.Trim();

            return version;
        }

        public DTO.Models.VersionEncript GetFirstVersionEncript()
        {
            VersionEncript enc = repo.GetFirstVersion();

            DTO.Models.VersionEncript version = new DTO.Models.VersionEncript();

            version.VersionEncriptID = enc.VersionEncriptID;
            version.Cadena = enc.Cadena.Trim();

            return version;
        }

        public DTO.Models.VersionEncript GetVersionEncriptById(int id)
        {
            VersionEncript enc = repo.GetVersionEncriptById(id);

            DTO.Models.VersionEncript version = new DTO.Models.VersionEncript();

            version.VersionEncriptID = enc.VersionEncriptID;
            version.Cadena = enc.Cadena.Trim();

            return version;
        }

        #endregion

        #region Uploads
        public IList<DTO.Models.TipoFormato> GetTipoFormato()
        {
            IList<TipoFormato> TipoFormato = repo.GetTipoFormato();

            IList<DTO.Models.TipoFormato> _dtoList = new List<DTO.Models.TipoFormato>();

            foreach (var item in TipoFormato)
            {
                DTO.Models.TipoFormato dto = new DTO.Models.TipoFormato();
                dto.TipoFormatoID = item.TipoFormatoID;
                dto.Descripcion = item.Descripcion;
                dto.Vigente = item.Vigente;
                dto.ExtraCss = item.ExtraCss;

                dto.FamiliasMimeType = new List<DTO.Models.FamiliasMimeType>();
                foreach (var mt in item.FamiliasMimeType)
                {
                    Mapper.CreateMap<FamiliasMimeType, DTO.Models.FamiliasMimeType>().ForMember(d => d.TipoFormato, o => o.Ignore());
                    DTO.Models.FamiliasMimeType _dto = Mapper.Map<DTO.Models.FamiliasMimeType>(mt);

                    dto.FamiliasMimeType.Add(_dto);
                }

                _dtoList.Add(dto);
            }

            return _dtoList;
        }

        public IList<DTO.Models.TipoDocumento> GetTipoDocumento()
        {
            IList<TipoDocumento> repoList = repo.GetTipoDocumento();
            IList<DTO.Models.TipoDocumento> listDTO = new List<DTO.Models.TipoDocumento>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoDocumento dto = new DTO.Models.TipoDocumento();
                dto.TipoDocumentoID = item.TipoDocumentoID;
                dto.Descripcion = item.Descripcion;
                dto.Etiqueta = item.Etiqueta.Trim();
                dto.Visible = item.Visible;
                dto.Obligatorio = item.Obligatorio;
                dto.Cantidad = item.Cantidad;
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.MaximoTamanoArchivo> GetMaximoTamanoArchivo()
        {
            IList<MaximoTamanoArchivo> MaximoTamanoArchivo = repo.GetMaximoTamanoArchivo();

            IList<DTO.Models.MaximoTamanoArchivo> _dtoList = new List<DTO.Models.MaximoTamanoArchivo>();

            foreach (var item in MaximoTamanoArchivo)
            {
                DTO.Models.MaximoTamanoArchivo dto = new DTO.Models.MaximoTamanoArchivo();
                dto.MaximoTamanoArchivoID = item.MaximoTamanoArchivoID;
                dto.Tamano = item.Tamano;
                dto.Vigente = item.Vigente;

                _dtoList.Add(dto);
            }

            return _dtoList;
        }
        
        #endregion

        #region Mantenedor de MimeTypes

            public int SaveTipoFormato(DTO.Models.TipoFormato tipoFormato)
            {
                TipoFormato model = new TipoFormato();
                model.TipoFormatoID = tipoFormato.TipoFormatoID;
                model.Descripcion = tipoFormato.Descripcion.Trim();
                model.ExtraCss = tipoFormato.ExtraCss.Trim();
                model.Vigente = tipoFormato.Vigente;

                return repo.SaveTipoFormato(model);
            }
            public void DeleteTipoFormato(int TipoFormatoID)
            {
                repo.DeleteTipoFormato(TipoFormatoID);
            }

            public IList<DTO.Models.FamiliasMimeType> GetFamiliasMimeType()
            {

                IList<DTO.Models.FamiliasMimeType> listDTO = new List<DTO.Models.FamiliasMimeType>();
                IList<FamiliasMimeType> repoList = repo.GetFamiliasMimeType();

                foreach (var item in repoList)
                {
                    DTO.Models.FamiliasMimeType dto = new DTO.Models.FamiliasMimeType();
                    dto.FamiliasMimeTypeID = item.FamiliasMimeTypeID;
                    dto.TipoFormatoID = item.TipoFormatoID;
                    dto.Descripcion = item.Descripcion;
                    dto.Vigente = item.Vigente;

                    listDTO.Add(dto);
                }

                return listDTO;
            }

            public DTO.Models.FamiliasMimeType GetMimeType(int FamiliasMimeTypeID)
            {

                FamiliasMimeType result = repo.GetMimeType(FamiliasMimeTypeID);
                DTO.Models.FamiliasMimeType _mimeType = new DTO.Models.FamiliasMimeType();

                _mimeType.FamiliasMimeTypeID = result.FamiliasMimeTypeID;
                _mimeType.TipoFormatoID = result.TipoFormatoID;
                _mimeType.Descripcion = result.Descripcion;
                _mimeType.Vigente = result.Vigente;

                return _mimeType;
            }

            public int SaveMimeType(DTO.Models.FamiliasMimeType familiasMimeType)
            {
                FamiliasMimeType model = new FamiliasMimeType();
                model.FamiliasMimeTypeID = familiasMimeType.FamiliasMimeTypeID;
                model.TipoFormatoID = familiasMimeType.TipoFormatoID;
                model.Descripcion = familiasMimeType.Descripcion;
                model.Vigente = familiasMimeType.Vigente;

                return repo.SaveMimeType(model);
            }

            public void DeleteMimeType(int FamiliasMimeTypeID)
            {
                repo.DeleteMimeType(FamiliasMimeTypeID);
            }

        #endregion

        #region Documento Adjunto

        public IList<DTO.Models.AsocTipoDocumentoAdjunto> GetTipoDocumentoAdjuntoByID(int TipoDocumentoID)
        {
            IList<AsocTipoDocumentoAdjunto> lista = repo.GetTipoDocumentoAdjuntoByID(TipoDocumentoID);

            IList<DTO.Models.AsocTipoDocumentoAdjunto> dtoList = new List<DTO.Models.AsocTipoDocumentoAdjunto>();

            foreach (var model in lista)
            {
                DTO.Models.AsocTipoDocumentoAdjunto dto = new DTO.Models.AsocTipoDocumentoAdjunto();
                dto.AsocTipoDocumentoAdjuntoID = model.AsocTipoDocumentoAdjuntoID;
                dto.DocumentoAdjuntoID = model.DocumentoAdjuntoID;
                dto.TipoDocumentoID = model.TipoDocumentoID;

                dto.TipoDocumento = new DTO.Models.TipoDocumento();
                dto.TipoDocumento.TipoDocumentoID = model.TipoDocumento.TipoDocumentoID;
                dto.TipoDocumento.Descripcion = model.TipoDocumento.Descripcion.Trim();
                dto.TipoDocumento.Etiqueta = model.TipoDocumento.Etiqueta.Trim();
                dto.TipoDocumento.Visible = model.TipoDocumento.Visible;
                dto.TipoDocumento.Obligatorio = model.TipoDocumento.Obligatorio;
                dto.TipoDocumento.Cantidad = model.TipoDocumento.Cantidad;
                dto.TipoDocumento.Vigente = model.TipoDocumento.Vigente;

                dto.DocumentoAdjunto = new DTO.Models.DocumentoAdjunto();
                dto.DocumentoAdjunto.DocumentoAdjuntoID = model.DocumentoAdjunto.DocumentoAdjuntoID;
                dto.DocumentoAdjunto.VersionEncriptID = model.DocumentoAdjunto.VersionEncriptID;
                dto.DocumentoAdjunto.MaximoTamanoArchivoID = model.DocumentoAdjunto.MaximoTamanoArchivoID;
                dto.DocumentoAdjunto.TipoFormatoID = model.DocumentoAdjunto.TipoFormatoID;
                dto.DocumentoAdjunto.NombreDocumento = model.DocumentoAdjunto.NombreDocumento;
                dto.DocumentoAdjunto.Descripcion = model.DocumentoAdjunto.Descripcion.Trim();
                dto.DocumentoAdjunto.Hash = model.DocumentoAdjunto.Hash;
                dto.DocumentoAdjunto.NombreArchivoFisico = model.DocumentoAdjunto.NombreArchivoFisico;

                dto.DocumentoAdjunto.TipoFormato = new DTO.Models.TipoFormato();
                dto.DocumentoAdjunto.TipoFormato.TipoFormatoID = model.DocumentoAdjunto.TipoFormato.TipoFormatoID;
                dto.DocumentoAdjunto.TipoFormato.Descripcion = model.DocumentoAdjunto.TipoFormato.Descripcion.Trim();
                dto.DocumentoAdjunto.TipoFormato.Vigente = model.DocumentoAdjunto.TipoFormato.Vigente;
                dto.DocumentoAdjunto.TipoFormato.ExtraCss = model.DocumentoAdjunto.TipoFormato.ExtraCss;
                dto.DocumentoAdjunto.TipoFormato.UsoSolicitud = model.DocumentoAdjunto.TipoFormato.UsoSolicitud;

                dto.DocumentoAdjunto.MaximoTamanoArchivo = new DTO.Models.MaximoTamanoArchivo();
                dto.DocumentoAdjunto.MaximoTamanoArchivo.MaximoTamanoArchivoID = model.DocumentoAdjunto.MaximoTamanoArchivo.MaximoTamanoArchivoID;
                dto.DocumentoAdjunto.MaximoTamanoArchivo.Tamano = model.DocumentoAdjunto.MaximoTamanoArchivo.Tamano;
                dto.DocumentoAdjunto.MaximoTamanoArchivo.Vigente = model.DocumentoAdjunto.MaximoTamanoArchivo.Vigente;

                dto.DocumentoAdjunto.VersionEncript = new DTO.Models.VersionEncript();
                dto.DocumentoAdjunto.VersionEncript.VersionEncriptID = model.DocumentoAdjunto.VersionEncript.VersionEncriptID;
                dto.DocumentoAdjunto.VersionEncript.Cadena = model.DocumentoAdjunto.VersionEncript.Cadena.Trim();

                dto.DocumentoAdjunto.TipoFormato.FamiliasMimeType = new List<DTO.Models.FamiliasMimeType>();

                foreach (FamiliasMimeType item in model.DocumentoAdjunto.TipoFormato.FamiliasMimeType)
                {
                    DTO.Models.FamiliasMimeType mime = new DTO.Models.FamiliasMimeType();
                    mime.FamiliasMimeTypeID = item.FamiliasMimeTypeID;
                    mime.TipoFormatoID = item.TipoFormatoID;
                    mime.Descripcion = item.Descripcion.Trim();
                    mime.Vigente = item.Vigente;

                    dto.DocumentoAdjunto.TipoFormato.FamiliasMimeType.Add(mime);
                }

                dtoList.Add(dto);
            }

            return dtoList;
        }

        public int SaveDocumentoAdjunto(DTO.Models.DocumentoAdjunto dto)
        {
            DocumentoAdjunto model = new DocumentoAdjunto();
            model.DocumentoAdjuntoID = dto.DocumentoAdjuntoID;
            model.VersionEncriptID = dto.VersionEncriptID;
            model.MaximoTamanoArchivoID = dto.MaximoTamanoArchivoID;
            model.TipoFormatoID = dto.TipoFormatoID;
            model.NombreDocumento = dto.NombreDocumento;
            model.Descripcion = dto.Descripcion;
            model.Hash = dto.Hash;
            model.NombreArchivoFisico = dto.NombreArchivoFisico;

            return repo.SaveDocumentoAdjunto(model);
        }

        public int SaveAsocTipoDocumentoAdjunto(DTO.Models.AsocTipoDocumentoAdjunto dto)
        {
            AsocTipoDocumentoAdjunto model = new AsocTipoDocumentoAdjunto();
            model.AsocTipoDocumentoAdjuntoID = dto.AsocTipoDocumentoAdjuntoID;
            model.DocumentoAdjuntoID = dto.DocumentoAdjuntoID;
            model.TipoDocumentoID = dto.TipoDocumentoID;

            return repo.SaveAsocTipoDocumentoAdjunto(model);
        }

        public void DeleteDocumentoAdjunto(int DocumentoAdjuntoID, int AsocTipoDocumentoAdjuntoID)
        {
            repo.DeleteDocumentoAdjunto(DocumentoAdjuntoID, AsocTipoDocumentoAdjuntoID);
        }

        #endregion

        #region Usuarios
        public IList<DTO.Models.Usuario> GetUsuarios()
        {
            IList<Usuario> repoList = repo.GetUsuarios();
            IList<DTO.Models.Usuario> listDTO = new List<DTO.Models.Usuario>();

            foreach (var item in repoList)
            {
                DTO.Models.Usuario dto = new DTO.Models.Usuario();
                dto.UsuarioID = item.UsuarioID;
                dto.AdID = item.AdID;
                dto.Rut = item.Rut;                
                dto.Nombres = item.Nombres.Trim();
                dto.Apellidos = item.Apellidos.Trim();
                dto.Mail = item.Mail.Trim();
                dto.Telefono = item.Telefono.Trim();
                dto.IsClaveUnica = item.IsClaveUnica;
                dto.FechaRegistro = item.FechaRegistro;
                dto.FechaModificacion = item.FechaModificacion;
                dto.TipoGeneroID = item.TipoGeneroID;
                dto.Signer = item.Signer;
                dto.IsPresidente = (bool)item.IsPresidente;
                dto.IsSecretarioAbogado = (bool)item.IsSecretarioAbogado;
                dto.UsuarioModificacion = item.UsuarioModificacion;


                dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                foreach (var asoc in item.AsocUsuarioPerfil)
                {
                    DTO.Models.AsocUsuarioPerfil a = new DTO.Models.AsocUsuarioPerfil();
                    a.AsocUsuarioPerfilID = asoc.AsocUsuarioPerfilID;
                    a.PerfilID = asoc.PerfilID;
                    a.UsuarioID = asoc.UsuarioID;

                    a.Perfil = new DTO.Models.Perfil();
                    a.Perfil.Descripcion = asoc.Perfil.Descripcion.Trim();
                    a.Perfil.Vigente = asoc.Perfil.Vigente;

                    dto.AsocUsuarioPerfil.Add(a);
                }

                dto.TipoGenero = new DTO.Models.TipoGenero();
                dto.TipoGenero.TipoGeneroID = item.TipoGenero.TipoGeneroID;
                dto.TipoGenero.Descripcion = item.TipoGenero.Descripcion;
                dto.TipoGenero.Vigente = item.TipoGenero.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.AsocUsuarioPerfil> GetAsocUsuarioPerfil()
        {
            IList<AsocUsuarioPerfil> repoList = repo.GetAsocUsuarioPerfil();

            IList<DTO.Models.AsocUsuarioPerfil> listDTO = new List<DTO.Models.AsocUsuarioPerfil>();

            foreach (var item in repoList)
            {
                DTO.Models.AsocUsuarioPerfil dto = new DTO.Models.AsocUsuarioPerfil();
                dto.AsocUsuarioPerfilID = item.AsocUsuarioPerfilID;
                dto.PerfilID = item.PerfilID;
                dto.UsuarioID = item.UsuarioID;
                dto.Usuario = GetUsuarioByID(item.UsuarioID);

                listDTO.Add(dto);
            }

            return listDTO;
        }
        
        public IList<DTO.Models.Perfil> PerfilesFuncionario(int FuncionarioID)
        {
            IList<Perfil> repoList = repo.PerfilesFuncionario(FuncionarioID);
            IList<DTO.Models.Perfil> listDTO = new List<DTO.Models.Perfil>();

            foreach (var item in repoList)
            {
                DTO.Models.Perfil dto = new DTO.Models.Perfil();
                dto.PerfilID = item.PerfilID;
                dto.Descripcion = item.Descripcion;
                dto.Vigente = item.Vigente;

                dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                foreach (var a in item.AsocUsuarioPerfil)
                {
                    DTO.Models.AsocUsuarioPerfil add = new DTO.Models.AsocUsuarioPerfil();
                    add.AsocUsuarioPerfilID = a.AsocUsuarioPerfilID;
                    add.UsuarioID = a.UsuarioID;
                    add.PerfilID = a.PerfilID;                   

                    dto.AsocUsuarioPerfil.Add(add);
                }

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.Perfil> GetPerfil(bool SoloVigentes = false)
        {
            IList<Perfil> listModel = null;

            if (SoloVigentes)
            {
                listModel = repo.GetPerfil().Where(x => x.Vigente).ToList();
            }
            else
            {
                listModel = repo.GetPerfil();
            }

            IList<DTO.Models.Perfil> listDTO = new List<DTO.Models.Perfil>();

            foreach (var item in listModel)
            {
                DTO.Models.Perfil dto = new DTO.Models.Perfil();
                dto.PerfilID = item.PerfilID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public DTO.Models.Usuario GetUsuarioByID(int usuarioID)
        {
            var model = repo.GetUsuarioByID(usuarioID);

            DTO.Models.Usuario dto = new DTO.Models.Usuario();
            dto.UsuarioID = (int)Domain.Infrastructure.GenericJson.UserTMP;
            dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();

            if (model != null)
            {
                dto.UsuarioID = model.UsuarioID;
                dto.AdID = model.AdID;
                dto.Rut = model.Rut;
                dto.Nombres = model.Nombres.Trim();
                dto.Apellidos = model.Apellidos.Trim();
                dto.Mail = model.Mail.Trim();
                dto.Telefono = model.Telefono.Trim();
                dto.IsClaveUnica = model.IsClaveUnica;
                dto.FechaRegistro = model.FechaRegistro;
                dto.FechaModificacion = model.FechaModificacion;
                dto.TipoGeneroID = model.TipoGeneroID;
                dto.Signer = model.Signer;

                dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                foreach (var item in model.AsocUsuarioPerfil)
                {
                    DTO.Models.AsocUsuarioPerfil asoc = new DTO.Models.AsocUsuarioPerfil();
                    asoc.AsocUsuarioPerfilID = item.AsocUsuarioPerfilID;
                    asoc.UsuarioID = item.UsuarioID;
                    asoc.PerfilID = item.PerfilID;

                    dto.AsocUsuarioPerfil.Add(asoc);
                }

                dto.TipoGenero = new DTO.Models.TipoGenero();
                dto.TipoGenero.TipoGeneroID = model.TipoGenero.TipoGeneroID;
                dto.TipoGenero.Descripcion = model.TipoGenero.Descripcion.Trim();
                dto.TipoGenero.Vigente = model.TipoGenero.Vigente;
            }

            return dto;
        }

        public int SaveUser(DTO.Models.Usuario dto)
        {
            Usuario model = new Usuario();
            model.UsuarioID = dto.UsuarioID;
            model.AdID = dto.AdID;
            model.Rut = dto.Rut;
            model.Nombres = dto.Nombres.Trim();
            model.Apellidos = dto.Apellidos.Trim();
            model.Mail = dto.Mail.Trim();
            model.Telefono = dto.Telefono.Trim();
            model.IsClaveUnica = dto.IsClaveUnica;
            model.FechaRegistro = dto.FechaRegistro;
            model.FechaModificacion = dto.FechaModificacion;
            model.TipoGeneroID = dto.TipoGeneroID;
            model.Signer = dto.Signer;
            model.IsPresidente = dto.IsPresidente;
            model.IsSecretarioAbogado = dto.IsSecretarioAbogado;
            model.UsuarioModificacion = dto.UsuarioModificacion;

            return repo.SaveUser(model);
        }

        public void SaveAsocPerfilUsuario(DTO.Models.AsocUsuarioPerfil dto)
        {
            AsocUsuarioPerfil model = new AsocUsuarioPerfil();
            model.AsocUsuarioPerfilID = dto.AsocUsuarioPerfilID;
            model.UsuarioID = dto.UsuarioID;
            model.PerfilID = dto.PerfilID;

            repo.SaveAsocPerfilUsuario(model);
        }


        public void DeletePerfilesUser(int UsuarioID)
        {
            repo.DeletePerfilesUser(UsuarioID);
        }


        public void DeleteUser(int UsuarioID, bool Acceso)
        {
            repo.DeleteUser(UsuarioID);
        }

        public void SaveAsocDocumentoUsuario(DTO.Models.AsocDocumentoUsuario dto)
        {
            AsocDocumentoUsuario model = new AsocDocumentoUsuario();
            model.AsocDocumentoUsuarioID = dto.AsocDocumentoUsuarioID;
            model.DocumentoSistemaID = dto.DocumentoSistemaID;
            model.UsuarioID = dto.UsuarioID;

            repo.SaveAsocDocumentoUsuario(model);
        }

        public IList<DTO.Models.AsocDocumentoUsuario> GetAsocDocumentoUsuario(int UsuarioID)
        {
            IList<DTO.Models.AsocDocumentoUsuario> listDTO = new List<DTO.Models.AsocDocumentoUsuario>();

            IList<AsocDocumentoUsuario> repoList = repo.GetAsocDocumentoUsuario(UsuarioID);

            foreach (var item in repoList)
            {
                DTO.Models.AsocDocumentoUsuario dto = new DTO.Models.AsocDocumentoUsuario();
                dto.AsocDocumentoUsuarioID = item.AsocDocumentoUsuarioID;
                dto.DocumentoSistemaID= item.DocumentoSistemaID;
                dto.UsuarioID = item.UsuarioID;

                dto.DocumentoSistema = new DTO.Models.DocumentoSistema();
                dto.DocumentoSistema.DocumentoSistemaID = item.DocumentoSistema.DocumentoSistemaID;
                dto.DocumentoSistema.VersionEncriptID = item.DocumentoSistema.VersionEncriptID;
                dto.DocumentoSistema.TipoDocumentoID = item.DocumentoSistema.TipoDocumentoID;
                dto.DocumentoSistema.Hash = item.DocumentoSistema.Hash;
                dto.DocumentoSistema.NombreArchivoFisico = item.DocumentoSistema.NombreArchivoFisico;
                dto.DocumentoSistema.Fecha = item.DocumentoSistema.Fecha;
                dto.DocumentoSistema.Descripcion = item.DocumentoSistema.Descripcion;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        #endregion

        #region Ventana

        public DTO.Models.Ventana GetVentanaByTipoVentanaID(int TipoVentanaID)
        {
            Ventana ventana = repo.GetVentanaByTipoVentanaID(TipoVentanaID);

            DTO.Models.Ventana ventanaDTO = new DTO.Models.Ventana();
            ventanaDTO.VentanaID = ventana.VentanaID;
            ventanaDTO.TipoVentanaID = ventana.TipoVentanaID;
            ventanaDTO.IntroduccionEspanol = ventana.IntroduccionEspanol.Trim();
            ventanaDTO.IntroduccionIngles = ventana.IntroduccionIngles.Trim();
            ventanaDTO.TextoAyuda = new Dictionary<string, string>();
            ventanaDTO.TextoAyuda.Add(LanguageEnum.EN.ToString(), ventana.IntroduccionIngles.Trim());
            ventanaDTO.TextoAyuda.Add(LanguageEnum.ES.ToString(), ventana.IntroduccionEspanol.Trim());

            return ventanaDTO;
        }

        public DTO.Models.Ventana GetInfoByTipoVentanaID(int TipoVentanaID)
        {
            Ventana ventana = repo.GetVentanaByTipoVentanaID(TipoVentanaID);

            DTO.Models.Ventana ventanaDTO = new DTO.Models.Ventana();
            ventanaDTO.VentanaID = ventana.VentanaID;
            ventanaDTO.TipoVentanaID = ventana.TipoVentanaID;
            ventanaDTO.IntroduccionEspanol = ventana.IntroduccionEspanol.Trim();
            ventanaDTO.IntroduccionIngles = ventana.IntroduccionIngles.Trim();

            ventanaDTO.TextoAyuda = new Dictionary<string, string>();
            ventanaDTO.TextoAyuda.Add(LanguageEnum.EN.ToString(), ventana.IntroduccionIngles.Trim());
            ventanaDTO.TextoAyuda.Add(LanguageEnum.ES.ToString(), ventana.IntroduccionEspanol.Trim());

            return ventanaDTO;
        }

        public IList<DTO.Models.TipoVentana> GetTipoVentana()
        {
            IList<TipoVentana> listModel = repo.GetTipoVentana();

            IList<DTO.Models.TipoVentana> listDTO = new List<DTO.Models.TipoVentana>();

            foreach (var item in listModel)
            {
                DTO.Models.TipoVentana nuevo = new DTO.Models.TipoVentana();
                nuevo.Descripcion = item.Descripcion;
                nuevo.TipoVentanaID = item.TipoVentanaID;
                listDTO.Add(nuevo);
            }

            return listDTO;
        }

        public int SaveTipoVentana(DTO.Models.TipoVentana _dto)
        {
            TipoVentana model = new TipoVentana();
            model.TipoVentanaID = _dto.TipoVentanaID;
            model.Descripcion = _dto.Descripcion;

            model.TipoVentanaID = repo.SaveTipoVentana(model);

            return model.TipoVentanaID;
        }

        public int SaveVentana(DTO.Models.Ventana _ventana)
        {
            Ventana model = new Ventana();
            model.VentanaID = _ventana.VentanaID;
            model.TipoVentanaID = _ventana.TipoVentanaID;
            model.IntroduccionEspanol = _ventana.IntroduccionEspanol;
            model.IntroduccionIngles = _ventana.IntroduccionIngles;

            model.VentanaID = repo.SaveVentana(model);

            return model.VentanaID;
        }

        #endregion

        #region Documento Tmp
        public void DeleteDocumentosTmp(DateTime fecha)
        {
            repo.DeleteDocumentosTmp(fecha);
        }

        public int SaveDocumentoTmp(DTO.Models.DocumentoTmp dto)
        {
            DocumentoTmp model = new DocumentoTmp();
            model.DocumentoTmpID = dto.DocumentoTmpID;
            model.TipoDocumentoID = dto.TipoDocumentoID;
            model.VersionEncriptID = dto.VersionEncriptID;
            model.Hash = dto.Hash;
            model.Fecha = dto.Fecha;

            return repo.SaveDocumentoTmp(model);
        }

        public DTO.Models.DocumentoTmp GetDocumentoTmp(int DocumentoTmpID)
        {
            DocumentoTmp model = repo.GetDocumentoTmp(DocumentoTmpID);

            DTO.Models.DocumentoTmp dto = new DTO.Models.DocumentoTmp();
            dto.DocumentoTmpID = model.DocumentoTmpID;
            dto.TipoDocumentoID = model.TipoDocumentoID;
            dto.VersionEncriptID = model.VersionEncriptID;
            dto.Hash = model.Hash;
            dto.Fecha = model.Fecha;

            dto.VersionEncript = new DTO.Models.VersionEncript();
            dto.VersionEncript.Cadena = model.VersionEncript.Cadena.Trim();

            return dto;
        }

        public void DeleteDocumentoTmpByID(int DocumentoTmpID)
        {
            repo.DeleteDocumentoTmpByID(DocumentoTmpID);
        }


        public void DeleteDocumento(Domain.Infrastructure.TipoDocumento tipoDocumento, int DocumentoID)
        {
            if (tipoDocumento == Domain.Infrastructure.TipoDocumento.Temporal)
            {
                repo.DeleteDocumentoTmp(DocumentoID);
            }

            if (tipoDocumento == Domain.Infrastructure.TipoDocumento.Causa)
            {
                repo.DeleteDocumentoCausa(DocumentoID);
            }

            if (tipoDocumento == Domain.Infrastructure.TipoDocumento.Expediente)
            {
                repo.DeleteDocumentoExpediente(DocumentoID);
            }
        }
        #endregion

        #region Tipo Notificacion
        

        public DTO.Models.TipoNotificacion GetTipoNotificacionByID(int TipoNotificacionID)
        {
            TipoNotificacion model = repo.GetTipoNotificacionByID(TipoNotificacionID);

            DTO.Models.TipoNotificacion dto = new DTO.Models.TipoNotificacion();
            dto.TipoNotificacionID = model.TipoNotificacionID;
            dto.Descripcion = model.Descripcion.Trim();
            dto.Asunto = model.Asunto.Trim();
            dto.Mensaje = model.Mensaje.Trim();
            dto.Vigente = model.Vigente;
            dto.ConCopia = model.ConCopia.Trim();

            foreach (var item in model.AsocTipoNotificacionPerfil)
            {
                DTO.Models.AsocTipoNotificacionPerfil _asoc = new DTO.Models.AsocTipoNotificacionPerfil();
                _asoc.AsocTipoNotificacionPerfilID = item.AsocTipoNotificacionPerfilID;
                _asoc.PerfilID = item.PerfilID;
                _asoc.TipoNotificacionID = item.TipoNotificacionID;

                dto.AsocTipoNotificacionPerfil.Add(_asoc);
            }

            return dto;
        }

        public IList<DTO.Models.TipoNotificacion> GetTipoNotificacion()
        {
            IList<TipoNotificacion> listModel = repo.GetTipoNotificacion();
            IList<DTO.Models.TipoNotificacion> listDTO = new List<DTO.Models.TipoNotificacion>();

            foreach (var item in listModel)
            {
                DTO.Models.TipoNotificacion dto = new DTO.Models.TipoNotificacion();
                dto.TipoNotificacionID = item.TipoNotificacionID;
                dto.Descripcion = item.Descripcion;
                dto.Asunto = item.Asunto;
                dto.Mensaje = item.Mensaje;
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveTipoNotificacion(DTO.Models.TipoNotificacion dto)
        {
            TipoNotificacion model = new TipoNotificacion();
            model.TipoNotificacionID = dto.TipoNotificacionID;
            model.Descripcion = dto.Descripcion;
            model.Asunto = dto.Asunto;
            model.Mensaje = dto.Mensaje;
            model.Vigente = dto.Vigente;
            model.ConCopia= dto.ConCopia;

            model.TipoNotificacionID = repo.SaveTipoNotificacion(model);

            return model.TipoNotificacionID;
        }

        public void SaveAsocTipoNotificacionPerfil(DTO.Models.AsocTipoNotificacionPerfil dto)
        {
            AsocTipoNotificacionPerfil model = new AsocTipoNotificacionPerfil();
            model.AsocTipoNotificacionPerfilID  = dto.AsocTipoNotificacionPerfilID;
            model.TipoNotificacionID = dto.TipoNotificacionID;
            model.PerfilID = dto.PerfilID;

            repo.SaveAsocTipoNotificacionPerfil(model);
        }

        public void DeleteAsocTipoNotificacionPerfil(int TipoNotificacionID)
        {
            repo.DeleteAsocTipoNotificacionPerfil(TipoNotificacionID);
        }

        #endregion

        #region Feriados
        public IList<DTO.Models.Feriado> GetAllFeriados()
        {
            IList<DTO.Models.Feriado> listDTO = new List<DTO.Models.Feriado>();
            IList<Feriado> repoList = repo.GetAllFeriados();

            foreach (var item in repoList.OrderBy(x => x.Fecha))
            {
                DTO.Models.Feriado dto = new DTO.Models.Feriado();
                dto.FeriadoID = item.FeriadoID;
                dto.Fecha = item.Fecha;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public int SaveFeriado(DTO.Models.Feriado dto)
        {
            Feriado model = new Feriado();
            model.FeriadoID = dto.FeriadoID;
            model.Fecha = dto.Fecha;

            return repo.SaveFeriado(model);
        }

        public void DeleteFeriado(int FeriadoID)
        {
            repo.DeleteFeriado(FeriadoID);
        }
        #endregion

        public IList<DTO.Models.TipoCausa> GetTipoCausa(bool SoloVigente = false)
        {
            IList<TipoCausa> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoCausa().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoCausa();
            }

            IList<DTO.Models.TipoCausa> listDTO = new List<DTO.Models.TipoCausa>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoCausa dto = new DTO.Models.TipoCausa();
                dto.TipoCausaID = item.TipoCausaID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.DescripcionLarga = item.DescripcionLarga.Trim();
                dto.Vigente = item.Vigente;
                dto.IsPublico = item.IsPublico;
                dto.IsInterno = item.IsInterno;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.EstadoCausa> GetEstadoCausa(bool SoloVigente = false)
        {
            IList<EstadoCausa> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetEstadoCausa().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetEstadoCausa();
            }

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

        public IList<DTO.Models.OpcionesTramite> GetOpcionesTramite(bool SoloVigente = false)
        {
            IList<OpcionesTramite> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetOpcionesTramite().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetOpcionesTramite();
            }

            IList<DTO.Models.OpcionesTramite> listDTO = new List<DTO.Models.OpcionesTramite>();

            foreach (var item in repoList)
            {
                DTO.Models.OpcionesTramite dto = new DTO.Models.OpcionesTramite();
                dto.OpcionesTramiteID = item.OpcionesTramiteID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.TipoParte> GetTipoParte(bool SoloVigente = false)
        {
            IList<TipoParte> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoParte().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoParte();
            }

            IList<DTO.Models.TipoParte> listDTO = new List<DTO.Models.TipoParte>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoParte dto = new DTO.Models.TipoParte();
                dto.TipoParteID = item.TipoParteID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.TipoTramite> GetTipoTramite(bool SoloVigente = false)
        {
            IList<TipoTramite> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoTramite().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoTramite();
            }

            IList<DTO.Models.TipoTramite> listDTO = new List<DTO.Models.TipoTramite>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoTramite dto = new DTO.Models.TipoTramite();
                dto.TipoTramiteID = item.TipoTramiteID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.Pais> GetPais()
        {
            IList<Pais> repoList = repo.GetPais();
            IList<DTO.Models.Pais> listDTO = new List<DTO.Models.Pais>();

            foreach (var item in repoList)
            {
                DTO.Models.Pais dto = new DTO.Models.Pais();
                dto.PaisID = item.PaisID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.CodigoArea = item.CodigoArea.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.EstadoTabla> GetEstadoTabla(bool SoloVigente = false)
        {
            IList<EstadoTabla> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetEstadoTabla().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetEstadoTabla();
            }

            IList<DTO.Models.EstadoTabla> listDTO = new List<DTO.Models.EstadoTabla>();

            foreach (var item in repoList)
            {
                DTO.Models.EstadoTabla dto = new DTO.Models.EstadoTabla();
                dto.EstadoTablaID = item.EstadoTablaID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.TipoTabla> GetTipoTabla(bool SoloVigente = false)
        {
            IList<TipoTabla> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoTabla().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoTabla();
            }

            IList<DTO.Models.TipoTabla> listDTO = new List<DTO.Models.TipoTabla>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoTabla dto = new DTO.Models.TipoTabla();
                dto.TipoTablaID = item.TipoTablaID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.Sala> GetSala(bool SoloVigente = false)
        {
            IList<Sala> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetSala().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetSala();
            }

            IList<DTO.Models.Sala> listDTO = new List<DTO.Models.Sala>();

            foreach (var item in repoList)
            {
                DTO.Models.Sala dto = new DTO.Models.Sala();
                dto.SalaID = item.SalaID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public IList<DTO.Models.TipoContencioso> GetTipoContencioso(bool SoloVigente = false)
        {
            IList<TipoContencioso> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoContencioso().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoContencioso();
            }

            IList<DTO.Models.TipoContencioso> listDTO = new List<DTO.Models.TipoContencioso>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoContencioso dto = new DTO.Models.TipoContencioso();
                dto.TipoContenciosoID = item.TipoContenciosoID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        
        public DTO.Models.ConfTipoCausa GetConfTipoCausa(int tipoCausaID)
        {
            ConfTipoCausa model = repo.GetConfTipoCausa(tipoCausaID);

            DTO.Models.ConfTipoCausa dto = new DTO.Models.ConfTipoCausa();
            dto.ConfTipoCausaID = model.ConfTipoCausaID;
            dto.TipoCausaID = model.TipoCausaID;
            dto.IsAnio = model.IsAnio;
            dto.IsObservacion = model.IsObservacion;
            dto.IsNumeroRegistro = model.IsNumeroRegistro;
            dto.IsContencioso = model.IsContencioso;
            dto.TipoParteID1 = model.TipoParteID1;
            dto.TipoParteID2 = model.TipoParteID2;
            dto.IsNumeroSolicitud = model.isNumeroSolicitud;
            dto.IsConsignacion = model.IsConsignacion;

            return dto;
        }

        public DTO.Models.Usuario GetFirmanteTable(int tipoFirma)
        {
            var model = repo.GetFirmanteTable(tipoFirma);

            DTO.Models.Usuario dto = new DTO.Models.Usuario();
            dto.UsuarioID = (int)Domain.Infrastructure.GenericJson.UserTMP;
            dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();

            if (model != null)
            {
                dto.UsuarioID = model.UsuarioID;
                dto.AdID = model.AdID;
                dto.Rut = model.Rut;
                dto.Nombres = model.Nombres.Trim();
                dto.Apellidos = model.Apellidos.Trim();
                dto.Mail = model.Mail.Trim();
                dto.Telefono = model.Telefono.Trim();
                dto.IsClaveUnica = model.IsClaveUnica;
                dto.FechaRegistro = model.FechaRegistro;
                dto.FechaModificacion = model.FechaModificacion;
                dto.TipoGeneroID = model.TipoGeneroID;
                dto.Signer = model.Signer;

                dto.AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();
                foreach (var item in model.AsocUsuarioPerfil)
                {
                    DTO.Models.AsocUsuarioPerfil asoc = new DTO.Models.AsocUsuarioPerfil();
                    asoc.AsocUsuarioPerfilID = item.AsocUsuarioPerfilID;
                    asoc.UsuarioID = item.UsuarioID;
                    asoc.PerfilID = item.PerfilID;

                    dto.AsocUsuarioPerfil.Add(asoc);
                }

                dto.TipoGenero = new DTO.Models.TipoGenero();
                dto.TipoGenero.TipoGeneroID = model.TipoGenero.TipoGeneroID;
                dto.TipoGenero.Descripcion = model.TipoGenero.Descripcion.Trim();
                dto.TipoGenero.Vigente = model.TipoGenero.Vigente;
            }

            return dto;
        }

        public IList<DTO.Models.AsocTipoTramiteOpciones> GetAsocTipoTramiteOpciones(int TipoTramiteID)
        {
            IList<DTO.Models.AsocTipoTramiteOpciones> listDTO = new List<DTO.Models.AsocTipoTramiteOpciones>();

            IList<AsocTipoTramiteOpciones> repoList = repo.GetAsocTipoTramiteOpciones(TipoTramiteID);

            foreach (var item in repoList)
            {
                DTO.Models.AsocTipoTramiteOpciones dto = new DTO.Models.AsocTipoTramiteOpciones();
                dto.AsocTipoTramiteOpcionesID = item.AsocTipoTramiteOpcionesID;
                dto.TipoTramiteID = item.TipoTramiteID;
                dto.OpcionesTramiteID = item.OpcionesTramiteID;
                dto.IsTabla = item.IsTabla;
                dto.PlazoDias = item.PlazoDias;
                dto.IsDiasHabiles = item.IsDiasHabiles;
                dto.IsPermiteFechaAnterior = item.IsPermiteFechaAnterior;
                dto.IsInformaAtraso = item.IsInformaAtraso;
                dto.NumeroFirmas = item.NumeroFirmas;
                dto.Status1 = item.Status1;
                dto.Status2 = item.Status2;
                dto.Vigente = item.Vigente;
                dto.IsFinalizaIngreso = item.IsFinalizaIngreso;
                dto.EstadoCausaID = item.EstadoCausaID;

                listDTO.Add(dto);
            }

            return listDTO;
        }
        public int SaveAsocTipoTramiteOpciones(DTO.Models.AsocTipoTramiteOpciones dto)
        {
            AsocTipoTramiteOpciones model = new AsocTipoTramiteOpciones();
            model.AsocTipoTramiteOpcionesID = dto.AsocTipoTramiteOpcionesID;
            model.TipoTramiteID = dto.TipoTramiteID;
            model.OpcionesTramiteID = dto.OpcionesTramiteID;
            model.IsTabla = dto.IsTabla;
            model.PlazoDias = dto.PlazoDias;
            model.IsDiasHabiles = dto.IsDiasHabiles;
            model.IsPermiteFechaAnterior = dto.IsPermiteFechaAnterior;
            model.IsInformaAtraso = dto.IsInformaAtraso;
            model.NumeroFirmas = dto.NumeroFirmas;
            model.Status1 = dto.Status1;
            model.Status2 = dto.Status2;
            model.Vigente = dto.Vigente;
            model.IsFinalizaIngreso = dto.IsFinalizaIngreso;
            model.EstadoCausaID = dto.EstadoCausaID;

            return repo.SaveAsocTipoTramiteOpciones(model);
        }
        
        public void DeleteAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID)
        {
            repo.DeleteAsocTipoTramiteOpciones(AsocTipoTramiteOpcionesID);         
        }

        public IList<DTO.Models.EstadosAplica> GetEstadosAplicaByAsocTipoTramiteOpciones(int AsocTipoTramiteOpcionesID)
        {
            IList<DTO.Models.EstadosAplica> listDTO = new List<DTO.Models.EstadosAplica>();

            IList<EstadosAplica> repoList = repo.GetEstadosAplicaByAsocTipoTramiteOpciones(AsocTipoTramiteOpcionesID);

            foreach (var item in repoList)
            {
                DTO.Models.EstadosAplica dto = new DTO.Models.EstadosAplica();
                dto.EstadosAplicaID = item.EstadosAplicaID;
                dto.EstadoCausaID = item.EstadoCausaID;
                dto.AsocTipoTramiteOpcionesID = item.AsocTipoTramiteOpcionesID;
                dto.IsSiguiente = item.IsSiguiente;

                listDTO.Add(dto);
            }
            return listDTO;
        }

        public int SaveEstadosAplica(DTO.Models.EstadosAplica dto)
        {
            EstadosAplica model = new EstadosAplica();
            model.EstadosAplicaID = dto.EstadosAplicaID;
            model.EstadoCausaID = dto.EstadoCausaID;
            model.AsocTipoTramiteOpcionesID = dto.AsocTipoTramiteOpcionesID;
            model.IsSiguiente = dto.IsSiguiente;

            return repo.SaveEstadosAplica(model);
        }

        public void DeleteEstadosAplica(int EstadosAplicaID)
        {
            repo.DeleteEstadosAplica(EstadosAplicaID);
        }

        public int SaveLogSistema(DTO.Models.LogSistema dto)
        {
            LogSistema model = new LogSistema();
            model.LogSistemaID = dto.LogSistemaID;
            model.Fecha = dto.Fecha;
            model.Pagina = dto.Pagina;
            model.Accion = dto.Accion;
            model.ExpedienteID = dto.ExpedienteID;
            model.UsuarioID = dto.UsuarioID;
            model.Parametros = dto.Parametros;
            model.IpUsuario = dto.IpUsuario;
            model.Tipo = dto.Tipo;
            model.Descripcion = dto.Descripcion;

            return repo.SaveLogSistema(model);
        }

        public IList<DTO.Models.TipoGenero> GetTipoGenero(bool SoloVigente = false)
        {
            IList<TipoGenero> repoList;

            if (SoloVigente)
            {
                repoList = repo.GetTipoGenero().Where(x => x.Vigente).ToList();
            }
            else
            {
                repoList = repo.GetTipoGenero();
            }

            IList<DTO.Models.TipoGenero> listDTO = new List<DTO.Models.TipoGenero>();

            foreach (var item in repoList)
            {
                DTO.Models.TipoGenero dto = new DTO.Models.TipoGenero();
                dto.TipoGeneroID = item.TipoGeneroID;
                dto.Descripcion = item.Descripcion;
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }


        public IList<DTO.Models.Perfil> GetPerfilUsuario(int UsuarioID)
        {
            IList<Perfil> listModel = repo.GetPerfilUsuario(UsuarioID);

            IList<DTO.Models.Perfil> listDTO = new List<DTO.Models.Perfil>();

            foreach (var item in listModel)
            {
                DTO.Models.Perfil dto = new DTO.Models.Perfil();
                dto.PerfilID = item.PerfilID;
                dto.Descripcion = item.Descripcion.Trim();
                dto.Vigente = item.Vigente;

                listDTO.Add(dto);
            }

            return listDTO;
        }

        public void SaveSigner(int usuarioActive, string signer)
        {
            repo.SaveSigner(usuarioActive, signer);
        }
    }
}
