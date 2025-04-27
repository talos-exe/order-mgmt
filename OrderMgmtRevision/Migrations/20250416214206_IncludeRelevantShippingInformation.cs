using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class IncludeRelevantShippingInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingRequest_ProductName",
                table: "Shipments",
                newName: "ShippingRequest_FromPhone");

            migrationBuilder.RenameColumn(
                name: "Rate_ShippingRequest_ProductName",
                table: "Shipments",
                newName: "ShippingRequest_FromEmail");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseEmail",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromEmail",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromPhone",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseEmail",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromEmail",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromPhone",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "ShippingRequest_FromPhone",
                table: "Shipments",
                newName: "ShippingRequest_ProductName");

            migrationBuilder.RenameColumn(
                name: "ShippingRequest_FromEmail",
                table: "Shipments",
                newName: "Rate_ShippingRequest_ProductName");
        }
    }
}
