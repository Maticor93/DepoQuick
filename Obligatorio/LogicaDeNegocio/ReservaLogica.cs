using System.IO.Enumeration;
using System.Security.Cryptography;
using Dominio;
using Dominio.Enums;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocio;

public class ReservaLogica
{

    private readonly IRepository<Reserva,int> _repository;

    public ReservaLogica(IRepository<Reserva,int> repository)
    {
        _repository = repository;
    }
    
    public Reserva Agregar(Reserva reserva)
    {
        VerificarSiReservaExiste(reserva);

        return _repository.Agregar(reserva);
    }

    private void VerificarSiReservaExiste(Reserva reserva)
    {
        var reservaExistente = _repository.Encontrar(r => r.Id == reserva.Id);
        if (reservaExistente != null)
        {
            throw new ReservaLogicaExcepcion($"La reserva con Id {reserva.Id} ya existe.");
        }
    }

    public Reserva EncontrarPorId(int id)
    {
        return _repository.Encontrar(r => r.Id == id);
    }

    public Reserva Actualizar(Reserva reserva)
    {
        return _repository.Actualizar(reserva);
    }

    public void Eliminar(int id)
    {
        _repository.Eliminar(id);
    }

    public List<Reserva> ObtenerTodas()
    {
        return _repository.ObtenerTodos();
    }

    public List<Reserva> ObtenerReservasDe(string? clienteEmail)
    {
        List<Reserva> reservasDeCliente = new List<Reserva>();
        var listaReservas = ObtenerTodas();

        foreach (var reserva in listaReservas)
        {
            if (reserva.Cliente.Email == clienteEmail)
                reservasDeCliente.Add(reserva);
        }

        return reservasDeCliente;
    }

    public List<Reserva> ObtenerReservasAsociadasA(int depositoId)
    {
        List<Reserva> reservasAsociaciadaADeposito = new List<Reserva>();
        var reservas = ObtenerTodas();

        foreach (var reserva in reservas)
        {
            if (reserva.Deposito != null && reserva.Deposito.Id == depositoId)
                reservasAsociaciadaADeposito.Add(reserva);
        }
        return reservasAsociaciadaADeposito;
    }

    public void PagarReserva(int reservaId)
    {
        var reserva = EncontrarPorId(reservaId);
        if (reserva != null && reserva.RangoFecha.FechaInicio < DateTime.Today)
        {
            Eliminar(reservaId);
            throw new ReservaLogicaExcepcion("La reserva no se puede pagar porque la fecha reservada es anterior a hoy. La reserva fue eliminada");
        }
            
        reserva.Pago = Pago.Reservado;
        Actualizar(reserva);
    }

    public void CapturarPago(int reservaId)
    {
        var reservaACapturar = EncontrarPorId(reservaId);
        foreach (var reserva in ObtenerReservasAsociadasA(reservaACapturar.Deposito.Id))
        {
            if (reserva.Id == reservaACapturar.Id) continue;
            bool fechaInicioDentroDeRango = reservaACapturar.RangoFecha.FechaInicio >= reserva.RangoFecha.FechaInicio &&
                                            reservaACapturar.RangoFecha.FechaInicio <= reserva.RangoFecha.FechaFin;
                
            bool fechaFinDentroDeRango = reservaACapturar.RangoFecha.FechaFin >= reserva.RangoFecha.FechaInicio &&
                                         reservaACapturar.RangoFecha.FechaFin <= reserva.RangoFecha.FechaFin;
            
            bool fechaSobreRango = reservaACapturar.RangoFecha.FechaInicio <= reserva.RangoFecha.FechaInicio &&
                                   reservaACapturar.RangoFecha.FechaFin >= reserva.RangoFecha.FechaFin;

            if ((fechaInicioDentroDeRango || fechaFinDentroDeRango || fechaSobreRango) && reserva.ConfAdmin == Estado.Aprobada)
            {
                reservaACapturar.Pago = Pago.Cancelado;
                reservaACapturar.ConfAdmin = Estado.Rechazada;
                reservaACapturar.Comentario = $"La reserva superpone las fechas de la reserva de Id #{reserva.Id}";
                Actualizar(reservaACapturar);
                throw new ReservaLogicaExcepcion($"La reserva se superpone con las fechas de otra reserva de Id #{reserva.Id}");
            }
        }

        reservaACapturar.ConfAdmin = Estado.Aprobada;
        reservaACapturar.Pago = Pago.Capturado;
        Actualizar(reservaACapturar);
        RechazarReservasDelMismoDepositoQueCoincidanConLaReservaActual(reservaACapturar);
    }

    private void RechazarReservasDelMismoDepositoQueCoincidanConLaReservaActual(Reserva reserva)
    {
        foreach (var reservaAEvaluar in ObtenerReservasAsociadasA(reserva.Deposito.Id))
        {
            if(reservaAEvaluar.Id == reserva.Id) continue;
            bool fechaInicioDentroDeRango = reservaAEvaluar.RangoFecha.FechaInicio >= reserva.RangoFecha.FechaInicio &&
                                            reservaAEvaluar.RangoFecha.FechaInicio <= reserva.RangoFecha.FechaFin;
                
            bool fechaFinDentroDeRango = reservaAEvaluar.RangoFecha.FechaFin >= reserva.RangoFecha.FechaInicio &&
                                         reservaAEvaluar.RangoFecha.FechaFin <= reserva.RangoFecha.FechaFin;
            
            bool fechaSobreRango = reservaAEvaluar.RangoFecha.FechaInicio <= reserva.RangoFecha.FechaInicio &&
                                         reservaAEvaluar.RangoFecha.FechaFin >= reserva.RangoFecha.FechaFin;

            if ((fechaInicioDentroDeRango || fechaFinDentroDeRango || fechaSobreRango) && reservaAEvaluar.ConfAdmin == Estado.Pendiente)
            {
                reservaAEvaluar.Pago = Pago.Cancelado;
                reservaAEvaluar.Comentario = "El rango de fechas seleccionado ya fue capturado en otra reserva";
                reservaAEvaluar.ConfAdmin = Estado.Rechazada;
                Actualizar(reservaAEvaluar);
            }
        }
    }

    public void RechazarReserva(int idReserva, string comentario)
    {
        Reserva reserva = EncontrarPorId(idReserva);
        reserva.ConfAdmin = Estado.Rechazada;
        reserva.Pago = Pago.Cancelado;
        reserva.Comentario = comentario;
        Actualizar(reserva);
    }

    public bool DepositoTieneReservaAprobadaEnRangoFecha(Deposito deposito,DateTime desde, DateTime hasta)
    {
        bool tiene = false;
        foreach (var reserva in ObtenerReservasAsociadasA(deposito.Id))
        {
            var rangoFechas = new RangoFechas(desde, hasta);
            if ( reserva.ConfAdmin == Estado.Aprobada && (reserva.RangoFecha.ContenidaEnRango(rangoFechas) || (reserva.RangoFecha.PerteneceAlRango(desde) || reserva.RangoFecha.PerteneceAlRango(hasta) || rangoFechas.ContenidaEnRango(reserva.RangoFecha))))
                tiene = true;
        }
        
        return tiene;
    }

    public double CalcularPrecioReserva(Deposito? deposito, DateTime desde, DateTime hasta)
    {
        var rangoFechas = new RangoFechas(desde, hasta);
        var calculadora = new CalculadoraPrecio(rangoFechas, deposito);

        return calculadora.CalcularPrecio();
    }
}