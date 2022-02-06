using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoonlapseServer.Migrations
{
    public partial class AddedComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Entities_EntityId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Components_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComponentId = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_EntityId",
                table: "Components",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionComponents_ComponentId",
                table: "PositionComponents",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Entities_EntityId",
                table: "Users",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Entities_EntityId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "PositionComponents");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.AlterColumn<int>(
                name: "EntityId",
                table: "Users",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Entities_EntityId",
                table: "Users",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id");
        }
    }
}
