namespace Dominio.ExcepcionesDominio;

public class RangoFechasExcepcion : Exception
{
    public RangoFechasExcepcion(String mensaje) : base(mensaje){ }
}