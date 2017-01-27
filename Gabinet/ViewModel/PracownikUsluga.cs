using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabinet.ViewModel
{
    class PracownikUsluga
    {
        public int uzytkownik_sluga_id { get; set; }
        public int uzytkownik_id { get; set; }
        public int usluga_id { get; set; }
        public string nazwa { get; set; }
        public decimal? cena { get; set; }
        public TimeSpan? czas { get; set; }
        public string opis { get; set; }
    }
}
