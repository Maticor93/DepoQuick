using Dtos.DepositoDtos;

namespace Dtos.ReservaDtos;

public class ReservaConIdDto
{
    public int Id { get; set; }
    public double Precio { get; set; }
    
    public RangoFechasDto RangoFechasDto { get; set; }
    
    public DepositoConIdDto DepositoDto { get; set; }
    
    public String EmailUsuario { get; set; }
    public String ConfAdmin { get; set; }
    
    public String? Pago { get; set; }

    public ReservaConIdDto(int id,double precio, RangoFechasDto rangoFechasDto, DepositoConIdDto depositoDto,String emailUsuario, String confAdmin,
        String pago)
    {
        Id = id;
        Precio = precio;
        RangoFechasDto = rangoFechasDto;
        DepositoDto = depositoDto;
        EmailUsuario = emailUsuario;
        ConfAdmin = confAdmin;
        Pago = pago;
    }
}