using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using CodenApp.Sdk.Infrastructure.Data;
using Crefaz.Documento.Domain.Entities;
using Crefaz.Documento.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Crefaz.Documento.Infra.Data.Repository
{
    public class VideoRepository : Repository<Video, int>, IVideoRepository
    {
        private readonly CreditoContext _context;

        public VideoRepository(CreditoContext context) : base(context)
        {
            _context = context;
        }

         public async Task<string> InsertVideo(Video video)
        {
                await _context.AddAsync(video);
                await _context.SaveChangesAsync();
                return video.Nome;
        }
        public async Task<IEnumerable<Video>> GetAllVideos()
        {
            return await _context.video.ToListAsync();
        }

    }
}
