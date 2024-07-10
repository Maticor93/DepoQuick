using Dominio;
using Dominio.Enums;
using LogicaDeNegocio;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocioTest;
[TestClass]
public class UsuarioLogicaTest
{
    private UsuarioRepository _usuarioRepository;
    private EFContext _context;
    private UsuarioLogica _usuarioLogica;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _usuarioRepository = new UsuarioRepository(_context); 
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Agregar_Usuario_Test()
    {
        Usuario usuario = new Usuario()
        {
            Email = "usuario@gmail.com",
            NombreCompleto = "Usuario uno",
            Password = "Password1@",
            Rol = Rol.Cliente
        };

        Usuario usuarioAgregado = _usuarioLogica.Agregar(usuario);
        
        Assert.AreEqual(usuario,usuarioAgregado);
    }

    [TestMethod]
    public void Agregar_Usuario_Repetido_Deberia_Tirar_Excepcion()
    {
        Usuario usuario = new Usuario()
        {
            Email = "usuario@gmail.com",
            NombreCompleto = "Usuario uno",
            Password = "Password1@",
            Rol = Rol.Cliente
        };
        _usuarioLogica.Agregar(usuario);

        Assert.ThrowsException<UsuarioLogicaExcepcion>((() => _usuarioLogica.Agregar(usuario)));
    }

    [TestMethod]
    public void Actualizar_Un_Usuario_Test()
    {
        var emailOriginal = "correo1@direc.com";
        var nombreCompletooOriginal = "Nombre Apellido";
        Usuario usuario = new Usuario()
        {
            Email = emailOriginal,
            NombreCompleto = nombreCompletooOriginal,
            Password = "Password1@",
            Rol = Rol.Cliente
        };
        _usuarioLogica.Agregar(usuario);

        usuario.NombreCompleto = "Solo Nombre";
        Usuario usuarioActualizado = _usuarioLogica.Actualizar(usuario);
        
        Assert.AreNotEqual(nombreCompletooOriginal,usuarioActualizado.NombreCompleto);

    }

    [TestMethod]
    public void Eliminar_Un_Usuario_Test()
    {
        var email = "correo1@direc.com";
        Usuario usuario = new Usuario()
        {
            Email = email,
            NombreCompleto = "Nombre Apellido",
            Password = "Password1@",
            Rol = Rol.Cliente
        };
        _usuarioLogica.Agregar(usuario);

        _usuarioLogica.Eliminar(email);

        Assert.IsNull(_usuarioLogica.EncontrarPorEmail(email));

    }

    [TestMethod]
    public void Obtener_Todos_Los_Usuarios_Test()
    {
        List<Usuario> lista = _usuarioLogica.ObtenerTodos();
        
        Assert.IsNotNull(lista);
    }

    [TestMethod]
    public void Registrar_Mas_De_Un_Administrador_Deberia_Tirar_Excepcion()
    {
        Usuario administrador1 = new Usuario("correo1@direc.com","nombre","passWord1@,",Rol.Administrador);
        Usuario administrador2 = new Usuario("correo2@direc.com","nombre","passWord1@,",Rol.Administrador);

        _usuarioLogica.Agregar(administrador1);

        Assert.ThrowsException<UsuarioLogicaExcepcion>((() => _usuarioLogica.Agregar(administrador2)));

    }

    [TestMethod]
    public void Validar_Credenciales_De_Un_Usuario()
    {
        var email = "correo1@direc.com";
        var password = "passwordW1@";
        Usuario newUsuario = new Usuario(email, "nombre", password,Rol.Cliente);
        _usuarioLogica.Agregar(newUsuario);
        
        Assert.IsTrue(_usuarioLogica.ValidarCredenciales(email,password));
        
    }

    [TestMethod]
    public void Rechazar_Credenciales_De_Usuario_Inexistente()
    {
        var correoSinRegistrar = "correoNoRegistrado@direc.com";
        
        Assert.IsFalse(_usuarioLogica.ValidarCredenciales(correoSinRegistrar,"password"));
    }

    [TestMethod]
    public void Verificar_Si_Hay_Administrador_Test()
    {
        Usuario newUsuario = new Usuario("correo1@direc.com", "nombre", "passwordW1@",Rol.Administrador);
        
        _usuarioLogica.Agregar(newUsuario);
        
        Assert.IsTrue(_usuarioLogica.HayAdministrador());
    }

    [TestMethod]
    public void Verificar_Si_Hay_Administrador_Sin_Haber_Agregado_Uno()
    {
        Usuario newUsuario = new Usuario("correo1@direc.com", "nombre", "passwordW1@",Rol.Cliente);
        
        _usuarioLogica.Agregar(newUsuario);
        
        Assert.IsFalse(_usuarioLogica.HayAdministrador());
    }
}