namespace Dtos;

public class RangoFechasDto
{
    
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public RangoFechasDto(DateTime fechaInicio, DateTime fechaFin)
    {
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
    }

}