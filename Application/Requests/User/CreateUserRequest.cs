using Domain.Enums;

namespace Application.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public Roles Role { get; set; }

    }
}
