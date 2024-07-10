using Dominio;
using LogicaDeNegocio;
using Dominio.Enums;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocioTest;

[TestClass]
public class DepositoLogicaTest
{
    
    private DepositoRepository _depositoRepository;
    private EFContext _context;
    private DepositoLogica _depositoLogica;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();
    
    //para los dos ultimos test que implican usar promociones y reservas
    private PromocionRepository _promocionRepository;
    private PromocionLogica _promocionLogica;

    private ReservaRepository _reservaRepository;
    private ReservaLogica _reservaLogica;
    
    private UsuarioRepository _usuarioRepository;
    private UsuarioLogica _usuarioLogica;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _depositoRepository = new DepositoRepository(_context); 
        _depositoLogica = new DepositoLogica(_depositoRepository);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void Agregar_Un_Deposito_Test()
    {
        Deposito newDeposito = new Deposito()
        {
            Nombre = "Nombre Deposito"
        };
        Deposito depositoAgregado = _depositoLogica.Agregar(newDeposito);
        
        Assert.AreEqual(newDeposito,depositoAgregado);
    }
    
    [TestMethod]
    public void Agregar_Deposito_Repetido_Deberia_Tirar_Excepcion()
    {
        Deposito deposito = new Deposito()
        {
            Nombre = "Nombre Deposito"
        };

        _depositoLogica.Agregar(deposito);

       Assert.ThrowsException<DepositoLogicaExcepcion>((() => _depositoLogica.Agregar(deposito)));

    }
    
    
    [TestMethod]
    public void Actualizar_Un_Deposito_Test()
    {
        Area areaOriginal = Area.B;
        Tamanio tamanioOriginal = Tamanio.Grande;
        Deposito deposito = new Deposito("Nombre deposito",areaOriginal,tamanioOriginal,true, new List<Promocion>());
        _depositoLogica.Agregar(deposito);
        
        deposito.Tamanio = Tamanio.Mediano;
        deposito.Area = Area.C;
        Deposito actualizado = _depositoLogica.Actualizar(deposito);
        
        Assert.AreNotEqual(areaOriginal,actualizado.Area);
        Assert.AreNotEqual(tamanioOriginal, actualizado.Tamanio);
    }

    [TestMethod]
    public void Eliminar_Un_Deposito_Test()
    {
        Deposito deposito = new Deposito()
        {
            Nombre = "Nombre deposito"
        };
        _depositoLogica.Agregar(deposito);
        var id = deposito.Id;
        _depositoLogica.Eliminar(id);
        
        Assert.IsNull(_depositoLogica.EncontrarPorId(id));
    }

    [TestMethod]
    public void Obtener_Todos_Los_Depositos_Test()
    {
        List<Deposito> lista = _depositoLogica.ObtenerTodos();
        
        Assert.IsNotNull(lista);
    }

    [TestMethod]
    public void Asigno_Una_Promocion_A_Un_Deposito_Y_Obtengo_Una_Lista_De_Depositos_Que_Contienen_Esa_Promocion_Test()
    {
        _promocionRepository = new PromocionRepository(_context);
        _promocionLogica = new PromocionLogica(_promocionRepository);
        
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };
        _promocionLogica.Agregar(promocion);
        var listaProm = new List<Promocion>() { promocion };
        var deposito = new Deposito("Nombre deposito",Area.A, Tamanio.Grande, false, listaProm);
        _depositoLogica.Agregar(deposito);
        
        List<Deposito> listaDepositosAsociadasAPromocion = _depositoLogica.ObtenerDepositosAsociadosA(promocion.Id);
        
        Assert.AreEqual(deposito,listaDepositosAsociadasAPromocion[0]);
    }

    [TestMethod]
    public void Intentar_Eliminar_Deposito_Asignado_A_Reserva_Activa_Deberia_Tirar_Excepcion()
    {
        _reservaRepository = new ReservaRepository(_context);
        _reservaLogica = new ReservaLogica(_reservaRepository);
        var deposito = new Deposito()
        {
            Nombre = "Nombre deposito"
        };
        _depositoLogica.Agregar(deposito);
        var rangoFecha = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reserva = new Reserva(rangoFecha);
        reserva.Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        reserva.Deposito = deposito;
        
        _reservaLogica.Agregar(reserva);
        var reservasDeDeposito = _reservaLogica.ObtenerReservasAsociadasA(deposito.Id);
        Assert.ThrowsException<DepositoLogicaExcepcion>((() => _depositoLogica.EliminarDepositoEnCasoDeNoEstarReservado(deposito.Id,reservasDeDeposito)));
    }
    
    [TestMethod]
    public void Eliminar_Deposito_Asignado_A_Reserva_No_Activa()
    {
        _reservaRepository = new ReservaRepository(_context);
        _reservaLogica= new ReservaLogica(_reservaRepository);
        var deposito = new Deposito()
        {
            Nombre = "Nombre deposito"
        };
        _depositoLogica.Agregar(deposito);
        var reserva = new Reserva()
        {
            Cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente),
            Deposito = deposito,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = DateTime.Today.AddDays(-3),
                FechaFin = DateTime.Today.AddDays(-1)
            }
        };
        int idDeposito = deposito.Id;
        _reservaLogica.Agregar(reserva);
        
        var reservasDeDeposito = _reservaLogica.ObtenerReservasAsociadasA(idDeposito);
        _depositoLogica.EliminarDepositoEnCasoDeNoEstarReservado(idDeposito,reservasDeDeposito);
        
        Assert.IsNull(_depositoLogica.EncontrarPorId(idDeposito));
    }


    [TestMethod]
    public void Eliminar_Deposito_Asignado_A_Reserva_Rechazada_Deberia_Borrarse()
    {
        _reservaRepository = new ReservaRepository(_context);
        _reservaLogica= new ReservaLogica(_reservaRepository);
        _usuarioRepository = new UsuarioRepository(_context);
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
        var nombre = "Nombre deposito";
        var cliente = new Usuario("direccion@dom.com","nombre apellido","Password1@",Rol.Cliente);
        var deposito = new Deposito(nombre,Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(DateTime.Today, DateTime.Today.AddDays(9))});
        _depositoLogica.Agregar(deposito);
        _usuarioLogica.Agregar(cliente);
        var rangoFecha = new RangoFechas(DateTime.Today.AddDays(4), DateTime.Today.AddDays(8));
        var reserva = new Reserva(rangoFecha,deposito,cliente);

        reserva.ConfAdmin = Estado.Rechazada;
        _reservaLogica.Agregar(reserva);
        
        var idDeposito = _context.Deposito.FirstOrDefault(d => d.Nombre == nombre).Id;
        var reservasDeDeposito = _reservaLogica.ObtenerReservasAsociadasA(idDeposito);
        _depositoLogica.EliminarDepositoEnCasoDeNoEstarReservado(idDeposito,reservasDeDeposito);
        
        Assert.IsNull(_depositoLogica.EncontrarPorId(idDeposito));
    }

}