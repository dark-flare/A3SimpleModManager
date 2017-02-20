using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using A3SimpleModManagerCommon.Models;
using Newtonsoft.Json;

namespace RepoGen
{
    public class Utils
    {
        public static int GenerateRepo(string repodir, string reponame)
        {
            if (Directory.Exists(repodir))
            {
                var repo = new Repository {Name = reponame};
                var di = new DirectoryInfo(repodir);
                Console.WriteLine("Beginning repo generation in " + repodir);
                foreach (var dir in di.EnumerateDirectories())
                {
                    Console.WriteLine("Found directory " + dir.Name);
                    var modcpp =
                        dir.EnumerateFiles()
                            .FirstOrDefault(x => string.Equals(x.Name, "mod.cpp", StringComparison.OrdinalIgnoreCase));
                    if (modcpp != null)
                    {
                        Console.WriteLine("mod.cpp found - this is a mod folder.");
                        var mod = new Mod
                        {
                            Name = ExtractModInfo(modcpp),
                            FolderName = dir.Name
                        };
                        repo.Mods.Add(mod);

                    }
                    else
                    {
                        Console.WriteLine("This doesn't appear to be a mod folder.");
                    }
                }
                Console.WriteLine(JsonConvert.SerializeObject(repo));
            }
            else
            {
                Console.WriteLine("Directory not found");
                return 0;
            }

            return 1;
        }

        private static string ExtractModInfo(FileInfo modcpp)
        {
            var lines = System.IO.File.ReadAllLines(modcpp.FullName);
            var modName = lines.FirstOrDefault(x => x.StartsWith("name", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(modName))
            {
                var result = from Match match in Regex.Matches(modName, "\"([^\"]*)\"")
                    select match.ToString();
                return result.First();
            }
            else
            {
                return modcpp.DirectoryName;
            }
        }
    }
}
