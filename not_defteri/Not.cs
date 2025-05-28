using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace not_defteri
{
    internal class Not
    {
        public string Baslik { get; set; }
        public string Kategori { get; set; }
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }

        public override string ToString()
        {
            return $"{Baslik} ({Kategori})";
        }
    }
}
