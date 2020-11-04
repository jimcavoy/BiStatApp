﻿// <auto-generated />
using System;
using BiStatApp.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BiStatApp.Migrations
{
    [DbContext(typeof(BiStatContext))]
    [Migration("20201027125816_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("BiStatApp.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("BiStatApp.Models.ShootingBout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Alpha")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Bravo")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Charlie")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Delta")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Echo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Bouts");
                });

            modelBuilder.Entity("BiStatApp.Models.ShootingBout", b =>
                {
                    b.HasOne("BiStatApp.Models.Session", "Session")
                        .WithMany("Bouts")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}