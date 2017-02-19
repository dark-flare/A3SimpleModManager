using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A3SimpleModManagerCommon.Models
{
    public class File
    {
        public string Filename { get; set; }
        public string RelativePath { get; set; }
        public string Hash { get; set; }
    }
}
