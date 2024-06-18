using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    public partial class ApplicationUsers_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_MoodType_MoodTypeId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoodType",
                table: "MoodType");

            migrationBuilder.RenameTable(
                name: "MoodType",
                newName: "MoodTypes");

            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "PostComments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoodTypes",
                table: "MoodTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_MoodTypes_MoodTypeId",
                table: "Posts",
                column: "MoodTypeId",
                principalTable: "MoodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_MoodTypes_MoodTypeId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoodTypes",
                table: "MoodTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "MoodTypes",
                newName: "MoodType");

            migrationBuilder.AlterColumn<string>(
                name: "CommentText",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "ApplicationUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoodType",
                table: "MoodType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_MoodType_MoodTypeId",
                table: "Posts",
                column: "MoodTypeId",
                principalTable: "MoodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
