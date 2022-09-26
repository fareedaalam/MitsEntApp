using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Otps
    {

        public int Id { get; set; }
        public string OTP { get; set; }
      //  public AppUser User { get; set; }
        public string Mobile { get; set; }

    }

    public class OtpsDto{
        public string otp { get; set; }
       
        public string mobile { get; set; }

    }
}