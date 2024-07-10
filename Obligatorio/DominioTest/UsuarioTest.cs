using Dominio;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace DominioTest;
[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuarioTest()
    {
        Usuario newUsuario = new Usuario();
        
        Assert.IsNotNull(newUsuario);
    }
    
    
    [TestMethod]
    public void CrearUsuarioConEmailTest()
    {
        Usuario newUsuario = new Usuario();
        var email = "correoDeUsuario@direccion.com";
        newUsuario.Email = email;
        Assert.AreEqual(email,newUsuario.Email);
    }


    [TestMethod]
    public void IngresarEmailVacioExcepcion()
    {
        Usuario newUsuario = new Usuario();

        var email = "";
        
        Assert.ThrowsException<UsuarioExcepcion>(() => newUsuario.Email = email);

    }

    [TestMethod]
    public void IngresarEmailNoVacioSinFormatoExcepcion()
    {
        Usuario newUsuario = new Usuario();
        
        var email = "_emailSinFormato_";
        
        Assert.ThrowsException<UsuarioExcepcion>(() => newUsuario.Email = email);


    }

    [TestMethod]
    public void IngresarEmailNullException()
    {
        Usuario newUsuario = new Usuario();

        String email = null;

        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.Email = email));
    }

    [TestMethod]
    public void IngresarEmailConMasDe100CaracteresExcepcion()
    {
        Usuario newUsuario = new Usuario();
        
        String email = "correoDeMasDecienCaracterescorreoDeMasDecienCaracterescorreoDeMasDecienCaracterescorreoDeMasDecienCaracteres@direccion.com";

        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.Email = email));
    }
   

   
    [TestMethod]
    public void CrearUsuarioConNombreTest()
    {
        Usuario newUsuario = new Usuario();

        var nombreCompleto = "NombreUsuario";
        newUsuario.NombreCompleto = nombreCompleto;
        
        Assert.AreEqual(nombreCompleto,newUsuario.NombreCompleto);
    }

    [TestMethod]
    public void IngresarNombreVacioExcepcion()
    {
        Usuario newUsuario = new Usuario();

        var nombreCompleto = "";

        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.NombreCompleto = nombreCompleto));
    }

    [TestMethod]
    public void IngresarNombreNullExcepcion()
    {
        Usuario newUsuario = new Usuario();

        String nombreCompleto = null;
        
        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.NombreCompleto = nombreCompleto));

    }

    [TestMethod]
    public void IngresarNombreConNumerosExcepcion()
    {
        Usuario newUsuario = new Usuario();

        String nombreCompleto = "123124";

        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.NombreCompleto = nombreCompleto));
    }
    [TestMethod]
    public void IngresarNombreConMasDe100CaracteresExcepcion()
    {
        Usuario newUsuario = new Usuario();

        String nombreCompleto = "masdeciencaracteres masdeciencaracteres masdeciencaracteres masdeciencaracteres masdeciencaracteres masdeciencaracteres masdeciencaracteres masdeciencaracteres ";

        Assert.ThrowsException<UsuarioExcepcion>((() => newUsuario.NombreCompleto = nombreCompleto));
    }
    [TestMethod]
    public void Crear_Usuario_Usando_Constructor_Con_Parametros()
    {
        var email = "direccionDeCorreo@direccion.com";
        var nombreCompleto = "nombreUsuario ApellidoUsuario";

        Usuario newUsuario = new Usuario(email, nombreCompleto);
        
        Assert.IsNotNull(newUsuario);
    }

    [TestMethod]
    public void Crear_Usuario_Con_Password_Test()
    {
        var password = "Password1@";
        
        Usuario newUsuario = new Usuario("direccion@dominio.com", "Nombre Apellido", password);
        
        Assert.AreEqual(password,newUsuario.Password);
    }

    [TestMethod]
    public void Asignar_Password_Con_Menos_De_Ocho_Caracteres_Deberia_Tirar_Excepcion()
    {
        var password = "Wr0ng@";
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));
    }

    [TestMethod]
    public void Asignar_Dato_Null_A_Password_Debertia_Tirar_Excepcion()
    {
        String password = null;
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));

    }

    [TestMethod]
    public void Crear_Password_Sin_Caracter_Especial_Deberia_Tirar_Excepcion()
    {
        var password = "Wwwr0ng1";
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));

    }
    
    [TestMethod]
    public void Crear_Password_Sin_Mayuscula_Deberia_Tirar_Excepcion()
    {
        var password = "wwwr0ng1#";
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));

    }

    [TestMethod]
    public void Crear_Password_Sin_Minuscula_Deberia_Tirar_Excepcion()
    {
        var password = "WWWR0NG1#";
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));

    }

    [TestMethod]
    public void Crear_Password_Sin_Numeros_Deberia_Tirar_Excepcion()
    {
        var password = "Wronggg#";
        
        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));
    }

    [TestMethod]
    public void Asignar_Password_Con_Espacios_Sin_Ocho_Caracteres_Deberia_Tirar_Excepcion()
    {
        var password = "O k # 1 ";

        Assert.ThrowsException<UsuarioExcepcion>((() => new Usuario("direccion@dominio.com", "Nombre Apellido", password)));
        
    }

    [TestMethod]
    public void Asignar_Rol_Administrador_A_Usuario()
    {
        var rol = Rol.Administrador;
        var newUsuario = new Usuario("direccion@dominio.com", "Nombre Apellido", "Password1@",rol);
        
        Assert.AreEqual(rol,newUsuario.Rol);
    }
    
    [TestMethod]
    public void Asignar_Rol_Cliente_A_Usuario()
    {
        var rol = Rol.Cliente;
        var newUsuario = new Usuario("direccion@dominio.com", "Nombre Apellido", "Password1@",rol);
        
        Assert.AreEqual(rol,newUsuario.Rol);
    }
    
    
}
