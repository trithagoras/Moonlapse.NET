using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoonlapseServer.Migrations
{
    public partial class Components : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Components_ComponentId",
                table: "Components");

            migrationBuilder.DropIndex(
                name: "IX_Components_ComponentId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "Components");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "Components",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_ComponentId",
                table: "Components",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Components_ComponentId",
                table: "Components",
                column: "ComponentId",
                principalTable: "Components",
                principalColumn: "Id");
        }
    }
}
