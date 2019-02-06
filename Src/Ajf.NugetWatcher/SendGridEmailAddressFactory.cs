using System.Collections.Generic;
using System.Linq;
using SendGrid.Helpers.Mail;

namespace Ajf.NugetWatcher
{
    public class SendGridEmailAddressFactory : ISendGridEmailAddressFactory
    {
        public EmailAddress Create(string email, string name)
        {
            return new EmailAddress(email, name);
        }

        public EmailAddress CreateSingle(string mailAndName)
        {
            var strings = mailAndName.Split(';').ToArray();
            return Create(strings[0], strings[1]);
        }

        public IEnumerable<EmailAddress> CreateMany(string[] mailAndName)
        {
            foreach (var s in mailAndName)
            {
                yield return CreateSingle(s);
            }
        }
    }
}