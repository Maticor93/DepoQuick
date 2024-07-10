using System.Reflection.Metadata.Ecma335;
using Dominio;
using Dominio.Enums;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocio;

public class UsuarioLogica
{
    private readonly IRepository<Usuario,String> _repository;
    public UsuarioLogica(IRepository<Usuario,String> repository)
    {
        _repository = repository;
    }
    public Usuario Agregar(Usuario usuario)
    {
        TirarExcepcionSiEsAdministradorYaHabiendoUno(usuario);
        
        var findUsuario = EncontrarPorEmail(usuario.Email);
        if (findUsuario != null) throw new UsuarioLogicaExcepcion("No se puede registrar un mismo usuario dos veces");
        return _repository.Agregar(usuario);
    }

    private void TirarExcepcionSiEsAdministradorYaHabiendoUno(Usuario usuario)
    {
        if (HayAdministrador() && usuario.Rol == Rol.Administrador)
            throw new UsuarioLogicaExcepcion("No se puede registrar más de un administrador");
    }
    public bool HayAdministrador()
    {
        foreach (var usuario in ObtenerTodos())
        {
            if (usuario.Rol == Rol.Administrador)
                return true;
        }
        
        return false;
    }

    public Usuario EncontrarPorEmail(String email)
    {
        return _repository.Encontrar(u=>u.Email == email);
    }

    public Usuario Actualizar(Usuario usuarioActualizado)
    {
        return _repository.Actualizar(usuarioActualizado);
    }

    public void Eliminar(string email)
    {
        _repository.Eliminar(email);
    }

    public List<Usuario> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public bool ValidarCredenciales(String email, String password)
    {
        Usuario usuario = EncontrarPorEmail(email);
        
        if (usuario == null) return false;
        
        return usuario.Password == password;
    }

    public bool UsuarioEsAdministrador(String emailUsuario)
    {
        var usuario = EncontrarPorEmail(emailUsuario);
        return usuario.Rol == Rol.Administrador;
    }

    
}