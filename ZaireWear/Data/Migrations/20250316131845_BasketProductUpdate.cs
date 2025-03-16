using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class BasketProductUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "BasketProducts");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "BasketProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "BasketProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_ColorId",
                table: "BasketProducts",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProducts_SizeId",
                table: "BasketProducts",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Colors_ColorId",
                table: "BasketProducts",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProducts_Sizes_SizeId",
                table: "BasketProducts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Colors_ColorId",
                table: "BasketProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProducts_Sizes_SizeId",
                table: "BasketProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_ColorId",
                table: "BasketProducts");

            migrationBuilder.DropIndex(
                name: "IX_BasketProducts_SizeId",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "BasketProducts");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "BasketProducts");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "BasketProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "BasketProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
