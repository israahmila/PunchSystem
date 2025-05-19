using Microsoft.AspNetCore.Http;

namespace PunchSystem.DTOs;

public class FileUploadDto
{
    public IFormFile File { get; set; } = null!;
}
