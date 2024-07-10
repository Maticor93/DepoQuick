using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace Dominio;

public class Usuario
{
    private String? _email;
    private String? _nombreCompleto;
    private String? _password;
    //Regex para correo, verifica que no haya espacios, que haya un simbolo "@", un dominio y una direccion.
    private static readonly Regex RegexCorreo = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    //Regex para el nombre y apellido, permite espacios, maximo 100 caracteres y no permite caracteres especiales ni numeros. 
    private static readonly Regex RegexNombreCompleto = new Regex(@"^(?=.{1,100}$)[a-zA-ZÀ-ÿ][a-zA-ZÀ-ÿ\s]*$");
    //Regex para la password, verifica si hay al menos una mayuscula, una minuscula, un numero y uno de estos caracteres especiales [ "@" , "#", "$", "." , "," ] 
    private static readonly Regex RegexPassword = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$.,])");
    private Match? _matchCorreo;
    private Match? _matchNombreCompleto;
    private Match? _matchPassword;
    private const int MAXIMO_DE_CARACTERES_EMAIL = 100;
    private const int MINIMO_DE_CARACTERES_PASSWORD = 8;

   
    public String? NombreCompleto 
    { 
        get { return _nombreCompleto; }
        set
        {
            if (value == null) 
                throw new UsuarioExcepcion("El nombre no puede ser null");
            
            ValidarNombreCompleto(value);
            _nombreCompleto = value; 
        } 
    }
    [Key]
    public String? Email
    {
        get { return _email; }
        set
        {
            if (value == null) 
                throw new UsuarioExcepcion("El email no puede ser null");
            
            ValidarEmail(value);
            _email = value; 
        }
    }

    public String Password
    {
        get
        {
            return _password;
        }
        set
        {
            if (value == null)
                throw new UsuarioExcepcion("La contraseña no puede ser null");
            
            ValidarPassword(value);
            _password = value;
        }
    }
    
    public Rol Rol { get; set; }

    public Usuario()
    {
       
    }
    
    public Usuario(String email, String nombreCompleto) 
    {
        Email = email;
        NombreCompleto = nombreCompleto;
    }

    public Usuario(String email, String nombreCompleto, String password):this(email,nombreCompleto)
    {
        Password = password;
    }
    public Usuario(String email, String nombreCompleto, String password,Rol rol):this(email,nombreCompleto,password)
    {
        Rol = rol;
    }

    private void ValidarEmail(String email)
    {
        _matchCorreo = RegexCorreo.Match(email);
        
        if(email.Length > MAXIMO_DE_CARACTERES_EMAIL) throw new UsuarioExcepcion("El email no puede tener mas de 100 caracteres");
        if (email == "") throw new UsuarioExcepcion("El email no puede ser vacío");
        if (!_matchCorreo.Success) throw new UsuarioExcepcion("Formato de email no valido");
    }

    private void ValidarNombreCompleto(String nombre)
    {
       _matchNombreCompleto = RegexNombreCompleto.Match(nombre);
        
        if (nombre == "") throw new UsuarioExcepcion("El nombre no puede ser vacio");
        if (!_matchNombreCompleto.Success) throw new UsuarioExcepcion("El nombre no puede contener numeros");
    }

    private void ValidarPassword(String password)
    {
        var passwordSinEspacios = password.Replace(" ", "");
        if (passwordSinEspacios.Length < MINIMO_DE_CARACTERES_PASSWORD)
            throw new UsuarioExcepcion("La contraseña debe tener como minimo 8 caracteres");

        _matchPassword = RegexPassword.Match(password);
        if(!_matchPassword.Success)
            throw new UsuarioExcepcion("La contraseña debe contener al menos una letra mayúscula, una letra minúscula, un dígito y un caracter especial");
    }
}