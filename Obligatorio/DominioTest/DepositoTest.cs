using System.Net.NetworkInformation;
using Dominio;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace DominioTest;
[TestClass]
public class DepositoTest
{
    [TestMethod]
    public void Crear_Deposito_Test()
    {
        Deposito newDeposito = new Deposito();
        
        Assert.IsNotNull(newDeposito);
    }

    [TestMethod]
    public void Crear_Deposito_Con_Area_A_Test()
    {
        var newDeposito = new Deposito();

        var area = Area.A;
        newDeposito.Area = area;
        
        Assert.AreEqual(area,newDeposito.Area); 
    }

    [TestMethod]
    public void Crear_Deposito_Con_Tamanio_Mediano_Test()
    {
        var newDeposito = new Deposito();

        var tamanioMediano = Tamanio.Mediano;
        newDeposito.Tamanio = tamanioMediano;
        
        Assert.AreEqual(tamanioMediano,newDeposito.Tamanio);

    }

    [TestMethod]
    public void Crear_Deposito_Con_Climatizacion_Test()
    {
        var newDeposito = new Deposito();

        var climatizacion = true;
        newDeposito.Climatizacion = climatizacion;
        
        Assert.IsTrue(newDeposito.Climatizacion);

    }

    [TestMethod]
    public void Crear_Deposito_Con_Constructor_De_tres_Parametros_Test()
    {
        var area = Area.A;
        var tamanio = Tamanio.Mediano;
        var climatizacion = true;

        var newDeposito = new Deposito(area, tamanio, climatizacion);
        
        Assert.IsNotNull(newDeposito);
    }

    [TestMethod]
    public void Agregar_Promociones_A_Deposito_Test()
    {
        var listaPromociones = new List<Promocion>();
        var promocion1 = new Promocion();
        var promocion2 = new Promocion();
        listaPromociones.Add(promocion1);
        listaPromociones.Add(promocion2);


        var newDeposito = new Deposito(Area.A, Tamanio.Grande, false, listaPromociones);
        
        Assert.IsNotNull(newDeposito);
    }

    [TestMethod]
    public void Al_Crear_Deposito_Sin_Asignarle_Promociones_Deberia_Tener_Lista_Vacia_De_Promociones_Test()
    {
        var newDeposito = new Deposito();
        Assert.IsNotNull(newDeposito.Promociones);
    }

    [TestMethod]
    public void Agregar_Nombre_A_Deposito_Test()
    {
        var nombre = "Nombre";
        var nuevoDeposito = new Deposito();
        
        nuevoDeposito.Nombre = nombre;
        
        Assert.AreEqual(nombre,nuevoDeposito.Nombre);
    }

    [TestMethod]
    public void Agrego_Nombre_Vacio_A_Deposito_Deberia_Tirar_Excepcion()
    {
        var nombre = "";
        
        var nuevoDeposito = new Deposito();

        Assert.ThrowsException<DepositoExcepcion>((() => nuevoDeposito.Nombre = nombre));
    }

    [TestMethod]
    public void Agregar_Nombre_Nulo_A_Deposito_Deberia_Tirar_Excepcion()
    {
        String nombre = null;
        
        var nuevoDeposito = new Deposito();

        Assert.ThrowsException<DepositoExcepcion>((() => nuevoDeposito.Nombre = nombre));
    }

    [TestMethod]
    public void Agregar_Nombre_Con_Numeros_Deberia_Tirar_Excepcion()
    {
        String nombre = "Nombre Deposito 1";

        var nuevoDeposito = new Deposito();

        Assert.ThrowsException<DepositoExcepcion>((() => nuevoDeposito.Nombre = nombre));
    }

    [TestMethod]
    public void Agregar_Nombre_Con_Caracteres_Especiales_Deberia_Tirar_Excepcion()
    {
        String nombre = "#Nombre! Depo$it@";

        var nuevoDeposito = new Deposito();

        Assert.ThrowsException<DepositoExcepcion>((() => nuevoDeposito.Nombre = nombre));
    }

    [TestMethod]
    public void Agregar_Nombre_Sin_Caracteres_Pero_Solo_Con_Espacios_Deberia_Tirar_Excepcion()
    {
        String nombre = "      ";

        var nuevoDeposito = new Deposito();

        Assert.ThrowsException<DepositoExcepcion>((() => nuevoDeposito.Nombre = nombre));
    }

    [TestMethod]
    public void Crear_Deposito_Con_Nombre_En_Constructor()
    {
        var depositoConNombre = new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>());
        
        Assert.IsNotNull(depositoConNombre);
    }

    [TestMethod]
    public void Agregar_Un_Rango_De_Fechas_Para_La_Disponibilidad_De_Un_Deposito()
    {
        var deposito = new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>());

        var rangoFechas = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        deposito.Disponibilidad.Add(rangoFechas);
        
        Assert.AreEqual(rangoFechas,deposito.Disponibilidad[0]);
    }

    [TestMethod]
    public void Agregar_Dos_Rangos_De_Fechas_Iguales_Deberia_Triar_Excepcion()
    {
        var deposito = new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>());

        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var rangoFechas2 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        deposito.AgregarRangoFecha(rangoFechas1);
        Assert.ThrowsException<DepositoExcepcion>((() => deposito.AgregarRangoFecha(rangoFechas2)));

    }

    [TestMethod]
    public void Agregar_Rango_De_Fecha_Contenido_En_Otro_Rango_Pero_No_Igual_Deberia_Tirar_Excepcion()
    {
        var deposito = new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>());

        var rangoFechas1 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(6));
        var rangoFechas2 = new RangoFechas(DateTime.Today.AddDays(1), DateTime.Today.AddDays(4));
        deposito.AgregarRangoFecha(rangoFechas1);
        Assert.ThrowsException<DepositoExcepcion>(()=> deposito.AgregarRangoFecha(rangoFechas2));
    }
    

    [TestMethod]
    public void Agregar_Lista_Con_Rangos_De_Fecha_Desde_Constructor()
    {
        var rangoFechas1 = new RangoFechas(DateTime.Today.AddDays(1), DateTime.Today.AddDays(4));
        var rangoFechas2 = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(6));
        List<RangoFechas> disponibilidad = new List<RangoFechas>(){rangoFechas1,rangoFechas2};
        
        var deposito =  new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>(),disponibilidad);
        
        Assert.IsNotNull(deposito);
    }
    
    [TestMethod]
    public void Agregar_Rango_Fechas_Null_Deberia_Tirar_Excepcion()
    {
        var deposito =  new Deposito("nombre", Area.A, Tamanio.Grande, true, new List<Promocion>());
        
        Assert.ThrowsException<DepositoExcepcion>((() => deposito.AgregarRangoFecha(null)));
    }
}