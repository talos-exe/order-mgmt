using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderMgmtRevision.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPasswordChange = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ShipAmount = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAll",
                columns: table => new
                {
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAll", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_InventoryAll_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAll_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    SourceWarehouseID = table.Column<int>(type: "int", nullable: false),
                    DestinationWarehouseID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedArrival = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelectedRateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_RateObjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_Service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate_Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_EstimatedDays = table.Column<int>(type: "int", nullable: true),
                    Rate_ShippingRequestId = table.Column<int>(type: "int", nullable: false),
                    Rate_ShippingRequest_FromName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_FromStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_FromCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_FromState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_FromZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_ToName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_ToStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_ToCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_ToState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_ToZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate_ShippingRequest_Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate_ShippingRequest_Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate_ShippingRequest_Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate_ShippingRequest_Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Label_LabelUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label_TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label_TrackingUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label_Carrier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Label_Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tracking_Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tracking_StatusDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tracking_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_FromName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_FromStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_FromCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_FromState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_FromZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_ToName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_ToStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_ToCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_ToState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_ToZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingRequest_Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingRequest_Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingRequest_Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingRequest_Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentID);
                    table.ForeignKey(
                        name: "FK_Shipments_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID");
                    table.ForeignKey(
                        name: "FK_Shipments_Warehouses_DestinationWarehouseID",
                        column: x => x.DestinationWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID");
                    table.ForeignKey(
                        name: "FK_Shipments_Warehouses_SourceWarehouseID",
                        column: x => x.SourceWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseID");
                });

            migrationBuilder.CreateTable(
                name: "ShipmentStatusHistories",
                columns: table => new
                {
                    ShipmentStatusHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShipmentID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentStatusHistories", x => x.ShipmentStatusHistoryId);
                    table.ForeignKey(
                        name: "FK_ShipmentStatusHistories_Shipments_ShipmentID",
                        column: x => x.ShipmentID,
                        principalTable: "Shipments",
                        principalColumn: "ShipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAll_ProductID",
                table: "InventoryAll",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAll_WarehouseID",
                table: "InventoryAll",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DestinationWarehouseID",
                table: "Shipments",
                column: "DestinationWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_ProductID",
                table: "Shipments",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_SourceWarehouseID",
                table: "Shipments",
                column: "SourceWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentStatusHistories_ShipmentID",
                table: "ShipmentStatusHistories",
                column: "ShipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId",
                table: "UserLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "InventoryAll");

            migrationBuilder.DropTable(
                name: "ShipmentStatusHistories");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
