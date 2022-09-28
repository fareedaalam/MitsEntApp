using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class SmsSettings
    {
        public string UserDetails { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string Template_id { get; set; }
        public string SmsType { get; set; }

    }
}