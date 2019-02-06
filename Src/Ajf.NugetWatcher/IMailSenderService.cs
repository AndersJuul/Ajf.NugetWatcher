using System.Net;
using System.Threading.Tasks;

namespace Ajf.NugetWatcher
{
    public interface IMailSenderService
    {
        Task<HttpStatusCode> SendMailAsync(string subject, string plainTextContent, string htmlContent, string senderMailAndName, string[] recieverMailAndName);
        Task<HttpStatusCode> SendMailAsync(string subject, string plainTextContent, string htmlContent, string senderMailAndName, string recieverMailAndName);
        bool CanHandle(string senderType);
    }
}