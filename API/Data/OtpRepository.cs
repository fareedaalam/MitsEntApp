using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

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
        }

        public async Task<OtpsDto> GetOtpAsync(string mobile)
        {
            return await _context.Otps
                .Where(x => x.Mobile == mobile)
                .ProjectTo<OtpsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public void Update(Otps user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<bool> VerifyOtp(OtpsDto otp)
        {
            return await _context.Otps.AnyAsync(x => x.Mobile == otp.mobile && x.OTP == otp.otp);
        }


    }
}