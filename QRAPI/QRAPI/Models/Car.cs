using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QRAPI.Models
{
    public class Car
    {
        public int Id { get; set; }

        public string Model { get; set; } = "";

        public string Brand { get; set; } = "";

        public decimal Price { get; set; }

        public short CategoryID { get; set; }

        [StringLength(500)]
        public string? CoverImageUrl { get; set; } // Resim URL'si veya yolu

        [JsonIgnore]
        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }
    }
}
