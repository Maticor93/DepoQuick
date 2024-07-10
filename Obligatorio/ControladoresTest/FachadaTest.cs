using Controladores;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;
using Dtos;
using Dtos.DepositoDtos;
using Dtos.ReservaDtos;
using Dtos.UsuarioDtos;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Repositories;

namespace ControladoresTest;

[TestClass]
public class FachadaTest
{

    private EFContext _context;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Crear_Instancia_Del_Controlador()
    {
        Fachada controlador = new Fachada();

        Assert.IsNotNull(controlador);
    }

    [TestMethod]
    public void Asignarle_Contexto_A_Controlador()
    {
        Fachada controlador = new Fachada(_context);

        Assert.IsNotNull(controlador);
    }

    [TestMethod]
    public void Agregar_Promocion_A_Base_De_Datos()
    {
        var etiqueta = "nuevaPromocion";
        var promocionDto = new PromocionSinIdDto(etiqueta, 10, DateTime.Today, DateTime.Today.AddDays(2));

        Fachada fachada = new Fachada(_context);
        fachada.AgregarPromocion(promocionDto);
        var promocionRecienAgregada = _context.Promocion.FirstOrDefault(p => p.Etiqueta == etiqueta);

        Assert.IsNotNull(promocionRecienAgregada);
    }

    [TestMethod]
    public void Eliminar_Promocion_Recien_Agregada()
    {
        Fachada fachada = new Fachada(_context);
        var etiqueta = "nuevaPromocion";
        var promocionDto = new PromocionSinIdDto(etiqueta, 10, DateTime.Today, DateTime.Today.AddDays(2));
        fachada.AgregarPromocion(promocionDto);
        var promocion = _context.Promocion.FirstOrDefault(p => p.Etiqueta == etiqueta);
        var idPromocion = promocion.Id;
        
        fachada.BajaPromocion(idPromocion);
        var promocionEliminada = _context.Promocion.Find(idPromocion);

        Assert.IsNull(promocionEliminada);

    }

    [TestMethod]
    public void Agregar_Promociones_Y_Obtener_Una_Lista_Con_Los_Datos_De_Las_Promociones_Agregadas()
    {
        Fachada fachada = new Fachada(_context);
        var promocion1Dto = new PromocionSinIdDto("promocion1", 10, DateTime.Today, DateTime.Today.AddDays(2));
        var promocion2Dto = new PromocionSinIdDto("promocion2", 5, DateTime.Today.AddDays(3), DateTime.Today.AddDays(4));
        fachada.AgregarPromocion(promocion1Dto);
        fachada.AgregarPromocion(promocion2Dto);

        List<PromocionConIdDto> datosPromociones = fachada.ListarPromociones();

        Assert.IsTrue(datosPromociones.Count() == 2);
    }

    [TestMethod]
    public void Editar_La_Unica_Promocion_Otenida_De_Listar_Las_Promociones()
    {
        Fachada fachada = new Fachada(_context);
        var etiqueta = "promocionAEditar";
        var promocionDto = new PromocionSinIdDto(etiqueta, 10, DateTime.Today, DateTime.Today.AddDays(2));
        fachada.AgregarPromocion(promocionDto);

        List<PromocionConIdDto> datosPromociones = fachada.ListarPromociones();
        var promocionEditadaDto = new PromocionConIdDto(datosPromociones[0].Id, "promocionEditada", 10, DateTime.Today,
            DateTime.Today.AddDays(2));
        fachada.ModificarPromocion(promocionEditadaDto);
        var promoEditada = _context.Promocion.Find(datosPromociones[0].Id);
        
        Assert.AreNotEqual(etiqueta, promoEditada.Etiqueta);

    }

    [TestMethod]
    public void Obtener_Datos_De_Promocion_Por_Id()
    {
        Fachada fachada = new Fachada(_context);
        var promocionDto = new PromocionSinIdDto("etiqueta", 10, DateTime.Today, DateTime.Today.AddDays(2));
        fachada.AgregarPromocion(promocionDto);
        int idPromocionAgregada = fachada.ListarPromociones()[0].Id;
        
        PromocionConIdDto promocionObtenidaDto = fachada.ObtenerDatosPromocion(idPromocionAgregada);
        
        Assert.AreEqual( promocionDto.Etiqueta, promocionObtenidaDto.Etiqueta);
        Assert.AreEqual( promocionDto.Descuento, promocionObtenidaDto.Descuento);
        Assert.AreEqual( promocionDto.Desde, promocionObtenidaDto.Desde);
        Assert.AreEqual( promocionDto.Hasta, promocionObtenidaDto.Hasta);
    }

    [TestMethod]
    public void Registrar_Usuario()
    {
        Fachada fachada = new Fachada(_context);
        SesionLogica sesionLogica = new SesionLogica(); 
        var email = "correoDeUsuario@direccion.com";
        var usuarioRegistroDto = new UsuarioRegistroDto("Nombre completo",email , "Password1@");
        fachada.RegistrarUsuario(usuarioRegistroDto);
        sesionLogica.IniciarSesion(email);
        
        var usuarioRecienAgregado = _context.Usuario.FirstOrDefault(u => u.Email == sesionLogica.EmailUsuarioActual);
        Assert.IsNotNull(usuarioRecienAgregado);
    }

    [TestMethod]
    public void Registrar_Usuario_Para_Obtener_Nombre_Y_Correo_De_Usuario_Actual_Recien_Registrado()
    {
        Fachada fachada = new Fachada(_context);
        SesionLogica sesionLogica = new SesionLogica(); 
        var email = "correoDeUsuario@direccion.com";
        var usuarioRegistroDto = new UsuarioRegistroDto("Nombre completo",email , "Password1@");
        
        fachada.RegistrarUsuario(usuarioRegistroDto);
        sesionLogica.IniciarSesion(email);
        
        UsuarioListaDto usuarioDatos = fachada.ObtenerDatosPublicosDeUsuarioActual(sesionLogica.EmailUsuarioActual);
        
        Assert.AreEqual(email,usuarioDatos.Email);
    }

    [TestMethod]
    public void Al_Registrar_Primer_Usuario_Este_Deberia_Ser_Administrador()
    {
        Fachada fachada = new Fachada(_context);
        SesionLogica sesionLogica = new SesionLogica(); 
        var email = "correoDeUsuario@direccion.com";
        var usuarioRegistroDto = new UsuarioRegistroDto("Nombre completo",email , "Password1@");
        
        fachada.RegistrarUsuario(usuarioRegistroDto);
        sesionLogica.IniciarSesion(email);
        Assert.IsTrue(fachada.UsuarioActualEsAdministrador(sesionLogica.EmailUsuarioActual));
    }
    

    [TestMethod]
    public void Iniciar_Sesion()
    {
        Fachada fachada = new Fachada(_context);
        SesionLogica sesionLogica = new SesionLogica(); 
        var email = "correoDeUsuario@direccion.com";
        var password = "Password1@";
        var usuarioRegistroDto = new UsuarioRegistroDto("Nombre completo",email , password);
        
        
        fachada.RegistrarUsuario(usuarioRegistroDto);
        var usuarioInicioSesionDto = new UsuarioInicioSesionDto(email , password);
        fachada.IniciarSesion(usuarioInicioSesionDto);
        sesionLogica.IniciarSesion(email);
        
        Assert.AreEqual(sesionLogica.EmailUsuarioActual,fachada.ObtenerDatosPublicosDeUsuarioActual(sesionLogica.EmailUsuarioActual).Email);
    }
    
    [TestMethod]
    public void Desasignar_Usuario_De_Sesion_Actual()
    {
        var email = "correoDeUsuario@direccion.com";
        SesionLogica sesionLogica = new SesionLogica(); 
        sesionLogica.IniciarSesion(email);
        
        sesionLogica.CerrarSesion();
        
        Assert.IsNull(sesionLogica.EmailUsuarioActual);
    }
    
    [TestMethod]
    public void Agregar_Deposito_A_Base_De_Datos()
    {
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangosDeFechas = new List<RangoFechasDto>();
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechas);
        Fachada fachada = new Fachada(_context);
        fachada.AgregarDeposito(depositoDto);
        var depositoRecienAgregado = _context.Deposito.FirstOrDefault(d => d.Nombre == nombre);

        Assert.IsNotNull(depositoRecienAgregado);
    }

    [TestMethod]
    public void Lista_Deposito_Con_Promocion_Luego_De_Agregarlo()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var promocionDto = new PromocionSinIdDto("etiqueta", 10, DateTime.Today, DateTime.Today);
        fachada.AgregarPromocion(promocionDto);
        var promocionesConIdDto = fachada.ListarPromociones();
        var promocionesIds = new List<int>(){promocionesConIdDto[0].Id};
        var rangosDeFechas = new List<RangoFechasDto>();
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,promocionesIds,rangosDeFechas);
        
        fachada.AgregarDeposito(depositoDto);
        
        Assert.AreEqual(nombre,fachada.ListarDepositos()[0].Nombre);
    }

    [TestMethod]
    public void Dar_De_Baja_Deposito()
    {
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangosDeFechas = new List<RangoFechasDto>();
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechas);
        Fachada fachada = new Fachada(_context);
        
        fachada.AgregarDeposito(depositoDto);
        var idDeposito = fachada.ListarDepositos()[0].Id;
        fachada.BajaDeposito(idDeposito);
        
        Assert.IsNull(_context.Deposito.FirstOrDefault(d=>d.Nombre == nombre));
    }

    [TestMethod]
    public void Dar_De_Alta_Reserva_Guardandola_En_Base_De_Datos()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(1));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(1));
        var reservaDto = new ReservaAltaDto(rangoFechaReserva, idDeposito, email);

        fachada.AgregarReserva(reservaDto);
        
        Assert.IsNotNull(_context.Reserva.FirstOrDefault(r=>r.Cliente.Email == email));
    }

    [TestMethod]
         public void Listar_Todas_Las_Reservas_De_Un_Usuario()
         {
             Fachada fachada = new Fachada(_context);
             var email = "direccion@dominio.com";
             var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
             fachada.RegistrarUsuario(usuarioDto);
             var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
             var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
             fachada.AgregarDeposito(depositoDto);
     
             var idDeposito = fachada.ListarDepositos()[0].Id;
             var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
             var reserva1Dto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
             var rangoFechaReserva2 = new RangoFechasDto(DateTime.Today.AddDays(5), DateTime.Today.AddDays(10));
             var reserva2Dto = new ReservaAltaDto(rangoFechaReserva2, idDeposito, email);
             fachada.AgregarReserva(reserva1Dto);
             fachada.AgregarReserva(reserva2Dto);
             
             Assert.IsTrue(fachada.ObtenerDatosDeReservasDeUsuario(email).Any());
         }
         
    [TestMethod]
    public void Listar_Todas_Las_Reservas_Pagadas_De_Un_Usuario()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);
     
        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reserva1Dto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        var rangoFechaReserva2 = new RangoFechasDto(DateTime.Today.AddDays(5), DateTime.Today.AddDays(10));
        var reserva2Dto = new ReservaAltaDto(rangoFechaReserva2, idDeposito, email);
        fachada.AgregarReserva(reserva1Dto);
        fachada.AgregarReserva(reserva2Dto);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
             
        Assert.IsTrue(fachada.ObtenerDatosDeReservasDeUsuario(email).Any());
    }
         
    
    [TestMethod]
    public void Listar_Todas_Las_Reservas_Asociadas_A_Un_Deposito()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reserva1Dto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        var rangoFechaReserva2 = new RangoFechasDto(DateTime.Today.AddDays(5), DateTime.Today.AddDays(10));
        var reserva2Dto = new ReservaAltaDto(rangoFechaReserva2, idDeposito, email);
        fachada.AgregarReserva(reserva1Dto);
        fachada.AgregarReserva(reserva2Dto);
        
        fachada.PagarReserva(_context.Reserva.ToList()[1].Id);
        
        Assert.IsTrue(fachada.ObtenerReservasDeDepositoSiSePagaron(idDeposito).Any());
    }

    [TestMethod]
    public void Pagar_Reserva_Cambia_Estado_Del_Pago_A_Reservado()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reservaDto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
        int idReserva = fachada.ObtenerReservasDeDepositoSiSePagaron(idDeposito)[0].Id;
        fachada.PagarReserva(idReserva);
        var datosReserva = fachada.ObtenerReservasDeDepositoSiSePagaron(idDeposito)[0];
        
        Assert.AreEqual(Pago.Reservado.ToString(), datosReserva.Pago);
    }
    
    [TestMethod]
    public void Capturar_Pago_De_Reserva_Luego_De_Ser_Pagada_Cambia_Estado_Del_Pago_A_Capturado()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reservaDto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
        fachada.CapturarPago(_context.Reserva.ToList()[0].Id);
        var datosReserva = fachada.ObtenerReservasDeDepositoSiSePagaron(idDeposito)[0];
        
        Assert.AreEqual(Pago.Capturado.ToString(), datosReserva.Pago);
    }
    
    [TestMethod]
    public void Capturar_Pago_De_Reserva_Luego_De_Ser_Pagada_Cambia_Estado_De_La_Reserva_A_Aprobada()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reservaDto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        int idReserva = _context.Reserva.ToList()[0].Id;
        fachada.PagarReserva(idReserva);
        fachada.CapturarPago(idReserva);
        var datosReserva = fachada.ObtenerReservasDeDepositoSiSePagaron(idDeposito)[0];
        
        Assert.AreEqual(Estado.Aprobada.ToString(), datosReserva.ConfAdmin);
    }
    [TestMethod]
    public void Rechazar_Reserva_Deberia_Dejarla_En_Estado_Rechazada()
    {
        Fachada fachada = new Fachada(_context);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddMonths(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(4));
        var reservaDto = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        int idReserva = _context.Reserva.ToList()[0].Id;
        var comentario = "a";
        fachada.RechazarReserva(idReserva, comentario);
        var datosReserva = _context.Reserva.Find(idReserva);
        
        Assert.AreEqual(Estado.Rechazada.ToString(), datosReserva.ConfAdmin.ToString());
    }

    [TestMethod]
    public void Pasar_Email_De_Un_Usuario_Que_No_Existe_Al_Pedir_Datos_De_Usuario_Deberia_Tirar_Excepcion()
    {
        Fachada fachada = new Fachada(_context);

        Assert.ThrowsException<Exception>((() => fachada.ObtenerDatosPublicosDeUsuarioActual("emailSinRegistrar@gmail.com")));
    }
    
    [TestMethod]
    public void Pasar_Email_De_Un_Usuario_Que_No_Existe_Al_Consultar_Si_Usuario_Es_Admin_Deberia_Tirar_Excepcion()
    {
        Fachada fachada = new Fachada(_context);

        Assert.ThrowsException<Exception>((() => fachada.UsuarioActualEsAdministrador("emailSinRegistrar@gmail.com")));
    }

    [TestMethod]
    public void Pedir_Generar_Un_Reporte_Txt_Deberia_Generar_Un_Reporte_Txt()
    {
        Fachada fachada = new Fachada(_context);
        String pathTxt =  $"../../../../../Reportes";
        
        fachada.GenerarReporte("TXT",pathTxt);
        pathTxt =  pathTxt + "/ReporteReservas.txt";
        
        Assert.IsTrue(File.Exists(pathTxt));
        
        File.Delete(pathTxt);
    }

    [TestMethod]
    public void Pedir_Generar_Un_Reporte_Csv_Deberia_Generar_Un_Reporte_Csv()
    {
        Fachada fachada = new Fachada(_context);
        String pathCsv =  $"../../../../../Reportes";
        
        fachada.GenerarReporte("CSV",pathCsv);
        pathCsv += "/ReporteReservas.csv";
        Assert.IsTrue(File.Exists(pathCsv));
        
        File.Delete(pathCsv);
    }

    [TestMethod]
    public void Obtener_Todos_Los_Depositos_Que_Esten_Disponibles_Dentro_De_Un_Rango_De_Fechas()
    {
        Fachada fachada = new Fachada(_context);

        var datosDepositos = fachada.ObtenerDatosDepositosDisponiblesEn(DateTime.Today, DateTime.Today.AddDays(6));
        
        Assert.IsNotNull(datosDepositos);
    }
    
    [TestMethod]
    public void Agrego_Un_Deposito_Y_Lo_Obtengo_Consultando_Por_Un_Rango_De_Fechas_Del_Deposito()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofecha = new RangoFechasDto(DateTime.Today.AddDays(1), DateTime.Today.AddDays(6));
        var rangosDeFechas = new List<RangoFechasDto>(){rangofecha};
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechas);
        fachada.AgregarDeposito(depositoDto);

        var datosDepositos = fachada.ObtenerDatosDepositosDisponiblesEn(rangofecha.FechaInicio, rangofecha.FechaFin);
        
        Assert.AreEqual(nombre,datosDepositos[0].Nombre);
    }
    [TestMethod]
    public void Al_Consultar_Por_Un_Deposito_Ya_Reservado_Ingresando_Rango_De_Fechas_Correcto_Deberia_No_Mostrarme_Ningun_Deposito()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofechaDepo = new RangoFechasDto(DateTime.Today.AddDays(1), DateTime.Today.AddDays(6));
        var rangosDeFechasDepo = new List<RangoFechasDto>(){rangofechaDepo};
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechasDepo);
        fachada.AgregarDeposito(depositoDto);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva = new RangoFechasDto(rangofechaDepo.FechaInicio, rangofechaDepo.FechaFin);
        var reservaDto = new ReservaAltaDto(rangoFechaReserva, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
        fachada.CapturarPago(_context.Reserva.ToList()[0].Id);
        
        var datosDepositos = fachada.ObtenerDatosDepositosDisponiblesEn(rangofechaDepo.FechaInicio, rangofechaDepo.FechaFin);
        
        Assert.IsFalse(datosDepositos.Any());
    }
    
    [TestMethod]
    public void Al_Consultar_Por_Un_Deposito_Ya_Pagado_Pero_No_Capturado_Ingresando_Rango_De_Fechas_Correcto_Deberia_Mostrarme_El_Deposito()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var etiqueta = "nuevaPromocion";
        var promocionDto = new PromocionSinIdDto(etiqueta, 10, DateTime.Today, DateTime.Today.AddDays(2));
        fachada.AgregarPromocion(promocionDto);
        var PromocionesIds = new List<int>(){_context.Promocion.ToList()[0].Id};
        var rangofechaDepo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(7));
        var rangosDeFechasDepo = new List<RangoFechasDto>(){rangofechaDepo};
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechasDepo);
        fachada.AgregarDeposito(depositoDto);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva = new RangoFechasDto(rangofechaDepo.FechaInicio, rangofechaDepo.FechaFin);
        var reservaDto = new ReservaAltaDto(rangoFechaReserva, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
        
        var datosDepositos = fachada.ObtenerDatosDepositosDisponiblesEn(DateTime.Today, DateTime.Today.AddDays(7));
        
        Assert.IsTrue(datosDepositos.Any());
    }
    
    [TestMethod]
    public void Consultar_Por_Rango_De_Fecha_De_Deposito_Reservado_En_Otro_Rango_Deberia_Mostrar_El_Deposito()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofechaDepo = new RangoFechasDto(DateTime.Today.AddDays(1), DateTime.Today.AddDays(6));
        var rangosDeFechasDepo = new List<RangoFechasDto>(){rangofechaDepo};
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,PromocionesIds,rangosDeFechasDepo);
        fachada.AgregarDeposito(depositoDto);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        var rangoFechaReserva1 = new RangoFechasDto(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
        var reservaDto1 = new ReservaAltaDto(rangoFechaReserva1, idDeposito, email);
        var rangoFechaReserva2 = new RangoFechasDto(DateTime.Today.AddDays(3), DateTime.Today.AddDays(4));
        var reservaDto2 = new ReservaAltaDto(rangoFechaReserva2, idDeposito, email);
        fachada.AgregarReserva(reservaDto1);
        fachada.AgregarReserva(reservaDto2);
        fachada.PagarReserva(_context.Reserva.ToList()[0].Id);
        fachada.CapturarPago(_context.Reserva.ToList()[0].Id);
        fachada.CapturarPago(_context.Reserva.ToList()[0].Id);
        
        var datosDepositos = fachada.ObtenerDatosDepositosDisponiblesEn(DateTime.Today.AddDays(5), DateTime.Today.AddDays(6));
        
        Assert.IsTrue(datosDepositos.Any());
    }

    [TestMethod]
    public void Consultar_Precio_de_Reserva()
    {
        Fachada fachada = new Fachada(_context);
        var rangoDeFechasDpo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(2));
        var depositoDto = new DepositoAltaDto("nombre", "A","Grande",false,new List<int>(),new List<RangoFechasDto>(){rangoDeFechasDpo});
        fachada.AgregarDeposito(depositoDto);

        var idDeposito = fachada.ListarDepositos()[0].Id;
        
        var precioReserva = fachada.ObtenerPrecioReserva(idDeposito, rangoDeFechasDpo.FechaInicio, rangoDeFechasDpo.FechaFin);

        var precioEsperado = 200;
        
        Assert.AreEqual(precioEsperado, precioReserva);
    }



    [TestMethod]
    public void Obtener_Reserva_por_Id()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofechaDepo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(7));
        var rangosDeFechasDepo = new List<RangoFechasDto>() { rangofechaDepo };
        var depositoDto = new DepositoAltaDto(nombre, "A", "Grande", false, PromocionesIds, rangosDeFechasDepo);
        fachada.AgregarDeposito(depositoDto);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var idDeposito = fachada.ListarDepositos().First().Id;
        var rangoFechaReserva = new RangoFechasDto(rangofechaDepo.FechaInicio, rangofechaDepo.FechaFin);
        var reservaDto = new ReservaAltaDto(rangoFechaReserva, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        var idReserva = fachada.ObtenerDatosDeReservasDeUsuario(email).First().Id;
        var reservaObtenida = fachada.ObtenerReserva(idReserva);
        Assert.AreEqual(reservaObtenida.EmailUsuario, email);
        Assert.AreEqual(reservaObtenida.IdDeposito, idDeposito);
        Assert.AreEqual(reservaObtenida.RangoFechasDto.FechaInicio, rangoFechaReserva.FechaInicio);
        Assert.AreEqual(reservaObtenida.RangoFechasDto.FechaFin, rangoFechaReserva.FechaFin);
    }

    [TestMethod]
    public void ObtenerComentarioReserva_Deberia_Retornar_Comentario_Correcto()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofechaDepo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(7));
        var rangosDeFechasDepo = new List<RangoFechasDto>() { rangofechaDepo };
        var depositoDto = new DepositoAltaDto(nombre, "A", "Grande", false, PromocionesIds, rangosDeFechasDepo);
        fachada.AgregarDeposito(depositoDto);
        var email = "direccion@dominio.com";
        var usuarioDto = new UsuarioRegistroDto("nombre", email, "Password1@");
        fachada.RegistrarUsuario(usuarioDto);
        var idDeposito = fachada.ListarDepositos().First().Id;
        var rangoFechaReserva = new RangoFechasDto(rangofechaDepo.FechaInicio, rangofechaDepo.FechaFin);
        var reservaDto = new ReservaAltaDto(rangoFechaReserva, idDeposito, email);
        fachada.AgregarReserva(reservaDto);
        
        var idReserva = fachada.ObtenerDatosDeReservasDeUsuario(email).First().Id;
        fachada.RechazarReserva(idReserva, "Comentario");
        
        Assert.AreEqual("Comentario", fachada.ObtenerComentarioReserva(idReserva));
    }

    [TestMethod]
    public void Agregar_Deposito_Con_Area_No_Aceptada_Debe_Tirar_Excepcion()
    {
        Fachada fachada = new Fachada(_context);
        var nombre = "nombre";
        var PromocionesIds = new List<int>();
        var rangofechaDepo = new RangoFechasDto(DateTime.Today, DateTime.Today.AddDays(7));
        var rangosDeFechasDepo = new List<RangoFechasDto>() { rangofechaDepo };
        var depositoDto = new DepositoAltaDto(nombre, "J", "Grande", false, PromocionesIds, rangosDeFechasDepo);
        Assert.ThrowsException<Exception>((() => fachada.AgregarDeposito(depositoDto)));
    }
    
    [TestMethod]
    public void Obtener_Datos_Deposito_Por_Id()
    {
        Fachada fachada = new Fachada(_context);
        var etiqueta = "nuevaPromocion";
        var nombre = "nombre";
        var promocionDto = new PromocionSinIdDto(etiqueta, 10, DateTime.Today, DateTime.Today.AddDays(2));
        fachada.AgregarPromocion(promocionDto);
        var promocionId = _context.Promocion.FirstOrDefault(p => p.Etiqueta == etiqueta).Id;
        var rangoFechas = new RangoFechasDto(DateTime.Today.AddDays(2), DateTime.Today.AddDays(5));
        var depositoDto = new DepositoAltaDto(nombre, "A","Grande",false,new List<int>(){promocionId},new List<RangoFechasDto>(){rangoFechas});
        
        fachada.AgregarDeposito(depositoDto);
        var idDeposito = fachada.ListarDepositos()[0].Id;
        
        Assert.AreEqual(nombre, fachada.ObtenerDeposito(idDeposito).Nombre);
    }
    
}