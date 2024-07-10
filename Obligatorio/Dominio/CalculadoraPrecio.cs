using Dominio.Enums;

namespace Dominio;
public class CalculadoraPrecio
{
    private const double PRECIO_POR_DIA_DEPOSITO_PEQUENIO = 50;
    private const double PRECIO_POR_DIA_DEPOSITO_MEDIANO= 75;
    private const double PRECIO_POR_DIA_DEPOSITO_GRANDE = 100;
    private const double MINIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MINIMO = 7;
    private const double MAXIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MINIMO = 14;
    private const double MINIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MAXIMO = 15;
    private const double COSTO_ADICIONAL_POR_DIA_AL_TENER_CLIMATIZACION = 20;
    private const double DESCUENTO_MINIMO = 5;
    private const double DESCUENTO_MAXIMO = 10;
    public double Precio { get; set; }
    
    private RangoFechas RangoFecha { get; set; }
    
    private Deposito Deposito { get; set; }

    public CalculadoraPrecio(RangoFechas rango, Deposito deposito)
    {
        RangoFecha = rango;
        Deposito = deposito;
        Precio = CalcularPrecio();
    }
    
     public double CalcularPrecio()
    {
        var dias = DarCantidadDeDiasDeReserva();
        double precioPorDia = PrecioPorDiaSegunTamanio();
        precioPorDia = DarPrecioPorDiaAgregandoCostoPorClimatizacionSiCorresponde(precioPorDia);

        Precio = precioPorDia * dias;

        AplicarDescuentoPorDuracion(dias);
        AplicarDescuentoDePromocionesSiLasHay();

        SacarDecimalesAPrecio();

        return Precio;
    }
    

    private double PrecioPorDiaSegunTamanio()
    {
        switch (Deposito.Tamanio)
        {
            case Tamanio.Grande:
                return PRECIO_POR_DIA_DEPOSITO_GRANDE;
            case Tamanio.Pequeño:
                return PRECIO_POR_DIA_DEPOSITO_PEQUENIO;
            default:
                return PRECIO_POR_DIA_DEPOSITO_MEDIANO;
        }
    }

    private double DarPrecioPorDiaAgregandoCostoPorClimatizacionSiCorresponde(double precioPorDia)
    {
        if (Deposito.Climatizacion) 
            precioPorDia += COSTO_ADICIONAL_POR_DIA_AL_TENER_CLIMATIZACION;
        return precioPorDia;
    }

    private void AplicarDescuentoPorDuracion(int duracion)
    {
        AplicarDescuentoMinimoSiLaDuracionEsMayorALaNecesaria(duracion);
        AplicarDescuentoMaximoSiLaDuracionEsMayorALaNecesaria(duracion);
    }

    private void AplicarDescuentoMinimoSiLaDuracionEsMayorALaNecesaria(int duracion)
    {
        if (duracion >= MINIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MINIMO && duracion <= MAXIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MINIMO)
            AplicarDescuentoAPrecio(DESCUENTO_MINIMO);
    }
    
    private void AplicarDescuentoMaximoSiLaDuracionEsMayorALaNecesaria(int duracion)
    {
        if (duracion >= MINIMO_DE_DIAS_PARA_OBTENER_DESCUENTO_MAXIMO)
            AplicarDescuentoAPrecio(DESCUENTO_MAXIMO);
    }

    private void AplicarDescuentoDePromocionesSiLasHay()
    {
        foreach (var promocion in Deposito.Promociones)
        {   
            RangoFechas? rangoPromo = promocion.RangoFecha;
            if (rangoPromo.PerteneceAlRango(DateTime.Today))
                AplicarDescuentoAPrecio(promocion.Descuento);
        }
    }

    private void AplicarDescuentoAPrecio(double descuento)
    {
        Precio -= (Precio * descuento) / 100;
    }
    private void SacarDecimalesAPrecio()
    {
        Precio = Precio = Math.Round(Precio);
    }
    
    public int DarCantidadDeDiasDeReserva()
    {
        DateTime fechaInicio = RangoFecha.FechaInicio;
        DateTime fechaFin = RangoFecha.FechaFin;
        var diferenciaDias = fechaFin - fechaInicio;
        return (int)diferenciaDias.TotalDays;
    }
}