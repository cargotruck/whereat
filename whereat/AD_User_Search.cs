//Resources:
//  https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.accountmanagement?view=dotnet-plat-ext-3.1

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Text;

namespace whereat
{
    class AD_User_Search
    {
        private string Username { get; set; }
        private string Domain { get; set; }
        public List<string> Results { get; private set; }

        public AD_User_Search()
        {
            Results = new List<string>();
        }

        public AD_User_Search(string _username,string _domain)
        {
            Results = new List<string>();
            Username = _username;
            Domain = _domain;
        }

        public bool search(ContextType context_type = ContextType.Domain)
        {
            if(string.IsNullOrEmpty(Username)) //can't search without search term
            {
                Console.WriteLine("Error: No user to search for. Try again and enter a username this time.");
                return false;
            }

            PrincipalContext principal_context = new PrincipalContext(context_type,Domain);

            if(!search_sam(principal_context))
            {
                search_username(principal_context);
            }

            if(Results.Count > 0)
                return true; //results have been found

            return false;
        }

        private bool search_sam(PrincipalContext pc)
        {
            UserPrincipal user_to_search = new UserPrincipal(pc);
            user_to_search.SamAccountName = Username;
            PrincipalSearcher searcher = new PrincipalSearcher(user_to_search);
            PrincipalSearchResult<Principal> results = searcher.FindAll();

            foreach(UserPrincipal p in results)
                Results.Add(p.SamAccountName);

            if(Results.Count > 0) return true; //results have been found

            return false;
        }

        private bool search_username(PrincipalContext pc)
        {
            UserPrincipal user_to_search = new UserPrincipal(pc);
            user_to_search.Name = Username;
            PrincipalSearcher searcher = new PrincipalSearcher(user_to_search);
            PrincipalSearchResult<Principal> results = searcher.FindAll();

            foreach(UserPrincipal p in results)
                Results.Add(p.SamAccountName);

            if(Results.Count > 0) return true; //results have been found

            return false;
        }
    }
}
