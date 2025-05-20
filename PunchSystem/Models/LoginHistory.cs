using PunchSystem.Helpers;

namespace PunchSystem.Models
{
    public class LoginHistory
    {
        public String Id { get; set; }= IdGenerator.New("LIH");
        public string UserId { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public string IPAddress { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
