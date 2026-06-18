namespace HotelCc.Models
{
    public class Pago
    {
        public int Id { get; set; }

        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }

        public decimal Monto { get; set; }

        public string MetodoPago { get; set; } = "";

        public DateTime FechaPago { get; set; }

        public string Estado { get; set; } = "Pagado";

        public string CodigoOperacion { get; set; } = "";
        public string? QrBase64 { get; set; }
    }
}