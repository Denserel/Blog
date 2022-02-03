using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class UpdatedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "SubComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "MainComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "SubComments");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "MainComments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MainComments_Posts_PostId",
                table: "MainComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
