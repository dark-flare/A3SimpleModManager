using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using A3SimpleModManagerCommon.Models;
using Newtonsoft.Json;

namespace A3SimpleModManagerCommon
{
    public class Generate
    {
        private static Repository _repo;
        public static int GenerateRepo(string repodir, string reponame)
        {
            if (Directory.Exists(repodir))
            {
                _repo = new Repository { Name = reponame, Mods = new List<Mod>() };
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
                            Name = Utils.ExtractModInfo(modcpp),
                            FolderName = dir.Name,
                            Files = new List<ModFile>()
                        };
                        Utils.WalkDirectory(dir, mod);
                        _repo.Mods.Add(mod);

                    }
                    else
                    {
                        Console.WriteLine("This doesn't appear to be a mod folder.");
                    }
                }
                try
                {
                    Console.WriteLine("Writing repo file in " + repodir);
                    File.WriteAllText(Path.Combine(repodir, "repo.json"), JsonConvert.SerializeObject(_repo));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing json!");
                    Console.WriteLine(e.Message);
                    return 0;
                }
                //Console.WriteLine(JsonConvert.SerializeObject(_repo));
                return 1;
            }
            else
            {
                Console.WriteLine("Directory not found");
                return 0;
            }
        }
    }
}
