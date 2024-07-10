using System.Collections;
using Dominio;
using Dominio.Enums;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;
namespace LogicaDeNegocio;

public class DepositoLogica
{
    private readonly IRepository<Deposito,int> _repository;
    public DepositoLogica(IRepository<Deposito,int> depositoMemoryRepository)
    {
        _repository = depositoMemoryRepository;
    }
    
    public Deposito? Agregar(Deposito deposito)
    {
        Deposito findDeposito = EncontrarPorId(deposito.Id);
        if (findDeposito != null) throw new DepositoLogicaExcepcion("No se puede agregar un mismo deposito dos veces");
        
        return _repository.Agregar(deposito);
    }

    public Deposito? EncontrarPorId(int id)
    {
        return _repository.Encontrar(d => d.Id == id);
    }

    public Deposito Actualizar(Deposito depositoActualizado)
    {
        return _repository.Actualizar(depositoActualizado);
    }

    public void Eliminar(int idDeposito)
    {
        var deposito = EncontrarPorId(idDeposito);
        DesmarcarLasPromocionesUsadas(deposito);
        
        _repository.Eliminar(idDeposito);
    }

    private void DesmarcarLasPromocionesUsadas(Deposito deposito)
    {
        foreach (var promocion in deposito.Promociones)
        {
            promocion.Depositos.Remove(deposito);
        }
    }

    public List<Deposito> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public List<Deposito> ObtenerDepositosAsociadosA(int promocionId)
    {
        var depositosAsociadosAPromocion = new List<Deposito>();
        var listaDepositos = ObtenerTodos();

        foreach (var deposito in listaDepositos)
        {
            foreach (var promocion in deposito.Promociones)
            {
                if (promocion.Id == promocionId)
                    depositosAsociadosAPromocion.Add(deposito);
            }
        }
        return depositosAsociadosAPromocion;
    }

    public void EliminarDepositoEnCasoDeNoEstarReservado(int idDeposito, List<Reserva> reservasDelDeposito)
    {
        bool canceloEliminar = false;
        foreach (var reserva in reservasDelDeposito)
            if (reserva.ConfAdmin != Estado.Rechazada && reserva.RangoFecha.FechaFin >= DateTime.Today)
                canceloEliminar = true;

        if (canceloEliminar)
            throw new DepositoLogicaExcepcion(
                $"El deposito de Id #{idDeposito} esta reservado, no puede ser eliminada");
        
        foreach (var reserva in reservasDelDeposito)
        {
            reserva.Deposito = null;
        }  
        
        Eliminar(idDeposito);
    }

    public List<Deposito> ObtenerTodosDisponiblesEn(DateTime desde, DateTime hasta)
    {
        List<Deposito> depositos = new List<Deposito>();
        var rangoFechas = new RangoFechas(desde, hasta);
        foreach (var deposito in ObtenerTodos())
        {
            foreach (var disponibilidad in deposito.Disponibilidad)
            {
                if (disponibilidad.ContenidaEnRango(rangoFechas))
                {
                    depositos.Add(deposito);
                    break;
                }
                
            }
        }

        return depositos;
    }
}