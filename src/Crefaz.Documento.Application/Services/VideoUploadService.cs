using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Storage.Files.Shares;
using Crefaz.Documento.Domain.Entities;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using Azure;
using Crefaz.Documento.Application.Interfaces;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Crefaz.Documento.Application.Services
{
    public class VideoUploadService : IVideoService
    {
        private readonly IMediatorHandler _mediator;
        private readonly ShareClient _shareClient;
        private readonly IVideoRepository _videoRepository;
        private const long MaxFileSize = 1 * 1024 * 1024 * 1024; // 1GB

        public VideoUploadService(
            IConfiguration configuration,
            ILogger<VideoUploadService> logger,
            IMediatorHandler mediator,
            IVideoRepository videoRepository
        )
        {
            _mediator = mediator;
            _videoRepository = videoRepository;
            string connectionString = configuration
                .GetSection("StorageConnectionStringArquivos")
                .Value;
            string shareName = configuration.GetSection("StorageShareName").Value;
            _shareClient = new ShareClient(connectionString, shareName);
        }

        public async Task<string> UploadVideo(IFormFile file)
        {
            try
            {
                if (file.Length > MaxFileSize)
                {
                    Console.WriteLine("File size exceeds the limit (5MB).");
                    return null;
                }

                string tempFilePath = Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString() + Path.GetExtension(file.FileName)
                );
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                string duration = string.Empty;
                string fileName;
                try
                {
                    using (ShellObject shell = ShellObject.FromParsingName(tempFilePath)){
                    IShellProperty prop = shell.Properties.System.Media.Duration;
                    duration = prop.FormatForDisplay(PropertyDescriptionFormatOptions.None);
                    }
                }
                catch (Exception) { }

                using (var stream = new FileStream(tempFilePath, FileMode.Open))
                {
                    var directoryClient = _shareClient.GetDirectoryClient("temp");
                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var fileClient = directoryClient.GetFileClient(fileName);

                    await fileClient.CreateAsync(stream.Length);

                    var fileInfo = await fileClient.GetPropertiesAsync();

                    var retornoInsert = await _videoRepository.InsertVideo(
                        new Video(
                            nome:fileName,
                            caminhoArquivo: fileClient.Path.ToString(),
                            tamanho:fileInfo.Value.ContentLength.ToString(),
                            duracao: duration.ToString()
                        )
                    );
                    if (retornoInsert == fileName)
                        await fileClient.UploadAsync(stream);
                }

                File.Delete(tempFilePath);

                return fileName;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Video>> ListAllVideos()
        {
            try
            {
                return await _videoRepository.GetAllVideos();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing videos: {ex.Message}");
                return new List<Video>();
            }
        }

        public async Task<Stream> DownloadVideo(string videoName)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient("temp");
                var fileClient = directoryClient.GetFileClient(videoName);

                if (await fileClient.ExistsAsync())
                {
                    var downloadInfo = await fileClient.DownloadAsync();
                    return downloadInfo.Value.Content;
                }
                else
                {
                    Console.WriteLine("Video not found.");
                    return null;
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error downloading video: {ex.Message}");
                return null;
            }
        }
    }
}
