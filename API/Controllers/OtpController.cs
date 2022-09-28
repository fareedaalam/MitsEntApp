using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    public class OtpController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISmsService _smsService;

        public OtpController(IUnitOfWork unitOfWork, IMapper mapper, ISmsService smsService)
        {
            _smsService = smsService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        //depricated 
        [HttpPost("savesendotp/{mobile}")]
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

                // if (mobileExists != null) return Ok(new OtpDto { mobile = mobile });

                // var mobileExists = await _unitOfWork.OtpRepository.GetOtpAsync(mobile);

                //send sms...
                var sms = await _smsService.SendSms(data.Mobile, data.OTP);

                if (mobileExists == null)
                {   //add OTP into DB
                    _unitOfWork.OtpRepository.AddOtp(data);
                    await _unitOfWork.Complete();

                }
                else
                { //update db
                    _unitOfWork.OtpRepository.Update(data);
                }
                return Ok(mobile);
            }

            return BadRequest("Faild to send OTP");

        }

        [HttpPost("sendotp/{mobile}")]
        public async Task<ActionResult> SendOtp(string mobile)
        {
            if (!String.IsNullOrEmpty(mobile))
            {
                var data = new Otps
                {
                    Mobile = mobile,
                    OTP = mobile.GenrateOTP()
                };


                var mobileExists = await _unitOfWork.OtpRepository.GetOtpAsync(mobile);

                //Database operations...
                if (mobileExists == null)
                {   //Insert
                    _unitOfWork.OtpRepository.AddOtp(data);
                    await _unitOfWork.Complete();

                }
                else
                { //Update
                    _unitOfWork.OtpRepository.Update(data);
                }
                //send sms...
                var sms = await _smsService.SendSms(data.Mobile, data.OTP);
                return Ok(new OtpDto { phonenumber = mobile });

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
        public async Task<ActionResult> VerifyOtp(OtpDto otp)
        {
            var result = await _unitOfWork.OtpRepository.VerifyOtp(otp);
            if (!result) return BadRequest("OTP Not Valid");
            var delOtp = new Otps
            {
                OTP = otp.otp,
                Mobile = otp.phonenumber
            };

            _unitOfWork.OtpRepository.DeleteOtp(delOtp);

            if (await _unitOfWork.Complete()) return Ok(otp);

            return BadRequest("Something wrong to wipe OTP");

        }


        [HttpDelete("delete-otp/{mobile}")]
        public async Task<ActionResult> DeleteOtp(string mobile)
        {
            var otp = await _unitOfWork.OtpRepository.GetOtp(mobile);
            if (otp != null) _unitOfWork.OtpRepository.DeleteOtp(otp);
            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Somethig wrong");
        }

        [HttpPost("forgot-pwd")]
        public async Task<ActionResult> ForgotPassword(ForgotPwdDto forgotPwdDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(forgotPwdDto.username.ToLower());
            if (user == null)
            {
                return BadRequest("User not valid");
            }
            else
            {
                var mobile = await _unitOfWork.UserRepository.GetUserMobile(user.UserName);
                if (mobile == null) return BadRequest("Mobile number not registered");
            }

            var otp = new OtpDto
            {
                phonenumber = forgotPwdDto.mobile,
                otp = forgotPwdDto.otp
            };

            var result = await _unitOfWork.OtpRepository.VerifyOtp(otp);
            if (!result) return BadRequest("Not Valid");

            return Ok(forgotPwdDto);

        }


    }
}