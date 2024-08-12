using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRAPI.Models
{
    namespace LibraryAPI.Models
    {
        public class ApplicationUser : IdentityUser
        {
            public long IdNumber { get; set; }
            public string Name { get; set; } = "";
            public string? MiddleName { get; set; }
            public string? FamilyName { get; set; }
            public string Address { get; set; } = "";
            public bool Gender { get; set; }
            public DateTime BirthDate { get; set; }
            public DateTime RegisterDate { get; set; }
            public byte Status { get; set; }
            [NotMapped]
            public string? Password { get; set; }

            [NotMapped]  //Veri tabanına kaydedilmicektir.
            [Compare(nameof(Password))]
            public string? ConfirmPassword { get; set; }

            public bool IsActive { get; set; } = true; // Varsayılan olarak aktif


        }

        public class Person
        {
            [Key]
            public string Id { get; set; } = "";

            [StringLength(500)]
            public string? CoverImageUrl { get; set; } // Resim URL'si veya yolu

            [ForeignKey(nameof(Id))]
            public ApplicationUser? ApplicationUser { get; set; }

        }


    }

}

