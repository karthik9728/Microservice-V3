﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoMobile.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineAndFuelType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Engine = table.Column<int>(type: "int", nullable: false),
                    TopSpeed = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<int>(type: "int", nullable: false),
                    SeatingCapacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceFrom = table.Column<double>(type: "float", nullable: false),
                    PriceTo = table.Column<double>(type: "float", nullable: false),
                    Ratings = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
