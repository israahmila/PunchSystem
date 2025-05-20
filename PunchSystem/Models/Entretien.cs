namespace PunchSystem.Models
{
    using PunchSystem.Helpers;

    public class Entretien : AuditableEntity
    {
        public string Id { get; set; } = IdGenerator.New("ENT");
        public string Reference { get; set; } = string.Empty;
        public DateTime DateEntretien { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = new User();

        public string ReferenceUtilisation { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
        public string? Commentaire { get; set; }
    }

}
