using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using A3SimpleModManagerCommon.Models;

namespace A3SimpleModManagerCommon
{
    public class Utils
    {
        public static string GetMd5(FileInfo file)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(file.FullName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
        }

        public static string ExtractModInfo(FileInfo modcpp)
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

        public static void WalkDirectory(DirectoryInfo dir, Mod mod)
        {
            Console.WriteLine("Found directory " + dir.Name + " belonging to " + mod.Name);
            foreach (var file in dir.EnumerateFiles())
            {
                var relativePathExtract = file.DirectoryName.Split(
                    new[] { mod.FolderName + Path.DirectorySeparatorChar }, StringSplitOptions.None);
                var relpath = "";
                if (relativePathExtract.Length > 1)
                {
                    relpath = relativePathExtract.Last();
                }
                var modfile = new ModFile
                {
                    Filename = file.Name,
                    RelativePath = relpath,
                    Hash = GetMd5(file)
                };
                mod.Files.Add(modfile);
            }
            foreach (var subdir in dir.EnumerateDirectories())
            {
                // wew recursion
                WalkDirectory(subdir, mod);
            }
        }
    }
}
