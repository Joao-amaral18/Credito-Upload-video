namespace Crefaz.Documento.Application.Models;

public class AcessoMicroServicoModel
{

    public string Nome { get; set; }
    public string Parametros { get; set; }
}

public class AcessoMicroServicoParametroModel
{
    public string urlBase { get; set; }
    public string token { get; set; }
}