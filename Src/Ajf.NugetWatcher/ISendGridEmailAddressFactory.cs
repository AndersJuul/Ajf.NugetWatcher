using System.Collections.Generic;
using SendGrid.Helpers.Mail;

namespace Ajf.NugetWatcher
{
    public interface ISendGridEmailAddressFactory
    {
        EmailAddress Create(string email, string name);
        EmailAddress CreateSingle(string mailAndName);
        IEnumerable<EmailAddress> CreateMany(string[] mailAndName);
    }
}