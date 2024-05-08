using CodenApp.Sdk.Domain.Abstraction.Entities;
using System.Globalization;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        static readonly string initialDelimiter = "{{";
        static readonly string finalDelimiter = "}}";

        public static DateTime ToDateTime(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return DateTime.MinValue;

            DateTime data;
            DateTime.TryParse(source, out data);

            return data;
        }

        public static Guid ToGuid(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return Guid.Empty;

            Guid guid;
            Guid.TryParse(source, out guid);

            return guid;
        }

        public static int ToInt(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return 0;

            int inteiro;
            int.TryParse(source, out inteiro);

            return inteiro;
        }

        public static bool ToBool(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            bool.TryParse(source, out bool valor);

            return valor;
        }

        private static bool VerifyProperty(string property) => property.ToUpper().Equals("ASSINATURACLIENTE") || property.ToUpper().Equals("ASSINATURACREFAZ");

        public static string ReplacePropertyNameForPropertyValue(
            string body,
            EntityBase<int> entity
        )
        {
            var properties = entity.GetType().GetProperties();

            foreach (var property in properties)
            {
                var valor = property.GetValue(entity);
                body = body
                        .Replace(
                            initialDelimiter + property.Name.ToUpper() + finalDelimiter, 
                            (valor == null) 
                                ? string.Empty 
                                : (
                                    VerifyProperty(property.Name) 
                                        ? valor.ToString() 
                                        : valor.ToString().ToUpper()    
                                )
                        );
            }

            return body;
        }

        public static string RemoveDataTypeBase64(string base64)
        {
            base64 = base64.Substring(0, 4).Equals("data") ? base64.Substring(base64.IndexOf(",") + 1): base64;
            return base64.Trim();
        }

        public static string RemoveContentBase64(this string base64)
        {
            return base64[..(base64.IndexOf(";") + 1)];
        }

        public static string RemoveSymbolsAccents(this string text)
        {
            string textClean = text.Normalize(NormalizationForm.FormD);
            StringBuilder resultEnd = new StringBuilder();

            foreach (char letter in textClean)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    resultEnd.Append(letter);
            }

            return resultEnd.ToString();
        }

    }
}
