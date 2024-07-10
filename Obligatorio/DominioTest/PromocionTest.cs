using Dominio;
using Dominio.ExcepcionesDominio;

namespace DominioTest;

[TestClass]
public class PromocionTest
{
    [TestMethod]
    public void Crear_Promocion_Test()
    {
        var newPromocion = new Promocion();
        
        Assert.IsNotNull(newPromocion);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Test()
    {
        var p = new Promocion();
        var etiqueta = "etiqueta de ejemplo";
        p.Etiqueta = etiqueta;
        Assert.AreEqual(etiqueta,p.Etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Vacia_Test()
    {
        var p = new Promocion();
        var etiqueta = "";
        Assert.ThrowsException<PromocionExcepcion>(() => p.Etiqueta = etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Null_Test()
    {
        var p = new Promocion();
        string etiqueta = null;
        Assert.ThrowsException<PromocionExcepcion>(() => p.Etiqueta = etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Alfanumerica_Test()
    {
        var p = new Promocion();
        var etiqueta = "3s7oe5A1f4n9m3r1c0";
        p.Etiqueta = etiqueta;
        Assert.AreEqual(etiqueta,p.Etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Con_Espacios_Test()
    {
        var p = new Promocion();
        var etiqueta = "e s p a c i o s    ";
        p.Etiqueta = etiqueta;
        Assert.AreEqual(etiqueta,p.Etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Con_Simbolos_Test()
    {
        var p = new Promocion();
        var etiqueta = "!@#$%^&*()_+{}|:<>=";
        p.Etiqueta = etiqueta;
        Assert.AreEqual(etiqueta,p.Etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Con_Caracteres_Especiales_Test()
    {
        var p = new Promocion();
        var etiqueta = "áéíóúñ";
        p.Etiqueta = etiqueta;
        Assert.AreEqual(etiqueta,p.Etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Etiqueta_Con_Mas_De_20_Caracteres_Test()
    {
        var p = new Promocion();
        var etiqueta = "estaetiquetatienemasde20caracteres";
        Assert.ThrowsException<PromocionExcepcion>(() => p.Etiqueta = etiqueta);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Descuento_Test()
    {
        var p = new Promocion();
        var descuento = 45;
        p.Descuento = descuento;
        Assert.AreEqual(descuento,p.Descuento);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Descuento_Menor_A_5_Test()
    {
        var p = new Promocion();
        var descuento = 4;
        Assert.ThrowsException<PromocionExcepcion>(() => p.Descuento = descuento);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Descuento_Mayor_A_75_Test()
    {
        var p = new Promocion();
        var descuento = 76;
        Assert.ThrowsException<PromocionExcepcion>(() => p.Descuento = descuento);
    }

    [TestMethod]
    public void Crear_Promocion_Con_Descuento_Maximo_Test()
    {
        var p = new Promocion();
        var descuento = 75;
        p.Descuento = descuento;
        Assert.AreEqual(descuento, p.Descuento);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Descuento_Minimo_Test()
    {
        var p = new Promocion();
        var descuento = 5;
        p.Descuento = descuento;
        Assert.AreEqual(descuento, p.Descuento);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Rango_de_Fechas_1_Test()
    {
        var p = new Promocion();
        var inicio = DateTime.Today;
        var fin = DateTime.Today.AddDays(2);
        
        var rango = new RangoFechas(inicio, fin);
        p.RangoFecha = rango;
        
        Assert.AreEqual(rango.FechaInicio, p.RangoFecha.FechaInicio);
        Assert.AreEqual(rango.FechaFin, p.RangoFecha.FechaFin);
    }
    
    [TestMethod]
    public void Crear_Promocion_Con_Rango_de_Fechas_2_Test()
    {
        var p = new Promocion();
        var inicio = DateTime.Today;
        var fin = DateTime.Today.AddDays(2);
        
        var rango = new RangoFechas(inicio, fin);
        
        p.RangoFecha = rango;
        
        Assert.AreEqual(rango.FechaInicio, p.RangoFecha.FechaInicio);
        Assert.AreEqual(rango.FechaFin, p.RangoFecha.FechaFin);
    }
    
    [TestMethod] public void Crear_Promocion_Con_Fecha_Final_Igual_A_Fecha_Inicial_Test()
    {
        var desde = DateTime.Today.AddDays(2);
        var hasta = DateTime.Today.AddDays(2);
        var p = new Promocion()
        {
            RangoFecha = new RangoFechas(desde,hasta)
        };
        Assert.AreEqual(p.RangoFecha.FechaFin , desde);
        Assert.AreEqual(p.RangoFecha.FechaInicio , hasta);
    }
    
    
    [TestMethod]
    public void Crear_Promocion_Desde_Constructor_Test()
    {
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(10));
        var p = new Promocion("a1b2", 10, rango);
        var p2 = new Promocion();
        p2.Etiqueta = "a1b2";
        p2.Descuento = 10;
        var inicio = DateTime.Today;
        var fin = DateTime.Today.AddDays(10);
        p2.RangoFecha = new RangoFechas(inicio, fin);
        
        Assert.AreEqual(p.Etiqueta, p2.Etiqueta);
        Assert.AreEqual(p.Descuento, p2.Descuento);
        var rango1 = p.RangoFecha;
        var rango2 = p2.RangoFecha;
        Assert.AreEqual(rango1.FechaFin , rango2.FechaFin);
        Assert.AreEqual(rango1.FechaInicio , rango2.FechaInicio);
    }

    [TestMethod]
    public void Asignar_Etiqueta_Sin_Caracteres_Pero_Con_Espacios_Deberia_Tirar_Excepcion()
    {
        var promocion = new Promocion();

        var etiquetaVaciaConEspacios = "             ";

        Assert.ThrowsException<PromocionExcepcion>((() => promocion.Etiqueta = etiquetaVaciaConEspacios));
    }
    
    [TestMethod]
    public void Agregarle_Deposito_A_Poromocion()
    {
        var promocion = new Promocion();
        var deposito = new Deposito();
        
        promocion.Depositos.Add(deposito);
        
        Assert.AreEqual(deposito, promocion.Depositos[0]);
    }

    [TestMethod]
    public void Asignar_Rango_De_Fechas_Null_Deberia_Tirar_Excepcion()
    {
        var promocion = new Promocion();

        Assert.ThrowsException<PromocionExcepcion>((() => promocion.RangoFecha = null));
    }
}
