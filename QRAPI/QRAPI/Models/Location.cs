using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAPI.Models
{
    public class Location
	{
        [Key]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Column(TypeName = "varchar(20)")]
        public string Place { get; set; } = "";

        public List<Ticket>? Tickets { get; set; }

	}
}

