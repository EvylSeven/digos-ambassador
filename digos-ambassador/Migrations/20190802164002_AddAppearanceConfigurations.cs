﻿// <auto-generated />
#pragma warning disable CS1591
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUsingDirective
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGOS.Ambassador.Migrations
{
    public partial class AddAppearanceConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppearanceConfigurations",
                schema: "TransformationModule",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CharacterID = table.Column<long>(nullable: false),
                    DefaultAppearanceID = table.Column<long>(nullable: false),
                    CurrentAppearanceID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppearanceConfigurations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppearanceConfigurations_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalSchema: "CharacterModule",
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppearanceConfigurations_Appearances_CurrentAppearanceID",
                        column: x => x.CurrentAppearanceID,
                        principalSchema: "TransformationModule",
                        principalTable: "Appearances",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppearanceConfigurations_Appearances_DefaultAppearanceID",
                        column: x => x.DefaultAppearanceID,
                        principalSchema: "TransformationModule",
                        principalTable: "Appearances",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppearanceConfigurations_CharacterID",
                schema: "TransformationModule",
                table: "AppearanceConfigurations",
                column: "CharacterID");

            migrationBuilder.CreateIndex(
                name: "IX_AppearanceConfigurations_CurrentAppearanceID",
                schema: "TransformationModule",
                table: "AppearanceConfigurations",
                column: "CurrentAppearanceID");

            migrationBuilder.CreateIndex(
                name: "IX_AppearanceConfigurations_DefaultAppearanceID",
                schema: "TransformationModule",
                table: "AppearanceConfigurations",
                column: "DefaultAppearanceID");

            // Seed the new table with the existing data
            const string seedQuery = "insert into \"TransformationModule\".\"AppearanceConfigurations\"" +
                                     "(\"CharacterID\", \"DefaultAppearanceID\", \"CurrentAppearanceID\")" +
                                     "select \"ID\", \"DefaultAppearanceID\", \"CurrentAppearanceID\"" +
                                     "from \"CharacterModule\".\"Characters\";";

            migrationBuilder.Sql(seedQuery);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppearanceConfigurations",
                schema: "TransformationModule");
        }
    }
}
