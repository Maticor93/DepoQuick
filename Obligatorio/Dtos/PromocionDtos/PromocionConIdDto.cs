namespace Dtos;

public class PromocionConIdDto
{
    public int Id { get; set; }
    public String Etiqueta { get; set; }
    
    public int Descuento { get; set; }
    
    public DateTime Desde { get; set; }
    
    public DateTime Hasta { get; set; }


    public PromocionConIdDto(int id,String etiqueta, int descuento, DateTime desde, DateTime hasta)
    {
        Id = id;
        Etiqueta = etiqueta;
        Descuento = descuento;
        Desde = desde;
        Hasta = hasta;
    }
}