using HotelCc.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelCc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // TABLAS
        // =========================

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Habitacion> Habitaciones { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        // =========================
        // CONFIGURACIONES
        // =========================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // FECHAS RESERVA
            // =========================

            modelBuilder.Entity<Reserva>()
                .Property(r => r.FechaEntrada)
                .HasColumnType("date");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.FechaSalida)
                .HasColumnType("date");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.FechaReserva)
                .HasColumnType("timestamp without time zone");

            // =========================
            // RELACIONES
            // =========================

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Habitacion)
                .WithMany()
                .HasForeignKey(r => r.HabitacionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // PRECISIÓN DECIMAL
            // =========================

            modelBuilder.Entity<Habitacion>()
                .Property(h => h.Precio)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Total)
                .HasColumnType("decimal(10,2)");
        }
    }
}