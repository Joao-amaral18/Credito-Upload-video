using System;

namespace CodenApp.Sdk.Shared.Extensions
{
    public static class GenericReturnValidation
    {
        public static string ItemNaoExiste(string campo)
        {
            return $"{campo} não existe.";
        }

        public static string CampoVazio(string campo)
        {
            return $"O campo {campo} não pode ser vazio ou nulo.";
        }

        public static string Caracteres(string campo, int n1, int n2)
        {
            return $"O campo {campo} deve conter entre {n1} e {n2} caracteres.";
        }

        public static string CampoEmUso(string campo)
        {
            return $"O campo {campo} já está em uso.";
        }

        public static string CampoNaoExiste(string campo)
        {
            return $"O campo {campo} informado não existe.";
        }

        public static string CampoIgualZero(string campo1)
        {
            return $"Não é possivel utilizar o valor 0 em {campo1}.";
        }

        public static string CampoIgualZero(string campo1, string campo2, string valor)
        {
            return $"Não é possivel utilizar o valor 0 em {campo1} e {campo2} ao mesmo tempo.";
        }

        public static string CampoUnico(string a, string b)
        {
            return $"Não pode ser cadastrado mais de um {a} para o mesmo campo {b}.";
        }

        public static string MinimoDuasPalavras(string a, string b)
        {
            return $"O campo {a} deve possuir pelo menos {b} palavras.";
        }

        public static string DataMenorQueHoje(string a)
        {
            return $"O campo {a} deve ser preenchido com uma data maior que a data atual.";
        }

        public static string ConteudoDiferenteDoTipo(string campo, string tipo)
        {
            return $"O conteúdo do campo {campo} não condiz com o campo {tipo}.";
        }

        public static string DataMaiorQueHoje(string a)
        {
            return $"O campo {a} deve ser preenchido com uma data menor que a data atual.";
        }

        public static string CampoNaoValido(string a)
        {
            return $"O valor do campo {a} não é válido.";
        }

        public static string CampoTamanhoMaximo(string campo, int max)
        {
            return $"O campo {campo} deve possuir no máximo {max} caracteres.";
        }

        public static string CampoMaiorQue(string campo, int min)
        {
            return $"O campo {campo} deve possuir valor maior que {min}.";
        }
    }
}