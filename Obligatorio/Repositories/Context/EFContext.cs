using Dominio;
using Dominio.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Repositories;



public class EFContext : DbContext
{
    public DbSet<Promocion> Promocion { get; set; }
    public DbSet<Deposito> Deposito { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Reserva> Reserva { get; set; }
    
    public DbSet<RangoFechas> RangoFechas { get; set; }

    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reserva>()
            .Property(r => r.ConfAdmin)
            .HasConversion(new EnumToStringConverter<Estado>());
        
        modelBuilder.Entity<Reserva>()
            .Property(r => r.Pago)
            .HasConversion(new EnumToStringConverter<Pago>());
        
        modelBuilder.Entity<Deposito>()
            .Property(d => d.Tamanio)
            .HasConversion(new EnumToStringConverter<Tamanio>());
        
        modelBuilder.Entity<Deposito>()
            .Property(d => d.Area)
            .HasConversion(new EnumToStringConverter<Area>());
        
        modelBuilder.Entity<Deposito>()
            .Property(d => d.Climatizacion)
            .IsRequired();

        modelBuilder.Entity<Deposito>()
            .Property(d => d.Nombre)
            .IsRequired();
        
        modelBuilder.Entity<Reserva>()
            .Property(r => r.Comentario)
            .IsRequired(false);
        modelBuilder.Entity<Reserva>()
            .Property(r => r.ConfAdmin)
            .IsRequired();
        modelBuilder.Entity<Reserva>()
            .Property(r => r.Precio)
            .IsRequired();
        
        modelBuilder.Entity<Usuario>()
            .Property(u => u.NombreCompleto)
            .IsRequired();
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Rol)
            .HasConversion(new EnumToStringConverter<Rol>())
            .IsRequired();
        
        modelBuilder.Entity<Promocion>()
            .Property(p => p.Descuento)
            .IsRequired();
        modelBuilder.Entity<Promocion>()
            .Property(p => p.Etiqueta)
            .IsRequired();
        
        modelBuilder.Entity<Promocion>()
            .HasMany(p => p.Depositos)
            .WithMany(d => d.Promociones)
            .UsingEntity<Dictionary<string, object>>(
                "PromocionDeposito",
                j => j.HasOne<Deposito?>().WithMany().HasForeignKey("DepositoId"),
                j => j.HasOne<Promocion?>().WithMany().HasForeignKey("PromocionId"),
                j =>
                {
                    j.HasKey("PromocionId", "DepositoId");
                });
    }
}