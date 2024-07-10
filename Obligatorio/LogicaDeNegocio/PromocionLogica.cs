using Dominio;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocio;

public class PromocionLogica
{
    private readonly IRepository<Promocion,int> _repository;

    public PromocionLogica(IRepository<Promocion,int> repository)
    {
        _repository = repository;
    }
    
    public Promocion Agregar(Promocion promocion)
    {
        var findPromocion = EncontrarPorId(promocion.Id);
        if (findPromocion != null)
            throw new PromocionLogicaExcepcion("No se puede agregar una misma promocion dos veces");
        
        return _repository.Agregar(promocion);
    }

    public Promocion EncontrarPorId(int id)
    {
        return _repository.Encontrar(p => p.Id == id);
    }

    public Promocion Actualizar(Promocion promocionActualizada)
    {
        return _repository.Actualizar(promocionActualizada);
    }

    public void Eliminar(int id)
    {
        Promocion promocion = EncontrarPorId(id);
        
        if (promocion.Depositos.Count() != 0)
            throw new PromocionLogicaExcepcion($"La promocion de Id #{id} esta en uso por un deposito, no puede ser eliminada");
        
        _repository.Eliminar(id);
    }

    public List<Promocion> ObtenerTodas()
    {
        return _repository.ObtenerTodos();
    }
}