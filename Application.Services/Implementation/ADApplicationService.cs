using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.DirectoryServices;
using Domain.Infrastructure;
using Application.DTO.AD;
using Infrastructure.Logging;

namespace Application.Services
{
    public class ADApplicationService : IADApplicationService
    {

        //private string userName = "";        

        public Credential getCredential()
        {
            string DC_User = WebConfigValues.DC_User;
            string DC_Pass = WebConfigValues.DC_Pass;
            string DC_Active = WebConfigValues.LDAP_Server;

            NameValueCollection domains = (NameValueCollection)ConfigurationManager.GetSection("domains");

            string domainController = domains[DC_Active.ToLower()];

            Credential credencial = new Credential();
            credencial.DC_Server = domainController;
            credencial.DC_User = DC_User;
            credencial.DC_Pass = DC_Pass;

            return credencial;
        }


        public bool IsAuthenticated(string userName, string password)
        {

            string _path;
            string _filterAttribute;

            Credential credencial = getCredential();

            DirectoryEntry objADAM = default(DirectoryEntry); // Binding object.

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            try
            {
                objADAM = new DirectoryEntry(ldapConn, userName, password);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = objADAM.NativeObject;

                DirectorySearcher search = new DirectorySearcher(objADAM);

                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];

                userName = _filterAttribute;
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        public string getFullUserName(string userName, string password)
        {
            string _path;
            string _filterAttribute;

            Credential credencial = getCredential();

            DirectoryEntry objADAM = default(DirectoryEntry); // Binding object.

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            try
            {
                objADAM = new DirectoryEntry(ldapConn, userName, password);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = objADAM.NativeObject;

                DirectorySearcher search = new DirectorySearcher(objADAM);

                search.Filter = "(SAMAccountName=" + userName + ")";
                //search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("displayName");
                SearchResult result = search.FindOne();

                if (null == result)
                    return userName;

                _path = result.Path;
                //_filterAttribute = (String)result.Properties["cn"][0];
                _filterAttribute = (String)result.Properties["displayName"][0];

                userName = _filterAttribute;
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return userName;
        }


        public string getAttributeUser(string userName, string attributeName)
        {

            Credential credencial = getCredential();
            DirectoryEntry objADAM = default(DirectoryEntry);

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            try
            {
                objADAM = new DirectoryEntry(ldapConn, credencial.DC_User, credencial.DC_Pass);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            try
            {
                Object obj = objADAM.NativeObject;
                DirectorySearcher search = new DirectorySearcher(objADAM);

                search.Filter = "(&(objectClass=user)(|(cn=" + userName + ")(sAMAccountName=" + userName + ")))";
                SearchResult result = search.FindOne();

                //New
                //DirectorySearcher busca = new DirectorySearcher(objADAM);
                ////objSearchADAM.Filter = "(&(objectClass=user)(sAMAccountName=Antorio.Toro))";
                //busca.Filter = "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))";
                //busca.SearchScope = SearchScope.Subtree;
                //SearchResultCollection res = busca.FindAll();

                //foreach (SearchResult objResult in res)
                //{
                //    DirectoryEntry objGroupEntry = objResult.GetDirectoryEntry();
                //    //result.Add(objGroupEntry.Name);
                //}

                //End New


                string valorPropiedad = string.Empty;

                try
                {
                    valorPropiedad = (String)result.Properties[attributeName][0];
                }
                catch (Exception e)
                {
                    Logger.Execute().Error(e);
                    throw;
                }

                //ArrayList array = new ArrayList();
                //foreach (var item in result.Properties.PropertyNames)
                //{
                //    array.Add(item);
                //}

                return valorPropiedad;
            }
            catch (Exception ex)
            {
                Logger.Execute().Error(ex);
                //throw new Exception("Error authenticating user. " + ex.Message);
                return string.Empty;
            }

        }


        public List<string> getUserMemberOf(string userName)
        {
            DirectoryEntry objADAM = default(DirectoryEntry);
            Credential credencial = getCredential();

            List<string> result = new List<string>();

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            try
            {
                objADAM = new DirectoryEntry(ldapConn, credencial.DC_User, credencial.DC_Pass);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            DirectorySearcher mySearcher = new DirectorySearcher(objADAM);
            mySearcher.Filter = "(&(objectClass=user)(|(cn=" + userName + ")(sAMAccountName=" + userName + ")))";
            //mySearcher.Filter = "(&(objectClass=user)(|(cn=almarza)(sAMAccountName=almarza)))";

            SearchResult resultado = mySearcher.FindOne();


            DirectoryEntry personEntry = resultado.GetDirectoryEntry();
            PropertyValueCollection groups = personEntry.Properties["memberOf"];

            foreach (string g in groups)
            {
                var str = g.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var grupo = string.Format("{0},{1}", str[0].Replace("CN=", ""), str[1].Replace("CN=", ""));

                result.Add(grupo);
            }

            //foreach (string GroupPath in resultado.Properties["memberOf"])
            //{
            //    var str = GroupPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //    result.Add(GroupPath);
            //}

            return result;

            //ArrayList array = new ArrayList();
            //foreach (var item in resultado.Properties.PropertyNames)
            //{
            //    array.Add(item);
            //}

            //foreach (string GroupPath in result2.Properties["memberOf"])
            //{
            //    if (GroupPath.Contains("Santo Domingo"))
            //    {
            //        te = true;
            //    }
            //}

        }

        public List<string> getGroups(Credential credencial)
        {

            DirectoryEntry objADAM = default(DirectoryEntry); // Binding object.
            DirectoryEntry objGroupEntry = default(DirectoryEntry); // Group Results. 
            DirectorySearcher objSearchADAM = default(DirectorySearcher); //Search object. 
            SearchResultCollection objSearchResults = default(SearchResultCollection); // Results collection.

            //SortedList adGroups = new SortedList();

            List<string> result = new List<string>(); // Binding path. 

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            // Get the AD LDS object. 
            try
            {
                objADAM = new DirectoryEntry(ldapConn, credencial.DC_User, credencial.DC_Pass);
                objADAM.RefreshCache();
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            // Get search object, specify filter and scope, 
            // perform search. 
            try
            {
                objSearchADAM = new DirectorySearcher(objADAM);
                //objSearchADAM.Filter = "(&(objectClass=user)(sAMAccountName=Antorio.Toro))";
                objSearchADAM.Filter = "(&(objectClass=group))";
                objSearchADAM.SearchScope = SearchScope.Subtree;
                objSearchResults = objSearchADAM.FindAll();

            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw e;
            }

            // Enumerate groups 
            try
            {
                if (objSearchResults.Count != 0)
                {
                    foreach (SearchResult objResult in objSearchResults)
                    {
                        objGroupEntry = objResult.GetDirectoryEntry();
                        result.Add(objGroupEntry.Name);
                    }
                }
                else
                {
                    throw new Exception("No hay grupos");
                }
            }
            catch (Exception e)
            {
                Logger.Execute().Error(e);
                throw new Exception(e.Message);
            }

            return result;
        }
        
        #region econtreras_20140702

        public IList<DTO.UserAD> getAllUsersFromAD()
        {
            string[] RetProps = new string[] { "SamAccountName", "name" };

            //List<string[]> users = new List<string[]>();
            //List<string> userList = new List<string>();
            IList<DTO.UserAD> users = new List<DTO.UserAD>();

            Credential credencial = getCredential();

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);
            string user = credencial.DC_User;
            string pass = credencial.DC_Pass; 
            foreach (SearchResult User in GetAllUsers(ldapConn, user, pass, RetProps))
            {
                DTO.UserAD dto = new DTO.UserAD();
                //DirectoryEntry DE = User.GetDirectoryEntry();
                try
                {

                    dto.AdID = User.Properties["SamAccountName"][0].ToString();

                    //var n = getRut(dto.AdID);

                    //if (User.Properties["displayName"][0] != null)
                    //{
                    //    dto.Nombre = User.Properties["displayName"][0].ToString();
                    //}
                    //else
                    //{
                    dto.Nombre = (User.Properties["name"][0] != null) ? User.Properties["name"][0].ToString() : "";

                    //}
                    //dto.Nombre = (DE.Properties["DisplayName"] != null) ? DE.Properties["name"].ToString() : DE.Properties["DisplayName"].ToString();
                    
                    //users.Add(new string[] { DE.Properties["SamAccountName"][0].ToString(), DE.Properties["DisplayName"][0].ToString() });
                    //userList.Add(DE.Properties["SamAccountName"][0].ToString());
                    
                }
                catch (Exception e)
                {
                    Logger.Execute().Error(e);
                    throw e;
                }
                users.Add(dto);
            }
            return users;
        }

        private string getRut(string userName = "almarza", string attributeName = "aldotest")
        {
       

            Credential credencial = getCredential();
            DirectoryEntry objADAM = default(DirectoryEntry);

            string ldapConn = string.Format("LDAP://{0}", credencial.DC_Server);

            objADAM = new DirectoryEntry(ldapConn, credencial.DC_User, credencial.DC_Pass);
            objADAM.RefreshCache();

            Object obj = objADAM.NativeObject;
            DirectorySearcher search = new DirectorySearcher(objADAM);

            //search.Filter = "(&(objectClass=user)(|(cn=" + userName + ")(sAMAccountName=" + userName + ")))";
            search.Filter = "(&(objectClass=User))";
            //SearchResult result = search.FindOne();
            SearchResultCollection RetObjects = search.FindAll();

            foreach (SearchResult User in RetObjects)
            {
                var d = User.Properties["SamAccountName"][0].ToString();

                string valorPropiedad = (String)User.Properties[attributeName][0];
            }

            //string valorPropiedad = (String)result.Properties[attributeName][0];

            return "";

        }

        internal static SearchResultCollection GetAllUsers(string ldapConn, string user, string pass, string[] Properties)
        {
             //DirectoryEntry DE = default(DirectoryEntry); // Binding object.
             
             DirectoryEntry DE = new DirectoryEntry(ldapConn, user, pass);

             string Filter = "(&(objectCategory=organizationalPerson)(objectClass=User))";
             DirectorySearcher DS = new DirectorySearcher(DE);
             DS.PageSize = 10000;
             DS.SizeLimit = 10000;
             DS.SearchScope = SearchScope.Subtree;
             DS.PropertiesToLoad.AddRange(Properties); 
             DS.Filter = Filter;
              
             try
             {
                 SearchResultCollection RetObjects = DS.FindAll();
                 return RetObjects;
             }
             catch (Exception e)
             {
                 Logger.Execute().Error(e);
                 throw e;                 
             }
             
        }


        #endregion

        public IList<UserAD> GetAllUsers(Domain.Infrastructure.ServerElement server)
        {
            bool IsVersion3 = WebConfigValues.IsVersion3;

            IList<UserAD> users = new List<UserAD>();

            IList<UserAD> usersFilter = new List<UserAD>();

            string[] Properties = new string[] { "sAMAccountName", "name", "cn", "mail" };

            Logger.Execute().Info($"start Get users: {server.Key}");

            using (DirectoryEntry DE = new DirectoryEntry(string.Format("LDAP://{0}/{1}", server.Hostname, server.Path), server._User, server._Pass))
            {
                DirectorySearcher DS = new DirectorySearcher(DE);
                DS.Filter = "(&(objectCategory=person)(objectClass=User))";
                DS.SizeLimit = 1000000;
                DS.PageSize = 1000000;

                DS.PropertiesToLoad.AddRange(Properties);

                SearchResultCollection resultado = DS.FindAll();

                foreach (SearchResult usuario in resultado)
                {
                    UserAD user = new UserAD();
                    user.Nombre = "";
                    user.Mail = "";

                    DirectoryEntry personEntry = usuario.GetDirectoryEntry();

                    try
                    {
                        user.ActiveDirectoryID = usuario.Properties["sAMAccountName"][0].ToString();
                    }
                    catch//(Exception ex)
                    {

                    }


                    try
                    {
                        if (personEntry.Properties["mail"].Count > 0)
                        {
                            user.Mail = personEntry.Properties["mail"][0].ToString();
                        }
                    }
                    catch// (Exception ex)
                    {

                    }

                    try
                    {
                        if (personEntry.Properties["cn"].Count > 0)
                        {
                            user.Nombre = personEntry.Properties["cn"][0].ToString();
                        }

                        if (string.IsNullOrEmpty(user.Nombre))
                        {
                            user.Nombre = personEntry.Properties["name"][0].ToString();
                        }
                    }
                    catch
                    {

                    }

                    if (IsVersion3)
                    {
                        try
                        {
                            PropertyValueCollection groups = personEntry.Properties["memberOf"];

                            foreach (string g in groups)
                            {
                                //var str = g.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                //foreach (var item in str)
                                //{
                                //    string _grupo = item.Replace("CN=", "").Replace("OU=", "");

                                //    user.Grupos.Add(_grupo);
                                //}
                            }
                        }
                        catch
                        {

                        }
                    }

                    if (!string.IsNullOrWhiteSpace(user.Nombre))
                    {
                        users.Add(user);
                    }
                }




                DS.Dispose();
                resultado.Dispose();
                DE.Close();

                Logger.Execute().Info($"SearchResult: {usersFilter.Count}");
            }

            return usersFilter;//.OrderBy(x => x.Nombre).ToList();
        }
    }
}
