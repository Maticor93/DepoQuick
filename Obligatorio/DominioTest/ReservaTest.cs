using Dominio;
using Dominio.Enums;
using Dominio.ExcepcionesDominio;

namespace DominioTest;
[TestClass]
public class ReservaTest
{
    
    [TestMethod]
    public void Crear_Reserva_Test()
    {
        Reserva newReserva = new Reserva();
        
        Assert.IsNotNull(newReserva);
    }

    [TestMethod]
    public void Crear_Reserva_Con_Rango_de_Fechas_1_Test()
    {
        Reserva newReserva = new Reserva();
        var fechaInicio = DateTime.Today.AddDays(1);
        var fechaFin = DateTime.Today.AddDays(2);
        
        newReserva.RangoFecha = new RangoFechas(fechaInicio, fechaFin);
        
        Assert.AreEqual(newReserva.RangoFecha.FechaInicio,fechaInicio);
        Assert.AreEqual(newReserva.RangoFecha.FechaFin,fechaFin);
    }
    
    [TestMethod]
    public void Crear_Reserva_Con_Rango_de_Fechas_2_Test()
    {
        Reserva newReserva = new Reserva();

        var fechaInicio = DateTime.Today.AddDays(1);
        var fechaFin = DateTime.Today.AddDays(2);
        
        var rango = new RangoFechas(fechaInicio, fechaFin);
        
        newReserva.RangoFecha = rango;
        
        Assert.AreEqual(newReserva.RangoFecha.FechaInicio,fechaInicio);
        Assert.AreEqual(newReserva.RangoFecha.FechaFin,fechaFin);
    }

    [TestMethod]
    public void Crear_Reserva_Con_Fecha_Inicial_Menor_A_Fecha_Actual_Deberia_Tirar_Excepcion()
    {
        var newReserva = new Reserva();
        
        var fechaInicio = DateTime.Today.AddDays(-2);
        var fechaFinal = fechaInicio.AddDays(2);
        
        Assert.ThrowsException<RangoFechasExcepcion>((() => newReserva.RangoFecha = new RangoFechas(fechaInicio, fechaFinal)));
    }

    [TestMethod]
    public void Crear_Reserva_Con_Confirmacion_De_Administrador_Test()
    {
        var newReserva = new Reserva();

        var administradorConf = Estado.Aprobada;
        newReserva.ConfAdmin = administradorConf;
        
        Assert.AreEqual(administradorConf, newReserva.ConfAdmin);

    }
    

    [TestMethod]
    public void Crear_Reserva_Usando_Contructor_Con_Un_Parametro_Test()
    {
        var fechaInicio = DateTime.Today;
        var fechaFinal = fechaInicio.AddDays(5);
        var rangoFecha = new RangoFechas(fechaInicio, fechaFinal);
        var newReserva = new Reserva(rangoFecha);
        
        Assert.IsNotNull(newReserva);
    }

    [TestMethod]
    public void Crear_Reserva_Asignandole_Cliente_Test()
    {
        var newCliente = new Usuario();
        var newReserva = new Reserva()
        {
            Cliente = newCliente
        };
        
        Assert.AreEqual(newCliente,newReserva.Cliente);
    }

    [TestMethod]
    public void Crear_Reserva_Usando_Constructor_Con_Tres_Parametros_Test()
    {
        var fechaInicial = DateTime.Today;
        var fechaFinal = DateTime.Today.AddDays(4);
        var deposito = new Deposito();
        deposito.AgregarRangoFecha(new RangoFechas(fechaInicial,fechaFinal));
        var cliente = new Usuario();
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        
        var reserva = new Reserva(rangoFecha, deposito, cliente);
        
        Assert.IsNotNull(reserva);

    }

    [TestMethod]
    public void Calcular_Precio_Al_Reservar_Deposito_De_Tamanio_Pequenio_Con_Duracion_De_Seis_Dias_Test()
    {
        var tamanio = Tamanio.Pequeño;
        var fechaActual = DateTime.Today;
        var seisDiasMasDeFechaActual = fechaActual.AddDays(6);
        var newDeposito = new Deposito("nombre",Area.A,tamanio,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,seisDiasMasDeFechaActual)});
        var precioDepositoPequenioPorDia = 50;
        var rangoFecha = new RangoFechas(fechaActual, seisDiasMasDeFechaActual);

        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        double precio = 6 * precioDepositoPequenioPorDia;
        
        Assert.AreEqual(precio,newReserva.Precio);
    }

    [TestMethod]
    public void Calcular_Precio_Al_Reservar_Deposito_De_Tamanio_Mediano_Con_Duracion_De_Cinco_Dias_Test()
    {
        var tamanio = Tamanio.Mediano;
        var fechaActual = DateTime.Today;
        var cincoDiasMasDeFechaActual = fechaActual.AddDays(5);
        var newDeposito = new Deposito("nombre",Area.A,tamanio,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,cincoDiasMasDeFechaActual)});
        var precioDepositoMedianoPorDia = 75;
        var rangoFecha = new RangoFechas(fechaActual, cincoDiasMasDeFechaActual);

        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        double precio = 5 * precioDepositoMedianoPorDia;
        
        Assert.AreEqual(precio,newReserva.Precio);
    }
    
    
    [TestMethod]
    public void Calcular_Precio_Al_Reservar_Deposito_De_Tamanio_Grande_Con_Duracion_De_Cuatro_Dias_Test()
    {
        var tamanio = Tamanio.Grande;
        var fechaActual = DateTime.Today;
        var cuatroDiasMasDeFechaActual = fechaActual.AddDays(4);
        var newDeposito = new Deposito("nombre",Area.A,tamanio,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,cuatroDiasMasDeFechaActual)});
        var precioDepositoGrandePorDia = 100;
        var rangoFecha = new RangoFechas(fechaActual, cuatroDiasMasDeFechaActual);

        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        double precio = 4 * precioDepositoGrandePorDia;
        
        Assert.AreEqual(precio,newReserva.Precio);
    }

    [TestMethod]
    public void Al_Reservar_Deposito_Por_7_Dias_O_Mas_Deberia_Aplicar_Cinco_Por_Ciento_De_Descuento_Test()
    {
        
        var tamanio = Tamanio.Grande;
        var fechaActual = DateTime.Today;
        var sieteDiasMasDeFechaActual = fechaActual.AddDays(7);
        var newDeposito = new Deposito("nombre",Area.A,tamanio,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,sieteDiasMasDeFechaActual)});
        var precioDepositoGrandePorDia = 100;
        var rangoFecha = new RangoFechas(fechaActual, sieteDiasMasDeFechaActual);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        double descuento = 5;
        
        double precio = 7 * precioDepositoGrandePorDia;
        double cantidadADescontar = (descuento * precio) / 100;
        precio -= cantidadADescontar;
        
        Assert.AreEqual(precio,newReserva.Precio);
    }

    [TestMethod]
    public void Al_Reservar_Deposito_Por_15_Dias_O_Mas_Deberia_Aplicar_Diez_Por_Ciento_De_Descuento_Test()
    {
        var tamanio = Tamanio.Grande;
        var fechaActual = DateTime.Today;
        var quinceDiasMasDeFechaActual = fechaActual.AddDays(15);
        var newDeposito = new Deposito("nombre",Area.A,Tamanio.Grande,false,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,quinceDiasMasDeFechaActual)});
        var precioDepositoGrandePorDia = 100;
        var rangoFecha = new RangoFechas(fechaActual, quinceDiasMasDeFechaActual);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        double descuento = 10;
        
        double precio = 15 * precioDepositoGrandePorDia;
        double cantidadADescontar = (descuento * precio) / 100;
        precio -= cantidadADescontar;
        
        Assert.AreEqual(precio,newReserva.Precio);
    }

    [TestMethod]
    public void Al_Reservar_Deposito_Con_Climatizacion_Agregar_Costo_Adicional_Por_Dia_Test()
    {
        var tamanio = Tamanio.Grande;
        var climatizacion = true;
        var fechaActual = DateTime.Today;
        var cincoDiasMasDeFechaActual = fechaActual.AddDays(5);
        var newDeposito = new Deposito("nombre",Area.A,tamanio,climatizacion,new List<Promocion>(),new List<RangoFechas>(){new RangoFechas(fechaActual,cincoDiasMasDeFechaActual)});
        var rangoFecha = new RangoFechas(fechaActual, cincoDiasMasDeFechaActual);
        
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        
        var precioDepositoGrandePorDia = 100;
        var precioDepositoGrandeMasClimtaizacionPorDia = precioDepositoGrandePorDia + 20;
        var precio = 5 * precioDepositoGrandeMasClimtaizacionPorDia;
        
        Assert.AreEqual(precio,newReserva.Precio);


    }

    [TestMethod]
    public void Al_Reservar_Deposito_Con_Promociones_Con_Fecha_De_Inicio_Menor_A_Hoy_Deberia_Aplicar_Sus_Descuentos_Test()
    {
        var fechaIni1 = DateTime.Today.AddDays(-2);
        var fechaFin1 = DateTime.Today.AddDays(2);
        var promocion1 = new Promocion()
        {
            Etiqueta = "a1b2", 
            Descuento = 10,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = fechaIni1,
                FechaFin = fechaFin1
            }
        };
        
        var fechaIni2 = DateTime.Today.AddDays(-1);
        var fechaFin2 = DateTime.Today.AddDays(1);
        var promocion2 = new Promocion()
        {
            Etiqueta = "a1b3",
            Descuento = 5,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = fechaIni2,
                FechaFin = fechaFin2
            }
        };
        
        List<Promocion> listaPromociones = new List<Promocion>(){ promocion1, promocion2 };
        var fechaInicial = DateTime.Today.AddDays(6);
        var fechaFinal = DateTime.Today.AddDays(10);
        var newDeposito = new Deposito("Nombre",Area.A, Tamanio.Grande, false, listaPromociones,new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        
        var precioDepositoGrandePorDia = 100;
        var diffDias = fechaFinal - fechaInicial;
        double dias = (int)diffDias.TotalDays;
        double precioSinDescuento = dias * precioDepositoGrandePorDia;
        double precioDescuentos = precioSinDescuento - (precioSinDescuento * promocion1.Descuento) / 100;
        precioDescuentos = precioDescuentos - (precioDescuentos * promocion2.Descuento) / 100;
        
        Assert.AreEqual(precioDescuentos,newReserva.Precio);
    }

    [TestMethod]
    public void Al_Reservar_Deposito_Con_Promociones_Con_Fecha_Final_Menor_A_La_Actual_No_Deberia_Aplicar_Sus_Descuentos_Test()
    {
        var fechaIni1 = DateTime.Today.AddDays(-5);
        var fechaFin1 = DateTime.Today.AddDays(-1);
        var promocion1 = new Promocion()
        {
            Etiqueta = "a1b2", 
            Descuento = 10,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = fechaIni1,
                FechaFin = fechaFin1
            }
        };
        
        var fechaIni2 = DateTime.Today.AddDays(-11);
        var fechaFin2 = DateTime.Today.AddDays(-5);
        var promocion2 = new Promocion()
        {
            Etiqueta = "a1b3",
            Descuento = 5,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = fechaIni2,
                FechaFin = fechaFin2
            }
        };
        
        
        List<Promocion> listaPromociones = new List<Promocion>(){ promocion1, promocion2 };
        var fechaInicial = DateTime.Today.AddDays(6);
        var fechaFinal = DateTime.Today.AddDays(10);
        var newDeposito = new Deposito("nombre",Area.A, Tamanio.Grande, false, listaPromociones,new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        
        var precioDepositoGrandePorDia = 100;
        var diffDias = fechaFinal - fechaInicial;
        double dias = (int)diffDias.TotalDays;
        double precioSinDescuento = dias * precioDepositoGrandePorDia;
        
        Assert.AreEqual(precioSinDescuento,newReserva.Precio);
        
    }

    [TestMethod]
    public void Al_Reservar_Deposito_Con_Promocion_Con_Fecha_De_Inicio_Igual_A_Hoy_Deberia_Aplicar_Su_Descuento_Test()
    {
        var rango = new RangoFechas(DateTime.Today, DateTime.Today.AddDays(2));
        var promocion = new Promocion("a1b2", 10, rango);
        List<Promocion> listaPromociones = new List<Promocion>(){ promocion };
        var fechaInicial = DateTime.Today.AddDays(6);
        var fechaFinal = DateTime.Today.AddDays(10);
        var newDeposito = new Deposito("Nombre",Area.A, Tamanio.Grande, false, listaPromociones,new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        
        var precioDepositoGrandePorDia = 100;
        var diffDias = fechaFinal - fechaInicial;
        double dias = (int)diffDias.TotalDays;
        double precio = dias * precioDepositoGrandePorDia;
        precio = precio - (precio * promocion.Descuento) / 100;
        
        Assert.AreEqual(precio,newReserva.Precio);

    }
    
    [TestMethod]
    public void Al_Reservar_Deposito_Con_Promocion_Con_Fecha_De_Fin_Igual_A_Hoy_Deberia_Aplicar_Su_Descuento_Test()
    {
        var fechaIni = DateTime.Today.AddDays(-2);
        var fechaFin = DateTime.Today;
        var promocion = new Promocion()
        {
            Etiqueta= "a1b2",
            Descuento = 10,
            RangoFecha = new RangoFechas()
            {
                FechaInicio = fechaIni,
                FechaFin = fechaFin
            }
        };
        
        List<Promocion> listaPromociones = new List<Promocion>(){ promocion };
        var fechaInicial = DateTime.Today.AddDays(6);
        var fechaFinal = DateTime.Today.AddDays(10);
        var newDeposito = new Deposito("Nombre",Area.A, Tamanio.Grande, false, listaPromociones,new List<RangoFechas>(){new RangoFechas(fechaInicial,fechaFinal)});
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        var newReserva = new Reserva(rangoFecha,newDeposito,new Usuario());
        
        var precioDepositoGrandePorDia = 100;
        var diffDias = fechaFinal - fechaInicial;
        double dias = (int)diffDias.TotalDays;
        double precio = dias * precioDepositoGrandePorDia;
        precio = precio - (precio * promocion.Descuento) / 100;
        
        Assert.AreEqual(precio,newReserva.Precio);

    }

    [TestMethod]
    public void Agregar_Comentario_A_Reserva()
    {
        var newReserva = new Reserva();

        var comentario = "Rechazada por motivo x";
        newReserva.Comentario = comentario;
        
        Assert.AreEqual(comentario,newReserva.Comentario);


    }

    [TestMethod]
    public void Agregar_Comentario_De_Mas_De_Trecientos_A_Reserva_Deberia_Tirar_Excepcion()
    {
        var newReserva = new Reserva();

        var comentarioDeMasDetrecientosCaracteres = "Comentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteresComentario De trecientos caracteres";
        
        Assert.ThrowsException<ReservaExcepcion>((() => newReserva.Comentario = comentarioDeMasDetrecientosCaracteres));
    }

    [TestMethod]
    public void Comentario_No_Puede_Ser_Vacio_Si_Adimnistrador_Rechazo_La_Reserva_Deberia_Tirar_Excepcion()
    {
        var comentarioVacio = "";
        var newReserva = new Reserva();

        newReserva.ConfAdmin = Estado.Rechazada;

        Assert.ThrowsException<ReservaExcepcion>((() => newReserva.Comentario = comentarioVacio));
    }
    
    
    [TestMethod]
    public void Asignar_Cliente_Null_A_Reserva_Deberia_Tirar_Excepcion()
    {
        var reserva = new Reserva();

        Assert.ThrowsException<ReservaExcepcion>((() => reserva.Cliente = null));
    }

    [TestMethod]
    public void Si_El_Rango_De_Fechas_De_La_Reserva_No_Esta_Contenido_En_Uno_De_Los_Rangos_De_Fecha_Del_Deposito_Deberia_Tirar_Excepcion()
    {
        var rangosFechas = new List<RangoFechas>();
        rangosFechas.Add(new RangoFechas(DateTime.Today, DateTime.Today.AddDays(4)));
        var deposito = new Deposito("Nombre", Area.A, Tamanio.Grande, true, new List<Promocion>(), rangosFechas);
        var usuario = new Usuario("direccion@dominio.com", "nombre", "Password1@");
        
        var rangoFecha = new RangoFechas(DateTime.Today.AddDays(5), DateTime.Today.AddDays(8));

         Assert.ThrowsException<ReservaExcepcion>((() => new Reserva(rangoFecha, deposito, usuario)));
    }

    [TestMethod]
    public void Si_El_Rango_De_Fechas_De_La_Reserva_No_Esta_Totalmente_Contenido_En_Uno_De_Los_Rangos_De_Fecha_Del_Deposito_Deberia_Tirar_Excepcion()
    {
        var rangosFechas = new List<RangoFechas>();
        rangosFechas.Add(new RangoFechas(DateTime.Today, DateTime.Today.AddDays(4)));
        var deposito = new Deposito("Nombre", Area.A, Tamanio.Grande, true, new List<Promocion>(), rangosFechas);
        var usuario = new Usuario("direccion@dominio.com", "nombre", "Password1@");
        
        var rangoFecha = new RangoFechas(DateTime.Today.AddDays(5), DateTime.Today.AddDays(8));

        Assert.ThrowsException<ReservaExcepcion>((() => new Reserva(rangoFecha, deposito, usuario)));
    }

    [TestMethod]
    public void Si_El_Rango_De_Fechas_De_La_Reserva_Es_Igual_A_Un_Rango_De_Fecha_Del_Deposito_Deberia_Estar_Bien()
    {
        var fechaInicial = DateTime.Today;
        var fechaFinal = DateTime.Today.AddDays(4);
        var rangosFechas = new List<RangoFechas>();
        rangosFechas.Add(new RangoFechas(fechaInicial,fechaFinal));
        var deposito = new Deposito("Nombre", Area.A, Tamanio.Grande, true, new List<Promocion>(), rangosFechas);
        var usuario = new Usuario("direccion@dominio.com", "nombre", "Password1@");
        
        var rangoFecha = new RangoFechas(fechaInicial, fechaFinal);
        var reserva = new Reserva(rangoFecha, deposito, usuario);
        Assert.IsNotNull(reserva);
    }
    
    [TestMethod]
    public void Si_El_Rango_De_Fechas_De_La_Reserva_Esta_Dentro_De_Un_Rango_De_Fecha_Del_Deposito_Deberia_Estar_Bien()
    {
        var rangosFechas = new List<RangoFechas>();
        rangosFechas.Add(new RangoFechas(DateTime.Today,DateTime.Today.AddDays(9)));
        var deposito = new Deposito("Nombre", Area.A, Tamanio.Grande, true, new List<Promocion>(), rangosFechas);
        var usuario = new Usuario("direccion@dominio.com", "nombre", "Password1@");
        
        var rangoFecha = new RangoFechas(DateTime.Today.AddDays(2), DateTime.Today.AddDays(5));
        var reserva = new Reserva(rangoFecha, deposito, usuario);
        Assert.IsNotNull(reserva);
    }

    [TestMethod]
    public void Agrego_Estado_De_Pago_A_Reserva()
    {
        var reserva = new Reserva();

        reserva.Pago = Pago.Reservado;
        
        Assert.AreEqual(Pago.Reservado,reserva.Pago);
    }
}