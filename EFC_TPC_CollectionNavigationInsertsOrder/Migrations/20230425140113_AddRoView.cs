using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFC_TPC_CollectionNavigationInsertsOrder.Migrations
{
    /// <inheritdoc />
    public partial class AddRoView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SkillRoViewId",
                table: "PlayerToSkill",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SkillRoView",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillRoView", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToSkill_SkillRoViewId",
                table: "PlayerToSkill",
                column: "SkillRoViewId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_PlayerToSkill_SkillRoView_SkillRoViewId",
            //     table: "PlayerToSkill",
            //     column: "SkillRoViewId",
            //     principalTable: "SkillRoView",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerToSkill_SkillRoView_SkillRoViewId",
                table: "PlayerToSkill");

            migrationBuilder.DropTable(
                name: "SkillRoView");

            migrationBuilder.DropIndex(
                name: "IX_PlayerToSkill_SkillRoViewId",
                table: "PlayerToSkill");

            migrationBuilder.DropColumn(
                name: "SkillRoViewId",
                table: "PlayerToSkill");
        }
    }
}
