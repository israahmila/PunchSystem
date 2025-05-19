namespace PunchSystem.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public List<Utilisation> Utilisations { get; set; } = new List<Utilisation>();
    }
}

