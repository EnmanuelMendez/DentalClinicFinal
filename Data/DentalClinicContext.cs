using Microsoft.EntityFrameworkCore;
using DentalClinic.Models;

namespace DentalClinic.Data
{
    public class DentalClinicContext : DbContext
    {
        public DentalClinicContext(DbContextOptions<DentalClinicContext> options)
            : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Servicio> Servicios => Set<Servicio>();
        public DbSet<ItemServicio> ItemsServicio => Set<ItemServicio>();
        public DbSet<HorarioAtencion> HorariosAtencion => Set<HorarioAtencion>();
        public DbSet<ConfiguracionEmpresa> ConfiguracionesEmpresa => Set<ConfiguracionEmpresa>();
        public DbSet<Reserva> Reservas => Set<Reserva>();
        public DbSet<Dentista> Dentistas => Set<Dentista>();
        public DbSet<Consultorio> Consultorios => Set<Consultorio>();
        public DbSet<Queja> Quejas => Set<Queja>();
        public DbSet<Valoracion> Valoraciones => Set<Valoracion>();
        public DbSet<Clinica> Clinicas => Set<Clinica>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<RegistroCorreo> RegistrosCorreo => Set<RegistroCorreo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // CLIENTE -> RESERVAS
            // ============================
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Reservas)
                .WithOne(r => r.Cliente)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // SERVICIO -> ITEMS
            // ============================
            modelBuilder.Entity<Servicio>()
                .HasMany(s => s.Items)
                .WithOne(i => i.Servicio)
                .HasForeignKey(i => i.ServicioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // SERVICIO -> RESERVAS
            // ============================
            modelBuilder.Entity<Servicio>()
                .HasMany(s => s.Reservas)
                .WithOne(r => r.Servicio)
                .HasForeignKey(r => r.ServicioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // DENTISTA -> RESERVAS
            // ============================
            modelBuilder.Entity<Dentista>()
                .HasMany(d => d.Reservas)
                .WithOne(r => r.Dentista)
                .HasForeignKey(r => r.DentistaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // DENTISTA -> HORARIOS
            // ============================
            modelBuilder.Entity<Dentista>()
                .HasMany(d => d.Horarios)
                .WithOne(h => h.Dentista)
                .HasForeignKey(h => h.DentistaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // CONSULTORIO -> DENTISTAS
            // ============================
            modelBuilder.Entity<Consultorio>()
                .HasMany(c => c.Dentistas)
                .WithOne(d => d.Consultorio)
                .HasForeignKey(d => d.ConsultorioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // RESERVA -> QUEJAS
            // ============================
            modelBuilder.Entity<Reserva>()
                .HasMany(r => r.Quejas)
                .WithOne(q => q.Reserva)
                .HasForeignKey(q => q.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // RESERVA -> VALORACIONES
            // ============================
            modelBuilder.Entity<Reserva>()
                .HasMany(r => r.Valoraciones)
                .WithOne(v => v.Reserva)
                .HasForeignKey(v => v.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // RESERVA -> REGISTRO CORREO
            // ============================
            modelBuilder.Entity<Reserva>()
                .HasMany(r => r.Correos)
                .WithOne(c => c.Reserva)
                .HasForeignKey(c => c.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // PRECISION DECIMALES
            // ============================
            modelBuilder.Entity<Servicio>()
                .Property(s => s.CostoBase)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemServicio>()
                .Property(i => i.CostoAdicional)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasPrecision(10, 2);
        }
    }
}
