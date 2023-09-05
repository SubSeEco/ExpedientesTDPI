using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Domain.Infrastructure;
using Infrastructure.Logging;
using Application.Services;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;
using System.Globalization;
using System.Runtime.Caching;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Acciones de Utilidad para uso en controllers
    /// </summary>
    public class Commons
    {
        private readonly ICommonAppServices app = new CommonAppServices();

        private readonly int ExpirationCacheInMinutes = WebConfigValues.ExpirationCacheInMinutes;


        #region CACHE

        internal IList<DTO.AD.UserAD> GetAllUsersAD(bool notCache = false)
        {
            string nameCache = "UsersAD";
            IList<DTO.AD.UserAD> lista = new List<DTO.AD.UserAD>();
            var appAdService = new ADApplicationService();

            if (notCache)
            {
                AdConfig ad = new AdConfig();
                foreach (var server in ad.Servers)
                {
                    var objetos = appAdService.GetAllUsers(server);
                    foreach (var item in objetos)
                    {
                        lista.Add(item);
                    }

                    Logger.Execute().Info("Parse: " + objetos.Count);
                }
            }
            else
            {
                ObjectCache cache = MemoryCache.Default;

                lista = (IList<DTO.AD.UserAD>)cache.Get(nameCache);

                if (lista == null)
                {
                    lista = new List<DTO.AD.UserAD>();

                    AdConfig ad = new AdConfig();
                    foreach (var server in ad.Servers)
                    {
                        Logger.Execute().Info("Init: " + server.Key);

                        IList<DTO.AD.UserAD> objetos = appAdService.GetAllUsers(server);

                        Logger.Execute().Info("Find Init: " + objetos.Count);

                        foreach (var item in objetos)
                        {
                            lista.Add(item);
                        }

                        Logger.Execute().Info("End Parse: " + objetos.Count);
                    }

                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(ExpirationCacheInMinutes);
                    cache.Add(nameCache, lista, policy);
                }
            }

            return lista;
        }
        
        #endregion

        #region Get Form Variables

        internal string GetStringValueForm(string valorForm, string devuelve = "")
        {
            return (!string.IsNullOrEmpty(valorForm)) ? valorForm.ToString().Trim() : devuelve;
        }

        internal int GetIntValueForm(string valorForm, int devuelve = 0)
        {
            return (!string.IsNullOrEmpty(valorForm)) ? Convert.ToInt32(valorForm.ToString().Replace(",", "").Replace(".", "")) : devuelve;
        }

        internal decimal GetDecimalTouchSpin(string valorForm, int devuelve = 0)
        {
            string nuevoValor = valorForm.Replace(".", ",");

            return (!string.IsNullOrEmpty(valorForm)) ? decimal.Parse(nuevoValor) : devuelve;
        }

        internal decimal GetDecimalValueForm(string valorForm, int devuelve = 0)
        {
            if (string.IsNullOrWhiteSpace(valorForm))
            {
                return 0;
            }

            string nuevoValor = valorForm.Replace(WebConfigValues.SeparadorMiles, "");
            nuevoValor = nuevoValor.Replace(WebConfigValues.SeparadorDecimales, ",");

            return (!string.IsNullOrEmpty(valorForm)) ? decimal.Parse(nuevoValor) : devuelve;
        }

        internal bool GetBoolValueForm(string valorForm, bool devuelve = false)
        {
            return (!string.IsNullOrEmpty(valorForm)) ? bool.Parse(valorForm.ToString()) : devuelve;
        }

        internal bool? GetBoolValueFormOrNull(string valorForm)
        {
            if (!string.IsNullOrEmpty(valorForm))
            {
                return Boolean.Parse(valorForm.ToString());
            }

            return null;
        }

        internal int[] GetArrayIntValueForm(string valorForm)
        {
            if (string.IsNullOrEmpty(valorForm))
            {
                return new int[] { };
            }

            string[] check = valorForm.Split(',');
            int[] keys = Array.ConvertAll(check, int.Parse);

            return keys;
        }

        internal DateTime GetDateTimeValueOrNow(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {                
                return DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return System.DateTime.Now;
        }

        internal void CreateEventoExpediente(int causaID, int tipoTramiteID, int opcionesTramiteID, DateTime fecha, int UsuarioActiveID)
        {
            ICommonAppServices appCommon = new CommonAppServices();
            IExpedienteAppServices appExpediente = new ExpedienteAppServices();

            var asoc = appCommon.GetAsocTipoTramiteOpciones(tipoTramiteID).FirstOrDefault(x => x.OpcionesTramiteID == opcionesTramiteID);
            if (asoc != null)
            {
                DTO.Models.Expediente dto = new DTO.Models.Expediente();
                dto.ExpedienteID = 0;
                dto.TipoTramiteID = tipoTramiteID;
                dto.CausaID = causaID;
                dto.UsuarioID = UsuarioActiveID;
                dto.UsuarioResponsableID = UsuarioActiveID;
                dto.FechaExpediente = fecha;
                dto.IsAdmisible = false;
                dto.Observacion = "";
                dto.Comentario = "";
                dto.NumeroOficio = "";
                dto.PlazoDias = asoc.PlazoDias;
                dto.IsHabil = asoc.IsDiasHabiles;
                dto.IsTabla = asoc.IsTabla;
                dto.IsFinalizado = true;

                dto.ExpedienteID = appExpediente.SaveExpediente(dto);

                DTO.Models.AsocExpedienteOpcion opt = new DTO.Models.AsocExpedienteOpcion();
                opt.AsocExpedienteOpcionID = 0;
                opt.ExpedienteID = dto.ExpedienteID;
                opt.OpcionesTramiteID = opcionesTramiteID;

                opt.AsocExpedienteOpcionID = appExpediente.SaveAsocExpedienteOpcion(opt);

                DBLogger dbLog = new DBLogger();
                dbLog.Fecha = fecha;
                dbLog.UsuarioID = UsuarioActiveID;
                dbLog.ExpedienteID = dto.ExpedienteID;

                dbLog.TipoLog = Enums.TipoLog.SaveEventoExpediente;
                dbLog.Save();

                SetCambiaEstadoCausa(fecha, UsuarioActiveID, causaID, asoc, dbLog);
            }
        }


        /// <summary>
        /// SetCambiaEstadoCausa
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="UsuarioActiveID"></param>
        /// <param name="CausaID"></param>
        /// <param name="asoc"></param>
        /// <param name="dbLog"></param>
        public void SetCambiaEstadoCausa(DateTime fecha, int UsuarioActiveID, int CausaID, DTO.Models.AsocTipoTramiteOpciones asoc, DBLogger dbLog)
        {
            if (asoc.IsFinalizaIngreso && asoc.EstadoCausaID != null)
            {
                IExpedienteAppServices appExpediente = new ExpedienteAppServices();

                DTO.Models.Causa causa = appExpediente.GetCausa(CausaID);

                Enums.EstadoCausa estadoActual = (Enums.EstadoCausa)causa.EstadoCausaID;
                Enums.EstadoCausa estadoNew = (Enums.EstadoCausa)asoc.EstadoCausaID;

                appExpediente.CambiarEstadoCausa(CausaID, estadoNew);

                dbLog.TipoLog = Enums.TipoLog.CambiaEstadoCausa;
                dbLog.Save();

                LogCausa _logC = new LogCausa();
                _logC.Fecha = fecha;
                _logC.CausaID = CausaID;
                _logC.UsuarioID = UsuarioActiveID;
                _logC.EstadoCausa = estadoNew;
                _logC.Observaciones = $"{estadoActual} ==> {estadoNew}";
                _logC.TipoLog = dbLog.TipoLog;
                _logC.Save();
            }
        }

        internal DateTime? GetDateTimeValueOrNull(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                return DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }

            return null;
        }

        internal string GetDateTimeValueOrNull(DateTime? date)
        {
            if (date != null)
            {
                return date.Value.ToString("dd-MM-yyy");
            }

            return "";
        }

        internal int? GetIntValueOrNull(string valorForm)
        {
            if (!string.IsNullOrEmpty(valorForm))
            {
                return int.Parse(valorForm.ToString());
            }

            return null;
        }

        internal bool IsEmail(string email)
        {
            bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return isEmail;
        }

        internal bool EmailIsValid(string email)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        internal bool IsInputValue(string valorForm)
        {
            return (!string.IsNullOrEmpty(valorForm));
        }

        internal string CheckSessionVar(string variable)
        {
            return (HttpContext.Current.Session[variable] == null) ? "" : HttpContext.Current.Session[variable].ToString();
        }


        #endregion

        #region Verificación Email
        /// <summary>
        /// Guarda CodigoVerificacion en sessión para el captcha
        /// </summary>
        /// <param name="KeyCaptcha"></param>
        /// <param name="CodigoVerificacion"></param>
        public void SaveCodigoVerificacionSession(string KeyCaptcha, string CodigoVerificacion)
        {
            IList<DTO.ItemSession> lista = new List<DTO.ItemSession>();

            if (HttpContext.Current.Session["CodigoVerificacion"] != null)
            {
                var listSession = HttpContext.Current.Session["CodigoVerificacion"] as List<DTO.ItemSession>;

                foreach (var item in listSession)
                {
                    DTO.ItemSession x = new DTO.ItemSession();
                    x.Key = item.Key;
                    x.Value = item.Value;

                    lista.Add(x);
                }
            }

            DTO.ItemSession add = new DTO.ItemSession();
            add.Key = KeyCaptcha;
            add.Value = CodigoVerificacion;

            lista.Add(add);

            HttpContext.Current.Session["CodigoVerificacion"] = lista;
        }

        /// <summary>
        /// Devuelve CodigoVerificacion para captcha
        /// </summary>
        /// <param name="KeyCaptcha"></param>
        /// <returns></returns>
        public string GetCodigoVerificacionByKey(string KeyCaptcha)
        {
            string valor = KeyCaptcha;

            if (HttpContext.Current.Session["CodigoVerificacion"] != null)
            {
                var listSession = HttpContext.Current.Session["CodigoVerificacion"] as IList<DTO.ItemSession>;

                var obj = listSession.Where(x => x.Key == KeyCaptcha).LastOrDefault();
                if (obj != null)
                {
                    valor = obj.Value;
                }
            }

            return valor;
        }
        #endregion

        internal void SetError(Exception ex, DBLogger dbLog)
        {
            Logger.Execute().Error(ex);
            dbLog.Error = ex.Message;
            dbLog.TipoLog = TipoLog.Error;

            try
            {
                dbLog.Error = ex.InnerException.InnerException.Message + " - " + ex.Message;
            }
            catch (Exception error)
            {
                Logger.Execute().Error(error);
            }

            try
            {
                dbLog.Save();
            }
            catch (Exception ex2)
            {
                Logger.Execute().Error(ex2);
            }
        }

        internal bool IsDirectoryEmpty(string path)
        {
            try
            {
                IEnumerable<string> items = System.IO.Directory.EnumerateFileSystemEntries(path);
                using (IEnumerator<string> en = items.GetEnumerator())
                {
                    return !en.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                return false;
            }
        }
    }

    /// <summary>
    /// Gestion de servidores Active Directory
    /// </summary>
    public class AdConfig
    {
        /// <summary>
        /// Obtiene datos encriptados del web.config.
        /// </summary>
        public AdConfig()
        {
            this.Servers = new List<ServerElement>();

            var appCommon = new Application.Services.CommonAppServices();
            int VersionEncriptID = 1;
            var VersionEncript = appCommon.GetVersionEncriptById(VersionEncriptID);
            Infrastructure.Utils.TheHash xEnc = new Infrastructure.Utils.TheHash(VersionEncript.Cadena.Trim());

            foreach (ServerElement srv in ServersAD.GetServersAD())
            {
                srv._User = xEnc.DecryptData(srv.Username);
                srv._Pass = xEnc.DecryptData(srv.Password);

                this.Servers.Add(srv);
            }
        }

        /// <summary>
        /// Lista de servidores Active Directory
        /// </summary>
        public IList<ServerElement> Servers { get; set; }
    }


    /// <summary>
    /// Objeto que gestiona el Log de la solicitud
    /// </summary>
    public class LogCausa
    {
        /// <summary>
        /// 
        /// </summary>
        public int CausaID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EstadoCausa EstadoCausa { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Fecha { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int UsuarioID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TipoLog TipoLog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Observaciones { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LogCausaID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LogCausa()
        {
            this.Fecha = DateTime.Now;
            this.TipoLog = TipoLog.ActionLog;
            this.Observaciones = string.Empty;
        }

        /// <summary>
        /// Guarda un registro en la tabla LogSolicitud
        /// </summary>
        public void Save()
        {
            DTO.Models.LogCausa log = new DTO.Models.LogCausa();
            log.CausaID = this.CausaID;
            log.EstadoCausaID = (int)this.EstadoCausa;
            log.Fecha = this.Fecha;
            log.UsuarioID = this.UsuarioID;
            log.Descripcion = this.TipoLog.ToString();
            log.Observaciones = this.Observaciones;

            IExpedienteAppServices appSI = new ExpedienteAppServices();
            try
            {
                this.LogCausaID = appSI.SaveLogCausa(log);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DBLogger
    {
        private readonly ICommonAppServices app = new CommonAppServices();

        /// <summary>
        /// string Error
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// int ExpedienteID
        /// </summary>
        public int ExpedienteID { get; set; }
        /// <summary>
        /// TipoLog TipoLog
        /// </summary>
        public TipoLog TipoLog { get; set; }
        /// <summary>
        /// int UsuarioID
        /// </summary>
        public int UsuarioID { get; set; }
        /// <summary>
        /// DateTime Fecha
        /// </summary>
        public System.DateTime Fecha { get; set; }
        /// <summary>
        /// Parametros
        /// </summary>
        public List<DTO.ItemForm> Parametros { get; set; }
        /// <summary>
        /// string CustomParams
        /// </summary>
        public string CustomParams { get; set; }
        /// <summary>
        /// int LogSistemaID
        /// </summary>
        public int LogSistemaID { get; set; }

        /// <summary>
        /// DBLogger
        /// </summary>
        public DBLogger()
        {
            this.Error = string.Empty;
            this.TipoLog = TipoLog.ActionLog;
            this.Fecha = DateTime.Now;
            this.Parametros = new List<DTO.ItemForm>();
        }

        private string GetFormValues()
        {

            ArrayList lista = new ArrayList();

            foreach (var item in Parametros)
            {
                lista.Add(item.Nombre + "=" + item.Valor);
            }

            return string.Join("#", lista.ToArray());
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(bool IncluyeParametros = true, bool ReplaceParams = false)
        {

            DTO.Models.LogSistema log = new DTO.Models.LogSistema();
            log.LogSistemaID = 0;
            log.Fecha = this.Fecha;
            log.Accion = string.Empty;

            var _request = HttpContext.Current.Request;

            Parametros = new List<DTO.ItemForm>();

            //keys que no se guardan en el Log
            String[] Filtro = { "__RequestVerificationToken", "Password", "Clave" };

            foreach (string item in _request.Form.Keys)
            {
                DTO.ItemForm p = new DTO.ItemForm();
                p.Nombre = item;
                p.Valor = _request.Form[item];

                if (!Filtro.Contains(item))
                {
                    Parametros.Add(p);
                }
            }

            log.Pagina = _request.ServerVariables["REMOTE_ADDR"].ToString();

            try
            {
                log.Accion = _request.RequestContext.RouteData.GetRequiredString("action");
            }
            catch (Exception e2)
            {
                Logger.Execute().Error(e2);
            }

            log.IpUsuario = _request.ServerVariables["REMOTE_ADDR"].ToString();
            log.Descripcion = Error + " - " + _request.ServerVariables["HTTP_USER_AGENT"];


            log.ExpedienteID = ExpedienteID;
            log.UsuarioID = this.UsuarioID;

            log.Parametros = (IncluyeParametros) ? GetFormValues() : string.Empty;
            log.Tipo = TipoLog.ToString();

            if (ReplaceParams)
            {
                log.Parametros = CustomParams;
            }

            try
            {
                this.LogSistemaID = app.SaveLogSistema(log);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }

        }

    }

}