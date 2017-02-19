using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A3SimpleModManagerCommon.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public string FolderName { get; set; }
        
        public ICollection<File> Files { get; set; }
    }
}
