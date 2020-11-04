using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Mono.Options;


namespace whereat
{
    class Options
    {
        public string Username { get; private set; }
        public string Computer_name { get; private set; }
        public string Domain { get; private set; }

        public Options(string[] args)
        {
            bool show_help = false;
            Username = "";
            Computer_name = "*";
            Domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

            var options = new OptionSet()
                {
                    "Usage: whereat [OPTIONS]+",
                    "Searches for a given user, or user with name like one provided, on network computers.",
                    "",
                    "Options:",
                    {
                        "u|user=","Takes a username or a search request such as one of the following:"
                        + "\nJohn*"
                        + "\nJone Doe"
                        + "\n*John Doe*",
                        v => Username = v
                    },
                    {
                        "c|computer=","Takes a computer or a search request such as: SOO-D-*",
                        v => Computer_name = v
                    },
                    {
                        "d|domain=","Takes a domain, such as mydomain.com",
                        v => Domain = v
                    },
                    {
                        "h|help", "show this message and exit",
                        v => show_help = v != null
                    },
                };

            List<string> extra;
            try
            {
                extra = options.Parse(args);
            }
            catch(OptionException e)
            {
                Console.WriteLine("Try `whereat --help' for more information.");
            }

            if(args.Length == 1) Username = args[0];

            if(show_help || args.Length == 0)
            {
                options.WriteOptionDescriptions(Console.Out);
                return;
            }
        }
    }
}
