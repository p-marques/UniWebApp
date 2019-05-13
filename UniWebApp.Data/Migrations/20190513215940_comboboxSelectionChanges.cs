using Microsoft.EntityFrameworkCore.Migrations;

namespace UniWebApp.Data.Migrations
{
    public partial class comboboxSelectionChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEntityFields_AppEntityDataFieldComboboxOptions_SelectedOptionId",
                table: "AppEntityFields");

            migrationBuilder.DropIndex(
                name: "IX_AppEntityFields_SelectedOptionId",
                table: "AppEntityFields");

            migrationBuilder.RenameColumn(
                name: "SelectedOptionId",
                table: "AppEntityFields",
                newName: "SelectedOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SelectedOption",
                table: "AppEntityFields",
                newName: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEntityFields_SelectedOptionId",
                table: "AppEntityFields",
                column: "SelectedOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppEntityFields_AppEntityDataFieldComboboxOptions_SelectedOptionId",
                table: "AppEntityFields",
                column: "SelectedOptionId",
                principalTable: "AppEntityDataFieldComboboxOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
