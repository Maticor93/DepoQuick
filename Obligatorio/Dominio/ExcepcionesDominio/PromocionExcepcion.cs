namespace Dominio.ExcepcionesDominio;

public class PromocionExcepcion : Exception
{
    public PromocionExcepcion(String mensaje) : base(mensaje){ }
}