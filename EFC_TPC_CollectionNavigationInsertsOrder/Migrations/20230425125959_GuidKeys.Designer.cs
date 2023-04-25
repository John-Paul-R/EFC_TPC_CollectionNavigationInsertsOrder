﻿// <auto-generated />
using System;
using EFSampleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFC_TPC_CollectionNavigationInsertsOrder.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20230425125959_GuidKeys")]
    partial class GuidKeys
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("EFSampleApp.AbstractSkill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("EFSampleApp.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("EFSampleApp.PlayerToSkill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SkillId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SkillId");

                    b.ToTable("PlayerToSkill");
                });

            modelBuilder.Entity("EFSampleApp.MagicSkill", b =>
                {
                    b.HasBaseType("EFSampleApp.AbstractSkill");

                    b.Property<string>("RunicName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("MagicSkills");
                });

            modelBuilder.Entity("EFSampleApp.MartialSkill", b =>
                {
                    b.HasBaseType("EFSampleApp.AbstractSkill");

                    b.Property<bool>("HasStrike")
                        .HasColumnType("INTEGER");

                    b.ToTable("MartialSkills");
                });

            modelBuilder.Entity("EFSampleApp.PlayerToSkill", b =>
                {
                    b.HasOne("EFSampleApp.Player", "Player")
                        .WithMany("Skills")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFSampleApp.AbstractSkill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("EFSampleApp.Player", b =>
                {
                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
