using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ajf.NugetWatcher
{
    public abstract class MailSenderServiceBase
    {
        public async Task<HttpStatusCode> SendMailAsync(string subject, string plainTextContent, string htmlContent,
            string senderMailAndName,
            string[] recieverMailAndName)
        {
            await Task.FromResult(0);

            recieverMailAndName.Select(x =>
                SendMailAsync(subject, plainTextContent, htmlContent, senderMailAndName, x).Result);

            return HttpStatusCode.Accepted;
        }

        public abstract Task<HttpStatusCode> SendMailAsync(string subject, string plainTextContent, string htmlContent,
            string senderMailAndName,
            string recieverMailAndName);
    }
}