﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarrierId",
                table: "orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "delivered_date",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "shipped_date",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tracking_number",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "carriers",
                columns: table => new
                {
                    carrier_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    carrier_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    contact_url = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("carriers_pkey", x => x.carrier_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_CarrierId",
                table: "orders",
                column: "CarrierId");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_carrier",
                table: "orders",
                column: "CarrierId",
                principalTable: "carriers",
                principalColumn: "carrier_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_carrier",
                table: "orders");

            migrationBuilder.DropTable(
                name: "carriers");

            migrationBuilder.DropIndex(
                name: "IX_orders_CarrierId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivered_date",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipped_date",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "tracking_number",
                table: "orders");
        }
    }
}
