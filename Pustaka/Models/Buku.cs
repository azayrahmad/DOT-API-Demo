using System.Text.Json.Serialization;

namespace Pustaka.Models
{
    public class Buku
    {
        public int BukuId { get; set; }
        public string Judul { get; set; }
        public int? PenulisId { get; set; }
        [JsonIgnore]
        public Penulis? Penulis { get; set; }
    }
}
