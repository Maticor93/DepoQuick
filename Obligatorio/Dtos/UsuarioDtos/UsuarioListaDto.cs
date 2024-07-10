namespace Dtos.UsuarioDtos;

public class UsuarioListaDto
{
    public String NombreCompleto { get; set; }
    public String Email { get; set; }
    
    public UsuarioListaDto(String nombreCompleto, String email)
    {
        NombreCompleto = nombreCompleto;
        Email = email;
    }
}