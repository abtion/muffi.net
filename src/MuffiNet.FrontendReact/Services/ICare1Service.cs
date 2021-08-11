using System.Threading.Tasks;

namespace MuffiNet.FrontendReact.Services
{
    public interface ICare1Service
    {
        // /// <summary>
        // /// Creates Link between OSS and Care1Live
        // /// Returns OssId 
        // /// </summary>
        Task<string> CreateLinkToOss(string supportTicketId, string customerName, string technicianInitials);

    }
}