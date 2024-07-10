using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class PromocionRepository : IRepository<Promocion,int>
{
    private EFContext _context;

    public PromocionRepository(EFContext context)
    {
        _context = context;
    }
    
    public Promocion Agregar(Promocion promocion)
    {
        _context.Promocion.Add(promocion);
        _context.SaveChanges();
        return promocion;
    }

    public Promocion Encontrar(Func<Promocion, bool> funcion)
    {
        return _context.Promocion
            .Include(p=>p.Depositos)
            .Include(p=>p.RangoFecha)
            .Where(funcion).FirstOrDefault();
    }

    public Promocion Actualizar(Promocion promocionActualizada)
    {
        _context.Update<Promocion>(promocionActualizada);
        _context.SaveChanges();
        return promocionActualizada;
    }

    public void Eliminar(int id)
    {
        var entidad = Encontrar(p => p.Id == id);
        var rangoFechasDeEntidad = entidad.RangoFecha;
        _context.Promocion.Remove(entidad);
        _context.RangoFechas.Remove(rangoFechasDeEntidad);
        _context.SaveChanges();
    }

    public List<Promocion> ObtenerTodos()
    {
        return _context.Promocion
            .Include(p=>p.Depositos)
            .Include(p=>p.RangoFecha)
            .ToList();
    }
}