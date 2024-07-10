using Dominio;

namespace LogicaDeNegocio;

public class GeneradorReporteCsv : IGeneradorReporte
{
    private readonly String _path;
    public GeneradorReporteCsv(String path)
    {
        _path = path + "/ReporteReservas.csv";
    }

    public void GenerarReporte(List<Reserva> reservas)
    {
        List<String> lineas = new List<string>();
        lineas.Add("Nombre deposito,Fecha reserva,Usuario,Precio,Estado del pago");
        foreach (var reserva in reservas)
        {
            String pagoEstado = reserva.Pago != null ? reserva.Pago.ToString() : "Sin pago";
            var fechasString = $"{reserva.RangoFecha.FechaInicio.ToString("dd/MM/yyyy")}-{reserva.RangoFecha.FechaFin.ToString("dd/MM/yyyy")}";
            var depositoString = "Deposito eliminado";
            if (reserva.Deposito != null) depositoString = reserva.Deposito.Nombre;
            var linea = $"{depositoString},{fechasString},{reserva.Cliente.Email},${reserva.Precio},{pagoEstado}";
            lineas.Add(linea);
            
        }
        File.WriteAllLines(_path,lineas);
    }
}