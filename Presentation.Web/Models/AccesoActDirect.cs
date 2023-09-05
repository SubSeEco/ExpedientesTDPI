using Infrastructure.Logging;
using System;
using Domain.Infrastructure;
using System.DirectoryServices;

namespace DC
{
    /// <summary>
    /// AccesoActDirect
    /// </summary>
    public class AccesoActDirect
    {
        /// <summary>
        /// ValidaUsuario AD
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public bool ValidaUsuario(string usuario, string clave)
        {
            bool retorno;
            string dominio = WebConfigValues.ActiveDirectoryHost;

            try
            {
                DirectoryEntry entry = new DirectoryEntry(("LDAP://" + dominio), usuario, clave);
                object nativeObj = entry.NativeObject;

                retorno = true;
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                retorno = false;
            }

            return retorno;
        }

    }
}