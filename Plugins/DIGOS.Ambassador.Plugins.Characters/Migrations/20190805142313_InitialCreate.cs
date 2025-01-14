﻿// <auto-generated />
#pragma warning disable CS1591
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUsingDirective
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGOS.Ambassador.Plugins.Characters.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CharacterModule");

            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.CreateTable(
                name: "CharacterRoles",
                schema: "CharacterModule",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ServerID = table.Column<long>(nullable: false),
                    DiscordID = table.Column<long>(nullable: false),
                    Access = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CharacterRoles_Servers_ServerID",
                        column: x => x.ServerID,
                        principalSchema: "Core",
                        principalTable: "Servers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                schema: "CharacterModule",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ServerID = table.Column<long>(nullable: false),
                    OwnerID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsNSFW = table.Column<bool>(nullable: false),
                    PronounProviderFamily = table.Column<string>(nullable: true),
                    RoleID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Characters_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalSchema: "Core",
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characters_CharacterRoles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "CharacterModule",
                        principalTable: "CharacterRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "CharacterModule",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    IsNSFW = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    CharacterID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Characters_CharacterID",
                        column: x => x.CharacterID,
                        principalSchema: "CharacterModule",
                        principalTable: "Characters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRoles_ServerID",
                schema: "CharacterModule",
                table: "CharacterRoles",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_OwnerID",
                schema: "CharacterModule",
                table: "Characters",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RoleID",
                schema: "CharacterModule",
                table: "Characters",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CharacterID",
                schema: "CharacterModule",
                table: "Images",
                column: "CharacterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images",
                schema: "CharacterModule");

            migrationBuilder.DropTable(
                name: "Characters",
                schema: "CharacterModule");

            migrationBuilder.DropTable(
                name: "CharacterRoles",
                schema: "CharacterModule");
        }
    }
}
