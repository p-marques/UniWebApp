using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UniWebApp.Data.Migrations
{
    public partial class addedRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppEntityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEntityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEntities_AppEntityTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AppEntityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppEntityFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    Section = table.Column<string>(nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Value = table.Column<bool>(nullable: true),
                    SelectedOption = table.Column<int>(nullable: true),
                    AppEntityDataFieldDate_Value = table.Column<DateTime>(nullable: true),
                    AppEntityDataFieldNumber_Value = table.Column<decimal>(nullable: true),
                    AppEntityDataFieldText_Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEntityFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEntityFields_AppEntities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AppEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppEntityRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityId = table.Column<int>(nullable: false),
                    relatedEntityId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEntityRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEntityRelations_AppEntities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "AppEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppEntityDataFieldComboboxOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ComboboxId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEntityDataFieldComboboxOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEntityDataFieldComboboxOptions_AppEntityFields_ComboboxId",
                        column: x => x.ComboboxId,
                        principalTable: "AppEntityFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppEntityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Pessoa" });

            migrationBuilder.InsertData(
                table: "AppEntityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Pessoa Coletiva" });

            migrationBuilder.InsertData(
                table: "AppEntityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Empresa" });

            migrationBuilder.CreateIndex(
                name: "IX_AppEntities_TypeId",
                table: "AppEntities",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEntityDataFieldComboboxOptions_ComboboxId",
                table: "AppEntityDataFieldComboboxOptions",
                column: "ComboboxId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEntityFields_EntityId",
                table: "AppEntityFields",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEntityRelations_EntityId",
                table: "AppEntityRelations",
                column: "EntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppEntityDataFieldComboboxOptions");

            migrationBuilder.DropTable(
                name: "AppEntityRelations");

            migrationBuilder.DropTable(
                name: "AppEntityFields");

            migrationBuilder.DropTable(
                name: "AppEntities");

            migrationBuilder.DropTable(
                name: "AppEntityTypes");
        }
    }
}
