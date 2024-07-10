namespace Dtos.UsuarioDtos;

public class UsuarioRegistroDto
{
    public String NombreCompleto { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }

    public UsuarioRegistroDto(String nombreCompleto, String email, String password)
    {
        NombreCompleto = nombreCompleto;
        Email = email;
        Password = password;
    }
}