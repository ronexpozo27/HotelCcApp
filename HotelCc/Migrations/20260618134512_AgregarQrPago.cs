using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCc.Migrations
{
    /// <inheritdoc />
    public partial class AgregarQrPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrBase64",
                table: "Pagos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrBase64",
                table: "Pagos");
        }
    }
}
