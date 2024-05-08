namespace Crefaz.Documento.Application.Models;

public class VideoUploadResponseModel
{
    public VideoUploadResponseModel(byte[] Content)
    {
        this.Content = Content;
    }

    public byte[] Content { get; set; }
}