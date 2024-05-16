using Crefaz.Documento.Application.Models;
using Crefaz.Documento.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Crefaz.Documento.Application.Interfaces;

public interface IVideoService
{
    Task<bool> UploadVideo(IFormFile uploadResquest);
    Task<Stream> DownloadVideo(string fileName);
    Task<IEnumerable<Video>> ListAllVideos();
}
