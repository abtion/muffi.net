namespace MuffiNet.Backend.Services.Authorization {
    public class UserDetails {
        public string Email { get; init; }

        public UserDetails(string email) {
            Email = email;
        }
    }
}
