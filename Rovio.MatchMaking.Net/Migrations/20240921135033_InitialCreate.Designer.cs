﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rovio.MatchMaking.Net.Data;

#nullable disable

namespace Rovio.MatchMaking.Net.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240921135033_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Rovio.MatchMaking.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("LatencyMilliseconds")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Rovio.MatchMaking.QueuedPlayer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LatencyLevel")
                        .HasColumnType("int");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("QueuedPlayers");
                });

            modelBuilder.Entity("Rovio.MatchMaking.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Rovio.MatchMaking.Player", b =>
                {
                    b.HasOne("Rovio.MatchMaking.Session", null)
                        .WithMany("Players")
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("Rovio.MatchMaking.Session", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
