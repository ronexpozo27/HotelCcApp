using HotelCc.Data;
using HotelCc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data.SqlTypes;
using System.Net.Sockets;

namespace HotelCc.Controllers
{
    public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {

            // =========================
            // FINALIZAR RESERVAS VENCIDAS
            // =========================
            var vencidas = await _context.Reservas
                .Where(r =>
                    r.Estado == "Activa" &&
                    r.FechaSalida < DateTime.Today)
                .ToListAsync();

            foreach (var r in vencidas)
            {
                r.Estado = "Finalizada";
            }

            if (vencidas.Any())
            {
                await _context.SaveChangesAsync();
            }

            var rol = HttpContext.Session.GetString("Rol");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (rol == "Admin")
            {
                var reservasAdmin = await _context.Reservas
                    .Include(r => r.Usuario)
                    .Include(r => r.Habitacion)
                    .OrderByDescending(r => r.Id)
                    .ToListAsync();

                return View(reservasAdmin);
            }

            var reservasUser = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Where(r =>
                    r.UsuarioId == userId &&
                    r.Estado == "Activa")
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            return View(reservasUser);
        }

        // =========================
        // HISTORIAL USUARIO
        // =========================
        public async Task<IActionResult> Historial()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (rol == "Admin")
            {
                var historialAdmin = await _context.Reservas
                    .Include(r => r.Usuario)
                    .Include(r => r.Habitacion)
                    .Where(r => r.Estado == "Finalizada")
                    .OrderByDescending(r => r.FechaSalida)
                    .ToListAsync();

                return View(historialAdmin);
            }

            var historialUser = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .Where(r =>
                    r.UsuarioId == userId &&
                    r.Estado == "Finalizada")
                .OrderByDescending(r => r.FechaSalida)
                .ToListAsync();

            return View(historialUser);
        }

        // =========================
        // RESERVAR DESDE HABITACIÓN
        // =========================
        public async Task<IActionResult> Reservar(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Auth",
                    new
                    {
                        returnUrl = Url.Action(
                            "Reservar",
                            "Reservas",
                            new { id = id })
                    });
            }

            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
                return NotFound();

            var rol = HttpContext.Session.GetString("Rol");

            ViewBag.EsAdmin = (rol == "Admin");



            // SOLO USERS
            ViewData["UsuarioId"] = new SelectList(
                _context.Usuarios
                    .Where(u =>
                        u.Rol != null &&
                        u.Rol.Trim().ToLower() == "user")
                    .OrderBy(u => u.Nombre),
                "Id",
                "Nombre"
            );

            var model = new Reserva
            {
                HabitacionId = id,
                FechaEntrada = DateTime.Today,
                FechaSalida = DateTime.Today.AddDays(1)
            };

            ViewBag.Habitacion = habitacion;

            return View(model);
        }

        //============================================================
        //GET ENDPOINT PARA OBTENER FECHAS OCUPADAS DE UNA HABITACIÓN
        //============================================================

        [HttpGet]
        public async Task<IActionResult> FechasOcupadas(int habitacionId)
        {
            var reservas = await _context.Reservas
                .Where(r =>
                    r.HabitacionId == habitacionId &&
                    r.Estado == "Activa")
                .Select(r => new
                {
                    entrada = r.FechaEntrada,
                    salida = r.FechaSalida
                })
                .ToListAsync();

            return Json(reservas);
        }

        // =========================
        // PAGO
        // =========================
        public IActionResult Pago()
        {
            return View();
        }

        // =========================
        // POST RESERVAR
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(Reserva reserva)
        {

            var userId = HttpContext.Session.GetInt32("UserId");
            var rol = HttpContext.Session.GetString("Rol");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            // NUEVO
            if (string.IsNullOrEmpty(reserva.MetodoPago))
            {
                ViewBag.Error = "Seleccione un método de pago";
                return View(reserva);
            }

            // =========================
            // VALIDAR FECHAS
            // =========================
            if (reserva.FechaSalida <= reserva.FechaEntrada)
            {
                ViewBag.Error = "La fecha de salida debe ser mayor.";

                return View(reserva);
            }

            // =========================
            // ADMIN / USER
            // =========================
            if (rol == "Admin")
            {
                if (reserva.UsuarioId == null &&
                    string.IsNullOrWhiteSpace(reserva.NombreHuespedExterno))
                {
                    ViewBag.Error =
                        "Debe seleccionar un usuario o ingresar huésped externo.";

                    return View(reserva);
                }

                if (reserva.UsuarioId != null)
                {
                    reserva.NombreHuespedExterno = null;
                }
                else
                {
                    reserva.UsuarioId = null;
                }
            }
            else
            {
                reserva.UsuarioId = userId;
                reserva.NombreHuespedExterno = null;
            }

            // =========================
            // VALIDAR DISPONIBILIDAD
            // =========================
            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                reserva.FechaEntrada < r.FechaSalida &&
                reserva.FechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación ya está reservada para esa fecha.";

                return View(reserva);
            }

            // =========================
            // HABITACIÓN
            // =========================
            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            // =========================
            // TOTAL
            // =========================
            int dias = (reserva.FechaSalida - reserva.FechaEntrada).Days;

            if (dias <= 0)
            {
                dias = 1;
            }

            reserva.Total = dias * habitacion.Precio;

            // =========================
            // FECHA RESERVA
            // =========================
            reserva.FechaReserva = DateTime.Now;

                HttpContext.Session.SetString(
                    "HabitacionId",
                reserva.HabitacionId.ToString());

                HttpContext.Session.SetString(
                    "FechaEntrada",
                    reserva.FechaEntrada.ToString("yyyy-MM-dd"));

                HttpContext.Session.SetString(
                    "FechaSalida",
                    reserva.FechaSalida.ToString("yyyy-MM-dd"));

                HttpContext.Session.SetString(
                    "MetodoPago",
                    reserva.MetodoPago);

                HttpContext.Session.SetString(
                    "Total",
                    reserva.Total.ToString());

            return RedirectToAction("Pago");
        }

        // =========================
        // CREATE ADMIN
        // =========================
        public IActionResult Create()
        {
            var usuarios = _context.Usuarios
                .Where(u =>
                    u.Rol != null &&
                    u.Rol.Trim().ToLower() == "user")
                .OrderBy(u => u.Nombre)
                .ToList();

            ViewData["UsuarioId"] =
                new SelectList(usuarios, "Id", "Nombre");

            ViewData["HabitacionId"] =
                new SelectList(
                    _context.Habitaciones.OrderBy(h => h.Numero),
                    "Id",
                    "Numero"
                );

            return View();
        }

        // =========================
        // POST CREATE
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {


            // =========================
            // HABITACIÓN
            // =========================
            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == reserva.HabitacionId);

            if (habitacion == null)
            {
                return NotFound();
            }

            // =========================
            // FECHAS
            // =========================
            var fechaEntrada = reserva.FechaEntrada.Date;
            var fechaSalida = reserva.FechaSalida.Date;

            if (fechaSalida <= fechaEntrada)
            {
                ViewBag.Error = "La fecha de salida debe ser mayor.";

                return View(reserva);
            }

            // =========================
            // USER / EXTERNO
            // =========================
            if (reserva.UsuarioId != null)
            {
                reserva.NombreHuespedExterno = null;
            }
            else
            {
                reserva.UsuarioId = null;
            }

            // =========================
            // DISPONIBILIDAD
            // =========================
            bool ocupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                fechaEntrada < r.FechaSalida &&
                fechaSalida > r.FechaEntrada);

            if (ocupada)
            {
                ViewBag.Error = "La habitación ya está reservada.";

                return View(reserva);
            }

            // =========================
            // TOTAL
            // =========================
            int dias = (fechaSalida - fechaEntrada).Days;

            if (dias <= 0)
            {
                dias = 1;
            }

            decimal total = dias * habitacion.Precio;

            // =========================
            // NUEVA RESERVA
            // =========================
            var nuevaReserva = new Reserva
            {
                UsuarioId = reserva.UsuarioId,

                NombreHuespedExterno =
                    reserva.NombreHuespedExterno,

                HabitacionId = reserva.HabitacionId,

                FechaEntrada = fechaEntrada,

                FechaSalida = fechaSalida,

                FechaReserva = DateTime.Now,

                Total = total
            };

            TempData["MetodoPago"] = reserva.MetodoPago;
            TempData["Total"] = reserva.Total;

            return RedirectToAction("Pago");
        }

        // =========================
        // PAGO CONFIRMADO
        // =========================
        [HttpPost]
        public async Task<IActionResult> ConfirmarPago()

        {
            var habitacionId =
                int.Parse(HttpContext.Session.GetString("HabitacionId")!);

            var fechaEntrada =
                DateTime.Parse(HttpContext.Session.GetString("FechaEntrada")!);

            var fechaSalida =
                DateTime.Parse(HttpContext.Session.GetString("FechaSalida")!);

            var metodoPago =
                HttpContext.Session.GetString("MetodoPago")!;

            var total =
                decimal.Parse(HttpContext.Session.GetString("Total")!);

            var userId =
                HttpContext.Session.GetInt32("UserId");

            // =========================
            // CREAR RESERVA
            // =========================

            Reserva reserva = new()
            {
                HabitacionId = habitacionId,
                UsuarioId = userId,
                FechaEntrada = fechaEntrada,
                FechaSalida = fechaSalida,
                FechaReserva = DateTime.Now,
                Total = total,
                Estado = "Activa"
            };

            // GUARDAR RESERVA
            _context.Reservas.Add(reserva);

            await _context.SaveChangesAsync();

            //Generar código de operación
            var codigoOperacion =
                "PAGO-" +
                Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 8)
                    .ToUpper();

            // =========================
            // GENERAR QR
            // =========================

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.Id == habitacionId);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == userId);

            string textoQr =
                $@"HOTEL CC
                Reserva: {reserva.Id}
                Cliente: {usuario?.Nombre}
                Habitacion: {habitacion?.Numero}
                Entrada: {fechaEntrada:dd/MM/yyyy}
                Salida: {fechaSalida:dd/MM/yyyy}
                Monto: S/ {total}
                Codigo: {codigoOperacion}";

            QRCodeGenerator qrGenerator =
                new QRCodeGenerator();

            QRCodeData qrCodeData =
                qrGenerator.CreateQrCode(
                    textoQr,
                    QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode =
                new PngByteQRCode(qrCodeData);

            byte[] qrBytes =
                qrCode.GetGraphic(20);

            string qrBase64 =
                Convert.ToBase64String(qrBytes);

            //Crear Pago
            Pago pago = new()
            {
                ReservaId = reserva.Id,

                Monto = total,

                MetodoPago = metodoPago,

                FechaPago = DateTime.UtcNow,

                Estado = "Pagado",

                CodigoOperacion = codigoOperacion,

                QrBase64 = qrBase64
            };

            //Guardar Pago
            _context.Pagos.Add(pago);

            await _context.SaveChangesAsync();
                        
            //Guardar datos para Ticket
            TempData["CodigoOperacion"] =
                codigoOperacion;

            TempData["MetodoPago"] =
                metodoPago;

            TempData["Monto"] =
                total.ToString();

            TempData["ReservaId"] =
                reserva.Id.ToString();
            TempData["QR"] =
                qrBase64;

            TempData["Cliente"] =
                usuario?.Nombre;

            TempData["Habitacion"] =
                habitacion?.Numero;

            TempData["FechaEntrada"] =
                fechaEntrada.ToString("dd/MM/yyyy");

            TempData["FechaSalida"] =
                fechaSalida.ToString("dd/MM/yyyy");

            TempData["FechaPago"] =
                DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            //Redirigir a Ticket
            return RedirectToAction(
                "Ticket",
                new
                {
                    id = reserva.Id,
                    origen = "reserva"
                });

        }

        //Crear acción Ticket
        public async Task<IActionResult> Ticket(
            int id,
            string origen = "")
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(p => p.ReservaId == id);

            ViewBag.Reserva = reserva;
            ViewBag.Pago = pago;
            ViewBag.QR = pago?.QrBase64;
            ViewBag.Origen = origen;

            return View();
        }

        //descargar ticket en PDF tamaño A4
        [HttpGet]
        public async Task<IActionResult> DescargarTicketPDF(
            int id,
            string formato = "a4")
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(p => p.ReservaId == id);

            if (pago == null)
                return NotFound();

            byte[] qrBytes = Convert.FromBase64String(
                pago.QrBase64
            );

            var logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "habitaciones",
                "logo.jpg");

            byte[] logoBytes = System.IO.File.ReadAllBytes(
                logoPath);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    
                    page.Margin(50);
                 
                    page.Header().Column(header =>
                    {
                        header.Item()
                            .AlignCenter()
                            .Width(150)
                            .Image(logoBytes);

                        header.Item()
                            .PaddingTop(10)
                            .AlignCenter()
                            .Text("🎟 TICKET DE PAGO")
                            .FontSize(24)
                            .Bold();
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(8);

                        col.Item().Text($"Cliente: {reserva.Usuario?.Nombre}");

                        col.Item().Text($"Habitación: {reserva.Habitacion?.Numero}");

                        col.Item().Text(
                            $"Entrada: {reserva.FechaEntrada:dd/MM/yyyy}");

                        col.Item().Text(
                            $"Salida: {reserva.FechaSalida:dd/MM/yyyy}");

                        col.Item().Text(
                            $"Método de Pago: {pago.MetodoPago}");

                        col.Item().Text(
                            $"Código Operación: {pago.CodigoOperacion}");

                        col.Item().Text(
                            $"Monto: S/ {pago.Monto}");

                        col.Item().Text(
                            $"Fecha Pago: {pago.FechaPago:dd/MM/yyyy HH:mm}");

                        col.Item().PaddingTop(20);

                        col.Item()
                           .AlignCenter()
                           .Width(150)
                           .Image(qrBytes);

                        col.Item()
                            .AlignCenter()
                            .Text("Escanee este QR para verificar la reserva");
                    });

                    page.Footer().Column(footer =>
                    {
                        footer.Item()
                            .PaddingTop(5)
                            .AlignCenter()
                            .Text(
                                "El uso de este comprobante está sujeto a las normas, políticas y condiciones establecidas por Hotel CC. Conserve este documento para cualquier consulta o verificación futura.")
                            .FontSize(8)
                            .Italic();
                    });

                });
            });

            var pdfBytes = pdf.GeneratePdf();

            return File(
                pdfBytes,
                "application/pdf",
                $"HotelCC_{reserva.Usuario?.Nombre}.pdf");
        }

        //Descargar ticket en formato térmico (80mm)
        [HttpGet]
        public async Task<IActionResult> DescargarTicketTermicoPDF(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(p => p.ReservaId == id);

            if (pago == null)
                return NotFound();

            byte[] qrBytes =
                Convert.FromBase64String(pago.QrBase64);

            var logoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "habitaciones",
                "logo.jpg");

            byte[] logoBytes =
                System.IO.File.ReadAllBytes(logoPath);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(10);

                    page.Header().Column(header =>
                    {
                        header.Item()
                            .AlignCenter()
                            .Width(80)
                            .Image(logoBytes);

                        header.Item()
                            .AlignCenter()
                            .Text("HOTEL CC")
                            .Bold()
                            .FontSize(14);
                    });

                    page.Content().Column(col =>
                    {
                        col.Spacing(5);

                        col.Item().Text($"Reserva: {reserva.Id}");
                        col.Item().Text($"Cliente: {reserva.Usuario?.Nombre}");
                        col.Item().Text($"Habitación: {reserva.Habitacion?.Numero}");

                        col.Item().Text(
                            $"Entrada: {reserva.FechaEntrada:dd/MM/yyyy}");

                        col.Item().Text(
                            $"Salida: {reserva.FechaSalida:dd/MM/yyyy}");

                        col.Item().Text(
                            $"Pago: {pago.MetodoPago}");

                        col.Item().Text(
                            $"Monto: S/ {pago.Monto}");

                        col.Item().Text(
                            $"Código: {pago.CodigoOperacion}");

                        col.Item()
                            .PaddingTop(10)
                            .AlignCenter()
                            .Width(120)
                            .Image(qrBytes);

                        col.Item()
                            .AlignCenter()
                            .Text("Gracias por su preferencia")
                            .FontSize(8);
                    });
                });
            });

            var pdfBytes = pdf.GeneratePdf();

            return File(
                pdfBytes,
                "application/pdf",
                $"TicketTermico_{reserva.Id}.pdf");
        }


        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        // =========================
        // DELETE
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EXISTS
        // =========================
        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}