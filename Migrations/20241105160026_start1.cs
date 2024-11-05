using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HAIRCRAFT.Migrations
{
    /// <inheritdoc />
    public partial class start1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Salons_SalonId1",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SalonId1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SalonId1",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalonId1",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SalonId1",
                table: "Appointments",
                column: "SalonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Salons_SalonId1",
                table: "Appointments",
                column: "SalonId1",
                principalTable: "Salons",
                principalColumn: "Id");
        }
    }
}
