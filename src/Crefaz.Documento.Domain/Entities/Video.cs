using CodenApp.Sdk.Domain.Abstraction.Entities;

namespace Crefaz.Documento.Domain.Entities;

public class Video : EntityBase<int>
{
    public Video(string nome,string caminhoArquivo, string duracao, string tamanho)
    {
        Nome = nome;
        CaminhoArquivo = caminhoArquivo;
        Tamanho = tamanho;
        Duracao = duracao;
        DataHora = DateTime.UtcNow.AddHours(-3);
    }

    public string Nome { get; set; }
    public DateTime DataHora { get; set; }
    public string Tamanho { get; set; }
    public string Duracao { get; set; }
    public string CaminhoArquivo { get; set; }
}
