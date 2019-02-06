using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ajf.NugetWatcher
{
    public abstract class MailSenderServiceBase
    {
        public HttpStatusCode SendMail(string subject, string plainTextContent, string htmlContent,
            string senderMailAndName,
            string[] recieverMailAndName)
        {
            foreach (var recieverMailAndNa in recieverMailAndName)
            {
                SendMail(subject, plainTextContent, htmlContent, senderMailAndName, recieverMailAndNa);
            };

            return HttpStatusCode.Accepted;
        }

        public abstract HttpStatusCode SendMail(string subject, string plainTextContent, string htmlContent,
            string senderMailAndName,
            string recieverMailAndName);
    }
}