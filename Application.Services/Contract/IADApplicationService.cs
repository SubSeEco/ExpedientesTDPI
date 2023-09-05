using System.Collections.Generic;

using Application.DTO.AD;

namespace Application.Services
{
    public interface IADApplicationService
    {
        Credential getCredential();

        bool IsAuthenticated(string userName, string password);

        List<string> getGroups(Credential credencial);

        List<string> getUserMemberOf(string userName);

        string getFullUserName(string userName, string password);

        string getAttributeUser(string username, string attributeName);

        #region econtreras_20140702
        IList<DTO.UserAD> getAllUsersFromAD();
        #endregion 

        IList<UserAD> GetAllUsers(Domain.Infrastructure.ServerElement server);
    }
}

