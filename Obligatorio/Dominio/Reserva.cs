using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace Dominio;

public class Reserva
{
    private String _comentario;
    private Usuario? _cliente;
    private Deposito? _deposito;
    private RangoFechas _rangoFecha;
    private static CalculadoraPrecio _calculadoraPrecio;
    private const double MAXIMO_CARACTERES_EN_COMENTARIO = 300;
    
    
    public int Id { get; set; }

    public Estado ConfAdmin { get; set; }

    public virtual Deposito? Deposito
    {
        get
        {
            return _deposito;
        }
        set
        {
            _deposito = value;
        }
    }

    public virtual Usuario? Cliente
    {
        get
        {
            return _cliente;
        }
        set
        {
            if (value == null)
                throw new ReservaExcepcion("Debe asignarse un Cliente a la reserva");
            _cliente = value;
        }
    }

    public String Comentario 
    {
        get
        {
            return _comentario; 
            
        }
        set
        {
            ValidarComentario(value);
            _comentario = value; 
            
        } 
    }
    
    public virtual RangoFechas RangoFecha 
    {
        get
        {
            return _rangoFecha; 
            
        }
        set
        {
            if (value == null) throw new ReservaExcepcion("Debe asignarse un rango de fechas a la reserva");
            ValidarRangoReservaNoSeaCero(value.FechaInicio, value.FechaFin);
            _rangoFecha = value;
            
        } 
    }

    public double Precio { get; set; }
    public Pago? Pago { get; set; }

    public Reserva()
    {
        ConfAdmin = Estado.Pendiente;
        RangoFecha = new RangoFechas();
    }
    
    public Reserva(RangoFechas rango) : this()
    {
        RangoFecha = rango;
    }

    public Reserva(RangoFechas rango, Deposito deposito, Usuario cliente) : this(rango)
    {
        Deposito = deposito;
        Cliente = cliente;
        ValidarRangoReservaPertenceAUnRangoDeDeposito(rango);
        _calculadoraPrecio = new CalculadoraPrecio(rango, deposito);
        Precio = _calculadoraPrecio.Precio;
    }
    
    private void ValidarComentario(String comentario)
    {
        if (comentario.Length > MAXIMO_CARACTERES_EN_COMENTARIO) 
            throw new ReservaExcepcion("El comentario no puede tener mas de 300 caracteres");
        if (ConfAdmin == Estado.Rechazada && comentario == "") 
            throw new ReservaExcepcion("El comentario no puede ser vacio");
    }
    private void ValidarRangoReservaNoSeaCero(DateTime? desde, DateTime? hasta)
    {
        if (desde != null && hasta != null)
        {
            if (desde == hasta)
                throw new ReservaExcepcion("La reserva no puede ser solo un día");
        }
    }

    private void ValidarRangoReservaPertenceAUnRangoDeDeposito(RangoFechas rango)
    {
        bool pertenceAUno = false;
        foreach (var rangoFecha in Deposito.Disponibilidad)
        {
            if (rangoFecha.ContenidaEnRango(rango))
            {
                pertenceAUno = true;
                break;
            }
        }
        if (!pertenceAUno)
            throw new ReservaExcepcion(
                $"El rango de fechas {rango.FechaInicio}-{rango.FechaFin} no está disponible para el depósito seleccionado");
    }
}