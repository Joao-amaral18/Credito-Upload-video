using Crefaz.Documento.Application.Models;
using Microsoft.AspNetCore.Http;

namespace Crefaz.Documento.Application.Interfaces;

public interface IVideoService
{
    Task<bool> UploadVideo(IFormFile uploadResquest);
}
