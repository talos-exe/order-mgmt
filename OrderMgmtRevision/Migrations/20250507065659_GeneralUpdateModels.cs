using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class GeneralUpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductsActive",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductsTotal",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentsActive",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentsTotal",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "ProductsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProductsTotal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShipmentsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShipmentsTotal",
                table: "AspNetUsers");
        }
    }
}
