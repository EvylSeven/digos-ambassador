﻿// <auto-generated />
#pragma warning disable CS1591
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUsingDirective
using System;
using DIGOS.Ambassador.Plugins.Kinks.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DIGOS.Ambassador.Plugins.Kinks.Migrations
{
    [DbContext(typeof(KinksDatabaseContext))]
    [Migration("20190805131502_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("KinkModule")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("DIGOS.Ambassador.Plugins.Core.Model.Users.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<int>("Class");

                    b.Property<long>("DiscordID");

                    b.Property<bool>("HideNewRoleplays");

                    b.Property<int?>("Timezone");

                    b.HasKey("ID");

                    b.ToTable("Users","Core");
                });

            modelBuilder.Entity("DIGOS.Ambassador.Plugins.Kinks.Model.Kink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("Description");

                    b.Property<uint>("FListID");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Kinks","KinkModule");
                });

            modelBuilder.Entity("DIGOS.Ambassador.Plugins.Kinks.Model.UserKink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("KinkID");

                    b.Property<int>("Preference");

                    b.Property<long>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("KinkID");

                    b.HasIndex("UserID");

                    b.ToTable("UserKinks","KinkModule");
                });

            modelBuilder.Entity("DIGOS.Ambassador.Plugins.Kinks.Model.UserKink", b =>
                {
                    b.HasOne("DIGOS.Ambassador.Plugins.Kinks.Model.Kink", "Kink")
                        .WithMany()
                        .HasForeignKey("KinkID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DIGOS.Ambassador.Plugins.Core.Model.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
