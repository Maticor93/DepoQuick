using Dominio;

namespace LogicaDeNegocio;

public interface IGeneradorReporte
{
    void GenerarReporte(List<Reserva> reservas){}
}