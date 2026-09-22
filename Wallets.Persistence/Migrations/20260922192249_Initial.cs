using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallets.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wallets",
                columns: table => new
                {
                    UserExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    SchemaType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.UserExternalId);
                });

            migrationBuilder.CreateTable(
                name: "commissions",
                columns: table => new
                {
                    EventExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    SchemaType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commissions", x => new { x.EventExternalId, x.UserExternalId });
                    table.ForeignKey(
                        name: "FK_commissions_wallets_UserExternalId",
                        column: x => x.UserExternalId,
                        principalTable: "wallets",
                        principalColumn: "UserExternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commissions_EventExternalId_Level",
                table: "commissions",
                columns: new[] { "EventExternalId", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_commissions_UserExternalId",
                table: "commissions",
                column: "UserExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commissions");

            migrationBuilder.DropTable(
                name: "wallets");
        }
    }
}
