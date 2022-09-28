using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ForgotPwdDto
    {
        public string username { get; set; }
        public string mobile { get; set; }
        public string otp { get; set; }

    }
}