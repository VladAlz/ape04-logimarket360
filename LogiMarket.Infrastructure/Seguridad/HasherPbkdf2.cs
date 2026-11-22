using System.Security.Cryptography;
using LogiMarket.Application.Puertos;

namespace LogiMarket.Infrastructure.Seguridad;

// Formato guardado: PBKDF2$iteraciones$sal$hash (Base64)
public sealed class HasherPbkdf2 : IHasherContrasenas
{
    private const int Iteraciones = 100_000;
    private const int BytesSal = 16;
    private const int BytesHash = 32;

    public string Calcular(string contrasena)
    {
        var sal = RandomNumberGenerator.GetBytes(BytesSal);
        var hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, Iteraciones, HashAlgorithmName.SHA256, BytesHash);
        return $"PBKDF2${Iteraciones}${Convert.ToBase64String(sal)}${Convert.ToBase64String(hash)}";
    }

    public bool Verificar(string contrasena, string hashGuardado)
    {
        var partes = hashGuardado.Split('$');
        if (partes.Length != 4 || partes[0] != "PBKDF2" || !int.TryParse(partes[1], out var iteraciones))
            return false;

        var sal = Convert.FromBase64String(partes[2]);
        var esperado = Convert.FromBase64String(partes[3]);
        var calculado = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);
        return CryptographicOperations.FixedTimeEquals(calculado, esperado);
    }
}
