using Dominio;
using Dominio.ExcepcionesDominio;

namespace DominioTest;

[TestClass]
public class RangoFechasTest
{
    [TestMethod]
    public void Crear_Rango_de_Fechas_Test()
    {
        var newRango = new RangoFechas();
        
        Assert.IsNotNull(newRango);
    }
    
    [TestMethod]
    public void Crear_Rango_Con_Fechas_Test()
    {
        var ini = DateTime.Today;
        var fin = DateTime.Today.AddDays(2);
        
        var r = new RangoFechas(ini, fin);
        
        Assert.AreEqual(ini,r.FechaInicio);
        Assert.AreEqual(fin, r.FechaFin);
    }
    
    [TestMethod]
    public void Asignar_Fecha_De_Inicio_Test()
    {
        var r = new RangoFechas();
        var inicio = DateTime.Today;
        r.FechaInicio = inicio;
        Assert.AreEqual(inicio, r.FechaInicio);
    }
    
    [TestMethod]
    public void Asignar_Fecha_De_Fin_Test()
    {
        var r = new RangoFechas();
        var fin = DateTime.Today.AddDays(2);
        r.FechaFin = fin;
        Assert.AreEqual(fin, r.FechaFin);
    }
    
    [TestMethod]
    public void Crear_Rango_con_Fecha_Final_Anterior_a_Inicial_Test()
    {
        var r = new RangoFechas();
        var ini = DateTime.Today.AddDays(4);
        var fin = DateTime.Today.AddDays(2);
        
        Assert.ThrowsException<RangoFechasExcepcion>(() => r.ActualizarRangoFechas(ini, fin));
    }
    
    [TestMethod]
    public void Crear_Rango_con_Fecha_Inicial_Igual_a_Final_Deberia_Tirar_Excepcion()
    {

        var reserva = new Reserva();

        Assert.ThrowsException<ReservaExcepcion>((() =>
            reserva.RangoFecha = new RangoFechas(DateTime.Today, DateTime.Today)));
    }

    [TestMethod]
    public void Crear_Rango_con_Fecha_Inicial_Anterior_a_Fecha_Actual_Deberia_Tirar_Excepcion()
    {
        var r = new RangoFechas();
        var ini = DateTime.Today.AddDays(-2);
        var fin = DateTime.Today.AddDays(4);
        
        Assert.ThrowsException<RangoFechasExcepcion>(()=> r.ActualizarRangoFechas(ini, fin));
    }
    
    [TestMethod]
    public void Validar_si_fecha_Dentro_del_Rango_es_True()
    {   
        var ini = DateTime.Today.AddDays(2);
        var fin = DateTime.Today.AddDays(4);
        var r = new RangoFechas(ini, fin);
        
        var fecha = DateTime.Today.AddDays(3);
        
        Assert.IsTrue(r.PerteneceAlRango(fecha));
    }
    
    [TestMethod]
    public void Validar_si_fecha_Fuera_del_Rango_es_False()
    {   
        var ini = DateTime.Today.AddDays(2);
        var fin = DateTime.Today.AddDays(4);
        var r = new RangoFechas(ini, fin);
        
        var fecha = DateTime.Today.AddDays(1);
        
        Assert.IsFalse(r.PerteneceAlRango(fecha));
    }
    
    [TestMethod]
    public void Validar_si_fecha_Inicial_del_Rango_es_True()
    {   
        var ini = DateTime.Today.AddDays(2);
        var fin = DateTime.Today.AddDays(4);
        var r = new RangoFechas(ini, fin);
        
        var fecha = ini;
        
        Assert.IsTrue(r.PerteneceAlRango(fecha));
    }
    
    [TestMethod]
    public void Validar_si_fecha_Final_del_Rango_es_True()
    {   
        var ini = DateTime.Today.AddDays(2);
        var fin = DateTime.Today.AddDays(4);
        var r = new RangoFechas(ini, fin);
        
        var fecha = fin;
        
        Assert.IsTrue(r.PerteneceAlRango(fecha));
    }

    [TestMethod]
    public void Agregar_Rango_De_Fechas_Null_Deberia_Tirar_Excepcion()
    {
        var reserva = new Reserva();

        Assert.ThrowsException<ReservaExcepcion>((() => reserva.RangoFecha = null));
    }
    
}