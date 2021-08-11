using System.Threading.Tasks;
using Twilio.Rest.Video.V1;

namespace WebAppReact.Services
{
    public interface ITwilioService
    {
        /// <summary>
        /// Creates a Twilio room, returning the room
        /// </summary>
        Task<string> CreateRoom(string roomSid);

        /// <summary>
        ///
        /// </summary>
        Task<bool> IsRoomCompleted(string uniqueRoomName);
        /// <summary>
        /// Gets the Twilio JSON web token for the given room with uniqueRoomName
        /// </summary>
        string CreateVideoGrant(string identity, string uniqueRoomName);

        /// <summary>
        /// Closes a Twilio room - this method should be called when the video call is finished
        /// </summary>
        Task CompleteRoom(string uniqueRoomName);
    }
}
