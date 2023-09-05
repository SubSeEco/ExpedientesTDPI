using System;
using System.Collections.Generic;
using Infrastructure.Logging;

namespace Application.Services
{
    public class AccountApplicationService : IAccountApplicationService
    {
        public DTO.StatusLoginEnum ValidateUserLogin(string username, string password)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return DTO.StatusLoginEnum.Fail;

            bool logeado = LoginActiveDirectory(username, password);
            if (logeado)
            {
                return DTO.StatusLoginEnum.Success;
            }

            return DTO.StatusLoginEnum.Fail;
        }

        public string getFullUserName(string username, string password)
        {
            IADApplicationService ad = new ADApplicationService();

            string user;
            try
            {
                user = ad.getFullUserName(username, password);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                user = string.Empty;
            }
            return user;
        }

        /*
        public void CreateTokenUser(DTO.SSO.TokenUsuario token, IList<DTO.Sistema> sistemasConAcceso)
        {

            ITokenUsuarioRepository repo = new TokenUsuarioRepository();

            //Borrar Creados
            repo.DeleteByUserAndSystem(token.usuarioID, 0);

            foreach (var sistema in sistemasConAcceso)
            {

                TokenUsuario newToken = new TokenUsuario();

                newToken.usuarioID = token.usuarioID;
                //newToken.SistemaID = token.SistemaID;
                newToken.SistemaID = sistema.SistemaID;
                //newToken.TokenUsuarioID = token.TokenUsuarioID;
                newToken.VersionEncripID = token.VersionEncripID;
                newToken.Caducidad = token.Caducidad;
                newToken.Token = token.Token;
                newToken.IP = token.IP;
                newToken.HashVerificacion = token.HashVerificacion;

                repo.Create(newToken);
            }

        }
        */

        public bool LoginActiveDirectory(string username, string password)
        {
            bool logeado = false;
            IADApplicationService ad = new ADApplicationService();

            try
            {
                logeado = ad.IsAuthenticated(username, password);
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                return false;
            }

            return logeado;
        }


        public IList<DTO.AD.GrupoAD> getGruposADByUsername(string username)
        {

            IList<DTO.AD.GrupoAD> listaDTO = new List<DTO.AD.GrupoAD>();

            IADApplicationService ad = new ADApplicationService();

            List<string> userMemberOf = ad.getUserMemberOf(username);

            DTO.AD.GrupoAD grupoDTO = null;

            foreach (var item in userMemberOf)
            {
                var grupo = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                grupoDTO = new DTO.AD.GrupoAD();
                grupoDTO.CN = grupo[0];
                grupoDTO.OU = grupo[1];
                listaDTO.Add(grupoDTO);
            }

            //grupoDTO = new DTO.AD.GrupoAD();
            //grupoDTO.CN = "Santo Domingo";
            //grupoDTO.OU = "Grupos de Distribución";
            //listaDTO.Add(grupoDTO);

            return listaDTO;
        }


        public IList<DTO.AD.GrupoADEntity> getAtributosBySistemaID(int SistemaID)
        {
            IList<DTO.AD.GrupoADEntity> listaDTO = new List<DTO.AD.GrupoADEntity>();
            /*
            IGruposADRepository repo = new GruposADRepository();
            var grupos = repo.getAtributosBySistemaID(SistemaID);

            foreach (var g in grupos)
            {
                var ng = new DTO.AD.GrupoADEntity();
                ng.SistemaID = g.SistemaID;
                ng.AtributoAD = g.AtributoAD.Trim();
                ng.GrupoID = g.GrupoID;
                listaDTO.Add(ng);
            }
            */
            return listaDTO;
        }
    }
}
