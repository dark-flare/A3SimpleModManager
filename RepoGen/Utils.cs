using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RepoGen
{
    public class Utils
    {
        public int GenerateRepo(string repodir, string reponame)
        {
            if (Directory.Exists(repodir))
            {
                var di = new DirectoryInfo(repodir);
                Console.WriteLine("Beginning repo generation in " + repodir);
                foreach (var dir in di.EnumerateDirectories())
                {
                    Console.WriteLine("Found directory " + dir.Name);
                }
            }
            else
            {
                Console.WriteLine("Directory not found");
                return 0;
            }

            return 1;
        }
    }
}
