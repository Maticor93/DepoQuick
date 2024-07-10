namespace LogicaDeNegocio.ExcepcionesLogica;

public class ReservaLogicaExcepcion : Exception
{
    public ReservaLogicaExcepcion(String mensaje) : base(mensaje){ }
}