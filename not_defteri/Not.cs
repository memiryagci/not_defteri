using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace not_defteri
{
    public interface IAranabilir
    {
        bool IceriyorMu(string aramaMetni);
    }

    public class Not : IAranabilir
    {
        public string Baslik { get; set; }
        public string Kategori { get; set; }
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }

        public bool IceriyorMu(string aramaMetni)
        {
            aramaMetni = aramaMetni.ToLower();
            return Baslik.ToLower().Contains(aramaMetni) ||
                   Icerik.ToLower().Contains(aramaMetni) ||
                   Kategori.ToLower().Contains(aramaMetni);
        }

        public override string ToString()
        {
            return $"{Baslik} - {Kategori} ({Tarih:dd.MM.yyyy HH:mm})";
        }
    }
}
