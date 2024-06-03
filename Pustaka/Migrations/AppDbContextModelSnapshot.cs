﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Pustaka.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("Buku", b =>
                {
                    b.Property<int>("BukuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Judul")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PenulisId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BukuId");

                    b.HasIndex("PenulisId");

                    b.ToTable("Buku");
                });

            modelBuilder.Entity("Penulis", b =>
                {
                    b.Property<int>("PenulisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nama")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PenulisId");

                    b.ToTable("Penulis");
                });

            modelBuilder.Entity("Buku", b =>
                {
                    b.HasOne("Penulis", "Penulis")
                        .WithMany("Buku")
                        .HasForeignKey("PenulisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Penulis");
                });

            modelBuilder.Entity("Penulis", b =>
                {
                    b.Navigation("Buku");
                });
#pragma warning restore 612, 618
        }
    }
}
