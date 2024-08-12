using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QRAPI.Models
{
    public class Category
    {
        public short Id { get; set; }

        [Required]
        [StringLength(800)]
        [Column(TypeName = "varchar(800)")]
        public string Name { get; set; } = "";

        public List<Food>? Foods { get; set; }
        public List<Car>? Cars { get; set; }

        public List<Ticket>? Tickets { get; set; }

    }
}
