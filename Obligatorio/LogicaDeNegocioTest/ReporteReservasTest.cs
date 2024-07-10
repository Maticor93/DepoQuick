using Dominio;
using Dominio.Enums;
using LogicaDeNegocio;
using Repositories;

namespace LogicaDeNegocioTest;

[TestClass]
public class ReporteReservaTest
{
    private ReservaRepository _reservaRepository;
    private EFContext _context;
    private ReservaLogica _reservaLogica;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();

    private DepositoRepository _depositoRepository;
    private DepositoLogica _depositoLogica;

    private PromocionRepository _promocionRepository;
    private PromocionLogica _promocionLogica;

    private UsuarioRepository _usuarioRepository;
    private UsuarioLogica _usuarioLogica;

    private String _pathReporte;
    private String _pathCsv;
    private String _pathTxt;


    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _reservaRepository = new ReservaRepository(_context);
        _reservaLogica = new ReservaLogica(_reservaRepository);
        _depositoRepository = new DepositoRepository(_context);
        _depositoLogica = new DepositoLogica(_depositoRepository);
        _usuarioRepository = new UsuarioRepository(_context);
        _usuarioLogica = new UsuarioLogica(_usuarioRepository);
        _pathReporte = "../../../../../Reportes";
        _pathCsv = _pathReporte +"/ReporteReservas.csv";
        _pathTxt = _pathReporte + "/ReporteReservas.txt";
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Generar_Reporte_Con_Reserva_No_Pagada_En_Formato_Csv()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reserva = new Reserva(rangoFechas, deposito, usuario);
        _reservaLogica.Agregar(reserva);
        
        var generadorReporteTxt = new GeneradorReporteCsv(_pathReporte);
        generadorReporteTxt.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReserva = $"{rangoFechas.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas.FechaFin.ToString("dd/MM/yyyy")}";
        
        Assert.AreEqual($"{deposito.Nombre},{rangoFechaReserva},{usuario.Email},${reserva.Precio},Sin pago",File.ReadLines(_pathCsv).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathCsv);
    }

    [TestMethod]
    public void Generar_Reporte_Con_Reserva_Pagada_En_Formato_Csv()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddMonths(4));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reservaPagada = new Reserva(rangoFechas1, deposito, usuario);
        _reservaLogica.Agregar(reservaPagada);
        _reservaLogica.PagarReserva(_context.Reserva.ToList()[0].Id);
        
        var generadorReporteCsv = new GeneradorReporteCsv(_pathReporte);
        generadorReporteCsv.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReservaPagada = $"{rangoFechas1.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas1.FechaFin.ToString("dd/MM/yyyy")}";
        String linea = $"{deposito.Nombre},{rangoFechaReservaPagada},{usuario.Email},${reservaPagada.Precio},Reservado";
        Assert.AreEqual(linea,File.ReadLines(_pathCsv).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathCsv);
    }

    [TestMethod]
    public void Generar_Reporte_Con_Reserva_Pagada_Y_Capturada_En_Formato_Csv()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddMonths(4));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reservaPagada = new Reserva(rangoFechas1, deposito, usuario);
        _reservaLogica.Agregar(reservaPagada);
        
        _reservaLogica.PagarReserva(_context.Reserva.ToList()[0].Id);
        _reservaLogica.CapturarPago(_context.Reserva.ToList()[0].Id);
        var generadorReporteCsv = new GeneradorReporteCsv(_pathReporte);
        generadorReporteCsv.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReservaPagada = $"{rangoFechas1.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas1.FechaFin.ToString("dd/MM/yyyy")}";
        String linea = $"{deposito.Nombre},{rangoFechaReservaPagada},{usuario.Email},${reservaPagada.Precio},Capturado";
        
        Assert.AreEqual(linea,File.ReadLines(_pathCsv).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathCsv);
    }

    [TestMethod]
    public void Generar_Reporte_Con_Una_Reserva_Pagada_En_Formato_Txt()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddMonths(4));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reservaPagada = new Reserva(rangoFechas1, deposito, usuario);
        _reservaLogica.Agregar(reservaPagada);
        
        _reservaLogica.PagarReserva(_context.Reserva.ToList()[0].Id);
        var generadorReporteCsv = new GeneradorReporteTxt(_pathReporte);
        generadorReporteCsv.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReservaPagada = $"{rangoFechas1.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas1.FechaFin.ToString("dd/MM/yyyy")}";
        String lineaR1 = $"{deposito.Nombre}\t{rangoFechaReservaPagada}\t{usuario.Email}\t${reservaPagada.Precio}\tReservado";

        Assert.AreEqual(lineaR1,File.ReadLines(_pathTxt).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathTxt);
    }
    [TestMethod]
    public void Generar_Reporte_Con_Una_Reserva_Capturada_En_Formato_Txt()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddMonths(4));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reservaPagada = new Reserva(rangoFechas1, deposito, usuario);
        _reservaLogica.Agregar(reservaPagada);
        
        _reservaLogica.PagarReserva(_context.Reserva.ToList()[0].Id);
        _reservaLogica.CapturarPago(_context.Reserva.ToList()[0].Id);
        var generadorReporteTxt = new GeneradorReporteTxt(_pathReporte);
        generadorReporteTxt.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReservaPagada = $"{rangoFechas1.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas1.FechaFin.ToString("dd/MM/yyyy")}";
        String lineaR1 = $"{deposito.Nombre}\t{rangoFechaReservaPagada}\t{usuario.Email}\t${reservaPagada.Precio}\tCapturado";

        Assert.AreEqual(lineaR1,File.ReadLines(_pathTxt).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathTxt);
    }
    
    [TestMethod]
    public void Generar_Reporte_Con_Una_Reserva_Sin_Pago_En_Formato_Txt()
    {
        var usuario = new Usuario("Email@gmail.com", "nombre", "Password1#", Rol.Cliente);
        var rangoFechasDepo = new RangoFechas(DateTime.Today, DateTime.Today.AddMonths(4));
        var deposito = new Deposito("nombreDeposito",Area.A, Tamanio.Mediano, true,new List<Promocion>(),new List<RangoFechas>(){rangoFechasDepo});
        _usuarioLogica.Agregar(usuario);
        _depositoLogica.Agregar(deposito);
        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var reservaPagada = new Reserva(rangoFechas1, deposito, usuario);
        _reservaLogica.Agregar(reservaPagada);
        
        var generadorReporteTxt = new GeneradorReporteTxt(_pathReporte);
        generadorReporteTxt.GenerarReporte(_reservaLogica.ObtenerTodas());
        String rangoFechaReservaPagada = $"{rangoFechas1.FechaInicio.ToString("dd/MM/yyyy")}-{rangoFechas1.FechaFin.ToString("dd/MM/yyyy")}";
        String lineaR1 = $"{deposito.Nombre}\t{rangoFechaReservaPagada}\t{usuario.Email}\t${reservaPagada.Precio}\tSin pago";

        Assert.AreEqual(lineaR1,File.ReadLines(_pathTxt).Skip(1).Take(1).FirstOrDefault());
        
        File.Delete(_pathTxt);
    }

    [TestMethod]
    public void Obtener_Generador_De_Reportes_Txt_A_Usando_La_Fabrica_De_Generadores_De_Reporte_De_Reserva()
    {
        var fabrica = new FabricaGeneradorReporte(_pathReporte);
        IGeneradorReporte generador = fabrica.CrearGeneradorReporte("TXT");
        
        Assert.IsNotNull(generador);
    }
    
    [TestMethod]
    public void Obtener_Generador_De_Reportes_Csv_A_Usando_La_Fabrica_De_Generadores_De_Reporte_De_Reserva()
    {
        var fabrica = new FabricaGeneradorReporte(_pathReporte);
        IGeneradorReporte generador = fabrica.CrearGeneradorReporte("CSV");
        
        Assert.IsNotNull(generador);
    }
    
    [TestMethod]
    public void Si_La_Opcion_Pasada_A_La_Fabrica_No_Esta_Entre_Las_Opciones_Deberia_Tirar_Excepcion()
    {
        var fabrica = new FabricaGeneradorReporte(_pathReporte);
        
        Assert.ThrowsException<Exception>(()=>fabrica.CrearGeneradorReporte("OpcionNoValida"));
    }
    
    
}
