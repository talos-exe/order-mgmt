using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequestId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromCity",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromEmail",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromName",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromPhone",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromState",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromStreet",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_FromZip",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_Height",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_Length",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_ToCity",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_ToName",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_ToState",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_ToStreet",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_ToZip",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_Weight",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_ShippingRequest_Width",
                table: "Shipments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate_ShippingRequestId",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromCity",
                table: "Shipments",
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
                name: "Rate_ShippingRequest_FromName",
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

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromState",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromStreet",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_FromZip",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_ShippingRequest_Height",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_ShippingRequest_Length",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_ToCity",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_ToName",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_ToState",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_ToStreet",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_ShippingRequest_ToZip",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_ShippingRequest_Weight",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_ShippingRequest_Width",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
