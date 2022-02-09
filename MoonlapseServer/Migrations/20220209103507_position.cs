using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoonlapseServer.Migrations
{
    public partial class position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Components",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Garden" });

            migrationBuilder.CreateIndex(
                name: "IX_Components_RoomId",
                table: "Components",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Rooms_RoomId",
                table: "Components",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Rooms_RoomId",
                table: "Components");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Components_RoomId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Components");
        }
    }
}
