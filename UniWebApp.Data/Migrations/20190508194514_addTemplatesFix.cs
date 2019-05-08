using Microsoft.EntityFrameworkCore.Migrations;

namespace UniWebApp.Data.Migrations
{
    public partial class addTemplatesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataFieldsTemplateComboboxOptions_DataFieldsTemplate_ComboboxId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.DropIndex(
                name: "IX_DataFieldsTemplateComboboxOptions_ComboboxId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.DropColumn(
                name: "ComboboxId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.AddColumn<int>(
                name: "DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DataFieldsTemplateComboboxOptions_DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions",
                column: "DataFieldTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataFieldsTemplateComboboxOptions_DataFieldsTemplate_DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions",
                column: "DataFieldTemplateId",
                principalTable: "DataFieldsTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataFieldsTemplateComboboxOptions_DataFieldsTemplate_DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.DropIndex(
                name: "IX_DataFieldsTemplateComboboxOptions_DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.DropColumn(
                name: "DataFieldTemplateId",
                table: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.AddColumn<int>(
                name: "ComboboxId",
                table: "DataFieldsTemplateComboboxOptions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataFieldsTemplateComboboxOptions_ComboboxId",
                table: "DataFieldsTemplateComboboxOptions",
                column: "ComboboxId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataFieldsTemplateComboboxOptions_DataFieldsTemplate_ComboboxId",
                table: "DataFieldsTemplateComboboxOptions",
                column: "ComboboxId",
                principalTable: "DataFieldsTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
