using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Api.V2010.Account.Usage.Record;
using Twilio.Rest.Intelligence.V2.Transcript;
using Twilio.TwiML.Fax;


namespace exercise.main
{
    public class SMS : Isms
    {
        private static async Task SendSMS(string bodytext)
        {
            // Find your Account SID and Auth Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = Secrets.SID;
            string authToken = Secrets.AUTHTOKEN;

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
            body: bodytext,
            from: new Twilio.Types.PhoneNumber(Secrets.TWILIOPHONE),
            to: new Twilio.Types.PhoneNumber(Secrets.MYPHONE));

            Console.WriteLine(message.Body);
        }
        public async Task<string> Send(Basket basket)
        {
            string bodytext = basket.Receipt();

            await SMS.SendSMS(bodytext);

            return bodytext;
        }
    }
}
