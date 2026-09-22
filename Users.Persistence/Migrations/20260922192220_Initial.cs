using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ExternalId);
                });

            migrationBuilder.CreateTable(
                name: "partner_relations",
                columns: table => new
                {
                    UserExternalId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    PartnerExternalId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner_relations", x => new { x.UserExternalId, x.Level });
                    table.ForeignKey(
                        name: "FK_partner_relations_users_PartnerExternalId",
                        column: x => x.PartnerExternalId,
                        principalTable: "users",
                        principalColumn: "ExternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_partner_relations_users_UserExternalId",
                        column: x => x.UserExternalId,
                        principalTable: "users",
                        principalColumn: "ExternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_partner_relations_PartnerExternalId",
                table: "partner_relations",
                column: "PartnerExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_relations_UserExternalId_PartnerExternalId",
                table: "partner_relations",
                columns: new[] { "UserExternalId", "PartnerExternalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partner_relations");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
