using Dominio;

namespace Repositories;

public class UsuarioRepository : IRepository<Usuario,String>
{
    private EFContext _context;

    public UsuarioRepository(EFContext context)
    {
        _context = context;
    }
    public Usuario Agregar(Usuario usuario)
    {
        _context.Usuario.Add(usuario);
        _context.SaveChanges();
        return usuario;
    }

    public Usuario Encontrar(Func<Usuario, bool> funcion)
    {
        return _context.Usuario.Where(funcion).FirstOrDefault();
    }

    public Usuario Actualizar(Usuario usuarioActualizado)
    {
        var findUsuario = Encontrar(u => u.Email == usuarioActualizado.Email);
        findUsuario = usuarioActualizado;
        return findUsuario;
    }
    public void Eliminar(String email)
    {
        var entidad = Encontrar(u => u.Email == email);
        _context.Usuario.Remove(entidad);
        _context.SaveChanges();
    }

    public List<Usuario> ObtenerTodos()
    {
        return _context.Usuario.ToList();
    }
}