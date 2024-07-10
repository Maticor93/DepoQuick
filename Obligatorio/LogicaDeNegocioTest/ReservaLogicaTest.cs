using Dominio;
using Dominio.Enums;
using LogicaDeNegocio;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocioTest;
[TestClass]
public class ReservaLogicaTest
{
    private ReservaRepository _reservaRepository;
    private EFContext _context;
    private ReservaLogica _reservaLogica;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();
    
    //para los dos ultimos test que implican usar depositos
    private DepositoRepository _depositoRepository;
    private DepositoLogica _depositoLogica;

    private PromocionRepository _promocionRepository;
    private PromocionLogica _promocionLogica;

    private UsuarioRepository _usuarioRepository;
    private UsuarioLogica _usuarioLogica;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _reservaRepository = new ReservaRepository(_context); 
        _reservaLogica = new ReservaLogica(_reservaRepository);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Agregar_Reserva_Test()
    {
        Reserva reserva = new Reserva();
        reserva.Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        var agregado = _reservaLogica.Agregar(reserva);
        
        Assert.AreEqual(_reservaLogica.EncontrarPorId(reserva.Id),agregado);
    }

    [TestMethod]
    public void Agregar_Reserva_Repetida_Deberia_Tirar_Excepcion()
    {
        Reserva reserva = new Reserva()
        {
            Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente),
            Deposito = new Deposito()
            {
                Nombre = "Nombre deposito"
            },
            RangoFecha = new RangoFechas()
            {
                FechaInicio = DateTime.Today.AddDays(1),
                FechaFin = DateTime.Today.AddDays(4)
            }
        };
        _reservaLogica.Agregar(reserva);

        Assert.ThrowsException<ReservaLogicaExcepcion>((() => _reservaLogica.Agregar(reserva)));

    }


    [TestMethod]
    public void Actualizar_Una_Reserva_Test()
    {

        var fechaInicioOriginal = DateTime.Today.AddMonths(3);
        var fechaFinOriginal = DateTime.Today.AddMonths(4);
        var rangoFecha = new RangoFechas(fechaInicioOriginal, fechaFinOriginal);
        Reserva reserva = new Reserva(rangoFecha);
        reserva.Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        _reservaLogica.Agregar(reserva);

        reserva.RangoFecha.FechaInicio = DateTime.Today.AddMonths(4);
        reserva.RangoFecha.FechaFin = DateTime.Today.AddMonths(5);
        Reserva reservaActualizada = _reservaLogica.Actualizar(reserva);
        
        Assert.AreNotEqual(fechaInicioOriginal,reservaActualizada.RangoFecha.FechaInicio);
        Assert.AreNotEqual(fechaFinOriginal,reservaActualizada.RangoFecha.FechaFin);
    }

    [TestMethod]
    public void Eliminar_Una_Reserva_Test()
    {
        var reserva = new Reserva();
        reserva.Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        _reservaLogica.Agregar(reserva);
        var id = reserva.Id;

        _reservaLogica.Eliminar(id);
        
        Assert.IsNull(_reservaLogica.EncontrarPorId(id));

    }

    [TestMethod]
    public void Obtener_Todas_Las_Reservas_Test()
    {
        List<Reserva> lista = _reservaLogica.ObtenerTodas();

        Assert.IsNotNull(lista);

    }

    [TestMethod]
    public void Asigno_Una_Reserva_A_Un_Cliente_Y_Obtengo_Una_Lista_Asociada_Al_Cliente_Con_La_Reserva_Test()
    {
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        var fechaInicial = DateTime.Today.AddMonths(2);
        var fechaFinal = DateTime.Today.AddMonths(3);
        var rangoFechas = new RangoFechas(fechaInicial,fechaFinal);
        var deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        
        Reserva reserva = new Reserva(rangoFechas,deposito , cliente);
        _reservaLogica.Agregar(reserva);

        List<Reserva> reservasDeCliente = _reservaLogica.ObtenerReservasDe(cliente.Email);
        
        Assert.AreEqual(reserva, reservasDeCliente[0]);


    }

    [TestMethod]
    public void Asingo_Deposito_A_Reserva_Para_Obtener_Las_Reservas_Asociadas_A_Ese_Deposito_Test()
    {
        var fechaInicial = DateTime.Today.AddDays(10);
        var fechaFinal = DateTime.Today.AddDays(20);
        Deposito deposito = new Deposito("Nombre deposito",Area.A, Tamanio.Grande, true, new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        var cliente = new Usuario("direccion@dom.com", "nombre apellido", "Password1@", Rol.Cliente);
        var rangoFechas = new RangoFechas(fechaInicial,fechaFinal); 
        Reserva reserva = new Reserva(rangoFechas, deposito, cliente);
        _reservaLogica.Agregar(reserva);

        List<Reserva> reservasAsociadasADeposito = _reservaLogica.ObtenerReservasAsociadasA(deposito.Id);
        
        Assert.AreEqual(reserva,reservasAsociadasADeposito[0]);

    }

    [TestMethod]
    public void Usuario_Vuelve_A_Reservar_deposito_Pero_Con_Fecha_Fuera_Del_Rango_De_Deposito_Anterior_Deberia_Agregarse()
    {
        
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);

        var rangoFecha1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(4));
        var rangoFecha2 = new RangoFechas(DateTime.Today.AddDays(5), DateTime.Today.AddDays(9));
        Deposito deposito = new Deposito()
        {
            Nombre = "Nombre deposito",
            Area = Area.A,
            Tamanio = Tamanio.Grande,
            Climatizacion = false,
            Promociones = new List<Promocion>(),
            Disponibilidad = new List<RangoFechas>()
            {
                new RangoFechas(DateTime.Today,DateTime.Today.AddDays(4)),
                new RangoFechas(DateTime.Today.AddDays(5),DateTime.Today.AddDays(9))
            }
        };
        Reserva primeraReserva = new Reserva(rangoFecha1,deposito,cliente);
        Reserva segundaReserva = new Reserva(rangoFecha2, deposito,cliente);
        
        _reservaLogica.Agregar(primeraReserva);
        _reservaLogica.Agregar(segundaReserva);
        
        Assert.AreEqual(primeraReserva.Deposito, segundaReserva.Deposito);
        Assert.AreEqual(primeraReserva.Cliente,segundaReserva.Cliente);
    }

    [TestMethod]
    public void Usuario_Paga_Reserva()
    {
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(DateTime.Today,DateTime.Today.AddDays(1))});
        var reserva = new Reserva(new RangoFechas(DateTime.Today,DateTime.Today.AddDays(1)),deposito,cliente);
        _reservaLogica.Agregar(reserva);

        _reservaLogica.PagarReserva(reserva.Id);
        Assert.AreEqual(Pago.Reservado,reserva.Pago);
    }

    [TestMethod]
    public void Administrador_Captura_El_Pago_De_Reserva()
    {
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(DateTime.Today,DateTime.Today.AddDays(1))});
        var reserva = new Reserva(new RangoFechas(DateTime.Today,DateTime.Today.AddDays(1)),deposito,cliente);
        _reservaLogica.Agregar(reserva);

        _reservaLogica.PagarReserva(reserva.Id);

        _reservaLogica.CapturarPago(reserva.Id);
        Assert.AreEqual(Pago.Capturado,reserva.Pago);
    }
    [TestMethod]
    public void Administrador_Captura_El_Pago_De_Reserva_Deberia_Rechazar_Las_Reservas_No_Aceptadas_Del_Mismo_Deposito_Si_Se_Superpone_La_Fecha()
    {
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(DateTime.Today,DateTime.Today.AddDays(7))});
        var reserva1 = new Reserva(new RangoFechas(DateTime.Today,DateTime.Today.AddDays(2)),deposito,cliente);
        var reserva2 = new Reserva(new RangoFechas(DateTime.Today.AddDays(1),DateTime.Today.AddDays(3)),deposito,cliente);
        _reservaLogica.Agregar(reserva1);
        _reservaLogica.Agregar(reserva2);

        _reservaLogica.PagarReserva(reserva1.Id);

        _reservaLogica.CapturarPago(reserva1.Id);

        var reserva2Cliente = _reservaLogica.ObtenerReservasDe(cliente.Email)[1];
        Assert.AreEqual(Estado.Rechazada,reserva2Cliente.ConfAdmin);
    }

    [TestMethod]
    public void Pagar_Una_Reserva_Con_Fecha_Inicial_Anterior_A_Hoy_Deberia_Tirar_Excepcion_Y_Eliminarla()
    {
        _depositoRepository = new DepositoRepository(_context);
        _depositoLogica = new DepositoLogica(_depositoRepository);

        _usuarioRepository = new UsuarioRepository(_context);
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        _usuarioRepository.Agregar(cliente);
        var rangoFechaDep = new RangoFechas()
        {
            FechaInicio = DateTime.Today.AddDays(-10),
            FechaFin = DateTime.Today.AddDays(2)
        };
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){rangoFechaDep});
        _depositoLogica.Agregar(deposito);
        var rangoFechasRes = new RangoFechas()
        {
            FechaInicio = DateTime.Today.AddDays(-4),
            FechaFin = DateTime.Today.AddDays(-1)
        };
        var reserva = new Reserva(rangoFechasRes,deposito,cliente);
        _reservaLogica.Agregar(reserva);

        var idReserva = _reservaLogica.ObtenerReservasDe(cliente.Email)[0].Id;
        
        Assert.ThrowsException<ReservaLogicaExcepcion>((() => _reservaLogica.PagarReserva(idReserva)));
        Assert.IsFalse(_reservaLogica.ObtenerReservasDe(cliente.Email).Any());
    }
    
    [TestMethod]
    public void Intentar_Capturar_El_Pago_De_Una_Reserva_Habiendo_Una_Segunda_Reserva_Que_Ya_Esta_Aprobada_Deberia_Tirar_Excepcion_Y_Rechazar_La_Primera_Reserva()
    {
        _depositoRepository = new DepositoRepository(_context);
        _depositoLogica = new DepositoLogica(_depositoRepository);

        _usuarioRepository = new UsuarioRepository(_context);
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
        Usuario cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        _usuarioRepository.Agregar(cliente);
        var rangoFechaDep = new RangoFechas()
        {
            FechaInicio = DateTime.Today,
            FechaFin = DateTime.Today.AddDays(10)
        };
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){rangoFechaDep});
        _depositoLogica.Agregar(deposito);
        var rangoFechasRes1 = new RangoFechas()
        {
            FechaInicio = DateTime.Today.AddDays(1),
            FechaFin = DateTime.Today.AddDays(3)
        };
        
        var rangoFechasRes2 = new RangoFechas()
        {
            FechaInicio = DateTime.Today.AddDays(1),
            FechaFin = DateTime.Today.AddDays(3)
        };
        var reserva1 = new Reserva(rangoFechasRes1,deposito,cliente);
        _reservaLogica.Agregar(reserva1);
        
        _reservaLogica.PagarReserva(_reservaLogica.ObtenerTodas()[0].Id);
        _reservaLogica.CapturarPago(_reservaLogica.ObtenerTodas()[0].Id);
        var reserva2 = new Reserva(rangoFechasRes2,deposito,cliente);
        _reservaLogica.Agregar(reserva2);
        _reservaLogica.PagarReserva(_reservaLogica.ObtenerTodas()[1].Id);
        Assert.ThrowsException<ReservaLogicaExcepcion>((() =>
            _reservaLogica.CapturarPago(_reservaLogica.ObtenerTodas()[1].Id)));
        Assert.AreEqual(Estado.Rechazada,_reservaLogica.ObtenerTodas()[1].ConfAdmin);
    }

    [TestMethod]
    public void Calcular_Precio_Reserva()
    {
        RangoFechas rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(1));
        Deposito deposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){rango});
        
        CalculadoraPrecio calculadora = new CalculadoraPrecio(rango, deposito);
        var precio = calculadora.CalcularPrecio();

        var precioEsperado = 100;

        Assert.AreEqual(precio,precioEsperado);
    }
    
}