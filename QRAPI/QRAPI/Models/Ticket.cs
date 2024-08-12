using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QRAPI.Models
{
	public class Ticket
	{
		public int TicketId { get; set; }

		[Required]
		[StringLength(200)]
		public string Title { get; set; } = "";

		public string? Description { get; set; }

        public decimal Price { get; set; }

		[StringLength(20, MinimumLength = 3)]
        [Column(TypeName = "varchar(20)")]
        public string LocationPlace { get; set; } = "";


        public short CategoryID { get; set; }

		[StringLength(100)]
		public string Block { get; set; } = "";

		public int? RowNumber { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(LocationPlace))]
        public Location? Location { get; set; }


    }
}

