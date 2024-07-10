namespace Dtos.DepositoDtos;

public class DepositoConIdDto
{
    public int Id { get; set; }
    public String Nombre { get; set; }
    public String Area { get; set; }

    public String Tamanio { get; set; }

    public bool Climatizacion { get; set; }
    public List<PromocionSinIdDto> PromocionesDtos { get; set; }

    public List<RangoFechasDto> FechasDisponibilidad { get; set; }

    public DepositoConIdDto(int id,String nombre, String area, String tamanio,bool climatizacion, List<PromocionSinIdDto> promocionesIds,
        List<RangoFechasDto> fechasDisponibilidad)
    {
        Id = id;
        Nombre = nombre;
        Area = area;
        Tamanio = tamanio;
        Climatizacion = climatizacion;
        PromocionesDtos = promocionesIds;
        FechasDisponibilidad = fechasDisponibilidad;
    }
}