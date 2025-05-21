namespace PunchSystem.DTOs
{
    public class CreateProduitDto
    {
        public string Code { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
    }

    public class UpdateProduitDto
    {
        public string Code { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
    }
}
