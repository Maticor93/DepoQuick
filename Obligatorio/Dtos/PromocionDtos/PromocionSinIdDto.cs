namespace Dtos;

public class PromocionSinIdDto
{
    public String Etiqueta { get; set; }
    
    public int Descuento { get; set; }
    
    public DateTime Desde { get; set; }
    
    public DateTime Hasta { get; set; }


    public PromocionSinIdDto(String etiqueta, int descuento, DateTime desde, DateTime hasta)
    {
        Etiqueta = etiqueta;
        Descuento = descuento;
        Desde = desde;
        Hasta = hasta;
    }
}