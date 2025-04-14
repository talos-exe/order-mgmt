using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnedTypesToShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Label_Amount",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Label_Carrier",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label_Currency",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label_LabelUrl",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label_TrackingNumber",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label_TrackingUrl",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate_Amount",
                table: "Shipments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Rate_Currency",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rate_EstimatedDays",
                table: "Shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rate_Provider",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_RateObjectId",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rate_Service",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tracking_Location",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tracking_Status",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tracking_StatusDate",
                table: "Shipments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label_Amount",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Label_Carrier",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Label_Currency",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Label_LabelUrl",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Label_TrackingNumber",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Label_TrackingUrl",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_Amount",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_Currency",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_EstimatedDays",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_Provider",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_RateObjectId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Rate_Service",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Tracking_Location",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Tracking_Status",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "Tracking_StatusDate",
                table: "Shipments");
        }
    }
}
