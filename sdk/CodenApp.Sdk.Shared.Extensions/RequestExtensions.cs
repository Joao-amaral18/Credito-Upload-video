using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;

namespace CodenApp.Sdk.Shared.Extensions
{
    public sealed class RequestExtensions
    {
        /// <summary>
        /// Envia requisições RestFull json
        /// </summary>
        /// <param name="url">URL da chamada da API</param>
        /// <param name="action">Nome da Action da rota</param>
        /// <param name="verboHttp">Verbo HTTP (GET,POST,PUT)</param>
        /// <param name="data">Dados de envio</param>
        /// <param name="tokenJWT">Token JWT</param>
        /// <returns>Retorna dados de solicitação Rest a API's</returns>
        public static async Task<T> SendRequest<T>(string url, string action, RestSharp.Method verboHttp, object data = null, string tokenJWT = null)
        {
            var client = new RestSharp.RestClient($"{url}/{action}");
            var request = new RestSharp.RestRequest(string.Empty, verboHttp);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            if (tokenJWT != null)
                request.AddHeader("Authorization", tokenJWT);
            if (data != null)
                request.AddBody(data, "application/json");

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ToString());
            }

            return default(T);
        }
        /// <summary>
        /// Envia requisições RestFull json
        /// </summary>
        /// <param name="url">URL da chamada da API</param>
        /// <param name="action">Nome da Action da rota</param>
        /// <param name="verboHttp">Verbo HTTP (GET,POST,PUT)</param>
        /// <param name="data">Dados de envio</param>
        /// <param name="tokenJWT">Token JWT</param>
        /// <returns>Retorna dados de solicitação Rest a API's</returns>
        public static async Task<RestResponse> SendRequest_RestResponse(string url, string action, RestSharp.Method verboHttp, string data = null, string tokenJWT = null)
        {
            var client = new RestSharp.RestClient($"{url}/{action}");
            var request = new RestSharp.RestRequest(string.Empty, verboHttp);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            if (tokenJWT != null)
                request.AddHeader("Authorization", tokenJWT);
            if (data != null)
                request.AddStringBody(data, "application/json");

            return await client.ExecuteAsync(request);
        }

        /// <summary>
        /// Enviar requisições SOAP de arquivos XML para o API CreditTalk (API Crivo)
        /// </summary>
        /// <param name="XML">XML das Políticas do Crivo</param>
        /// <param name="url">URL da API SOAP (https://crivo.crefaz.com.br/ws/CreditTalkStateless.asmx)</param>
        /// <param name="soapAction">Método XML da Política (SetPolicyEvalValuesObjectXml)</param>
        /// <param name="userName">Nome do Usuário da API</param>
        /// <param name="userPassword">Senha deUsuário</param>
        /// <returns>Retorna uma string XML com o resultado da Consulta ao Crivo</returns>
        public static async Task<string> SendRequestSOAP(string XML, string url, string soapAction, string userName, string userPassword)
        {
            var credencialCrivo = Convert.ToBase64String(Encoding.Default.GetBytes($"{userName}:{userPassword}"));

            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;
            // construir objetos de solicitação para passar os dados / xml para o servidor     
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(XML);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            request.PreAuthenticate = true;
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + credencialCrivo);
            request.Headers.Add("Accept-Encoding", " gzip,deflate");
            request.Headers.Add("Content-Type", "text/xml;charset=utf-8");
            request.Headers.Add("SOAPAction", soapAction);
            request.ContentLength = buffer.Length;
            request.Timeout = 300000;

            // postar dados e fechar conexão
            Stream post = await request.GetRequestStreamAsync();
            post.Write(buffer, 0, buffer.Length);
            post.Close();

            // construir objeto de resposta
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            Stream responsedata = response.GetResponseStream();
            StreamReader responsereader = new StreamReader(responsedata);
            return await responsereader.ReadToEndAsync();
        }

        public static async Task<(String, bool)> SendRequestRPC(string url, string payload)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(payload, Encoding.UTF8, "text/xml");

                    // Definir o cabeçalho Content-Type como "text/xml"
                    //client.DefaultRequestHeaders.Add("Content-Type", "text/xml;charset=utf-8");

                    // Enviar a solicitação POST
                    //HttpResponseMessage response = await client.PostAsync(url, new StringContent(payload));
                    HttpResponseMessage response = await client.SendAsync(request);


                    var retorno = await response.Content.ReadAsStringAsync();
                    var IsSuccessStatusCode = response.IsSuccessStatusCode;

                    response.Dispose();

                    return (retorno, IsSuccessStatusCode);
                }
            }
            catch (Exception e)
            {
                return (e.Message, false);
            }
        }

        public static async Task<(String, bool)> SendWebRequestRPC(string url, string body)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = "POST";
            request.ContentType = "text/xml";
            request.Timeout = 400000;
            request.ServicePoint.Expect100Continue = true;


            byte[] bytes = Encoding.Default.GetBytes(body);
            request.ContentLength = bytes.Length;

            using (var requestStream = request.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);

            var response = await request.GetResponseAsync() as HttpWebResponse;

            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                string responseContent = await reader.ReadToEndAsync();
                return (responseContent, response.StatusCode == HttpStatusCode.OK);
            }
        }


        public static async Task<RestResponse> SendRequestSOAP(string url, string xml)
        {
            HttpClient request = new HttpClient();
            StringContent content = new StringContent(xml, Encoding.UTF8, "text/xml");
            var response = await request.PostAsync(url, content);

            RestResponse restResponse = new RestResponse()
            {
                StatusCode = (HttpStatusCode)(int)response.StatusCode,
                Content = await response.Content.ReadAsStringAsync()
            };

            return restResponse;
        }

    }
}