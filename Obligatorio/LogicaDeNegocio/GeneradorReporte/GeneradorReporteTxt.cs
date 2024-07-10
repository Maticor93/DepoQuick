using Dominio;

namespace LogicaDeNegocio;

public class GeneradorReporteTxt : IGeneradorReporte
{
    private readonly String _path;
    public GeneradorReporteTxt(String path)
    {
        _path = path + "/ReporteReservas.txt";
    }

    public void GenerarReporte(List<Reserva> reservas)
    {
        List<String> lineas = new List<string>();
        lineas.Add($"Nombre deposito\tFecha reserva\tUsuario\tPrecio\tEstado del pago");
        foreach (var reserva in reservas)
        {
            String pagoEstado = reserva.Pago != null ? reserva.Pago.ToString() : "Sin pago";
            var fechasString = $"{reserva.RangoFecha.FechaInicio.ToString("dd/MM/yyyy")}-{reserva.RangoFecha.FechaFin.ToString("dd/MM/yyyy")}";
            var depositoString = "Deposito eliminado";
            if (reserva.Deposito != null) depositoString = reserva.Deposito.Nombre;
            var linea = $"{depositoString}\t{fechasString}\t{reserva.Cliente.Email}\t${reserva.Precio}\t{pagoEstado}";
            lineas.Add(linea);
            
        }
        File.WriteAllLines(_path,lineas);
    }
}