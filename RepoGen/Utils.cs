using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using A3SimpleModManagerCommon.Models;
using Newtonsoft.Json;
using ModFile = A3SimpleModManagerCommon.Models.ModFile;


namespace RepoGen
{
    public class Utils
    {
        private static Repository _repo;
        public static int GenerateRepo(string repodir, string reponame)
        {
            if (Directory.Exists(repodir))
            {
                _repo = new Repository {Name = reponame, Mods = new List<Mod>()};
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
                            FolderName = dir.Name,
                            Files = new List<ModFile>()
                        };
                        WalkDirectory(dir, mod);
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
                catch(Exception e)
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

            return 1;
        }

        public static void WalkDirectory(DirectoryInfo dir, Mod mod)
        {
            Console.WriteLine("Found directory " + dir.Name + " belonging to " + mod.Name);
            foreach (var file in dir.EnumerateFiles())
            {
                var relativePathExtract = file.DirectoryName.Split(
                    new[] {mod.FolderName + Path.DirectorySeparatorChar}, StringSplitOptions.None);
                var relpath = "";
                if (relativePathExtract.Length > 1)
                {
                    relpath = relativePathExtract.Last();
                }
                var modfile = new ModFile
                {
                    Filename = file.Name,
                    RelativePath = relpath,
                    Hash = A3SimpleModManagerCommon.Utils.GetMd5(file)
                };
                mod.Files.Add(modfile);
            }
            foreach (var subdir in dir.EnumerateDirectories())
            {
                // wew recursion
                WalkDirectory(subdir, mod);
            }
        }

        private static string ExtractModInfo(FileInfo modcpp)
        {
            var lines = File.ReadAllLines(modcpp.FullName);
            var modName = lines.FirstOrDefault(x => x.StartsWith("name", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(modName))
            {
                var result = lines[0].Split('"')[1];
                return result;
            }
            else
            {
                return modcpp.Directory.Name;
            }
        }
    }
}
