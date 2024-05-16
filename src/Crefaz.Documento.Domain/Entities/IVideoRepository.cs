namespace Crefaz.Documento.Domain.Entities;

 public interface IVideoRepository
    {
        Task<string> InsertVideo(Video video);
        Task<IEnumerable<Video>> GetAllVideos();

    }