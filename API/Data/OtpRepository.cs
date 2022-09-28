using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;

namespace API.Data
{
    public class OtpRepository : IOtpRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public OtpRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public void AddOtp(Otps otps)
        {
            _context.Otps.Add(otps);
        }

        public void DeleteOtp(Otps otps)
        {
            _context.Otps.Remove(otps);
        }

        public async Task<Otps> GetOtp(string mobile)
         {
            return await _context.Otps
            .Where(x => x.Mobile == mobile)
            .FirstOrDefaultAsync();
            //return await _context.Otps.FindAsync(mobile);
        }

        public async Task<OtpDto> GetOtpAsync(string mobile)
        {
            return await _context.Otps
                .Where(x => x.Mobile == mobile)
                .ProjectTo<OtpDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }



        public void Update(Otps user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<bool> VerifyOtp(OtpDto otp)
        {
            return await _context.Otps.AnyAsync(x => x.Mobile == otp.phonenumber && x.OTP == otp.otp);
        }

              
        

       
    }
}