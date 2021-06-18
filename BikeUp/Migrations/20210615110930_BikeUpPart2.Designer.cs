﻿// <auto-generated />
using System;
using BikeUp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BikeUp.Migrations
{
    [DbContext(typeof(BikeUpContext))]
    [Migration("20210615110930_BikeUpPart2")]
    partial class BikeUpPart2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("BikeUp.Models.Bike", b =>
                {
                    b.Property<int>("BikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Capacity")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("RentDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TimesRented")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("BikeId");

                    b.ToTable("Bikes");
                });

            modelBuilder.Entity("BikeUp.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AmountSpent")
                        .HasColumnType("REAL");

                    b.Property<int?>("BikeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NrOfRentedBikes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalRentingHours")
                        .HasColumnType("INTEGER");

                    b.HasKey("CustomerId");

                    b.HasIndex("BikeId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("BikeUp.Models.Customer", b =>
                {
                    b.HasOne("BikeUp.Models.Bike", "Bike")
                        .WithOne("Customer")
                        .HasForeignKey("BikeUp.Models.Customer", "BikeId");

                    b.Navigation("Bike");
                });

            modelBuilder.Entity("BikeUp.Models.Bike", b =>
                {
                    b.Navigation("Customer");
                });
#pragma warning restore 612, 618
        }
    }
}
