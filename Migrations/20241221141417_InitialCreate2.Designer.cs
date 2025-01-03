﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using haircare.Models;

#nullable disable

namespace haircare.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241221141417_InitialCreate2")]
    partial class InitialCreate2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("haircare.Models.Calisan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ad")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Soyad")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UzmanlikAlani")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Calisanlar");
                });

            modelBuilder.Entity("haircare.Models.Islem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Fiyat")
                        .HasColumnType("numeric");

                    b.Property<string>("IslemAd")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IslemTur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Islemler");
                });

            modelBuilder.Entity("haircare.Models.Randevu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CalisanId")
                        .HasColumnType("integer");

                    b.Property<int>("IslemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Tarih")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CalisanId");

                    b.HasIndex("IslemId");

                    b.ToTable("Randevular");
                });

            modelBuilder.Entity("haircare.Models.Randevu", b =>
                {
                    b.HasOne("haircare.Models.Calisan", "Calisan")
                        .WithMany()
                        .HasForeignKey("CalisanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("haircare.Models.Islem", "Islem")
                        .WithMany()
                        .HasForeignKey("IslemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calisan");

                    b.Navigation("Islem");
                });
#pragma warning restore 612, 618
        }
    }
}
