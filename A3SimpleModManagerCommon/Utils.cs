using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
    }
}
