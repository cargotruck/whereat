//Resources:
//  https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.accountmanagement?view=dotnet-plat-ext-3.1

using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;

namespace whereat
{
    class AD_Computer_Search
    {
        private string Computer_name { get; set; }
        private string Domain { get; set; }
        public List<string> Results { get; private set; }
        
        public AD_Computer_Search()
        {
            Results = new List<string>();
            Computer_name = "*";
        }

        public AD_Computer_Search(string _computer_name,string _domain)
        {
            Results = new List<string>();
            Computer_name = _computer_name;
            Domain = _domain;
        }

        private void is_online(string host)
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            try
            {
                PingReply reply = ping.Send(host,timeout,buffer,options);

                if(reply.Status == IPStatus.Success)
                {
                    Results.Add(host);
                    return;
                }
            }
            catch
            {

            }
        }

        public bool search(ContextType context_type = ContextType.Domain)
        {
            if(string.IsNullOrEmpty(Computer_name)) //can't search without search term
            {
                Console.WriteLine("Error: No computer to search for. Try again and enter a computer name this time.");
                return false;
            }

            PrincipalContext principal_context = new PrincipalContext(context_type,Domain);
            ComputerPrincipal computer_to_search = new ComputerPrincipal(principal_context);
            computer_to_search.Name = Computer_name;
            PrincipalSearcher searcher = new PrincipalSearcher(computer_to_search);
            PrincipalSearchResult<Principal> results = searcher.FindAll();

            List<Thread> threads = new List<Thread>();
            foreach(ComputerPrincipal p in results)
            {
                Thread t = new Thread(()=> is_online(p.SamAccountName.ToString().TrimEnd('$')));
                threads.Add(t);
                t.Start();
            }

            for(int i = 0;i < threads.Count;i++)
            {
                threads[i].Join(500);
            }

            if(Results.Count > 0) //results have been found
            {
                return true;
            }
            else
            {
                Console.WriteLine("Error: Unable to find computers in this domain.");
                Console.WriteLine("T-800 says: \"Terminating.\"");
                return false;
            }
        }
    }
}
