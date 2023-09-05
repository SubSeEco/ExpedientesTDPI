using System.Collections.Generic;

namespace Application.Services
{
    public interface IAccountApplicationService
    {
        DTO.StatusLoginEnum ValidateUserLogin(string username, string password);

        string getFullUserName(string username, string password);

        //void CreateTokenUser(DTO.SSO.TokenUsuario token, IList<DTO.Sistema> sistemasConAcceso);

        bool LoginActiveDirectory(string username, string password);

        IList<DTO.AD.GrupoAD> getGruposADByUsername(string username);

        IList<DTO.AD.GrupoADEntity> getAtributosBySistemaID(int SistemaID);
    }
}
