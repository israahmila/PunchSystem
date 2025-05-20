namespace PunchSystem.DTOs
{
    public class AssignPermissionRequest
    {
        public string UserId { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
