namespace Repositories;

public interface IRepository <T, TKey>
{
    T Agregar(T element);

    T? Encontrar(Func<T, bool> func);

    T? Actualizar(T updateElement);
    
    void Eliminar(TKey id);
    
    List<T> ObtenerTodos();
}