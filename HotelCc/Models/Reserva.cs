using System.ComponentModel.DataAnnotations;

namespace HotelCc.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public int HabitacionId { get; set; }

        public Habitacion? Habitacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaEntrada { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }

        public DateTime FechaReserva { get; set; }
        public decimal Total { get; set; }
    }
}