using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure
{
    public static class WebConfigValues
    {
        private const string _url_MenuSistema = "Url_MenuSistemas";
        private const string _authenticationURL = "LoginAuthenticationSystem";
        private const string _LogOffAuthenticationSystem = "LogOffAuthenticationSystem";
        private const string _LocalCountryID = "LocalCountryID";


        private const string _CantidadResultadosGrilla = "CantidadResultadosGrilla";

        private const string _Auth_Token = "Auth_Token";
        private const string _Auth_Token_Validate = "Auth_Token_Validate";
     
        private const string _CookieSubdomain = "CookieSubdomain";
        private const string _CookieDomain = "CookieDomain";

        private const string _isEmailDesarrollo = "isEmailDesarrollo";
        private const string _EmailDesarrollo = "EmailDesarrollo";
        private const string _EmailConCopia = "EmailConCopia";

        private const string _PrefijoFolioSolicitud = "PrefijoFolioSolicitud";

        private const string _IsLocalRepository = "IsLocalRepository";
        private const string _PathBaseRepository = "PathBaseRepository";

        private const string _ValidarCorreoElectronico = "ValidarCorreoElectronico";
        private const string _DiasConsultaSolicitud = "DiasConsultaSolicitud";
        private const string _DiasParaSolicitarAutorizacion = "DiasParaSolicitarAutorizacion";
        
        private const string _SP_URL = "SP_URL";
        private const string _SP_User = "SP_User";
        private const string _SP_Pass = "SP_Pass";
        private const string _SP_Domain = "SP_Domain";
        private const string _SP_Biblioteca = "SP_Biblioteca";

        private const string _LDAP_Server = "LDAP_Server";
        private const string _DC_User = "DC_User";
        private const string _DC_Pass = "DC_Pass";
        private const string _DC_Path = "DC_Path";
        private const string _DC_FilterGroup = "DC_FilterGroup";

        private const string _EmailOIRS = "EmailOIRS";
        private const string _EmailTransparencia = "EmailTransparencia";

        private const string _IsAccesoPublico = "IsAccesoPublico";
        private const string _IsCanalDenuncias = "IsCanalDenuncias";

        private const string _URL_BuzonVirtual = "URL_BuzonVirtual";
        private const string _URL_BuzonVirtual_OIRS = "URL_BuzonVirtual_OIRS";
        private const string _URL_BuzonVirtual_Transparencia = "URL_BuzonVirtual_Transparencia";

        private const string _LoginAD = "LoginAD";

        private const string _DiasRecordatorioOIRS = "DiasRecordatorioOIRS";
        private const string _DiasRecordatorioTransparencia = "DiasRecordatorioTransparencia";

        private const string _IsBotonEscribaMinistra = "IsBotonEscribaMinistra";
        private const string _IsBotonIngresoTransparencia = "IsBotonIngresoTransparencia";

        private const string _NumeroMaximoArchivosIngreso = "NumeroMaximoArchivosIngreso";

        private const string _LoginAuthenticationCiudadano = "LoginAuthenticationCiudadano";
        private const string _ValidacionMimeTyme = "ValidacionMimeTyme";
        private const string _TipoAmbiente = "TipoAmbiente";

        private const string _IsAppDebug = "IsAppDebug";

        private const string _IsMostrarDatosTercero = "IsMostrarDatosTercero";
        private const string _IsMostrarRespuestaEnIngreso = "IsMostrarRespuestaEnIngreso";
        private const string _IsVersion3 = "IsVersion3";
        private const string _IsCiudadanoInLocalDatabase = "IsCiudadanoInLocalDatabase"; 

        private const string _IsEnableAnonymousUser = "IsEnableAnonymousUser";
        private const string _IsEnableSearchUser = "IsEnableSearchUser";
        private const string _SeparadorDecimales = "SeparadorDecimales";
        private const string _SeparadorMiles = "SeparadorMiles";

        private const string _IsDesarrollo = "IsDesarrollo";
        private const string _IsLoginSOA = "IsLoginSOA";

        private const string _ExpirationCacheInMinutes = "ExpirationCacheInMinutes";
        private const string _AbsoluteUriCreateSolicitud = "AbsoluteUriCreateSolicitud";

        private const string _GoogleAnalyticsUA = "GoogleAnalyticsUA"; 

        private const string _CU_URL_AUTHORIZE = "CU_URL_AUTHORIZE";
        private const string _CU_CLIENT_ID = "CU_CLIENT_ID";
        private const string _CU_CLIENT_SECRET = "CU_CLIENT_SECRET";
        private const string _CU_REDIRECT_URI = "CU_REDIRECT_URI";
        private const string _CU_RESPONSE_TYPE = "CU_RESPONSE_TYPE";
        private const string _CU_SCOPE = "CU_SCOPE";
        private const string _CU_GRANT_TYPE = "CU_GRANT_TYPE";
        private const string _CU_REDIRECT_URI_POST = "CU_REDIRECT_URI_POST";
        private const string _CU_URL_USER_INFO = "CU_URL_USER_INFO";

        private const string _IsAmbienteTest = "IsAmbienteTest";
        private const string _AmbienteTestInfo = "AmbienteTestInfo";

        private const string _LoginURL = "LoginURL";
        private const string _AnioInicial = "AnioInicial";
        private const string _NombrePresidente = "NombrePresidente"; 
        private const string _SistemaPublicoURL = "SistemaPublicoURL";
        private const string _ActiveDirectoryHost = "ActiveDirectoryHost";
        

        #region Mails

        private const string _SmtpClient_Host = "SmtpClient_Host";
        private const string _SmtpClient_Port = "SmtpClient_Port";
        private const string _SmtpClient_EnableSsl = "SmtpClient_EnableSsl";
        private const string _SmtpClient_User = "SmtpClient_User";
        private const string _SmtpClient_Password = "SmtpClient_Password";
        private const string _Mail_Name = "Mail_Name";
        private const string _Mail_Sender = "Mail_Sender";
        private const string _Mail_ConCopia = "Mail_ConCopia";
        private const string _Mail_IsSMTP = "Mail_IsSMTP";

        public static string SmtpClient_Host
        {
            get { return GetStrintConfigData(_SmtpClient_Host); }
        }
        public static int SmtpClient_Port
        {
            get { return GetIntConfigData(_SmtpClient_Port); }
        }
        public static bool SmtpClient_EnableSsl
        {
            get { return GetBooleanConfigData(_SmtpClient_EnableSsl); }
        }
        public static string SmtpClient_User
        {
            get { return GetStrintConfigData(_SmtpClient_User); }
        }
        public static string SmtpClient_Password
        {
            get { return GetStrintConfigData(_SmtpClient_Password); }
        }
        public static string Mail_Name
        {
            get { return GetStrintConfigData(_Mail_Name); }
        }
        public static string Mail_Sender
        {
            get { return GetStrintConfigData(_Mail_Sender); }
        }
        public static string Mail_ConCopia
        {
            get { return GetStrintConfigData(_Mail_ConCopia); }
        }
        public static bool Mail_IsSMTP
        {
            get { return GetBooleanConfigData(_Mail_IsSMTP); }
        }

        #endregion


        #region Strings
        public static string ActiveDirectoryHost
        {
            get { return GetStrintConfigData(_ActiveDirectoryHost); }
        }
        public static string SistemaPublicoURL
        {
            get { return GetStrintConfigData(_SistemaPublicoURL); }
        }
        public static string NombrePresidente
        {
            get { return GetStrintConfigData(_NombrePresidente); }
        }
        public static string LoginURL
        {
            get { return GetStrintConfigData(_LoginURL); }
        }

        public static string AmbienteTestInfo
        {
            get { return GetStrintConfigData(_AmbienteTestInfo); }
        }

        public static string CU_URL_AUTHORIZE
        {
            get { return GetStrintConfigData(_CU_URL_AUTHORIZE); }
        }

        public static string CU_CLIENT_ID
        {
            get { return GetStrintConfigData(_CU_CLIENT_ID); }
        }

        public static string CU_CLIENT_SECRET
        {
            get { return GetStrintConfigData(_CU_CLIENT_SECRET); }
        }

        public static string CU_REDIRECT_URI
        {
            get { return GetStrintConfigData(_CU_REDIRECT_URI); }
        }

        public static string CU_RESPONSE_TYPE
        {
            get { return GetStrintConfigData(_CU_RESPONSE_TYPE); }
        }

        public static string CU_SCOPE
        {
            get { return GetStrintConfigData(_CU_SCOPE); }
        }

        public static string CU_GRANT_TYPE
        {
            get { return GetStrintConfigData(_CU_GRANT_TYPE); }
        }

        public static string CU_REDIRECT_URI_POST
        {
            get { return GetStrintConfigData(_CU_REDIRECT_URI_POST); }
        }

        public static string CU_URL_USER_INFO
        {
            get { return GetStrintConfigData(_CU_URL_USER_INFO); }
        }

        public static string GoogleAnalyticsUA
        {
            get { return GetStrintConfigData(_GoogleAnalyticsUA); }
        }

        public static string AbsoluteUriCreateSolicitud
        {
            get { return GetStrintConfigData(_AbsoluteUriCreateSolicitud); }
        }


        public static string SeparadorDecimales
        {
            get { return GetStrintConfigData(_SeparadorDecimales); }
        }

        public static string SeparadorMiles
        {
            get { return GetStrintConfigData(_SeparadorMiles); }
        }


        public static string TipoAmbiente
        {
            get { return GetStrintConfigData(_TipoAmbiente); }
        }

        public static string LoginAuthenticationCiudadano
        {
            get { return GetStrintConfigData(_LoginAuthenticationCiudadano); }
        }

        public static string Url_MenuSistemas
            {
                get { return GetStrintConfigData(_url_MenuSistema); }
            }


            public static string UrlAuthentication
            {
                get { return GetStrintConfigData(_authenticationURL); }
            }

            public static string LogOffAuthenticationSystem
            {
                get { return GetStrintConfigData(_LogOffAuthenticationSystem); }
            }


            public static string Auth_Token
            {
                get { return GetStrintConfigData(_Auth_Token); }
            }

            public static string Auth_Token_Validate
            {
                get { return GetStrintConfigData(_Auth_Token_Validate); }
            }
            public static string CantidadResultadosGrilla
            {
                get { return GetStrintConfigData(_CantidadResultadosGrilla); }
            }

            public static string EmailDesarrollo
            {
                get { return GetStrintConfigData(_EmailDesarrollo); }
            }

            public static string EmailConCopia
            {
                get { return GetStrintConfigData(_EmailConCopia); }
            }

            public static string PrefijoFolioSolicitud
            {
                get { return GetStrintConfigData(_PrefijoFolioSolicitud); }
            }

            public static string PathBaseRepository
            {
                get { return GetStrintConfigData(_PathBaseRepository); }
            }

            public static string SP_URL
            {
                get { return GetStrintConfigData(_SP_URL); }
            }

            public static string SP_User
            {
                get { return GetStrintConfigData(_SP_User); }
            }

            public static string SP_Pass
            {
                get { return GetStrintConfigData(_SP_Pass); }
            }

            public static string SP_Domain
            {
                get { return GetStrintConfigData(_SP_Domain); }
            }

            public static string SP_Biblioteca
            {
                get { return GetStrintConfigData(_SP_Biblioteca); }
            }

            public static string LDAP_Server
            {
                get { return GetStrintConfigData(_LDAP_Server); }
            }
            
            public static string DC_User
            {
                get { return GetStrintConfigData(_DC_User); }
            }
            
            public static string DC_Pass
            {
                get { return GetStrintConfigData(_DC_Pass); }
            }
            public static string DC_Path
            {
                get { return GetStrintConfigData(_DC_Path); }
            }
        
            public static string EmailOIRS
            {
                get { return GetStrintConfigData(_EmailOIRS); }
            }

            public static string EmailTransparencia
            {
                get { return GetStrintConfigData(_EmailTransparencia); }
            }

            public static string URL_BuzonVirtual_OIRS
            {
                get { return GetStrintConfigData(_URL_BuzonVirtual_OIRS); }
            }

            public static string URL_BuzonVirtual_Transparencia
            {
                get { return GetStrintConfigData(_URL_BuzonVirtual_Transparencia); }
            }
            public static string URL_BuzonVirtual
            {
                get { return GetStrintConfigData(_URL_BuzonVirtual); }
            }



        #endregion


        #region Int

        public static int AnioInicial
        {
            get { return GetIntConfigData(_AnioInicial); }
        }

        public static int ExpirationCacheInMinutes
        {
            get { return GetIntConfigData(_ExpirationCacheInMinutes); }
        }

        public static int LocalCountryID
        {
            get { return GetIntConfigData(_LocalCountryID); }
        }

        public static int NumeroMaximoArchivosIngreso
        {
            get { return GetIntConfigData(_NumeroMaximoArchivosIngreso); }
        }

        public static int DiasConsultaSolicitud
        {
            get { return GetIntConfigData(_DiasConsultaSolicitud); }
        }

        public static int DiasParaSolicitarAutorizacion
        {
            get { return GetIntConfigData(_DiasParaSolicitarAutorizacion); }
        }

        public static int DiasRecordatorioOIRS
        {
            get { return GetIntConfigData(_DiasRecordatorioOIRS); }
        }
        public static int DiasRecordatorioTransparencia
        {
            get { return GetIntConfigData(_DiasRecordatorioTransparencia); }
        }

        #endregion


        #region Boolean
        public static bool IsAmbienteTest
        {
            get { return GetBooleanConfigData(_IsAmbienteTest); }
        }

        public static bool IsDesarrollo
        {
            get { return GetBooleanConfigData(_IsDesarrollo); }
        }
        public static bool IsLoginSOA
        {
            get { return GetBooleanConfigData(_IsLoginSOA); }
        }
        public static bool IsEnableAnonymousUser
        {
            get { return GetBooleanConfigData(_IsEnableAnonymousUser); }
        }
        public static bool IsEnableSearchUser
        {
            get { return GetBooleanConfigData(_IsEnableSearchUser); }
        }
        public static bool IsCiudadanoInLocalDatabase
        {
            get { return GetBooleanConfigData(_IsCiudadanoInLocalDatabase); }
        }
        public static bool IsVersion3
        {
            get { return GetBooleanConfigData(_IsVersion3); }
        }
        public static bool IsMostrarDatosTercero
        {
            get { return GetBooleanConfigData(_IsMostrarDatosTercero); }
        }
        public static bool IsMostrarRespuestaEnIngreso
        {
            get { return GetBooleanConfigData(_IsMostrarRespuestaEnIngreso); }
        }

        public static bool IsCanalDenuncias
        {
            get { return GetBooleanConfigData(_IsCanalDenuncias); }
        }

        public static bool IsAppDebug
        {
            get { return GetBooleanConfigData(_IsAppDebug); }
        }

        public static bool IsBotonEscribaMinistra
        {
            get { return GetBooleanConfigData(_IsBotonEscribaMinistra); }
        }

        public static bool ValidacionMimeTyme
        {
            get { return GetBooleanConfigData(_ValidacionMimeTyme); }
        }
        public static bool IsBotonIngresoTransparencia
        {
            get { return GetBooleanConfigData(_IsBotonIngresoTransparencia); }
        }

        public static bool CookieSubdomain
        {
            get { return GetBooleanConfigData(_CookieSubdomain); }
        }

        public static bool LoginAD
        {
            get { return GetBooleanConfigData(_LoginAD); }
        }

        public static bool CookieDomain
        {
            get { return GetBooleanConfigData(_CookieDomain); }
        }

        public static bool isEmailDesarrollo
        {
            get { return GetBooleanConfigData(_isEmailDesarrollo); }
        }

        public static bool IsLocalRepository
        {
            get { return GetBooleanConfigData(_IsLocalRepository); }
        }

        public static bool ValidarCorreoElectronico
        {
            get { return GetBooleanConfigData(_ValidarCorreoElectronico); }
        }

        public static bool IsAccesoPublico
        {
            get { return GetBooleanConfigData(_IsAccesoPublico); }
        }

        #endregion


        #region Arrays

        public static string[] DC_FilterGroup
        {
            get { return GetArrayConfigData(_DC_FilterGroup); }
        }

        #endregion

        private static bool GetBooleanConfigData(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return bool.Parse(ConfigurationManager.AppSettings[key]);
            }
            else
            {
                return false;
            }
        }

        private static int GetIntConfigData(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings[key]);
            }
            else
            {
                return 0;
            }
        }

        private static string GetStrintConfigData(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                return string.Empty;
            }
        }

        private static string[] GetArrayConfigData(string key)
        {
            string[] retorno = { };

            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                retorno = ConfigurationManager.AppSettings[key].Split(',');
            }

            return retorno;
        }

        private static int[] GetArrayIntConfigData(string key)
        {
            int[] retorno = { };

            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                string[] strings = ConfigurationManager.AppSettings[key].Split(',');

                retorno = Array.ConvertAll(strings, s => int.Parse(s));
            }

            return retorno;
        }
    }
}
