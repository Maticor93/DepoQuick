namespace Dtos.ReservaDtos;

public class ReservaAltaDto
{
    public RangoFechasDto RangoFechasDto{get; set; }
    
    public int IdDeposito { get; set; }
    
    public String EmailUsuario { get; set; }

    public ReservaAltaDto(RangoFechasDto rangoFechasDto, int idDeposito, String emailUsuario)
    {
        RangoFechasDto = rangoFechasDto;
        IdDeposito = idDeposito;
        EmailUsuario = emailUsuario;
    }
}