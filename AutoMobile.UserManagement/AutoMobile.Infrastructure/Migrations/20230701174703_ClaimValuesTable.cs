using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoMobile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClaimValuesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimValue",
                table: "CustomClaimTypeValue");

            migrationBuilder.CreateTable(
                name: "CustomClaimValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomClaimTypeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClaimValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomClaimValues_CustomClaimTypeValue_CustomClaimTypeValueId",
                        column: x => x.CustomClaimTypeValueId,
                        principalTable: "CustomClaimTypeValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomClaimValues_CustomClaimTypeValueId",
                table: "CustomClaimValues",
                column: "CustomClaimTypeValueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomClaimValues");

            migrationBuilder.AddColumn<string>(
                name: "ClaimValue",
                table: "CustomClaimTypeValue",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
