using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOtpRepository
    {
       // string SendOTP(SmsSettings smsSettings);
        void AddOtp(Otps otps);
        void DeleteOtp(Otps otps);
        Task<Otps> GetOtp(string mobile);
        Task<OtpDto> GetOtpAsync(string mobile);
        Task<bool> VerifyOtp(OtpDto otp);
        void Update(Otps user);
    }
}