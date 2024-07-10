INSERT INTO ObligatorioDB.dbo.Deposito (Nombre,Area,Tamanio,Climatizacion) VALUES
	 (N'Depósito Sur',N'E',N'Pequeño',0),
	 (N'Depósito Central',N'A',N'Grande',1),
	 (N'Depósito Temporal',N'B',N'Mediano',1),
	 (N'Depósito Quincenal',N'D',N'Grande',1),
	 (N'Depósito Norte',N'C',N'Grande',0),
	 (N'Deposito Electrónica',N'A',N'Mediano',0),
	 (N'Depósito Reserva',N'C',N'Pequeño',0);

INSERT INTO ObligatorioDB.dbo.Promocion (Etiqueta,Descuento,RangoFechaId) VALUES
	 (N'Día de Entrega',50,1002),
	 (N'Semana de 10',10,1003),
	 (N'Semana de Entrega',5,1004),
	 (N'Finde Re especial',5,1005),
	 (N'Julio regalado',35,1006),
	 (N'Cumpleaños',75,1007),
	 (N'Felices Fiestas',30,1008),
	 (N'25 de Agosto',25,1009),
	 (N'Black Friday',40,1010),
	 (N'Copa America',16,1011);

INSERT INTO ObligatorioDB.dbo.PromocionDeposito (PromocionId,DepositoId) VALUES
	 (1,2),
	 (5,2),
	 (9,2),
	 (10,2),
	 (1,3),
	 (2,3),
	 (3,3),
	 (4,3),
	 (5,3),
	 (6,3);
INSERT INTO ObligatorioDB.dbo.PromocionDeposito (PromocionId,DepositoId) VALUES
	 (7,3),
	 (8,3),
	 (9,3),
	 (10,3),
	 (7,5),
	 (8,5),
	 (9,5),
	 (10,5),
	 (2,6),
	 (4,6);
INSERT INTO ObligatorioDB.dbo.PromocionDeposito (PromocionId,DepositoId) VALUES
	 (6,6),
	 (8,6),
	 (4,7),
	 (7,7),
	 (9,7);

INSERT INTO ObligatorioDB.dbo.Reserva (ConfAdmin,DepositoId,ClienteEmail,Comentario,RangoFechaId,Precio,Pago) VALUES
	 (N'Rechazada',3,N'mati@a.com',N'El rango de fechas seleccionado ya fue capturado en otra reserva',1022,1026.0,N'Cancelado'),
	 (N'Aprobada',7,N'mati@a.com',NULL,1023,75.0,N'Capturado'),
	 (N'Aprobada',3,N'mati@a.com',NULL,1024,379.0,N'Capturado'),
	 (N'Pendiente',5,N'mati@a.com',NULL,1025,600.0,N'Reservado'),
	 (N'Pendiente',3,N'mati@a.com',NULL,1026,3437.0,NULL),
	 (N'Pendiente',7,N'mati@a.com',NULL,1027,225.0,N'Reservado'),
	 (N'Aprobada',8,N'mati@a.com',NULL,1028,200.0,N'Capturado'),
	 (N'Rechazada',4,N'mati@a.com',N'El rango de fechas seleccionado ya fue capturado en otra reserva',1029,190.0,N'Cancelado'),
	 (N'Aprobada',4,N'juan@a.com',NULL,1030,285.0,N'Capturado'),
	 (N'Pendiente',6,N'juan@a.com',NULL,1031,200.0,NULL);
INSERT INTO ObligatorioDB.dbo.Reserva (ConfAdmin,DepositoId,ClienteEmail,Comentario,RangoFechaId,Precio,Pago) VALUES
	 (N'Rechazada',5,N'juan@a.com',N'El rango de fechas seleccionado ya fue capturado en otra reserva',1032,1596.0,N'Cancelado'),
	 (N'Aprobada',3,N'juan@a.com',NULL,1033,542.0,N'Capturado'),
	 (N'Aprobada',7,N'juan@a.com',NULL,1034,150.0,N'Capturado'),
	 (N'Pendiente',7,N'juan@a.com',NULL,1035,225.0,N'Reservado'),
	 (N'Pendiente',3,N'juan@a.com',NULL,1036,114.0,NULL),
	 (N'Aprobada',2,N'juan@a.com',NULL,1037,214.0,N'Capturado'),
	 (N'Pendiente',2,N'juan@a.com',NULL,1038,25.0,N'Reservado'),
	 (N'Aprobada',3,N'admin@a.com',NULL,1039,57.0,N'Capturado'),
	 (N'Aprobada',5,N'admin@a.com',NULL,1040,798.0,N'Capturado'),
	 (N'Aprobada',8,N'admin@a.com',NULL,1041,200.0,N'Capturado');

INSERT INTO ObligatorioDB.dbo.Usuario (Email,NombreCompleto,Password,Rol) VALUES
	 (N'admin@a.com',N'Admin Depositos',N'Admin#12',N'Administrador'),
	 (N'juan@a.com',N'Juan Pérez',N'Juan#123',N'Cliente'),
	 (N'mati@a.com',N'Matías Corvetto',N'Mati#123',N'Cliente');


INSERT INTO ObligatorioDB.dbo.RangoFechas (FechaInicio,FechaFin,DepositoId) VALUES
	 ('2024-06-10 00:00:00.0000000','2024-06-10 00:00:00.0000000',NULL),
	 ('2024-06-17 00:00:00.0000000','2024-06-23 00:00:00.0000000',NULL),
	 ('2024-06-10 00:00:00.0000000','2024-06-16 00:00:00.0000000',NULL),
	 ('2024-06-22 00:00:00.0000000','2024-06-23 00:00:00.0000000',NULL),
	 ('2024-07-01 00:00:00.0000000','2024-07-31 00:00:00.0000000',NULL),
	 ('2024-07-14 00:00:00.0000000','2024-07-14 00:00:00.0000000',NULL),
	 ('2024-12-23 00:00:00.0000000','2024-12-31 00:00:00.0000000',NULL),
	 ('2024-08-25 00:00:00.0000000','2024-08-25 00:00:00.0000000',NULL),
	 ('2024-11-29 00:00:00.0000000','2024-11-29 00:00:00.0000000',NULL),
	 ('2024-06-20 00:00:00.0000000','2024-07-14 00:00:00.0000000',NULL);
INSERT INTO ObligatorioDB.dbo.RangoFechas (FechaInicio,FechaFin,DepositoId) VALUES
	 ('2024-06-10 00:00:00.0000000','2024-07-31 00:00:00.0000000',2),
	 ('2024-06-10 00:00:00.0000000','2024-12-31 00:00:00.0000000',3),
	 ('2024-06-15 00:00:00.0000000','2024-06-30 00:00:00.0000000',4),
	 ('2024-07-01 00:00:00.0000000','2024-07-15 00:00:00.0000000',5),
	 ('2024-08-01 00:00:00.0000000','2024-08-15 00:00:00.0000000',5),
	 ('2024-09-01 00:00:00.0000000','2024-09-15 00:00:00.0000000',5),
	 ('2024-10-01 00:00:00.0000000','2024-10-15 00:00:00.0000000',5),
	 ('2024-06-10 00:00:00.0000000','2024-07-31 00:00:00.0000000',6),
	 ('2024-07-01 00:00:00.0000000','2024-12-31 00:00:00.0000000',7),
	 ('2024-06-10 00:00:00.0000000','2024-07-10 00:00:00.0000000',8);
INSERT INTO ObligatorioDB.dbo.RangoFechas (FechaInicio,FechaFin,DepositoId) VALUES
	 ('2024-06-10 00:00:00.0000000','2024-06-30 00:00:00.0000000',NULL),
	 ('2024-07-14 00:00:00.0000000','2024-07-15 00:00:00.0000000',NULL),
	 ('2024-12-24 00:00:00.0000000','2024-12-31 00:00:00.0000000',NULL),
	 ('2024-09-05 00:00:00.0000000','2024-09-10 00:00:00.0000000',NULL),
	 ('2024-08-10 00:00:00.0000000','2024-10-16 00:00:00.0000000',NULL),
	 ('2024-08-28 00:00:00.0000000','2024-08-31 00:00:00.0000000',NULL),
	 ('2024-06-10 00:00:00.0000000','2024-06-14 00:00:00.0000000',NULL),
	 ('2024-06-28 00:00:00.0000000','2024-06-30 00:00:00.0000000',NULL),
	 ('2024-06-26 00:00:00.0000000','2024-06-29 00:00:00.0000000',NULL),
	 ('2024-07-02 00:00:00.0000000','2024-07-04 00:00:00.0000000',NULL);
INSERT INTO ObligatorioDB.dbo.RangoFechas (FechaInicio,FechaFin,DepositoId) VALUES
	 ('2024-08-01 00:00:00.0000000','2024-08-15 00:00:00.0000000',NULL),
	 ('2024-11-05 00:00:00.0000000','2024-11-15 00:00:00.0000000',NULL),
	 ('2024-12-28 00:00:00.0000000','2024-12-30 00:00:00.0000000',NULL),
	 ('2024-09-16 00:00:00.0000000','2024-09-19 00:00:00.0000000',NULL),
	 ('2024-09-19 00:00:00.0000000','2024-09-21 00:00:00.0000000',NULL),
	 ('2024-06-14 00:00:00.0000000','2024-06-23 00:00:00.0000000',NULL),
	 ('2024-06-28 00:00:00.0000000','2024-06-29 00:00:00.0000000',NULL),
	 ('2024-06-29 00:00:00.0000000','2024-06-30 00:00:00.0000000',NULL),
	 ('2024-08-05 00:00:00.0000000','2024-08-12 00:00:00.0000000',NULL),
	 ('2024-07-01 00:00:00.0000000','2024-07-05 00:00:00.0000000',NULL);
