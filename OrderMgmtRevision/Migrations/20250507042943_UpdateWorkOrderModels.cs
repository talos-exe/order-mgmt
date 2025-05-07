using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkOrderModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "WorkOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "WorkOrders");
        }
    }
}
