
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace CodenApp.Sdk.Shared.Extensions.Services
{
    public static class CsvService
    {
        public static byte[] GenerateCsv<T>(T[] values)
        {
            byte[] csvFinal = null;
            using (var ms = new MemoryStream())
            {
                var ptbr = new CultureInfo("pt-BR");
                var windows = CodePagesEncodingProvider.Instance.GetEncoding(1252);
                var config = new CsvConfiguration(ptbr) { Delimiter = ";", Encoding = windows };
                using (var writer = new StreamWriter(ms, windows))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(values);
                    csv.Flush();
                }
                csvFinal = ms.ToArray();
            }
            return csvFinal;
        }  
    }      
}