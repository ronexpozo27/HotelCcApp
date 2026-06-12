namespace HotelCc.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int HabitacionId { get; set; }
        public Habitacion? Habitacion { get; set; }

        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaReserva { get; set; }

        public decimal Total { get; set; }
        public string Estado { get; set; } = "Activa";

        // ADMIN / EXTERNO
        public bool EsExterno { get; set; }

        public string? NombreHuespedExterno { get; set; } // 👈 ESTE FALTABA
    }
}