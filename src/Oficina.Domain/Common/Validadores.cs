using System.Text.RegularExpressions;

namespace Oficina.Domain.Common;

public static class Validadores
{
    public static bool Documento(string valor)
    {
        var numeros = ApenasNumeros(valor);
        if (numeros.Length is not (11 or 14) || numeros.Distinct().Count() == 1) return false;
        var tamanho = numeros.Length - 2;
        var soma = 0;
        for (var i = 0; i < tamanho; i++) soma += (numeros[i] - '0') * (tamanho + 1 - i);
        var digito = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (digito != numeros[tamanho] - '0') return false;
        tamanho++;
        soma = 0;
        for (var i = 0; i < tamanho; i++) soma += (numeros[i] - '0') * (tamanho + 1 - i);
        digito = soma % 11 < 2 ? 0 : 11 - soma % 11;
        return digito == numeros[tamanho] - '0';
    }

    public static bool Placa(string valor) => Regex.IsMatch(valor.Trim().ToUpperInvariant(), "^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$");
    private static string ApenasNumeros(string valor) => new(valor.Where(char.IsDigit).ToArray());
}
