using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersMicroservice.Models
{
    public class Users
    {
        [Required]// GENERUJE BLAD!!!
        [Key]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string HashPassword { get; set; }

        [Required]
        public string UserAccountStatus { get; set; }

        public DateTime? AccountStatusChangeDate {get; set;}
        
        public string Salt { get; set; }

        public string AuthToken { get; set; }

        public DateTime? AuthTokenExpiration { get; set; }

        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [ForeignKey("PermissionId")]
        public int PermissionId { get; set; }
    }
}
