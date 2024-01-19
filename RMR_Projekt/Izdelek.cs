using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt
{
    public class Izdelek
    {
       public string src_slike { get; set; }
       public string ime { get; set; }

        public Izdelek (string slika,string ime)
        {
            this.src_slike = slika;
            this.ime = ime;
        }

    }

}
