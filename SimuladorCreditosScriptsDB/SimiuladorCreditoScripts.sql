--CREACION DE LA BASE DE DATOS Y TABLAS
USE master
go

CREATE DATABASE Simulador
go

USE Simulador
go

CREATE TABLE Credito
(
IdCredito int identity primary key not null,
DNI varchar(8) not null,
Monto decimal(18,2) not null,
TasaAnual decimal(18,2) not null,
DiaPago int not null,
FechaDesembolso datetime not null,
FechaPrimeraCuota datetime not null,
NumeroCuotas int not null,
TasaDiaria decimal(24,18) not null,
TasaMensual decimal(24,18) not null,
Activo int default 1 
)
go 

CREATE TABLE Detalle
(
IdDetalle int identity primary key not null, 
IdCredito int references Credito(IdCredito) not null, 
Item int not null, 
FechaCuota datetime not null, 
DiasCouta int not null, 
SaldoCapital decimal(18,2) not null, 
Capital decimal(18,2) not null, 
Interes decimal(18,2) not null, 
Cuota decimal(18,2) not null,
Activo int default 1 
)
go

--CREACION DE PROCEDIMIENTOS ALMACENADOS
CREATE PROCEDURE CRE_ActualizarCredito_Sp
@json NVARCHAR(max),
@IdCreditoAct int,
@Existe int output,
@DNIAct varchar(8)
as
if NOT EXISTS(select * from Credito where DNI=@DNIAct and IdCredito <> @IdCreditoAct)
	begin
		begin try
			BEGIN TRANSACTION
			declare @IdCredito int
			declare	@DNI varchar(8)
			declare @Monto decimal(18,2)
			declare @TasaAnual decimal(18,2)
			declare @DiaPago int
			declare @FechaDesembolso datetime
			declare @FechaPrimeraCuota datetime
			declare @Cuotas int
			declare @TasaDiaria decimal(24,18)
			declare @TasaMensual decimal(24,18)
			declare @Activo int
			SELECT @IdCredito=IdCredito, @DNI=DNI, @Monto=Monto, @TasaAnual=TasaAnual, @DiaPago=DiaPago, @FechaDesembolso=FechaDesembolso,
			@FechaPrimeraCuota=FechaPrimeraCuota, @Cuotas=Cuotas, @TasaDiaria=TasaDiaria, @TasaMensual=TasaMensual, @Activo=Activo
			FROM OpenJson(@json)
						with
						(
							IdCredito int,
							DNI varchar(8),
							Monto decimal(18,2),
							TasaAnual decimal(18,2),
							DiaPago int,
							FechaDesembolso datetime,
							FechaPrimeraCuota datetime,
							Cuotas int,
							TasaDiaria decimal(24,18),
							TasaMensual decimal(24,18),
							Activo int
						);
			--Actualizar el Credito
			update Credito set DNI=@DNI, Monto=@Monto, TasaAnual=@TasaAnual, DiaPago=@DiaPago, FechaDesembolso=@FechaDesembolso,
			FechaPrimeraCuota=@FechaPrimeraCuota, NumeroCuotas=@Cuotas, TasaDiaria =@TasaDiaria, TasaMensual =@TasaMensual,
			Activo=@Activo where IdCredito=@IdCredito
			--Desactivar los detalles de credito
			update Detalle set Activo=0 where IdCredito=@IdCredito
			--Agregar los nuevos detalles
			insert into Detalle
			SELECT *
			FROM OPENJSON(@json, '$.Detalles')
			WITH  (
					IdCredito int, 
					Item int, 
					FechaCuota datetime, 
					DiasCouta int, 
					SaldoCapital decimal(18,2), 
					Capital decimal(18,2), 
					Interes decimal(18,2), 
					Cuota decimal(18,2),
					Activo int
				);
			--;throw 50000, 'Error', 1
			set @Existe = 0;
			COMMIT TRANSACTION
		end try
		begin catch
			ROLLBACK TRANSACTION
		end catch
		
	end
else
	set @Existe = 1;

go

CREATE PROCEDURE CRE_AgregarCredito_Sp
@json NVARCHAR(max),
@DNI varchar(8),
@IdCredito int output,
@Existe int output
as
if NOT EXISTS(select * from Credito where DNI=@DNI)
	begin
		begin try
			BEGIN TRANSACTION
			insert into Credito
			SELECT * FROM OpenJson(@json)
			with
			(
				DNI varchar(8),
				Monto decimal(18,2),
				TasaAnual decimal(18,2),
				DiaPago int,
				FechaDesembolso datetime,
				FechaPrimeraCuota datetime,
				Cuotas int,
				TasaDiaria decimal(24,18),
				TasaMensual decimal(24,18),
				Activo int
			);

			declare @id int;
			set @id=SCOPE_IDENTITY();
			set @Existe = 0;
			set @IdCredito=SCOPE_IDENTITY();
  
			create table #T1
			(
				IdCredito int, 
				Item int, 
				FechaCuota datetime, 
				DiasCouta int, 
				SaldoCapital decimal(18,2), 
				Capital decimal(18,2), 
				Interes decimal(18,2), 
				Cuota decimal(18,2),
				Activo int
			)

			insert into #T1
			SELECT *
			FROM OPENJSON(@json, '$.Detalles')
			WITH  (
					IdCredito int, 
					Item int, 
					FechaCuota datetime, 
					DiasCouta int, 
					SaldoCapital decimal(18,2), 
					Capital decimal(18,2), 
					Interes decimal(18,2), 
					Cuota decimal(18,2),
					Activo int
				);

			UPDATE #T1 set IdCredito=@id

			insert into Detalle
			select * from #T1
			--;throw 50000, 'Error', 1
			COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
		END CATCH
	end
else
	begin
		set @Existe = 1;
		set @IdCredito = 0;
	end

go

CREATE PROCEDURE CRE_ObtenerCreditoPorIdCredito_Sp
@IdCredito int 
as
select IdCredito, DNI, Monto, TasaAnual, DiaPago, FechaDesembolso, FechaPrimeraCuota, NumeroCuotas, TasaDiaria, TasaMensual, Activo
from Credito 
where IdCredito=@IdCredito and Activo=1

go

CREATE PROCEDURE CRE_ObtenerCreditos_Sp
as
select IdCredito, DNI, Monto, TasaAnual, DiaPago, FechaDesembolso, FechaPrimeraCuota, NumeroCuotas, TasaDiaria, 
TasaMensual, Activo
from Credito where Activo=1

go

CREATE PROCEDURE CRE_ObtenerDetallesPorIdCredito_Sp
@IdCredito int 
as
select IdDetalle, IdCredito, Item, FechaCuota, DiasCouta, SaldoCapital, Capital, Interes, Cuota, Activo
from Detalle
where IdCredito = @IdCredito and Activo=1

go