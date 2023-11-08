using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Infrastructure;
using Infrastructure.Logging;
using DTO = Application.DTO;
using Application.Services;

namespace Presentation.Web.Controllers
{
    /// <summary>
    /// Clase que gestiona la sesión del usuario
    /// </summary>
    public class SsoActionResult : ActionResult
    {
        private readonly ICommonAppServices appCommon = new CommonAppServices();

        /// <summary>
        /// 
        /// </summary>
        public bool _IsActiveSession { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private bool IsDesarrollo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<DTO.Models.Perfil> Perfiles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DTO.Models.Usuario UserActive { get; set; }

        internal bool IsTDPI()
        {
            return GetPerfilUsuario().Any(x => x.PerfilID == (int)Perfil.TDPI);
        }

        internal bool IsINAPI()
        {
            return GetPerfilUsuario().Any(x => x.PerfilID == (int)Perfil.INAPI);
        }

        internal bool IsAdministrador()
        {
            return GetPerfilUsuario().Any(x => x.PerfilID == (int)Perfil.Administrador);
        }

        internal bool IsSAG()
        {
            return GetPerfilUsuario().Any(x => x.PerfilID == (int)Perfil.SAG);
        }

        internal bool IsInvitado()
        {
            bool r = false;

            if (HttpContext.Current.Session["IsInvitado"] != null)
            {
                r = (bool)HttpContext.Current.Session["IsInvitado"];
            }

            return r;
        }

        internal bool IsClaveUnica()
        {
            bool r = false;

            if (HttpContext.Current.Session["IsUsuarioCU"] != null)
            {
                r = (bool)HttpContext.Current.Session["IsUsuarioCU"];
            }

            return r;
        }

        internal bool IsAbogado()
        {
            return GetPerfilUsuario().Any(x => x.PerfilID == (int)Perfil.Abogado);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsView"></param>
        public SsoActionResult(bool IsView = false)
        {
            this.Perfiles = new List<DTO.Models.Perfil>();
            this.IsDesarrollo = WebConfigValues.IsDesarrollo;
        }

        internal bool IsSinPerfil()
        {
            return Perfiles.Count == 0;
        }

        /// <summary>
        /// Se reemplaza ExecuteResult de los controladores para validar el usuario.
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            _IsActiveSession = true;
            bool isValidUser = false;

            try
            {
                isValidUser = SetAutenticateValues();
                if (isValidUser)
                {
                    HttpContext.Current.Session["PerfilDescription"] = GetPerfilDescription();
                }
                else
                {
                    if (HttpContext.Current.Session["IsInvitado"] != null)
                    {
                        bool IsInvitado = (bool)HttpContext.Current.Session["IsInvitado"];
                        if (IsInvitado)
                        {
                            HttpContext.Current.Session["PerfilDescription"] = "";
                            isValidUser = true;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw;
            }

            if (!isValidUser)
            {
                string authenticationSystemLogin = WebConfigValues.LogOffAuthenticationSystem;
                context.HttpContext.Response.Redirect(authenticationSystemLogin, true);
                context.HttpContext.Response.End();
                return;
            }
        }


        private bool SetAutenticateValues()
        {
            int usuario = GetUsuarioActivoID();
            if (usuario == (int)GenericJson.UserTMP)
                return false;

            string nombre = this.UserActive.GetFullName();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                if (this.UserActive == null) return false;

                FormsAuthentication.SetAuthCookie(nombre, true);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// AsyncAuthenticate
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool AsyncAuthenticate(ControllerContext context)
        {
            bool IsValidUser = false;

            try
            {
                IsValidUser = SetAutenticateValues();

                if (IsValidUser)
                {
                    HttpContext.Current.Session["PerfilDescription"] = GetPerfilDescription();
                }
                else
                {
                    if (HttpContext.Current.Session["IsInvitado"] != null)
                    {
                        bool IsInvitado = (bool)HttpContext.Current.Session["IsInvitado"];
                        if (IsInvitado)
                        {
                            HttpContext.Current.Session["PerfilDescription"] = "";
                            IsValidUser = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
            }

            return IsValidUser;
        }


        private string GetPerfilDescription()
        {
            List<string> perfil = new List<string>();

            HttpContext.Current.Session["IsAbogado"] = false;
            HttpContext.Current.Session["IsTDPI"] = false;
            HttpContext.Current.Session["IsINAPI"] = false;
            HttpContext.Current.Session["IsSAG"] = false;
            HttpContext.Current.Session["IsAdministrador"] = false;

            if (IsTDPI())
            {
                HttpContext.Current.Session["IsTDPI"] = true;
                perfil.Add(GetPerfilName(Perfil.TDPI));
            }

            if (IsINAPI())
            {
                HttpContext.Current.Session["IsINAPI"] = true;
                perfil.Add(GetPerfilName(Perfil.INAPI));
            }

            if (IsSAG())
            {
                HttpContext.Current.Session["IsSAG"] = true;
                perfil.Add(GetPerfilName(Perfil.SAG));
            }

            if (IsAdministrador())
            {
                HttpContext.Current.Session["IsAdministrador"] = true;
                perfil.Add(GetPerfilName(Perfil.Administrador));
            }

            if (IsAbogado())
            {
                HttpContext.Current.Session["IsAbogado"] = true;
                perfil.Add(GetPerfilName(Perfil.Abogado));
            }

            if (IsSinPerfil())
            {
                perfil.Add("Invitado");
            }

            return string.Join(" &#183; ", perfil);

        }

        private string GetPerfilName(Perfil perfil)
        {
            var p = Perfiles.FirstOrDefault(x => x.PerfilID == (int)perfil);
            return p.Descripcion.Trim();
        }


        /// <summary>
        /// Devuelve ID del usuario conectado
        /// </summary>
        /// <returns></returns>
        public int GetUsuarioActivoID()
        {
            int UsuarioID = (int)GenericJson.UserTMP;

            if (HttpContext.Current.Session["UsuarioID"] != null)
            {
                UsuarioID = int.Parse(HttpContext.Current.Session["UsuarioID"].ToString());
            }

            if (this.UserActive == null || UsuarioID != (int)GenericJson.UserTMP)
            {
                this.UserActive = appCommon.GetUsuarioByID(UsuarioID);

                UsuarioID = UserActive.UsuarioID;
            }

            return UsuarioID;
        }


        /// <summary>
        /// GetPerfilUsuario
        /// </summary>
        /// <returns></returns>
        public IList<DTO.Models.Perfil> GetPerfilUsuario()
        {
            this.Perfiles = appCommon.GetPerfilUsuario(GetUsuarioActivoID());

            return this.Perfiles;
        }

    }
}