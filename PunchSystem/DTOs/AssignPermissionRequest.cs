namespace PunchSystem.DTOs
{
    public class AssignPermissionRequest
    {
        public int UserId { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
