using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersMicroservice.Models
{
    public class Users
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //[Required]
        [Key]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string Salt { get; set; }

        public string AuthToken { get; set; }

        public DateTime? AuthTokenExpiration { get; set; }

        [ForeignKey("PermissionId")]
        public int PermissionId { get; set; }
    }
}
