using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFC_TPC_CollectionNavigationInsertsOrder.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrateLeafDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MagicSkills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RunicName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MartialSkills",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    HasStrike = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MartialSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerToSkill",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    SkillId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerToSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerToSkill_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToSkill_PlayerId",
                table: "PlayerToSkill",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToSkill_SkillId",
                table: "PlayerToSkill",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MagicSkills");

            migrationBuilder.DropTable(
                name: "MartialSkills");

            migrationBuilder.DropTable(
                name: "PlayerToSkill");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
