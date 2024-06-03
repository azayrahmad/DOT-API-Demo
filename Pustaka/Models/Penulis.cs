using System.Text.Json.Serialization;

namespace Pustaka.Models
{
    public class Penulis
    {
        public int PenulisId { get; set; }
        public string Nama { get; set; }
        public List<Buku> Buku { get; set; } = new List<Buku>();
    }
}
