using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    Charge_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Charge_Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.Charge_ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Role_Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Warehouse_ID);
                });

            migrationBuilder.CreateTable(
                name: "BillingAccounts",
                columns: table => new
                {
                    Billing_Account_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Account_Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAccounts", x => x.Billing_Account_ID);
                    table.ForeignKey(
                        name: "FK_BillingAccounts_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Customer_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shipped_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order_Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Total_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_Orders_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Role_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.User_ID, x.Role_ID });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_Role_ID",
                        column: x => x.Role_ID,
                        principalTable: "Roles",
                        principalColumn: "Role_ID");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FreightOutbounds",
                columns: table => new
                {
                    Outbound_Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_Quantity = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estimated_Delivery_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_Ship_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Recipient_Post_Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Destination_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Shipping_Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Transport_Days = table.Column<int>(type: "int", nullable: false),
                    Related_Adjustment_Order = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Tracking_Number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Reference_Order_Number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FBA_Shipment_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FBA_Tracking_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Outbound_Method = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreightOutbounds", x => x.Outbound_Order_ID);
                    table.ForeignKey(
                        name: "FK_FreightOutbounds_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_FreightOutbounds_Warehouse_Warehouse_ID",
                        column: x => x.Warehouse_ID,
                        principalTable: "Warehouse",
                        principalColumn: "Warehouse_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InboundOrders",
                columns: table => new
                {
                    Inbound_Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Estimated_Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Product_Quantity = table.Column<int>(type: "int", nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Boxes = table.Column<int>(type: "int", nullable: false),
                    Inbound_Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Tracking_Number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Reference_Order_Number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Arrival_Method = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundOrders", x => x.Inbound_Order_ID);
                    table.ForeignKey(
                        name: "FK_InboundOrders_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_InboundOrders_Warehouse_Warehouse_ID",
                        column: x => x.Warehouse_ID,
                        principalTable: "Warehouse",
                        principalColumn: "Warehouse_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Product_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Product_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Product_Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Product_ID);
                    table.ForeignKey(
                        name: "FK_Inventories_Warehouse_Warehouse_ID",
                        column: x => x.Warehouse_ID,
                        principalTable: "Warehouse",
                        principalColumn: "Warehouse_ID");
                });

            migrationBuilder.CreateTable(
                name: "ParcelOutbounds",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estimated_Delivery_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ship_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Transport_Days = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Tracking_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Reference_Order_Number = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Related_Adjustment_Order = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Creation_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Boxes = table.Column<int>(type: "int", nullable: false),
                    Shipping_Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Latest_Information = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tracking_Update_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Internet_Posting_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Delivery_Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelOutbounds", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_ParcelOutbounds_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_ParcelOutbounds_Warehouse_Warehouse_ID",
                        column: x => x.Warehouse_ID,
                        principalTable: "Warehouse",
                        principalColumn: "Warehouse_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformOrders",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Warehouse_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_Quantity = table.Column<int>(type: "int", nullable: false),
                    User_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Buyer = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Recipient_Postcode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Recipient_Country = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Store = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Site = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Shipping_Service = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Tracking_Number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Payment_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_Source = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformOrders", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_PlatformOrders_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_PlatformOrders_Warehouse_Warehouse_ID",
                        column: x => x.Warehouse_ID,
                        principalTable: "Warehouse",
                        principalColumn: "Warehouse_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    Billing_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Billing_Account_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Charge_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date_Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.Billing_ID);
                    table.ForeignKey(
                        name: "FK_Billings_BillingAccounts_Billing_Account_ID",
                        column: x => x.Billing_Account_ID,
                        principalTable: "BillingAccounts",
                        principalColumn: "Billing_Account_ID");
                    table.ForeignKey(
                        name: "FK_Billings_Charges_Charge_ID",
                        column: x => x.Charge_ID,
                        principalTable: "Charges",
                        principalColumn: "Charge_ID");
                });

            migrationBuilder.CreateTable(
                name: "OrderBasedCharges",
                columns: table => new
                {
                    Order_Based_Charge_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Charge_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date_Charged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBasedCharges", x => x.Order_Based_Charge_ID);
                    table.ForeignKey(
                        name: "FK_OrderBasedCharges_Charges_Charge_ID",
                        column: x => x.Charge_ID,
                        principalTable: "Charges",
                        principalColumn: "Charge_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderBasedCharges_Orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Orders",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreightProductLists",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreightProductLists", x => new { x.Order_ID, x.Product_ID });
                    table.ForeignKey(
                        name: "FK_FreightProductLists_FreightOutbounds_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "FreightOutbounds",
                        principalColumn: "Outbound_Order_ID");
                    table.ForeignKey(
                        name: "FK_FreightProductLists_Inventories_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Inventories",
                        principalColumn: "Product_ID");
                });

            migrationBuilder.CreateTable(
                name: "InboundProductLists",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundProductLists", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_InboundProductLists_InboundOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "InboundOrders",
                        principalColumn: "Inbound_Order_ID");
                    table.ForeignKey(
                        name: "FK_InboundProductLists_Inventories_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Inventories",
                        principalColumn: "Product_ID");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderItems_Inventories_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Inventories",
                        principalColumn: "Product_ID");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Order_ID");
                });

            migrationBuilder.CreateTable(
                name: "ParcelProductLists",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelProductLists", x => new { x.Order_ID, x.Product_ID });
                    table.ForeignKey(
                        name: "FK_ParcelProductLists_Inventories_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Inventories",
                        principalColumn: "Product_ID");
                    table.ForeignKey(
                        name: "FK_ParcelProductLists_ParcelOutbounds_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "ParcelOutbounds",
                        principalColumn: "Order_ID");
                });

            migrationBuilder.CreateTable(
                name: "PlatformProductLists",
                columns: table => new
                {
                    Order_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Product_ID = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformProductLists", x => new { x.Order_ID, x.Product_ID });
                    table.ForeignKey(
                        name: "FK_PlatformProductLists_Inventories_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Inventories",
                        principalColumn: "Product_ID");
                    table.ForeignKey(
                        name: "FK_PlatformProductLists_PlatformOrders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "PlatformOrders",
                        principalColumn: "Order_ID");
                });

            migrationBuilder.CreateTable(
                name: "OrderBasedBillings",
                columns: table => new
                {
                    BillingAccountId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    OrderChargeId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBasedBillings", x => new { x.BillingAccountId, x.OrderChargeId });
                    table.ForeignKey(
                        name: "FK_OrderBasedBillings_BillingAccounts_BillingAccountId",
                        column: x => x.BillingAccountId,
                        principalTable: "BillingAccounts",
                        principalColumn: "Billing_Account_ID");
                    table.ForeignKey(
                        name: "FK_OrderBasedBillings_OrderBasedCharges_OrderChargeId",
                        column: x => x.OrderChargeId,
                        principalTable: "OrderBasedCharges",
                        principalColumn: "Order_Based_Charge_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillingAccounts_User_ID",
                table: "BillingAccounts",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Billing_Account_ID",
                table: "Billings",
                column: "Billing_Account_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_Charge_ID",
                table: "Billings",
                column: "Charge_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FreightOutbounds_User_ID",
                table: "FreightOutbounds",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FreightOutbounds_Warehouse_ID",
                table: "FreightOutbounds",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_FreightProductLists_Product_ID",
                table: "FreightProductLists",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrders_User_ID",
                table: "InboundOrders",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrders_Warehouse_ID",
                table: "InboundOrders",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InboundProductLists_ProductId",
                table: "InboundProductLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_Warehouse_ID",
                table: "Inventories",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBasedBillings_OrderChargeId",
                table: "OrderBasedBillings",
                column: "OrderChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBasedCharges_Charge_ID",
                table: "OrderBasedCharges",
                column: "Charge_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBasedCharges_Order_ID",
                table: "OrderBasedCharges",
                column: "Order_ID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_ID",
                table: "Orders",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelOutbounds_User_ID",
                table: "ParcelOutbounds",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelOutbounds_Warehouse_ID",
                table: "ParcelOutbounds",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelProductLists_Product_ID",
                table: "ParcelProductLists",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformOrders_User_ID",
                table: "PlatformOrders",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformOrders_Warehouse_ID",
                table: "PlatformOrders",
                column: "Warehouse_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformProductLists_Product_ID",
                table: "PlatformProductLists",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Role_ID",
                table: "UserRoles",
                column: "Role_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Billings");

            migrationBuilder.DropTable(
                name: "FreightProductLists");

            migrationBuilder.DropTable(
                name: "InboundProductLists");

            migrationBuilder.DropTable(
                name: "OrderBasedBillings");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ParcelProductLists");

            migrationBuilder.DropTable(
                name: "PlatformProductLists");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "FreightOutbounds");

            migrationBuilder.DropTable(
                name: "InboundOrders");

            migrationBuilder.DropTable(
                name: "BillingAccounts");

            migrationBuilder.DropTable(
                name: "OrderBasedCharges");

            migrationBuilder.DropTable(
                name: "ParcelOutbounds");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "PlatformOrders");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Charges");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
