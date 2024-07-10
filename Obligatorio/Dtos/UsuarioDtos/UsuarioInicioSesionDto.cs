namespace Dtos.UsuarioDtos;

public class UsuarioInicioSesionDto
{
    public String Email { get; set; }
    public String Password { get; set; }

    public UsuarioInicioSesionDto(String email, String password)
    {
        Email = email;
        Password = password;
    }
}