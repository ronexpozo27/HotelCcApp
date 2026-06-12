using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCc.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEstadoReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Reservas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Reservas");
        }
    }
}
