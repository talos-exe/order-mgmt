using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Warehouses_DestinationWarehouseID",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_DestinationWarehouseID",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DestinationWarehouseID",
                table: "Shipments");

            migrationBuilder.AddColumn<string>(
                name: "ShippingRequest_ToCountryCode",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingRequest_ToPhone",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingRequest_ToCountryCode",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ShippingRequest_ToPhone",
                table: "Shipments");

            migrationBuilder.AddColumn<int>(
                name: "DestinationWarehouseID",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DestinationWarehouseID",
                table: "Shipments",
                column: "DestinationWarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Warehouses_DestinationWarehouseID",
                table: "Shipments",
                column: "DestinationWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "WarehouseID");
        }
    }
}
