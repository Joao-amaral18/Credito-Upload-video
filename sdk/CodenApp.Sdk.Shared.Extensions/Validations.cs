using System.Linq;
using System.Text.RegularExpressions;

namespace System
{
    public static class Validations
    {
        public static bool IsCpf(this string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = Convertions.RemoveCaracteresEspeciais(cpf);

            if (cpf.Length != 11)
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();


            return cpf.EndsWith(digito);
        }

        public static bool IsPhone(string telefone)
        {
            var formatoCorreto = Regex.Match(telefone, "(\\(?\\d{2}\\)?\\s)?(\\d{4,5}\\-?\\d{4})").Success;
            var numerosIguais = Equals(telefone);

            if (formatoCorreto && !numerosIguais) return true;

            return false;
        }

        public static bool Equals(string num)
        {
            return num.ToString().All(c => c.Equals(num.ToString().First()));
        }

        public static bool TwoWords(string nome)
        {
            string[] source = nome.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return source.Count() >= 2;
        }

        public static bool IsCnpj(this string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = Convertions.RemoveCaracteresEspeciais(cnpj);

            if (cnpj.Length != 14)
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static bool IsDate(this string date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return false;

            date = date.Trim();

            return Regex.IsMatch(date, "([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01]))|" +
                                       "((0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01])-[12]\\d{3})|" +
                                       "((0[1-9]|[12]\\d|3[01])-(0[1-9]|1[0-2])-[12]\\d{3})|" +
                                       "([12]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[12]\\d|3[01]))|" +
                                       "((0[1-9]|1[0-2])/(0[1-9]|[12]\\d|3[01])/[12]\\d{3})|" +
                                       "((0[1-9]|[12]\\d|3[01])/(0[1-9]|1[0-2])/[12]\\d{3})");
        }

        public static bool CheckDate(this string date)
        {
            DateTime Temp;

            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }

        public static bool IsCep(this string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;
            
            string regex = @"^\d{8}$";

            return Regex.IsMatch(cep, (regex));
        }

        public static bool IsTelefone(this string tel)
        {
            if (string.IsNullOrWhiteSpace(tel))
                return false;

            tel = Convertions.RemoveCaracteresEspeciais(tel);

            return Regex.IsMatch(tel,
            "^1\\d\\d(\\d\\d)?$|^0800 ?\\d{3} ?\\d{4}$|" +
            "^(\\(0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\\d\\) ?|" +
            "0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\\d[ .-]?)?(9|9[ .-])?[2-9]\\d{3}[ .-]?\\d{4}$");
        }

        public static bool IsEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            email = email.Trim();

            return Regex.IsMatch(email, ("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,3}"));
        }

        public static bool ValidationFieldType(string campo, byte tipo)
        {
            switch (tipo)
            {
                case 1:
                    return true;
                case 2:
                    return IsInteger(campo);
                case 3:
                    return IsDecimal(campo);
                case 4:
                    return IsDate(campo);
                case 5:
                    return IsByte(campo);
                default:
                    return false;
            }
        }

        private static bool IsByte(string campo)
        {
            byte value;
            return byte.TryParse(campo, out value);
        }

        private static bool IsDecimal(string campo)
        {
            decimal value;
            return Decimal.TryParse(campo, out value);
        }

        private static bool IsInteger(string campo)
        {
            int value;
            return int.TryParse(campo, out value);
        }

        public static bool IsDateTime(string campo)
        {
            DateTime value;
            return DateTime.TryParse(campo, out value);
        }

        public static bool IsStringValidWithRegex(this string toValidation, string regex)
        {
            Regex rg = new Regex(regex);

            var match = rg.Match(toValidation);

            return match.Success; 
        }

        public static bool IsCaracter(this string nome)
        {
            string regex = @"^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$";
            return IsStringValidWithRegex(nome, regex);
        }

        public static bool IsNumeric(this string numeric)
        {
            string regex = @"^\d+$";
            return IsStringValidWithRegex(numeric, regex);
        }
    }
}
