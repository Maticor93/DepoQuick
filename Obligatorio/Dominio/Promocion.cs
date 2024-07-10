using Dominio.ExcepcionesDominio;

namespace Dominio;

public class Promocion
{
    private string _etiqueta;
    private int _descuento;
    private RangoFechas? _rangoFecha;
    const int MAXIMO_DE_CARACTERES_ETIQUETA = 20;
    const int MAXIMO_DESCUENTO = 75;
    const int MINIMO_DESCUENTO = 5;

   
    
    public int Id { get; set; }

    public string Etiqueta
    {
        get { return _etiqueta; }
        set {
            if (string.IsNullOrEmpty(value)) throw new PromocionExcepcion("La etiqueta no puede estar vacia");
            else
            {
                ValidarEtiqueta(value);
                _etiqueta = value;
            }
        }
    }

    public int Descuento
    {
        get { return _descuento; }
        set
        {
            ValidarPromocion(value);
            _descuento = value;
        }
    }
    

    public virtual RangoFechas? RangoFecha 
    {
        get
        {
            return _rangoFecha; 
            
        }
        set
        {
            if (value == null) throw new PromocionExcepcion("Debe asignarse un rango de fechas a la promocion");
            _rangoFecha = value;
            
            
        } 
    }
    
    public virtual IList<Deposito> Depositos { get; set; }
    
    public Promocion()
    {
        Depositos = new List<Deposito>();
        RangoFecha = new RangoFechas();
    }
    
    public Promocion(string etiqueta, int descuento, RangoFechas? rango) : this()
    {
        Etiqueta = etiqueta;
        Descuento = descuento;
        RangoFecha = rango;
    }

    private static void ValidarPromocion(int value)
    {
        if (value < MINIMO_DESCUENTO) throw new PromocionExcepcion($"El porcentaje de descuento no puede ser menor a {MINIMO_DESCUENTO}");
        if (value > MAXIMO_DESCUENTO) throw new PromocionExcepcion($"El porcentaje de descuento no puede ser mayor a {MAXIMO_DESCUENTO}");
    }

    private static void ValidarEtiqueta(string value)
    {
        if (value.Trim() == "")
            throw new PromocionExcepcion("La etiqueta no puede ser vacía");
        if (value.Length > MAXIMO_DE_CARACTERES_ETIQUETA)
            throw new PromocionExcepcion($"La etiqueta no puede tener mas de {MAXIMO_DE_CARACTERES_ETIQUETA} caracteres");
    }
}