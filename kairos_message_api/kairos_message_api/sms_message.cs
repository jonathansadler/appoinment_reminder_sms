using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Twilio;
using Twilio.Rest.Api.V2010.Account;

using Twilio.Clients;
using Twilio.Types;


namespace kairos_message_api
{
    public class sms_message {

        public void Send(int message_id, Guid message_unique_id, int message_job_type, DateTime time_for_delivery_utc, int client_id, string client_name, string message_body, string sms_phone_no) {

            // Some processing logic
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            
            //
            // Twilo API Key
            const string accountSid = "";
            //
            const string authToken = "";
            //

            TwilioClient.Init(accountSid, authToken);

            //PhoneNumber("+1619*******")

            var message = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(""),
                body: message_body,
                to: new PhoneNumber("+1" + sms_phone_no)
            );

            //Console.WriteLine(message.Sid);
            //message body
            //phone

        }
    }
}
