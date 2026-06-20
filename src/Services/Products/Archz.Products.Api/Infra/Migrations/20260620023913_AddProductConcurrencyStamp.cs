using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archz.Products.Api.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddProductConcurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "concurrency_stamp",
                table: "products",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "products");
        }
    }
}
