using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ISmsService
    {
        Task<string> SendSms(String mobile, string otp);
    }
}