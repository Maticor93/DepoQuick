namespace Dtos.DepositoDtos;

public class DepositoSinIdDto
{
    public String Nombre { get; set; }
    public String Area { get; set; }

    public String Tamanio { get; set; }

    public bool Climatizacion { get; set; }

    public DepositoSinIdDto(String nombre, String area, String tamanio,bool climatizacion)
    {
        Nombre = nombre;
        Area = area;
        Tamanio = tamanio;
        Climatizacion = climatizacion;
    }
}