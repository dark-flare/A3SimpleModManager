using System.Collections.Generic;

namespace A3SimpleModManagerCommon.Models
{
    public class Repository
    {
        public string Name { get; set; }
        public ICollection<Mod> Mods { get; set; }
    }
}
