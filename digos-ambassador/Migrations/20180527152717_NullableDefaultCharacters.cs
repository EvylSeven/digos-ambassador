﻿// <auto-generated />
#pragma warning disable CS1591
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUsingDirective
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;

namespace DIGOS.Ambassador.Migrations
{
    public partial class NullableDefaultCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Characters_DefaultCharacterID",
                table: "Users");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultCharacterID",
                table: "Users",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Characters_DefaultCharacterID",
                table: "Users",
                column: "DefaultCharacterID",
                principalTable: "Characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Characters_DefaultCharacterID",
                table: "Users");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultCharacterID",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Characters_DefaultCharacterID",
                table: "Users",
                column: "DefaultCharacterID",
                principalTable: "Characters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
