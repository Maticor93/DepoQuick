using Dominio;
using Dominio.Enums;
using LogicaDeNegocio;
using LogicaDeNegocio.ExcepcionesLogica;
using Repositories;

namespace LogicaDeNegocioTest;
[TestClass]
public class PromocionLogicaTest
{
    private PromocionRepository _promocionRepository;
    private EFContext _context;
    private PromocionLogica _promocionLogica;
    private readonly InMemoryEFContextFactory _contextFactory = new InMemoryEFContextFactory();
    
    //para los dos ultimos test que implican usar depositos
    private DepositoRepository _depositoRepository;
    private DepositoLogica _depositoLogica;

    [TestInitialize]
    public void SetUp()
    {
        _context = _contextFactory.CreateDbContext();
        _promocionRepository = new PromocionRepository(_context); 
        _promocionLogica = new PromocionLogica(_promocionRepository);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void Agregar_Una_Promocion_Test()
    {
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };

        var agregada = _promocionLogica.Agregar(promocion);
        
        Assert.AreEqual(promocion,agregada);

    }

    [TestMethod]
    public void Agregar_Promocion_Repetida_Deberia_Tirar_Excepcion()
    {
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };

        _promocionLogica.Agregar(promocion);

        Assert.ThrowsException<PromocionLogicaExcepcion>((() => _promocionLogica.Agregar(promocion)));
    }
    
    
    [TestMethod]
    public void Actualizar_Una_Promocion_Test()
    {
        var etiquetaOriginal = "a1b2";
        var desdeOriginal = DateTime.Today.AddMonths(-2);
        var hastaOriginal = DateTime.Today.AddDays(2);
        var promocion = new Promocion()
        {
            Etiqueta= etiquetaOriginal,
            Descuento = 10,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = desdeOriginal,
                FechaFin = hastaOriginal
            }
        };
        _promocionLogica.Agregar(promocion);

        promocion.Etiqueta = "a2b1";
        promocion.RangoFecha.FechaInicio = DateTime.Today.AddMonths(10);
        promocion.RangoFecha.FechaFin = DateTime.Today.AddMonths(12);
        Promocion actualizado = _promocionLogica.Actualizar(promocion);
        
        Assert.AreNotEqual(etiquetaOriginal,actualizado.Etiqueta);
        Assert.AreNotEqual(desdeOriginal,actualizado.RangoFecha.FechaInicio);
        Assert.AreNotEqual(hastaOriginal,actualizado.RangoFecha.FechaFin);

    }
    
    [TestMethod]
    public void Eliminar_Una_Promocion_Test()
    {
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };
        _promocionLogica.Agregar(promocion);
        var id = promocion.Id;
        _promocionLogica.Eliminar(promocion.Id);
        
        Assert.IsNull(_promocionLogica.EncontrarPorId(id));
    }
    
    [TestMethod]
    public void Obtener_Todas_Las_Promociones_Test()
    {
        List<Promocion> lista = _promocionLogica.ObtenerTodas();
        
        Assert.IsNotNull(lista);
        
        
    }

    [TestMethod]
    public void Intentar_Eliminar_Promocion_Asignada_A_Un_Deposito_Deberia_Tirar_Excepcion()
    {
        
        _depositoRepository = new DepositoRepository(_context);
        _depositoLogica = new DepositoLogica(_depositoRepository);
        
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };
        _promocionLogica.Agregar(promocion);
        List<Promocion> promociones = new List<Promocion>();
        promociones.Add(promocion);
        Deposito deposito = new Deposito("Nombre deposito",Area.A,Tamanio.Grande,true,promociones);
        _depositoLogica.Agregar(deposito);
        
        
        Assert.ThrowsException<PromocionLogicaExcepcion>((() => _promocionLogica.Eliminar(promocion.Id)));
    }


    [TestMethod]
    public void Deberia_De_Poder_Eliminar_Una_Promocion_Luego_De_Haber_Borrado_El_Deposito_Al_Cual_Estaba_Asignada()
    {
        _depositoRepository = new DepositoRepository(_context);
        _depositoLogica = new DepositoLogica(_depositoRepository);
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Prueba",
            Descuento = 10,
            RangoFecha = rango
        };
        _promocionLogica.Agregar(promocion);
        Deposito deposito = new Deposito("Nombre deposito",Area.A,Tamanio.Grande,true,new List<Promocion>(){promocion});
        _depositoLogica.Agregar(deposito);
        _depositoLogica.Eliminar(deposito.Id);
        
        int idPromocion = promocion.Id;
        _promocionLogica.Eliminar(promocion.Id);

        var promocionEliminada = _promocionLogica.EncontrarPorId(idPromocion);
        Assert.AreEqual(null, promocionEliminada);
    }
}