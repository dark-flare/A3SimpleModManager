using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A3SimpleModManagerCommon.Models
{
    public class Repository
    {
        public string Name { get; set; }
        public ICollection<Mod> Mods { get; set; }
    }
}
