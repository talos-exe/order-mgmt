using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystem.Migrations
{
  
    public partial class CompleteAppDbContext : Migration
    {
       
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreightOutbounds_Warehouse_Warehouse_ID",
                table: "FreightOutbounds");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundOrders_Warehouse_Warehouse_ID",
                table: "InboundOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundProductLists_InboundOrders_OrderId",
                table: "InboundProductLists");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundProductLists_Inventories_ProductId",
                table: "InboundProductLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Warehouse_Warehouse_ID",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBasedBillings_BillingAccounts_BillingAccountId",
                table: "OrderBasedBillings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBasedBillings_OrderBasedCharges_OrderChargeId",
                table: "OrderBasedBillings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Inventories_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ParcelOutbounds_Warehouse_Warehouse_ID",
                table: "ParcelOutbounds");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformOrders_Warehouse_Warehouse_ID",
                table: "PlatformOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Billings",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_Billing_Account_ID",
                table: "Billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Customer_ID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Order_Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Shipped_Date",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Billing_ID",
                table: "Billings");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "Warehouses");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "Date_Created");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "User_ID");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Roles",
                newName: "RoleName");

            migrationBuilder.RenameColumn(
                name: "Total_Amount",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "Order_Date",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "Product_ID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderItems",
                newName: "Order_ID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_Product_ID");

            migrationBuilder.RenameColumn(
                name: "Order_Based_Charge_ID",
                table: "OrderBasedCharges",
                newName: "OrderCharge_ID");

            migrationBuilder.RenameColumn(
                name: "OrderChargeId",
                table: "OrderBasedBillings",
                newName: "OrderCharge_ID");

            migrationBuilder.RenameColumn(
                name: "BillingAccountId",
                table: "OrderBasedBillings",
                newName: "Billing_Account_ID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderBasedBillings_OrderChargeId",
                table: "OrderBasedBillings",
                newName: "IX_OrderBasedBillings_OrderCharge_ID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InboundProductLists",
                newName: "Product_ID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "InboundProductLists",
                newName: "Inbound_Order_ID");

            migrationBuilder.RenameIndex(
                name: "IX_InboundProductLists_ProductId",
                table: "InboundProductLists",
                newName: "IX_InboundProductLists_Product_ID");

            migrationBuilder.RenameColumn(
                name: "Recipient_Post_Code",
                table: "FreightOutbounds",
                newName: "Recipient_Postcode");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ParcelOutbounds",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Inventories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Stock_Quantity",
                table: "Inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "InboundOrders",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "FreightOutbounds",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "FreightOutbounds",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Account_Balance",
                table: "BillingAccounts",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Billings",
                table: "Billings",
                column: "Billing_Account_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Warehouse_ID");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Admin_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Company_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account_Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_Need_Audit_Free = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_Availability = table.Column<int>(type: "int", nullable: false),
                    Billing_Account_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Date_Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.User_ID);
                    table.ForeignKey(
                        name: "FK_Customers_BillingAccounts_Billing_Account_ID",
                        column: x => x.Billing_Account_ID,
                        principalTable: "BillingAccounts",
                        principalColumn: "Billing_Account_ID");
                    table.ForeignKey(
                        name: "FK_Customers_Users_Admin_ID",
                        column: x => x.Admin_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                    table.ForeignKey(
                        name: "FK_Customers_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Admin_ID",
                table: "Customers",
                column: "Admin_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Billing_Account_ID",
                table: "Customers",
                column: "Billing_Account_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FreightOutbounds_Warehouses_Warehouse_ID",
                table: "FreightOutbounds",
                column: "Warehouse_ID",
                principalTable: "Warehouses",
                principalColumn: "Warehouse_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundOrders_Warehouses_Warehouse_ID",
                table: "InboundOrders",
                column: "Warehouse_ID",
                principalTable: "Warehouses",
                principalColumn: "Warehouse_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProductLists_InboundOrders_Inbound_Order_ID",
                table: "InboundProductLists",
                column: "Inbound_Order_ID",
                principalTable: "InboundOrders",
                principalColumn: "Inbound_Order_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProductLists_Inventories_Product_ID",
                table: "InboundProductLists",
                column: "Product_ID",
                principalTable: "Inventories",
                principalColumn: "Product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Warehouses_Warehouse_ID",
                table: "Inventories",
                column: "Warehouse_ID",
                principalTable: "Warehouses",
                principalColumn: "Warehouse_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBasedBillings_BillingAccounts_Billing_Account_ID",
                table: "OrderBasedBillings",
                column: "Billing_Account_ID",
                principalTable: "BillingAccounts",
                principalColumn: "Billing_Account_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBasedBillings_OrderBasedCharges_OrderCharge_ID",
                table: "OrderBasedBillings",
                column: "OrderCharge_ID",
                principalTable: "OrderBasedCharges",
                principalColumn: "OrderCharge_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Inventories_Product_ID",
                table: "OrderItems",
                column: "Product_ID",
                principalTable: "Inventories",
                principalColumn: "Product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_Order_ID",
                table: "OrderItems",
                column: "Order_ID",
                principalTable: "Orders",
                principalColumn: "Order_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParcelOutbounds_Warehouses_Warehouse_ID",
                table: "ParcelOutbounds",
                column: "Warehouse_ID",
                principalTable: "Warehouses",
                principalColumn: "Warehouse_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformOrders_Warehouses_Warehouse_ID",
                table: "PlatformOrders",
                column: "Warehouse_ID",
                principalTable: "Warehouses",
                principalColumn: "Warehouse_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreightOutbounds_Warehouses_Warehouse_ID",
                table: "FreightOutbounds");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundOrders_Warehouses_Warehouse_ID",
                table: "InboundOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundProductLists_InboundOrders_Inbound_Order_ID",
                table: "InboundProductLists");

            migrationBuilder.DropForeignKey(
                name: "FK_InboundProductLists_Inventories_Product_ID",
                table: "InboundProductLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Warehouses_Warehouse_ID",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBasedBillings_BillingAccounts_Billing_Account_ID",
                table: "OrderBasedBillings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderBasedBillings_OrderBasedCharges_OrderCharge_ID",
                table: "OrderBasedBillings");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Inventories_Product_ID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_Order_ID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ParcelOutbounds_Warehouses_Warehouse_ID",
                table: "ParcelOutbounds");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformOrders_Warehouses_Warehouse_ID",
                table: "PlatformOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Billings",
                table: "Billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "Stock_Quantity",
                table: "Inventories");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "Warehouse");

            migrationBuilder.RenameColumn(
                name: "Date_Created",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Roles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Orders",
                newName: "Total_Amount");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "Order_Date");

            migrationBuilder.RenameColumn(
                name: "Product_ID",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Order_ID",
                table: "OrderItems",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_Product_ID",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameColumn(
                name: "OrderCharge_ID",
                table: "OrderBasedCharges",
                newName: "Order_Based_Charge_ID");

            migrationBuilder.RenameColumn(
                name: "OrderCharge_ID",
                table: "OrderBasedBillings",
                newName: "OrderChargeId");

            migrationBuilder.RenameColumn(
                name: "Billing_Account_ID",
                table: "OrderBasedBillings",
                newName: "BillingAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderBasedBillings_OrderCharge_ID",
                table: "OrderBasedBillings",
                newName: "IX_OrderBasedBillings_OrderChargeId");

            migrationBuilder.RenameColumn(
                name: "Product_ID",
                table: "InboundProductLists",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Inbound_Order_ID",
                table: "InboundProductLists",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_InboundProductLists_Product_ID",
                table: "InboundProductLists",
                newName: "IX_InboundProductLists_ProductId");

            migrationBuilder.RenameColumn(
                name: "Recipient_Postcode",
                table: "FreightOutbounds",
                newName: "Recipient_Post_Code");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ParcelOutbounds",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "Customer_ID",
                table: "Orders",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Order_Status",
                table: "Orders",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Shipped_Date",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "InboundOrders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "FreightOutbounds",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "FreightOutbounds",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "Billing_ID",
                table: "Billings",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Account_Balance",
                table: "BillingAccounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Billings",
                table: "Billings",
                column: "Billing_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Billing_Account_ID",
                table: "Billings",
                column: "Billing_Account_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_FreightOutbounds_Warehouse_Warehouse_ID",
                table: "FreightOutbounds",
                column: "Warehouse_ID",
                principalTable: "Warehouse",
                principalColumn: "Warehouse_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboundOrders_Warehouse_Warehouse_ID",
                table: "InboundOrders",
                column: "Warehouse_ID",
                principalTable: "Warehouse",
                principalColumn: "Warehouse_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProductLists_InboundOrders_OrderId",
                table: "InboundProductLists",
                column: "OrderId",
                principalTable: "InboundOrders",
                principalColumn: "Inbound_Order_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InboundProductLists_Inventories_ProductId",
                table: "InboundProductLists",
                column: "ProductId",
                principalTable: "Inventories",
                principalColumn: "Product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Warehouse_Warehouse_ID",
                table: "Inventories",
                column: "Warehouse_ID",
                principalTable: "Warehouse",
                principalColumn: "Warehouse_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBasedBillings_BillingAccounts_BillingAccountId",
                table: "OrderBasedBillings",
                column: "BillingAccountId",
                principalTable: "BillingAccounts",
                principalColumn: "Billing_Account_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBasedBillings_OrderBasedCharges_OrderChargeId",
                table: "OrderBasedBillings",
                column: "OrderChargeId",
                principalTable: "OrderBasedCharges",
                principalColumn: "Order_Based_Charge_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Inventories_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Inventories",
                principalColumn: "Product_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Order_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParcelOutbounds_Warehouse_Warehouse_ID",
                table: "ParcelOutbounds",
                column: "Warehouse_ID",
                principalTable: "Warehouse",
                principalColumn: "Warehouse_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformOrders_Warehouse_Warehouse_ID",
                table: "PlatformOrders",
                column: "Warehouse_ID",
                principalTable: "Warehouse",
                principalColumn: "Warehouse_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
