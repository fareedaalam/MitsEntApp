using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class OtpController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OtpController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("sendotp/{mobile}")]
        public async Task<ActionResult> SendSaveOtp(string mobile)
        {
            if (!String.IsNullOrEmpty(mobile))
            {
                string result = await _unitOfWork.UserRepository.GetUserByMobile(mobile);
                //Check mobile exis

                if (!String.IsNullOrEmpty(result)) return BadRequest("Mobile already registered");

                var data = new Otps
                {
                    Mobile = mobile,
                    OTP = mobile.GenrateOTP()
                };

                var mobileExists = await _unitOfWork.OtpRepository.GetOtpAsync(mobile);
                
                if (mobileExists != null) return Ok(new OtpsDto{mobile=mobile});

                _unitOfWork.OtpRepository.AddOtp(data);

                if (await _unitOfWork.Complete()) return Ok(new OtpsDto{mobile=mobile});
            }


            return BadRequest("Faild to send OTP");

        }

        [HttpGet("{mobile}")]
        public async Task<ActionResult> GetOtp(string mobile)
        {
            var otp = await _unitOfWork.OtpRepository.GetOtp(mobile);
            if (otp == null) return NotFound();
            return Ok(otp);

        }

        [HttpPost("verifyotp")]
        public async Task<ActionResult> VarifyOtp(OtpsDto otp)
        {
            var result = await _unitOfWork.OtpRepository.VerifyOtp(otp);
            if (!result) return BadRequest("OTP Not Valid");
            return Ok(otp);

        }


        [HttpDelete("delete-otp/{mobile}")]
        public async Task<ActionResult> DeleteOtp(string mobile)
        {
            var otp = await _unitOfWork.OtpRepository.GetOtp(mobile);
            if (otp != null) _unitOfWork.OtpRepository.DeleteOtp(otp);
            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Somethig wrong");
        }


    }
}