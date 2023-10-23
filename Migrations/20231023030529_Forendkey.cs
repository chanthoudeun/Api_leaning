using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_leaning.Migrations
{
    /// <inheritdoc />
    public partial class Forendkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Characters_characterId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_characterId",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "characterId",
                table: "Weapons",
                newName: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons",
                column: "CharacterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Characters_CharacterId",
                table: "Weapons",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Characters_CharacterId",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_CharacterId",
                table: "Weapons");

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "Weapons",
                newName: "characterId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_characterId",
                table: "Weapons",
                column: "characterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Characters_characterId",
                table: "Weapons",
                column: "characterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
