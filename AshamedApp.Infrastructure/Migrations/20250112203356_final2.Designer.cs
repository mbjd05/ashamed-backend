﻿// <auto-generated />
using System;
using AshamedApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AshamedApp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250112203356_final2")]
    partial class final2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("AshamedApp.Application.DTOs.MqttMessageDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Payload")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Topic")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MqttMessages");
                });

            modelBuilder.Entity("AshamedApp.Application.DTOs.SnapshotDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Snapshots");
                });

            modelBuilder.Entity("SnapshotMqttMessage", b =>
                {
                    b.Property<int>("SnapshotId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MqttMessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SnapshotId", "MqttMessageId");

                    b.HasIndex("MqttMessageId");

                    b.ToTable("SnapshotMqttMessages", (string)null);
                });

            modelBuilder.Entity("SnapshotMqttMessage", b =>
                {
                    b.HasOne("AshamedApp.Application.DTOs.MqttMessageDto", null)
                        .WithMany()
                        .HasForeignKey("MqttMessageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AshamedApp.Application.DTOs.SnapshotDto", null)
                        .WithMany()
                        .HasForeignKey("SnapshotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}