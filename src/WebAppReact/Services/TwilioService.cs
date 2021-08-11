using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using WebAppReact.Models;
using WebAppReact.Exceptions;
using static Twilio.Rest.Video.V1.RoomResource;

namespace WebAppReact.Services
{
    public class TwilioService : ITwilioService
    {
        readonly TwilioSettings _twilioSettings;

        public TwilioService(Microsoft.Extensions.Options.IOptions<TwilioSettings> twilioOptions)
        {
            _twilioSettings = twilioOptions?.Value ?? throw new ArgumentNullException(nameof(twilioOptions));

            TwilioClient.Init(_twilioSettings.ApiKey, _twilioSettings.ApiSecret, _twilioSettings.AccountSid);
        }

        public async Task CompleteRoom(string uniqueRoomName)
        {
            RoomResource room;
            try
            {
                room = await RoomResource.FetchAsync(uniqueRoomName);
            }
            catch(Exception)
            {
                throw new TwilioRoomNotFoundException($"TwilioRoom with room name {uniqueRoomName} was not found");
            }
            if (room != null && room.Status == RoomStatusEnum.InProgress)
                await RoomResource.UpdateAsync(uniqueRoomName, RoomStatusEnum.Completed);
        }
        public async Task<bool> IsRoomCompleted(string roomSid)
        {
            if (roomSid == null)
                throw new NullReferenceException(roomSid);

            RoomResource room;
            try
            {
                room = await RoomResource.FetchAsync(roomSid);
            }
            catch(Exception)
            {
                throw new TwilioRoomNotFoundException($"TwilioRoom with room sid {roomSid} was not found");
            }
            return room != null && room.Status == RoomStatusEnum.Completed;
        }

        public async Task<string> CreateRoom(string roomName)
        {
            RoomResource roomInstance = await RoomResource.CreateAsync(
                recordParticipantsOnConnect: true,
                // statusCallback: new Uri("http://example.org"),
                type: RoomResource.RoomTypeEnum.Group,
                uniqueName: roomName
            );

            return roomInstance.Sid;
        }

        public string CreateVideoGrant(string identity, string uniqueRoomName)
        {
            var Token = new Token(
                _twilioSettings.AccountSid,
                _twilioSettings.ApiKey,
                _twilioSettings.ApiSecret,
                identity,
                grants: new HashSet<IGrant> { new VideoGrant() { Room = uniqueRoomName } });

            return Token.ToJwt();
        }
    }
}
