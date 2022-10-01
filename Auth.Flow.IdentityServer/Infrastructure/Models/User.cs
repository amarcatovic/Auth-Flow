using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auth.Flow.IdentityServer.Infrastructure.Models
{
    public class User : IdentityUser<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        public bool IsOwner { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string EmailAddress { get; set; }

        [StringLength(250)]
        public string PhoneNumber { get; set; }

        [StringLength(250)]
        public string MobileNumber { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? ChangeDate { get; set; }


        public User()
        {
        }
    }
}
