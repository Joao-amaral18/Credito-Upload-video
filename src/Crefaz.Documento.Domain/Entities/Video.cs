namespace Crefaz.Documento.Domain.Entities;

public class Video
{
    public string Nome { get; set; }
    public DateTime DataHora { get; set; }
    public string Duracao { get; set; }
    public string Tamanho { get; set; }
    public string CaminhoArquivo { get; set; }
}