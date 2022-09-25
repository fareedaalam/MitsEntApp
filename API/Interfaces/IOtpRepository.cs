using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOtpRepository
    {
        void AddOtp(Otps otps);
        void DeleteOtp(Otps otps);
        Task<Otps> GetOtp(string mobile);
        Task<OtpsDto> GetOtpAsync(string mobile);
        Task<bool> VerifyOtp(OtpsDto otp);

        void Update(Otps user);
    }
}