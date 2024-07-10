using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class ReservaRepository : IRepository<Reserva,int>
{
    private EFContext _context;

    public ReservaRepository(EFContext context)
    {
        _context = context;
    }

    public Reserva Agregar(Reserva reserva)
    {
        _context.Reserva.Add(reserva);
        _context.SaveChanges();
        return reserva;
    }

    public Reserva Encontrar(Func<Reserva, bool> funcion)
    {
        return _context.Reserva
            .Include(r=>r.Cliente)
            .Include(r=>r.Deposito)
            .Include(r=>r.RangoFecha)
            .Where(funcion).FirstOrDefault();
    }

    public Reserva Actualizar(Reserva reservaActualizada)
    {
        var findReserva = Encontrar(r => r.Id == reservaActualizada.Id);
        findReserva = reservaActualizada;
        _context.Update(findReserva);
        _context.SaveChanges();
        return findReserva;
    }

    public void Eliminar(int id)
    {
        var entidad = Encontrar(r => r.Id == id);
        _context.Reserva.Remove(entidad);
        _context.SaveChanges();
    }

    public List<Reserva> ObtenerTodos()
    {
        return _context.Reserva
            .Include(r=>r.Cliente)
            .Include(r=>r.Deposito)
            .Include(r=>r.RangoFecha)
            .ToList();
    }
}