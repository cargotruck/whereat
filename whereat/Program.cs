/*
 * whereat.exe written by Nicholas Flesch
 * nicholas.flesch@outlook.com
 * last modified: 24 September 2020
 * Searches for the workstation a user is logged into.
 * Contacts Active Directory server and gathers a list of users matching search.
 * Also gathers a list of computers in Active Directory.
 * Uses RPC and WMI to find if a user or list of users are logged in to the console
 * of any computer in the domain.
 * Writes results to console.
 * requires .NET Core 3.1.
*/

using System;

namespace whereat
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options(args);
         
            if(args.Length == 0) return;
            
            AD_User_Search ad_user_search = new AD_User_Search(options.Username,options.Domain);

            if(ad_user_search.search())
            {
                Console.WriteLine("Searching...");
                AD_Computer_Search ad_computer_search = new AD_Computer_Search(options.Computer_name,options.Domain);
                if(ad_computer_search.search())
                {
                    Search_For_Login login_search = new Search_For_Login(ad_user_search.Results,ad_computer_search.Results);
                   
                    try
                    {
                        login_search.search();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("ERROR! Something's amiss!");
                        Console.WriteLine(e.Message);
                    }

                }
            }
            else
            {
                Console.WriteLine("\"{0}\" not found. Modify search term and try again.",options.Username);
            }
        }
    }
}
