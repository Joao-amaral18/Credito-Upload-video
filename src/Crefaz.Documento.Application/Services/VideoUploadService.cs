using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using Crefaz.Documento.Application.Interfaces;
using Crefaz.Documento.Application.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Crefaz.Documento.Application.services;

public class VideoUploadService : IVideoService
{
    private readonly IMediatorHandler _mediator;
    private string connectionString;
    private string _shareName;
    private readonly ShareServiceClient _shareServiceClient;


    public VideoUploadService(
        IConfiguration Configuration,
        ILogger<VideoUploadService> logger,
        IMediatorHandler mediator
    )
    {
        _mediator = mediator;
        connectionString = Configuration.GetSection("StorageConnectionStringArquivos").Value;
        _shareName = Configuration.GetSection("StorageShareName").Value;
    }
        public async Task<bool> UploadVideo(IFormFile file)
        {
        try
        {
            var shareClient = _shareServiceClient.GetShareClient(_shareName);
            var directoryClient = shareClient.GetDirectoryClient("temp");
            var fileClient = directoryClient.GetFileClient(Guid.NewGuid().ToString() + ".mp4");

            using (var stream = file.OpenReadStream())
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }

            return true;
        }
        catch (RequestFailedException ex)
        {
            // Handle exception
            Console.WriteLine($"Error uploading file: {ex.Message}");
            return false;
        }
    }
    // public async Task<bool> DownloadFileAsync(string fileName)
    // {
    //     try
    //     {
    //         var shareClient = _shareServiceClient.GetShareClient(_shareName);
    //         var directoryClient = shareClient.GetDirectoryClient("temp");
    //         var fileClient = directoryClient.GetFileClient(fileName);

    //         if (!await fileClient.ExistsAsync())
    //         {
    //             Console.WriteLine("File does not exist.");
    //             return false;
    //         }

    //         var downloadFilePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    //         using (FileStream fs = File.OpenWrite(downloadFilePath));
    //         await fileClient.DownloadAsync(fs);

    //         Console.WriteLine($"File downloaded to: {downloadFilePath}");
    //         return true;
    //     }
    //     catch (RequestFailedException ex)
    //     {
    //         // Handle exception
    //         Console.WriteLine($"Error downloading file: {ex.Message}");
    //         return false;
    //     }
    // }

    // public async Task<bool> UploadVideo(IFormFile file)
    // {
    //     var fileName = Guid.NewGuid().ToString() + ".mp4";
    //     var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
    //     using FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
    //     file.CopyTo(stream);

    //     await shareClient.CreateIfNotExistsAsync();

    //     try
    //     {
    //         var nome
    //         string pastaStorage = nomeArquivo.Substring(0, nomeArquivo.LastIndexOf("/"));
    //         string nomeArquivoStorage = nomeArquivo.Substring(nomeArquivo.LastIndexOf("/") + 1);
    //         MemoryStream stream = new MemoryStream(conteudo);

    //         var file = await CreateFileAsync(pastaStorage, nomeArquivoStorage, stream);

    //         return await file.ExistsAsync();
    //     }
    //     catch (Exception)
    //     {
    //         await _mediator.RaiseEventAsync(
    //             new DomainNotification("error", "Não foi possível realizar UPLOAD de arquivo")
    //         );
    //         return false;
    //     }

    //     return true;
    // }
    // public async Task<string> UploadDocumento(int propostaId, PropostaDocumentoViewModel proposta)
    //     {

    //             var retornoProcedure = await _baseProcedureRepository.ExecProcedureService("stpCarregaImagemCrud", "upload-anexo-proposta", SerializeObject(putData), null, routeParams);

    //             int propostaImagemId = await UploadArquivoStorage(retornoProcedure, "PropostaImagem", null, proposta.conteudo);

    //             return propostaImagemId != 0 ? "Upload Concluido" : null;
    //         else
    //             return null;

    //     }
}
