namespace MuffiNet.FrontendReact.DomainModel.Commands.CreateRoom
{
    public class CreateRoomResponse
    {
        public string TwilioRoomName { get; set; }

        public string TwilioVideoGrantForTechnicianToken { get; set; }

        public string TwilioVideoGrantForCustomerToken { get; set; }
    }
}