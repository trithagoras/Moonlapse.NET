﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoonlapseServer;

#nullable disable

namespace MoonlapseServer.Migrations
{
    [DbContext(typeof(MoonlapseDbContext))]
    [Migration("20220206205132_Components")]
    partial class Components
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.Components.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EntityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("Components");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Component");
                });

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EntityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EntityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.Components.Position", b =>
                {
                    b.HasBaseType("MoonlapseNetworking.ServerModels.Components.Component");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Position");
                });

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.Components.Component", b =>
                {
                    b.HasOne("MoonlapseNetworking.ServerModels.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("MoonlapseNetworking.ServerModels.User", b =>
                {
                    b.HasOne("MoonlapseNetworking.ServerModels.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");
                });
#pragma warning restore 612, 618
        }
    }
}