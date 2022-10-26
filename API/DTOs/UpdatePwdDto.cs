using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdatePwdDto
    {
        [Required] public string username { get; set; }
         [Required] public string token { get; set; }
        [Required] public string password { get; set; }
    }
}