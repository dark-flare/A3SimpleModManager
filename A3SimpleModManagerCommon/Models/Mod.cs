using System.Collections.Generic;

namespace A3SimpleModManagerCommon.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public string FolderName { get; set; }
        
        public ICollection<ModFile> Files { get; set; }
    }
}
