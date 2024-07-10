﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repositories;

#nullable disable

namespace Repositories.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20240606230233_Final")]
    partial class Final
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Dominio.Deposito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Climatizacion")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tamanio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Deposito");
                });

            modelBuilder.Entity("Dominio.Promocion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Descuento")
                        .HasColumnType("int");

                    b.Property<string>("Etiqueta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RangoFechaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RangoFechaId");

                    b.ToTable("Promocion");
                });

            modelBuilder.Entity("Dominio.RangoFechas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("DepositoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DepositoId");

                    b.ToTable("RangoFechas");
                });

            modelBuilder.Entity("Dominio.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClienteEmail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfAdmin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DepositoId")
                        .HasColumnType("int");

                    b.Property<string>("Pago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.Property<int>("RangoFechaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteEmail");

                    b.HasIndex("DepositoId");

                    b.HasIndex("RangoFechaId");

                    b.ToTable("Reserva");
                });

            modelBuilder.Entity("Dominio.Usuario", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NombreCompleto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("PromocionDeposito", b =>
                {
                    b.Property<int>("PromocionId")
                        .HasColumnType("int");

                    b.Property<int>("DepositoId")
                        .HasColumnType("int");

                    b.HasKey("PromocionId", "DepositoId");

                    b.HasIndex("DepositoId");

                    b.ToTable("PromocionDeposito");
                });

            modelBuilder.Entity("Dominio.Promocion", b =>
                {
                    b.HasOne("Dominio.RangoFechas", "RangoFecha")
                        .WithMany()
                        .HasForeignKey("RangoFechaId");

                    b.Navigation("RangoFecha");
                });

            modelBuilder.Entity("Dominio.RangoFechas", b =>
                {
                    b.HasOne("Dominio.Deposito", null)
                        .WithMany("Disponibilidad")
                        .HasForeignKey("DepositoId");
                });

            modelBuilder.Entity("Dominio.Reserva", b =>
                {
                    b.HasOne("Dominio.Usuario", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteEmail");

                    b.HasOne("Dominio.Deposito", "Deposito")
                        .WithMany()
                        .HasForeignKey("DepositoId");

                    b.HasOne("Dominio.RangoFechas", "RangoFecha")
                        .WithMany()
                        .HasForeignKey("RangoFechaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Deposito");

                    b.Navigation("RangoFecha");
                });

            modelBuilder.Entity("PromocionDeposito", b =>
                {
                    b.HasOne("Dominio.Deposito", null)
                        .WithMany()
                        .HasForeignKey("DepositoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Promocion", null)
                        .WithMany()
                        .HasForeignKey("PromocionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dominio.Deposito", b =>
                {
                    b.Navigation("Disponibilidad");
                });
#pragma warning restore 612, 618
        }
    }
}
