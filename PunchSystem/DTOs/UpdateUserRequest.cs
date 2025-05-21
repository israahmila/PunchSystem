namespace PunchSystem.DTOs
{
    public class UpdateUserRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
