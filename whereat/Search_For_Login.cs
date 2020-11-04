using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace whereat
{
    class User_List
    {
        public string Computer_name { get; private set; }
        public List<string> Users { get; set; }

        public User_List(string _computer_name)
        {
            Computer_name = _computer_name;
            Users = new List<string>();
        }
    }

    class Search_For_Login
    {
        public List<string> Usernames { get; private set; }
        public List<string> Computers { get; private set; }
        public List<User_List> Results { get; set; }
        private bool Found { get; set; }

        private void print()
        {
            foreach(User_List ul in Results)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("{0}:",ul.Computer_name);

                foreach(string user in ul.Users)
                {
                    Console.WriteLine("\t{0}",user);
                }
            }
        }

        private void print(User_List ul)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine("{0}:",ul.Computer_name);

            foreach(string user in ul.Users)
            {
                Console.WriteLine("\t{0}",user);
            }
        }

        private void query_computer(string computer)
        {
            User_List user_list = new User_List(computer);
            ConnectionOptions connection_options = new ConnectionOptions();
            connection_options.EnablePrivileges = true;
            connection_options.Authentication = AuthenticationLevel.PacketPrivacy;
            connection_options.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope scope = new ManagementScope(string.Format("\\\\{0}\\root\\CIMV2",computer),connection_options);

            try
            {
                scope.Connect();

                ObjectQuery query = new ObjectQuery("SELECT Username FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope,query);
                var snag_login_id = new Regex(@"(?<=Name="").*(?="")");

                foreach(ManagementObject wmi_object in searcher.Get())
                {
                    foreach(string user in Usernames)
                    {
                        if(user.Equals(wmi_object["UserName"].ToString().Split('\\')[1],StringComparison.OrdinalIgnoreCase))
                        {
                            Found = true;
                            user_list.Users.Add(user);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                //Console.WriteLine("Problem connecting to {0}. Error Message:\n\n\t{1}\n\n",computer,e.Message);
            }

            if(user_list.Users.Count > 0) print(user_list);
        }

        public bool search()
        {
            List<Thread> threads = new List<Thread>();
            foreach(string computer in Computers)
            {
                Thread t = new Thread(()=> query_computer(computer));
                threads.Add(t);
                t.Start();
            }

            for(int i = 0;i < threads.Count;i++)
            {
                threads[i].Join(500);
            }
            
            if(Found)
            {
                return true;
            }
            else
            {
                Console.WriteLine("The user(s) you're looking for doesn't appear to be logged in to a computer. Maybe they're off today?");
            }
            
            return false;
        }

        public Search_For_Login(List<string> _usernames,List<string> _computers)
        {
            Found = false;
            Usernames = _usernames;
            Computers = _computers;
            Results = new List<User_List>();
        }
    }
}
