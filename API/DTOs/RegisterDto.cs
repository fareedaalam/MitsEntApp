using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] public string UserName { get; set; }
        [Required] public string knownAs { get; set; }
        [Required] public string Gender { get; set; }
         public DateTime DateOfBirth { get; set; }
        //[Required] public string City { get; set; }
        //[Required] public string Country { get; set; }
        [Required] public string PhoneNumber { get; set; }
        //[Required] public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 4)]
        public string Password { get; set; }

        public string Otp {get;set;}


    }
}