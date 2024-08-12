using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QRAPI.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int MenuId { get; set; }

        public short CategoryID { get; set; }

        [StringLength(500)]
        public string? CoverImageUrl { get; set; } // Resim URL'si veya yolu

        [JsonIgnore]
        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }
        public Menu? Menu { get; set; }
    }
}
