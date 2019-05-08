using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniWebApp.Data.Migrations
{
    public partial class addTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataFieldsTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    FieldType = table.Column<int>(nullable: false),
                    MustHave = table.Column<bool>(nullable: false),
                    EntityTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFieldsTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataFieldsTemplate_AppEntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "AppEntityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataFieldsTemplateComboboxOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ComboboxId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataFieldsTemplateComboboxOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataFieldsTemplateComboboxOptions_DataFieldsTemplate_ComboboxId",
                        column: x => x.ComboboxId,
                        principalTable: "DataFieldsTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataFieldsTemplate_EntityTypeId",
                table: "DataFieldsTemplate",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataFieldsTemplateComboboxOptions_ComboboxId",
                table: "DataFieldsTemplateComboboxOptions",
                column: "ComboboxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataFieldsTemplateComboboxOptions");

            migrationBuilder.DropTable(
                name: "DataFieldsTemplate");
        }
    }
}
