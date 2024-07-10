namespace Dtos.DepositoDtos;

public class DepositoAltaDto
{
    public String Nombre { get; set; }
    public String Area { get; set; }

    public String Tamanio { get; set; }

    public bool Climatizacion { get; set; }
    public List<int> PromocionesIds { get; set; }

    public List<RangoFechasDto> FechasDisponibilidad { get; set; }

    public DepositoAltaDto(String nombre, String area, String tamanio,bool climatizacion, List<int> promocionesIds,
        List<RangoFechasDto> fechasDisponibilidad)
    {
        Nombre = nombre;
        Area = area;
        Tamanio = tamanio;
        Climatizacion = climatizacion;
        PromocionesIds = promocionesIds;
        FechasDisponibilidad = fechasDisponibilidad;
    }

}