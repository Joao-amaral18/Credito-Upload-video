namespace Crefaz.Documento.Application.Models;

public class DocumentosPropostaModel
{
    public bool IsPdf { get; set; }
    public byte[] Conteudo { get; set; }
    public byte? TipoContrato { get; set; }
    public string TipoDocumento { get; set; }

    public DocumentosPropostaModel()
    {
    }

    public DocumentosPropostaModel(
        bool isPdf,
        byte[] conteudo,
        byte? tipoContrato,
        string tipoDocumento
    )
    {
        IsPdf = isPdf;
        Conteudo = conteudo;
        TipoContrato = tipoContrato;
        TipoDocumento = tipoDocumento;
    }
}