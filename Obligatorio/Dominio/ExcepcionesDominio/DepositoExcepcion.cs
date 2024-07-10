namespace Dominio.ExcepcionesDominio;

public class DepositoExcepcion : Exception
{
    public DepositoExcepcion(String mensaje) : base(mensaje){ }
}