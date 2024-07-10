using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class DepositoRepository: IRepository<Deposito,int>
{
    private EFContext _context;

    public DepositoRepository(EFContext context)
    {
        _context = context;
    }
    public Deposito Agregar(Deposito deposito)

    {
        _context.Deposito.Add(deposito);
        _context.SaveChanges();
        return deposito;
    }
    
    public Deposito? Encontrar(Func<Deposito, bool> funcion)
    {
        return _context.Deposito
            .Include(d=>d.Promociones)
            .Include(d=>d.Disponibilidad)
            .Where(funcion).FirstOrDefault();
    }

    public Deposito? Actualizar(Deposito depositoActualizado)
    {
        Deposito findDeposito = Encontrar(d => d.Id == depositoActualizado.Id);
        findDeposito = depositoActualizado;
        _context.SaveChanges();
        return findDeposito;
    }

    public void Eliminar(int depositoId)
    {
        var entidad = Encontrar(d => d.Id == depositoId);
        foreach (var entidadRangoFecha in entidad.Disponibilidad)
        {
            _context.RangoFechas.Remove(entidadRangoFecha);
        }
        _context.Deposito.Remove(entidad);
        _context.SaveChanges();
    }

    public List<Deposito> ObtenerTodos()
    {
        return _context.Deposito
            .Include(d=>d.Promociones)
            .Include(d=>d.Disponibilidad)
            .ToList();
    }
}