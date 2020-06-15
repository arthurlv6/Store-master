using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Server.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserRefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserRefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headimgurl",
                table: "UserRefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "UserRefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "UserRefreshTokens",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOrderLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UserOrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Quatity = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RRP = table.Column<decimal>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ProductCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrderLines_UserOrders_UserOrderId",
                        column: x => x.UserOrderId,
                        principalTable: "UserOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrderLines_UserOrderId",
                table: "UserOrderLines",
                column: "UserOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOrderLines");

            migrationBuilder.DropTable(
                name: "UserOrders");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Headimgurl",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "UserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProductCategories");
        }
    }
}
