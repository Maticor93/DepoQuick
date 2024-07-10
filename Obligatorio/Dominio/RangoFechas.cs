using Dominio.ExcepcionesDominio;

namespace Dominio;

public class RangoFechas
{
    public int Id { get; set; }
    
    public RangoFechas()
    {
        FechaInicio = DateTime.MinValue;
        FechaFin = DateTime.MinValue.AddDays(1);
    }
    public RangoFechas(DateTime ini, DateTime fin)
    {
        ValidarRangoFecha(ini, fin);
        FechaInicio = ini;
        FechaFin = fin;
    }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public void ActualizarRangoFechas(DateTime ini, DateTime fin)
    {
        ValidarRangoFecha(ini, fin);
        FechaInicio = ini;
        FechaFin = fin;
    }

    private void ValidarRangoFecha(DateTime ini, DateTime fin)
    {
        if (ini == null || fin == null) throw new RangoFechasExcepcion("Las fechas de inicio y fin no pueden ser nulas");
        if (ini > fin) throw new RangoFechasExcepcion("La fecha de inicio no puede ser posterior a la fecha de fin");
        if (ini < DateTime.Today) throw new RangoFechasExcepcion("La fecha de inicio no puede ser anterior a la fecha de hoy");
    }

    public bool PerteneceAlRango(DateTime fecha)
    {
        if (fecha >= FechaInicio && fecha <= FechaFin)
            return true;
        return false;
    }

    public bool ContenidaEnRango(RangoFechas rangoFechas)
    {
        return rangoFechas.FechaInicio >= FechaInicio && rangoFechas.FechaFin <= FechaFin;
    }
}