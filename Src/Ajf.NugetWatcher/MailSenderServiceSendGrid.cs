using System.Net;
using System.Threading.Tasks;
using JCI.ITC.COMP2.Common.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ajf.NugetWatcher
{
    /// <summary>
    ///     This is implemented using SendGrid free email service.
    ///     See https://sendgrid.com/
    ///     At the time of writing, the following account was used: jcianders / 21Bananer / anders.juul-ext@jci.com
    ///     Log in to create new APIKeys in case of problems. Also, should we switch to another (paid?) account,
    ///     you can log into that and create APIKey.
    ///     The free account is limited to sending 100 emails each day.
    ///     We might need an account for each environment?
    ///     Notice, that the input parameters er SendGrid specific, but this class should be easy to replace as needed.
    /// </summary>
    public class MailSenderServiceSendGrid : MailSenderServiceBase, IMailSenderService
    {
        private readonly IMailSenderSettings _mailSenderSettings;
        private readonly ISendGridEmailAddressFactory _sendGridEmailAddressFactory;

        public MailSenderServiceSendGrid(IMailSenderSettings mailSenderSettings,
            ISendGridEmailAddressFactory sendGridEmailAddressFactory)
        {
            _mailSenderSettings = mailSenderSettings;
            _sendGridEmailAddressFactory = sendGridEmailAddressFactory;
        }

        public override async Task<HttpStatusCode> SendMailAsync(string subject, string plainTextContent,
            string htmlContent, string senderMailAndName,
            string recieverMailAndName)
        {
            // Sendgrid recommends that the APIKey is stored in Environment Variables on each machine.
            // It's certainly more secure, but this is flexible.
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(_mailSenderSettings.SendGridApiKey);

            var receiverParts = recieverMailAndName.Split(';');
            var receiver = _sendGridEmailAddressFactory.Create(receiverParts[0], receiverParts[1]);

            var msg = MailHelper.CreateSingleEmail(_sendGridEmailAddressFactory.CreateSingle(senderMailAndName),
                receiver, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            return response.StatusCode;
        }

        public bool CanHandle(string senderType)
        {
            return senderType.ToLower() == "sendgrid";
        }
    }
}