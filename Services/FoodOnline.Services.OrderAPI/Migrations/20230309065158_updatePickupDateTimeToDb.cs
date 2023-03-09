using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOnline.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatePickupDateTimeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PiclupDateTime",
                table: "OrderHeaders",
                newName: "PickupDateTime");

            migrationBuilder.AlterColumn<bool>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickupDateTime",
                table: "OrderHeaders",
                newName: "PiclupDateTime");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
