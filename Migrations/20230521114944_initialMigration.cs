using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SubsAPI.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenExpiration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenExpiration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscribeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnsubscribeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscribers_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "DateCreated", "Password" },
                values: new object[] { "fdf8fe94d", new DateTime(2023, 5, 21, 11, 49, 44, 478, DateTimeKind.Utc).AddTicks(9593), "DAAAAO/ETBY5JLXHIkNeuRAAAACQc/FfqR79d1QA8ucx/SBW4VCAsUAsiiSn4Q==" });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "DateCreated", "Password" },
                values: new object[] { "1e471916b", new DateTime(2023, 5, 21, 11, 49, 44, 479, DateTimeKind.Utc).AddTicks(155), "DAAAAGozHmFsp6U3h7X9aBAAAADqAJCyM0rzChYg/9MuBtBNNXmMJJtywpiYmw==" });

            migrationBuilder.InsertData(
                table: "TokenExpiration",
                columns: new[] { "Id", "Hours" },
                values: new object[] { 1, 8 });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_ServiceId",
                table: "Subscribers",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_ServiceId",
                table: "UserTokens",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribers");

            migrationBuilder.DropTable(
                name: "TokenExpiration");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
