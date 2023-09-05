using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebConfig = Domain.Infrastructure.WebConfigValues;

namespace Presentation.Web
{
    /// <summary>
    /// Llamada a Clave Unica
    /// </summary>
    public class Llamada
    {
        private string UrlAuthorize { get; set; }
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private string RedirectUrl { get; set; }
        private string RedirectUrlPOST { get; set; }
        private string UrlUserInfo { get; set; }
        private string ResponseType { get; set; }
        private string Scope { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// GrantType
        /// </summary>
        public string GrantType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state"></param>
        public Llamada(string state)
        {
            this.UrlAuthorize = WebConfig.CU_URL_AUTHORIZE;
            this.ClientId = WebConfig.CU_CLIENT_ID;
            this.ClientSecret = WebConfig.CU_CLIENT_SECRET;
            this.RedirectUrl = WebConfig.CU_REDIRECT_URI;
            this.ResponseType = WebConfig.CU_RESPONSE_TYPE;
            this.Scope = WebConfig.CU_SCOPE;
            this.State = state;
            this.GrantType = WebConfig.CU_GRANT_TYPE;
            this.RedirectUrlPOST = WebConfig.CU_REDIRECT_URI_POST;
            this.UrlUserInfo = WebConfig.CU_URL_USER_INFO;
        }

        /// <summary>
        /// GetUrlAuthorize
        /// </summary>
        /// <returns></returns>
        public string GetUrlAuthorize()
        {
            return this.UrlAuthorize;
        }

        /// <summary>
        /// GetClientId
        /// </summary>
        /// <returns></returns>
        public string GetClientId()
        {
            return this.ClientId;
        }

        /// <summary>
        /// GetRedirectUrl
        /// </summary>
        /// <returns></returns>
        public string GetRedirectUrl()
        {
            return HttpUtility.UrlEncode(this.RedirectUrl);
        }

        /// <summary>
        /// GetResponseType
        /// </summary>
        /// <returns></returns>
        public string GetResponseType()
        {
            return this.ResponseType;
        }


        /// <summary>
        /// GetScope
        /// </summary>
        /// <returns></returns>
        public string GetScope()
        {
            return this.Scope;
        }

        /// <summary>
        /// GetState
        /// </summary>
        /// <returns></returns>
        public string GetState()
        {
            return this.State;
        }

        /// <summary>
        /// GetGrantType
        /// </summary>
        /// <returns></returns>
        public string GetGrantType()
        {
            return this.GrantType;
        }

        /// <summary>
        /// GetClientSecret
        /// </summary>
        /// <returns></returns>
        public string GetClientSecret()
        {
            return this.ClientSecret;
        }

        /// <summary>
        /// GetRedirectUrlPOST
        /// </summary>
        /// <returns></returns>
        public string GetRedirectUrlPOST()
        {
            return this.RedirectUrlPOST;
        }


        /// <summary>
        /// GetUrlUserInfo
        /// </summary>
        /// <returns></returns>
        public string GetUrlUserInfo()
        {
            return this.UrlUserInfo;
        }


        private static Random random = new Random();

        /// <summary>
        /// RandomString
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}