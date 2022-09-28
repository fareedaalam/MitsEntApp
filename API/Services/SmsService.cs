using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace API.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;
        public SmsService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        public async Task<string> SendSms(String mobile, string otp)
        {
            string url = "http://api.bulksmsgateway.in/sendmessage.php?";
            string result = "";
            string Message = "Dear User OTP Number is " + otp + ", regards Test Mits Entertainment";

            StringBuilder sb = new StringBuilder();
            sb.Append("user=" + _smsSettings.UserDetails);
            sb.Append("&password=" + _smsSettings.Password);
            sb.Append("&mobile=" + mobile);
            sb.Append("&message=" + Message);
            sb.Append("&sender=" + _smsSettings.Sender);
            sb.Append("&type=" + _smsSettings.SmsType);
            sb.Append("&template_id=" + _smsSettings.Template_id);

            //String strPost = "user=mitsentertainment&password=alri&mobile=7017773534&message=Dear User OTP Number is abcd 1234, regards Test Mits Entertainment&sender=MITSMJ&type=3&template_id=1507166418871434342";
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url + sb.ToString());
            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(sb.ToString());
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(sb.ToString());
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = await sr.ReadToEndAsync();

            }
            return result;

        }
    }
}