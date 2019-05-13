using Microsoft.EntityFrameworkCore.Migrations;

namespace UniWebApp.Data.Migrations
{
    public partial class removedMustHaveField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustHave",
                table: "DataFieldsTemplate");

            migrationBuilder.DropColumn(
                name: "Major",
                table: "AppEntityFields");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustHave",
                table: "DataFieldsTemplate",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Major",
                table: "AppEntityFields",
                nullable: false,
                defaultValue: false);
        }
    }
}
