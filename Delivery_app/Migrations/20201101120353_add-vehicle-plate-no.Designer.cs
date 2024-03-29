﻿// <auto-generated />
using System;
using Delivery_app.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Delivery_app.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201101120353_add-vehicle-plate-no")]
    partial class addvehicleplateno
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Delivery_app.Entities.Couriers", b =>
                {
                    b.Property<int>("courier_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("name")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("otp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("password_salt")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("phone_num")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("profile_picture")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("vehicle_plate_no")
                        .HasColumnType("VARCHAR(10)");

                    b.Property<byte>("vehicle_type")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("courier_id");

                    b.ToTable("couriers");
                });

            modelBuilder.Entity("Delivery_app.Entities.DropPoints", b =>
                {
                    b.Property<int>("drop_point_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("contact_num")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("latitude")
                        .HasColumnType("FLOAT(10, 6)");

                    b.Property<float>("longitude")
                        .HasColumnType("FLOAT(10, 6)");

                    b.Property<int>("order_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime(6)");

                    b.HasKey("drop_point_id");

                    b.HasIndex("order_id");

                    b.ToTable("drop_points");
                });

            modelBuilder.Entity("Delivery_app.Entities.Orders", b =>
                {
                    b.Property<int>("order_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("contact_num")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("delivery_status")
                        .HasColumnType("tinyint unsigned");

                    b.Property<float>("latitude")
                        .HasColumnType("FLOAT(10, 6)");

                    b.Property<float>("longitude")
                        .HasColumnType("FLOAT(10, 6)");

                    b.Property<string>("name")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<bool>("notify_recipient")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("notify_sender")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("pick_up_address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("pick_up_datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("price")
                        .HasColumnType("double");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<byte>("vehicle_type")
                        .HasColumnType("tinyint unsigned");

                    b.Property<double>("weight")
                        .HasColumnType("double");

                    b.HasKey("order_id");

                    b.HasIndex("user_id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("Delivery_app.Entities.Users", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("email")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("name")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("password_salt")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("phone_num")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime(6)");

                    b.Property<byte>("user_type")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("user_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Delivery_app.Entities.DropPoints", b =>
                {
                    b.HasOne("Delivery_app.Entities.Orders", "order")
                        .WithMany("drop_points")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Delivery_app.Entities.Orders", b =>
                {
                    b.HasOne("Delivery_app.Entities.Users", "user")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
