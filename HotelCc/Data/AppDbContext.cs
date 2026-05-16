using Microsoft.EntityFrameworkCore;
using HotelCc.Models;

namespace HotelCc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
            {
            }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Habitacion> Habitaciones { get; set; }

        public DbSet<Reserva> Reservas { get; set; }
    }
}
