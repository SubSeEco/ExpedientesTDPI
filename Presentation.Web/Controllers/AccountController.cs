using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Application.Services;
using Domain.Infrastructure;
using Application.DTO;
using Presentation.Web.Context;
using Infrastructure.Logging;
using Enums = Domain.Infrastructure;
using DTO = Application.DTO;

using System.Net;
using System.Collections.Specialized;
using System.Text;


namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Controller de login de usuarios internos
    /// </summary>
    [SiteContext]
    public class AccountController : GlobalController
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();

        private readonly bool IsDesarrollo = WebConfigValues.IsDesarrollo;

        private readonly Commons active = new Commons();

        /// <summary>
        /// Vista Inicial
        /// </summary>
        public ActionResult Index()
        {
            if (!WebConfigValues.IsAccesoPublico)
            {
                return Redirect(WebConfigValues.Url_MenuSistemas);
            }

            return View();
        }


        /// <summary>
        /// Login (Process)
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            string errorMsg1 = "No posible conectar con el servidor";
            string errorMsg2 = "Usuario desconocido o contraseña incorrecta";
            string errorMsg3 = "Servicio no disponible";
            string errorMsg4 = "Usuario AD no autorizado";

            try
            {
                IList<Application.DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                if (IsDesarrollo)
                {
                    StatusLoginEnum status = StatusLoginEnum.Fail;
                    
                    try
                    {
                        int UserID = Convert.ToInt32(Request.Form["UsuarioID"].ToString());
                        var user = Usuarios.FirstOrDefault(x => x.UsuarioID == UserID);
                        
                        CreateSessionUser(user, false);

                        status = StatusLoginEnum.Success;
                                                                      
                    }
                    catch (Exception ex)
                    {
                        Logger.Execute().Error(ex);
                        ModelState.AddModelError("", "Error en Login");
                        return View(model);
                    }

                    if (status == StatusLoginEnum.Success)
                    {
                        var perfiles = appCommon.PerfilesFuncionario(Convert.ToInt32(Request.Form["UsuarioID"].ToString()));

                        if (perfiles.Count == 0)
                        {
                            ModelState.AddModelError("", errorMsg4);
                            return View("Login", model);
                        }

                        if (perfiles.Count == 1 && perfiles.Any(x => x.PerfilID == (int)Enums.Perfil.Administrador))
                        {
                            return RedirectToRoute("ActionInitialAdmin");
                        }

                        return RedirectToRoute("ActionInitialSystem");
                    }
                    else if (status == StatusLoginEnum.Fail)
                    {
                        ModelState.AddModelError("", errorMsg3);
                        return View("Login", model);
                    }
                    else
                    {
                        ModelState.AddModelError("", errorMsg2);
                        return View("Login", model);
                    }
                }
                else if (WebConfigValues.LoginAD)
                {
                    var AD = new DC.AccesoActDirect();
                    bool login = AD.ValidaUsuario(model.Usuario, model.Password);
                    if (login)
                    {
                        var user = Usuarios.FirstOrDefault(x => x.AdID.ToLower().Trim() == model.Usuario.ToLower().Trim());
                        if (user != null)
                        {
                            CreateSessionUser(user, false);
                        }

                        int FuncionarioID = Convert.ToInt32(Session["AtributoAD"]);
                        var perfiles = appCommon.PerfilesFuncionario(FuncionarioID);

                        if (perfiles.Count == 0)
                        {
                            ModelState.AddModelError("", errorMsg4);
                            return View("Login", model);
                        }

                        if (perfiles.Count == 1 && perfiles.Any(x=> x.PerfilID == (int)Enums.Perfil.Administrador))
                        {
                            return RedirectToRoute("ActionInitialAdmin");
                        }

                        return RedirectToRoute("ActionInitialSystem");
                    }
                    else
                    {
                        ModelState.AddModelError("", errorMsg2);
                        return View("Login", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", errorMsg1);
                    return View("Login", model);
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                ModelState.AddModelError("", "Error en Login");
                return View(model);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session.Abandon();
            Session.Clear();

            if (WebConfigValues.IsAccesoPublico)
            {
                return RedirectToRoute("VistaPublicaAcceso");
            }

            return View();
        }

        /// <summary>
        ///  LogOut - LogOff
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            Logger.Execute().Info("ActionResult LogOff");

            Session.Abandon();
            Session.Clear();

            return Redirect(WebConfigValues.UrlAuthentication.ToString());
        }

        /// <summary>
        /// Logout de usuario externo
        /// </summary>
        /// <returns></returns>
        public ActionResult Salir()
        {
            bool IsUsuarioCU = false;
            bool IsInvitado = false;

            if (active.IsExistSessionVar("IsUsuarioCU"))
            {
                IsUsuarioCU = (bool)Session["IsUsuarioCU"];
            }

            if (active.IsExistSessionVar("IsInvitado"))
            {
                IsInvitado = (bool)Session["IsInvitado"];
            }

            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();

            if (IsUsuarioCU)
                return RedirectToRoute("VistaPublicaAcceso");

            if (IsInvitado)
                return Redirect("http://www.tdpi.cl/");

            return RedirectToRoute("LoginAnonymous");
        }

        #region ClaveUnica
        /// <summary>
        /// Login clave unica, vista inicial
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginCU()
        {
            Session.Abandon();

            string login_token = Request.Cookies.Get("__RequestVerificationToken").Value;
            if (login_token == null)
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);

            Llamada llamada = new Llamada(login_token);

            string url = string.Format("{0}client_id={1}&redirect_uri={2}&response_type={3}&scope={4}&state={5}",
                llamada.GetUrlAuthorize(), llamada.GetClientId(), llamada.GetRedirectUrl(),
                llamada.GetResponseType(), llamada.GetScope(), llamada.GetState());

            return Redirect(url);
        }

        /// <summary>
        /// Redireccion de Clave Unica
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResponseCU(string code, string state)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            {
                Logger.Execute().Info("Error ResponseCU: code or state is empty");
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);
            }

            string login_token = Request.Cookies.Get("__RequestVerificationToken").Value;
            if (login_token == null)
            {
                Logger.Execute().Info("Error ResponseCU: login_token == null");
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);
            }

            if (state != login_token)
            {
                Logger.Execute().Info("Error ResponseCU: state != login_token");
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);
            }

            DBLogger dbLog = new DBLogger();

            try
            {
                Llamada llamada = new Llamada(state);

                TokenAccess TokenAccess = new TokenAccess();
                Persona Persona = new Persona();
                string HtmlResult = "";

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["client_id"] = llamada.GetClientId();
                    values["client_secret"] = llamada.GetClientSecret();
                    values["redirect_uri"] = llamada.GetRedirectUrl();
                    values["grant_type"] = llamada.GetGrantType();
                    values["code"] = code;
                    values["state"] = state;

                    var response = client.UploadValues(llamada.GetRedirectUrlPOST(), values);
                    var responseString = Encoding.Default.GetString(response);

                    TokenAccess = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenAccess>(responseString);

                    //Datos Usuario
                    string URI = llamada.GetUrlUserInfo();
                    client.Headers.Add("Content-Type", "text");
                    client.Headers[HttpRequestHeader.Authorization] = "Bearer " + TokenAccess.access_token;
                    HtmlResult = client.DownloadString(URI);

                    Persona = Newtonsoft.Json.JsonConvert.DeserializeObject<Persona>(HtmlResult);
                }

                ViewBag.TokenAccess = TokenAccess;
                ViewBag.Persona = Persona;

                DateTime ahora = DateTime.Now;

                if (Persona.IsValid())
                {
                    #region SaveLog

                    DTO.Models.LogSistema log = new DTO.Models.LogSistema();
                    log.LogSistemaID = 0;
                    log.Fecha = DateTime.Now;
                    log.Pagina = "Login";
                    log.Accion = "ResponseCU";
                    log.Parametros = HtmlResult;
                    log.IpUsuario = Request.ServerVariables["REMOTE_ADDR"].ToString();
                    log.Tipo = "Login Clave Unica";
                    log.Descripcion = $"OK - {Persona.GetRolUnico()}";

                    appCommon.SaveLogSistema(log);

                    #endregion

                    #region Verifica Usuario + si no existe lo crea
                    
                    var usuarios = appCommon.GetUsuarios();
                    bool ExisteUsuario = usuarios.Any(x => x.Rut == Persona.RolUnico.numero);
                                   
                    DTO.Models.Usuario u = new DTO.Models.Usuario();

                    if (ExisteUsuario)
                    {
                        u = appCommon.GetUsuarios().FirstOrDefault(x => x.Rut == Persona.RolUnico.numero);
                    }
                    else
                    {
                        #region Crea Usuario
                        u.UsuarioID = -1;
                        u.IsClaveUnica = true;
                        u.Nombres = Persona.GetNombres();
                        u.Apellidos = Persona.GetApellidos();
                        u.FechaRegistro = ahora;
                        u.FechaModificacion = ahora;

                        u.AdID = string.Empty;
                        u.Rut = Persona.RolUnico.numero;                        
                        u.Mail = string.Empty;
                        u.Telefono = string.Empty;
                        u.TipoGeneroID = (int)Enums.TipoGenero.Otro;
                        u.Signer = string.Empty;

                        u.UsuarioID = appCommon.SaveUser(u);
                        
                        #endregion

                        dbLog.TipoLog = TipoLog.CreaCiudadanoClaveUnica;
                        dbLog.Save();
                    }

                    #endregion

                    #region Realiza Login

                    bool IsInvitado = u.AsocUsuarioPerfil.Count == 0;

                    CreateSessionUser(u, IsInvitado);       

                    dbLog.TipoLog = TipoLog.LoginSuccessCU;
                    dbLog.UsuarioID = u.UsuarioID;
                    dbLog.Save();

                    return RedirectToRoute("VistaPublicaDashboard");

                    #endregion
                }

                return RedirectToRoute("VistaPublicaAcceso", new { typeMsg = 10 });
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);
            }

        }
        #endregion

        /// <summary>
        /// Acceso Público General
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public ActionResult LoginAnonymous()
        {
            DBLogger dbLog = new DBLogger();

            try
            {
                //string login_token = Request.Cookies.Get("__RequestVerificationToken").Value;
                //if (login_token == null)
                //    return Redirect(WebConfigValues.LogOffAuthenticationSystem);

                DTO.Models.Usuario u = new Application.DTO.Models.Usuario();
                u.UsuarioID = (int)Enums.GenericJson.UserInvitado;
                u.Nombres = "Invitado";
                u.Apellidos = "";
                u.IsClaveUnica = false;
                CreateSessionUser(u, true);

                dbLog.TipoLog = TipoLog.LoginAnonymous;
                dbLog.UsuarioID = u.UsuarioID;
                dbLog.Save();

                return RedirectToRoute("EscritorioCausas");
            }
            catch (Exception ex)
            {
                active.SetError(ex, dbLog);
                return Redirect(WebConfigValues.LogOffAuthenticationSystem);
            }
        }

        private void CreateSessionUser(DTO.Models.Usuario user, bool IsInvitado)
        {
            Session.Add("FullUserName", user.GetFullName());
            Session.Add("UsuarioID", user.UsuarioID);
            Session.Add("AtributoAD", user.UsuarioID);
            Session.Add("IsUsuarioCU", user.IsClaveUnica);
            Session.Add("IsInvitado", IsInvitado);

            var asoc = appCommon.GetAsocUsuarioPerfil();
            IList<DTO.Models.AsocUsuarioPerfil> AsocUsuarioPerfil = new List<DTO.Models.AsocUsuarioPerfil>();

            if (asoc.Any(x => x.UsuarioID == user.UsuarioID))
            {
                var filterUser = asoc.Where(x => x.UsuarioID == user.UsuarioID).ToList();
                foreach (var f in filterUser)
                {
                    AsocUsuarioPerfil.Add(f);
                }
            }

            //if (AsocUsuarioPerfil.Count > 0)
            //{
            //    AsocUsuarioPerfil.Add(new Application.DTO.Models.AsocUsuarioPerfil()
            //    {
            //        NombreUsuario = user.GetFullName(),
            //        UsuarioID = user.UsuarioID
            //    });
            //}

            Session.Add("AsocUsuarioPerfil", AsocUsuarioPerfil);
        }





        /// <summary>
        /// Método para cambiar Rol para usuarios Subrogantes.
        /// </summary>
        /// <param name="RolID"></param>
        /// <returns>0</returns>
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ChangeRol(int RolID)
        {
            try
            {
                int Retorno = 0;

                IList<Application.DTO.Models.Usuario> Usuarios = appCommon.GetUsuarios();

                var user = Usuarios.FirstOrDefault(x => x.UsuarioID == RolID);

                Session.Add("FullUserName", user.GetFullName().Trim());
                Session.Add("UsuarioID", user.UsuarioID);
                Session.Add("AtributoAD", user.UsuarioID);
                Session.Add("IsUsuarioCU", user.IsClaveUnica);

                return Json(new { result = Retorno });
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }
        }

    }
}