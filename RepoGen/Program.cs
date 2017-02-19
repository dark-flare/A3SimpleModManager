using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;


namespace RepoGen
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var cla = new CommandLineApplication
            {
                Name = "RepoGenerator",
                FullName = "A3 Simple Mod Manager Repository Generator",
                Description = "Command line utility to generate ArmA3 Simple Mod Manager compatible repositories"
            };
            cla.HelpOption("-h|--help");
            var dirOption = cla.Option("-d|--dir", "The folder which you wish to build the repository from", CommandOptionType.SingleValue);
            var nameOption = cla.Option("-n|--name", "The repository name, optional", CommandOptionType.SingleValue);

            cla.OnExecute(() =>
            {
                var dir = dirOption.Value();
                var name = nameOption.Value() ?? "";
                if (dir == null)
                {
                    cla.ShowHelp();
                    
                    return 0;
                }

                return 1;
            });
            try
            {
                return cla.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error launching repo generator");
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }
}
