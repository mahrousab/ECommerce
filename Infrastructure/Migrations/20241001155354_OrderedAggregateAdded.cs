using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderedAggregateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddressName = table.Column<string>(name: "ShippingAddress_Name", type: "nvarchar(max)", nullable: false),
                    ShippingAddressLine1 = table.Column<string>(name: "ShippingAddress_Line1", type: "nvarchar(max)", nullable: false),
                    ShippingAddressLine2 = table.Column<string>(name: "ShippingAddress_Line2", type: "nvarchar(max)", nullable: true),
                    ShippingAddressCity = table.Column<string>(name: "ShippingAddress_City", type: "nvarchar(max)", nullable: false),
                    ShippingAddressState = table.Column<string>(name: "ShippingAddress_State", type: "nvarchar(max)", nullable: false),
                    ShippingAddressPostalCode = table.Column<string>(name: "ShippingAddress_PostalCode", type: "nvarchar(max)", nullable: false),
                    ShippingAddressCountry = table.Column<string>(name: "ShippingAddress_Country", type: "nvarchar(max)", nullable: false),
                    DeliveryMethodId = table.Column<int>(type: "int", nullable: false),
                    PaymentSummaryLast4 = table.Column<int>(name: "PaymentSummary_Last4", type: "int", nullable: false),
                    PaymentSummaryBrand = table.Column<string>(name: "PaymentSummary_Brand", type: "nvarchar(max)", nullable: false),
                    PaymentSummaryExpMonth = table.Column<int>(name: "PaymentSummary_ExpMonth", type: "int", nullable: false),
                    PaymentSummaryExpYear = table.Column<int>(name: "PaymentSummary_ExpYear", type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemOrderedProductId = table.Column<int>(name: "ItemOrdered_ProductId", type: "int", nullable: false),
                    ItemOrderedProductName = table.Column<string>(name: "ItemOrdered_ProductName", type: "nvarchar(max)", nullable: false),
                    ItemOrderedPictureUrl = table.Column<string>(name: "ItemOrdered_PictureUrl", type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
