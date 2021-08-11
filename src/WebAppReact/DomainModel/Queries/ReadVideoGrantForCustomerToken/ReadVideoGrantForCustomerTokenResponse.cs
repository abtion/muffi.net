namespace WebAppReact.DomainModel.Queries.ReadVideoGrantForCustomerToken
{
    public class ReadVideoGrantForCustomerTokenResponse
    {
        public VideoGrantForCustomerToken Token { get; set; }

        public record VideoGrantForCustomerToken(
            string CustomerName,
            string TwilioRoomName,
            string TwilioVideoGrantForCustomerToken,
            string TechnicianFullName,
            string OssId
        );
    }
}
