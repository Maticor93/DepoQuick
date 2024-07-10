namespace Dominio.ExcepcionesDominio;

public class ReservaExcepcion : Exception
{
    public ReservaExcepcion(String mensaje) : base(mensaje){}
}