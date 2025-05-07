using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentLogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentStatusHistories_Shipments_ShipmentID",
                table: "ShipmentStatusHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentStatusHistories",
                table: "ShipmentStatusHistories");

            migrationBuilder.RenameTable(
                name: "ShipmentStatusHistories",
                newName: "ShipmentLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentStatusHistories_ShipmentID",
                table: "ShipmentLogs",
                newName: "IX_ShipmentLogs_ShipmentID");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ShipmentLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentID",
                table: "ShipmentLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ShipmentLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "ShipmentLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentLogs",
                table: "ShipmentLogs",
                column: "ShipmentStatusHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentLogs_Shipments_ShipmentID",
                table: "ShipmentLogs",
                column: "ShipmentID",
                principalTable: "Shipments",
                principalColumn: "ShipmentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentLogs_Shipments_ShipmentID",
                table: "ShipmentLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipmentLogs",
                table: "ShipmentLogs");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "ShipmentLogs");

            migrationBuilder.RenameTable(
                name: "ShipmentLogs",
                newName: "ShipmentStatusHistories");

            migrationBuilder.RenameIndex(
                name: "IX_ShipmentLogs_ShipmentID",
                table: "ShipmentStatusHistories",
                newName: "IX_ShipmentStatusHistories_ShipmentID");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ShipmentStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentID",
                table: "ShipmentStatusHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "ShipmentStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipmentStatusHistories",
                table: "ShipmentStatusHistories",
                column: "ShipmentStatusHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentStatusHistories_Shipments_ShipmentID",
                table: "ShipmentStatusHistories",
                column: "ShipmentID",
                principalTable: "Shipments",
                principalColumn: "ShipmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
