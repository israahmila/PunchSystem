namespace PunchSystem.DTOs
{
    public class AssignPermissionRequest
    {
        public string UserId { get; set; }
        public string PermissionId { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
