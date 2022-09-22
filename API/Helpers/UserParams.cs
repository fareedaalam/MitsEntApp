using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class UserParams : PaginationsParams
    {
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
        public int MinAge { get; set; } = 8;
        public int MaxAge { get; set; } = 150;
        public int RegistrationCount { get; set; } = 50;
        //srting 
        public string OrderBy { get; set; } = "lastActive";

        public string? KnownAs { get; set; } = "contestant";

        public bool? IsActive { get; set; } = true;
    }
}