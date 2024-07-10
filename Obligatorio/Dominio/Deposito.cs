using System.Text.RegularExpressions;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace Dominio;

public class Deposito
{
    private String _nombre;
    private static readonly Regex RegexNombre = new Regex(@"^[\p{L}\s]+$");
    private Match? _matchNombre;
    public int Id { get; set; }

    public String? Nombre
    {
        get
        {
            return _nombre;
        }
        set
        {
            ValidarNombre(value);
            _nombre = value;
        }
    }

    public Area Area { get; set; }
    
    public Tamanio Tamanio { get; set; }
    
    public bool Climatizacion { get; set; }
    
     
    public virtual IList<RangoFechas> Disponibilidad { get; set; }
    
    public virtual IList<Promocion>? Promociones { get; set; }
    
    public Deposito()
    {
        Promociones = new List<Promocion>();
        Disponibilidad = new List<RangoFechas>();
    }

    public Deposito(Area area, Tamanio tamanio, bool climatizacion):this()
    {
        Area = area;
        Tamanio = tamanio;
        Climatizacion = climatizacion;
    }

    public Deposito(Area area, Tamanio tamanio, bool climatizacion, List<Promocion> promociones) : this(area, tamanio, climatizacion)
    {
        Promociones = promociones;
    }
    public Deposito(String nombre,Area area, Tamanio tamanio, bool climatizacion, List<Promocion> promociones) : this(area, tamanio, climatizacion,promociones)
    {
        Nombre = nombre;
    }
    public Deposito(String nombre,Area area, Tamanio tamanio, bool climatizacion, List<Promocion> promociones,List<RangoFechas> disponibilidad) : this(nombre,area, tamanio, climatizacion,promociones)
    {
        Disponibilidad = disponibilidad;
    }

    private void ValidarNombre(String? nombre)
    {
        nombre = nombre?.Trim();
        if (nombre == "" || nombre == null)
            throw new DepositoExcepcion("El nombre del depósito no puede ser vacio");
        
        _matchNombre = RegexNombre.Match(nombre);
        if (!_matchNombre.Success)
            throw new DepositoExcepcion("El nombre del deposito no puede tener numeros ni caracteres especiales");
    }

    public void AgregarRangoFecha(RangoFechas? nuevoRango)
    {
        if (nuevoRango == null) throw new DepositoExcepcion("El rango de fechas no puede ser nulo");
        foreach (var rangoExistente in Disponibilidad)
        {
            if(rangoExistente.ContenidaEnRango(nuevoRango))
                throw new DepositoExcepcion("La fecha está incluida en el rango de otra fecha de disponibilidad");
        }
        Disponibilidad.Add(nuevoRango);
    }
}