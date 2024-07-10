using Dominio;
using Dominio.Enums;
using Dtos;
using Dtos.DepositoDtos;
using Dtos.ReservaDtos;
using Dtos.UsuarioDtos;
using LogicaDeNegocio;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;
namespace Controladores;

public class Fachada
{
    private PromocionRepository _promocionRepository;
    private PromocionLogica _promocionLogica;
    
    private DepositoRepository _depositoRepository;
    private DepositoLogica _depositoLogica;

    private ReservaRepository _reservaRepository;
    private ReservaLogica _reservaLogica;
    
    private UsuarioRepository _usuarioRepository;
    private UsuarioLogica _usuarioLogica;

    public Fachada() {} 
    public Fachada(EFContext context)
    {
        _promocionRepository = new PromocionRepository(context);
        _promocionLogica = new PromocionLogica(_promocionRepository);

        _depositoRepository = new DepositoRepository(context);
        _depositoLogica = new DepositoLogica(_depositoRepository);

        _reservaRepository = new ReservaRepository(context);
        _reservaLogica = new ReservaLogica(_reservaRepository);

        _usuarioRepository = new UsuarioRepository(context);
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
    }

    public void AgregarPromocion(PromocionSinIdDto datosPromocion)
    {
        var rangoFechas = new RangoFechas(datosPromocion.Desde, datosPromocion.Hasta);
        var nuevaPromocion = new Promocion(datosPromocion.Etiqueta,datosPromocion.Descuento,rangoFechas);
        _promocionLogica.Agregar(nuevaPromocion);
    }

    public void BajaPromocion(int idPromocion)
    {
        _promocionLogica.Eliminar(idPromocion);
    }

    public List<PromocionConIdDto> ListarPromociones()
    {
        List<PromocionConIdDto> datosPromociones = new List<PromocionConIdDto>();
        foreach (var promocion in _promocionLogica.ObtenerTodas())
        {
            PromocionConIdDto conIdDto = new PromocionConIdDto(promocion.Id,promocion.Etiqueta, promocion.Descuento, promocion.RangoFecha.FechaInicio, promocion.RangoFecha.FechaFin);
            datosPromociones.Add(conIdDto);
        }

        return datosPromociones;
    }

    public void ModificarPromocion(PromocionConIdDto promocionEditadaDto)
    {
        var promocion = _promocionLogica.EncontrarPorId(promocionEditadaDto.Id);
        promocion.Etiqueta = promocionEditadaDto.Etiqueta;
        promocion.Descuento = promocionEditadaDto.Descuento;
        promocion.RangoFecha.ActualizarRangoFechas(promocionEditadaDto.Desde,promocionEditadaDto.Hasta);
        _promocionLogica.Actualizar(promocion);
    }

    public PromocionConIdDto ObtenerDatosPromocion(int idPromocion)
    {
        var promocion = _promocionLogica.EncontrarPorId(idPromocion);
        PromocionConIdDto dtoPromocion = new PromocionConIdDto(promocion.Id, promocion.Etiqueta, promocion.Descuento,
            promocion.RangoFecha.FechaInicio, promocion.RangoFecha.FechaFin);
        return dtoPromocion;
    }

    public void RegistrarUsuario(UsuarioRegistroDto usuarioRegistroDto)
    {
        Rol rol = _usuarioLogica.HayAdministrador() ? Rol.Cliente : Rol.Administrador;
        var usuario = new Usuario(usuarioRegistroDto.Email,usuarioRegistroDto.NombreCompleto,
            usuarioRegistroDto.Password,rol);
        _usuarioLogica.Agregar(usuario);
    }

    public UsuarioListaDto ObtenerDatosPublicosDeUsuarioActual(String email)
    {
        var usuario = _usuarioLogica.EncontrarPorEmail(email);
        if (usuario == null) throw new Exception("El usuario consultado no esta registrado");
        return new UsuarioListaDto(usuario.NombreCompleto,usuario.Email);
    }

    public bool UsuarioActualEsAdministrador(String email)
    {
        if (_usuarioLogica.EncontrarPorEmail(email) == null) throw new Exception("El usuario consultado no esta registrado");
        return _usuarioLogica.UsuarioEsAdministrador(email);
    }

    public bool IniciarSesion(UsuarioInicioSesionDto usuarioInicioSesionDto)
    {
        return _usuarioLogica.ValidarCredenciales(usuarioInicioSesionDto.Email, usuarioInicioSesionDto.Password);
    }
    
    public void AgregarDeposito(DepositoAltaDto depositoDto)
    {
        if (Enum.TryParse<Area>(depositoDto.Area, out Area area) && Enum.TryParse<Tamanio>(depositoDto.Tamanio, out Tamanio tamanio))
        {
            var promociones = ObtenerPromocionesPorIds(depositoDto.PromocionesIds);
            var deposito = new Deposito(depositoDto.Nombre, area, tamanio,depositoDto.Climatizacion,promociones);
            
            foreach (var rangoFechaDto in depositoDto.FechasDisponibilidad)
            {
                var rangoFecha = new RangoFechas(rangoFechaDto.FechaInicio, rangoFechaDto.FechaFin);
                deposito.AgregarRangoFecha(rangoFecha);
            }
            
            _depositoLogica.Agregar(deposito);
        }
        else
        {
            throw new Exception("El valor de área o tamaño no es valido");
        }
    }
    
    private List<Promocion> ObtenerPromocionesPorIds(List<int> ids)
    {
        List<Promocion> promociones = new List<Promocion>();
        foreach (var promocionId in ids)
        {
            var promocion = _promocionLogica.EncontrarPorId(promocionId);
            promociones.Add(promocion);
        }

        return promociones;
    }

    public DepositoConIdDto ObtenerDeposito(int idDeposito)
    {
        var deposito = _depositoLogica.EncontrarPorId(idDeposito);
        
        var promocionDtos = new List<PromocionSinIdDto>();
        foreach (var promocion in deposito.Promociones)
        {
            var promocionDto = new PromocionSinIdDto(promocion.Etiqueta, promocion.Descuento,
                promocion.RangoFecha.FechaInicio, promocion.RangoFecha.FechaFin);
            promocionDtos.Add(promocionDto);
        }
            
        var rangoFechasDtos = new List<RangoFechasDto>();
        foreach (var rangoFecha in deposito.Disponibilidad)
        {
            var rangoFechasDto = new RangoFechasDto(rangoFecha.FechaInicio, rangoFecha.FechaFin);
            rangoFechasDtos.Add(rangoFechasDto);
        }
        DepositoConIdDto depositoDto = new DepositoConIdDto(deposito.Id,deposito.Nombre,deposito.Area.ToString(), deposito.Tamanio.ToString(),deposito.Climatizacion,promocionDtos,rangoFechasDtos);

        return depositoDto;
    }

    

    public List<DepositoConIdDto> ListarDepositos()
    {
        List<DepositoConIdDto> datosDepositos = new List<DepositoConIdDto>();
        foreach (var deposito in _depositoLogica.ObtenerTodos())
        {
            var promocionDtos = new List<PromocionSinIdDto>();
            foreach (var promocion in deposito.Promociones)
            {
                var promocionDto = new PromocionSinIdDto(promocion.Etiqueta, promocion.Descuento,
                    promocion.RangoFecha.FechaInicio, promocion.RangoFecha.FechaFin);
                promocionDtos.Add(promocionDto);
            }
            
            var rangoFechasDtos = new List<RangoFechasDto>();
            foreach (var rangoFecha in deposito.Disponibilidad)
            {
                var rangoFechasDto = new RangoFechasDto(rangoFecha.FechaInicio, rangoFecha.FechaFin);
                rangoFechasDtos.Add(rangoFechasDto);
            }
            DepositoConIdDto depositoDto = new DepositoConIdDto(deposito.Id,deposito.Nombre,deposito.Area.ToString(), deposito.Tamanio.ToString(),deposito.Climatizacion,promocionDtos,rangoFechasDtos);
            datosDepositos.Add(depositoDto);
        }

        return datosDepositos;
    }

    public void BajaDeposito(int idDeposito)
    {
        _depositoLogica.EliminarDepositoEnCasoDeNoEstarReservado(idDeposito,_reservaLogica.ObtenerReservasAsociadasA(idDeposito));
    }

    public void AgregarReserva(ReservaAltaDto reservaDto)
    {
        var rangoFechas = new RangoFechas(reservaDto.RangoFechasDto.FechaInicio, reservaDto.RangoFechasDto.FechaFin);
        var deposito = _depositoLogica.EncontrarPorId(reservaDto.IdDeposito);
        var usuario = _usuarioLogica.EncontrarPorEmail(reservaDto.EmailUsuario);
        var reserva = new Reserva(rangoFechas, deposito, usuario);
        _reservaLogica.Agregar(reserva);
    }
    
    
    public List<ReservaConIdDto> ObtenerDatosDeReservasDeUsuario(String emailUsuario)
    {
        List<ReservaConIdDto> datosReservasDeUsuario = new List<ReservaConIdDto>();
        foreach (var reserva in _reservaLogica.ObtenerReservasDe(emailUsuario))
        {
            var rangoFechasDto = new RangoFechasDto(reserva.RangoFecha.FechaInicio, reserva.RangoFecha.FechaFin);
            DepositoConIdDto depositoDto = null;
            if (reserva.Deposito != null)
            {
                depositoDto = new DepositoConIdDto(reserva.Deposito.Id, reserva.Deposito.Nombre,
                    reserva.Deposito.Area.ToString(),
                    reserva.Deposito.Tamanio.ToString(), reserva.Deposito.Climatizacion, null, null);
            }
            String? stringPago = null;
            if (reserva.Pago != null) stringPago = reserva.Pago.ToString();
            var reservaConIdDto = new ReservaConIdDto(reserva.Id, reserva.Precio, rangoFechasDto, depositoDto,reserva.Cliente.Email, reserva.ConfAdmin.ToString(), stringPago);
            datosReservasDeUsuario.Add(reservaConIdDto);
        }

        return datosReservasDeUsuario;
    }

    public List<ReservaConIdDto> ObtenerReservasDeDepositoSiSePagaron(int idDeposito)
    {
        List<ReservaConIdDto> datosReservasDeDeposito = new List<ReservaConIdDto>();
        foreach (var reserva in _reservaLogica.ObtenerReservasAsociadasA(idDeposito))
        {
            if(reserva.Pago == null && reserva.ConfAdmin.ToString() == "Pendiente") continue;
            var rangoFechasDto = new RangoFechasDto(reserva.RangoFecha.FechaInicio, reserva.RangoFecha.FechaFin);
            var depositoDto = new DepositoConIdDto(reserva.Deposito.Id, reserva.Deposito.Nombre, reserva.Deposito.Area.ToString(),
                reserva.Deposito.Tamanio.ToString(), reserva.Deposito.Climatizacion, null, null);
            var reservaConIdDto = new ReservaConIdDto(reserva.Id,reserva.Precio, rangoFechasDto, depositoDto,reserva.Cliente.Email,reserva.ConfAdmin.ToString(),reserva.Pago.ToString());
            datosReservasDeDeposito.Add(reservaConIdDto);
        }

        return datosReservasDeDeposito;
    }

    public void PagarReserva(int idReserva)
    {
        _reservaLogica.PagarReserva(idReserva);
    }

    public void CapturarPago(int idReserva)
    {
        _reservaLogica.CapturarPago(idReserva);
    }

    public void RechazarReserva(int idReserva, string comentario)
    {
        _reservaLogica.RechazarReserva(idReserva, comentario);
    }

    public void GenerarReporte(string tipo,String pathReportes)
    {
        var fabricaGeneradorReporte = new FabricaGeneradorReporte(pathReportes);
        IGeneradorReporte generadorReporte = fabricaGeneradorReporte.CrearGeneradorReporte(tipo);

        generadorReporte.GenerarReporte(_reservaLogica.ObtenerTodas());
    }

    public List<DepositoConIdDto> ObtenerDatosDepositosDisponiblesEn(DateTime desde, DateTime hasta)
    {
        var datosDepositos = new List<DepositoConIdDto>();
        foreach (var deposito in _depositoLogica.ObtenerTodosDisponiblesEn(desde,hasta))
        {
            if(_reservaLogica.DepositoTieneReservaAprobadaEnRangoFecha(deposito,desde,hasta)) continue;
            
            var promocionDtos = new List<PromocionSinIdDto>();
            foreach (var promocion in deposito.Promociones)
            {
                var promocionDto = new PromocionSinIdDto(promocion.Etiqueta, promocion.Descuento,
                    promocion.RangoFecha.FechaInicio, promocion.RangoFecha.FechaFin);
                promocionDtos.Add(promocionDto);
            }
            
            var rangoFechasDtos = new List<RangoFechasDto>();
            foreach (var rangoFecha in deposito.Disponibilidad)
            {
                var rangoFechasDto = new RangoFechasDto(rangoFecha.FechaInicio, rangoFecha.FechaFin);
                rangoFechasDtos.Add(rangoFechasDto);
            }
            DepositoConIdDto depositoDto = new DepositoConIdDto(deposito.Id,deposito.Nombre,deposito.Area.ToString(), deposito.Tamanio.ToString(),deposito.Climatizacion,promocionDtos,rangoFechasDtos);
            datosDepositos.Add(depositoDto);
        }
        return datosDepositos;
    }

    public double ObtenerPrecioReserva(int idDeposito, DateTime desde, DateTime hasta)
    {
        var deposito = _depositoLogica.EncontrarPorId(idDeposito);
        return _reservaLogica.CalcularPrecioReserva(deposito, desde, hasta);
    }
    
    public ReservaAltaDto ObtenerReserva(int idReserva)
    {
        var reserva = _reservaLogica.EncontrarPorId(idReserva);
        var rangoFechasDto = new RangoFechasDto(reserva.RangoFecha.FechaInicio, reserva.RangoFecha.FechaFin);
        var reservaDto = new ReservaAltaDto(rangoFechasDto, reserva.Deposito.Id, reserva.Cliente.Email);

        return reservaDto;
    }
    
    public string ObtenerComentarioReserva(int idReserva)
    {
        var reserva = _reservaLogica.EncontrarPorId(idReserva);
        return reserva.Comentario;
    }
}