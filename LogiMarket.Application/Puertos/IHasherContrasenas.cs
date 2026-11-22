namespace LogiMarket.Application.Puertos;

public interface IHasherContrasenas
{
    string Calcular(string contrasena);

    bool Verificar(string contrasena, string hashGuardado);
}
