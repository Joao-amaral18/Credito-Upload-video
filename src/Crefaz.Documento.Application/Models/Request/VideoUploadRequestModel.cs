using Microsoft.AspNetCore.Http;

namespace Crefaz.Documento.Application.Models;

public class VideoUploadRequestModel
{
    public IFormFile VideoFile { get; set; }
}