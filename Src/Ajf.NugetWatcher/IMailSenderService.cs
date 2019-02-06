using System.Net;
using System.Threading.Tasks;

namespace Ajf.NugetWatcher
{
    public interface IMailSenderService
    {
        HttpStatusCode SendMail(string subject, string plainTextContent, string htmlContent, string senderMailAndName, string[] recieverMailAndName);
        HttpStatusCode SendMail(string subject, string plainTextContent, string htmlContent, string senderMailAndName, string recieverMailAndName);
        bool CanHandle(string senderType);
    }
}