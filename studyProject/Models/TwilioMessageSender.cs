using System.Configuration;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace studyProject.Models
{
    public interface ITwilioMessageSender
    {
        Task SendMessageAsync(string to, string from, string body);
    }
    public class TwilioMessageSender : ITwilioMessageSender
    {
        public TwilioMessageSender()
        {
            var accountSid = ConfigurationManager.AppSettings["twilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["twilioAuthToken"];

            TwilioClient.Init(accountSid, authToken);
        }

        public async Task SendMessageAsync(string to, string from, string body)
        {
            await MessageResource.CreateAsync(new PhoneNumber(to),
                                              from: new PhoneNumber(from),
                                              body: body);
        }
    }
}