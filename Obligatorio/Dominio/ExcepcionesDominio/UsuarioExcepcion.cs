namespace Dominio.ExcepcionesDominio;

public class UsuarioExcepcion : Exception
{
    public UsuarioExcepcion(String mensaje) : base(mensaje){ }
}