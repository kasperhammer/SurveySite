﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20241009081744_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Anwser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnwserId")
                        .HasColumnType("int");

                    b.Property<string>("AnwserText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnwserId");

                    b.HasIndex("CompId");

                    b.ToTable("Anwsers");
                });

            modelBuilder.Entity("Models.AnwserModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("AnwserModules");
                });

            modelBuilder.Entity("Models.CompModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CompMultiId")
                        .HasColumnType("int");

                    b.Property<int?>("CompSingleId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompMultiId");

                    b.HasIndex("CompSingleId");

                    b.ToTable("CompModules");
                });

            modelBuilder.Entity("Models.SComp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Required")
                        .HasColumnType("bit");

                    b.Property<int>("SurveyId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SurveyId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("Models.Survey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("Models.Anwser", b =>
                {
                    b.HasOne("Models.AnwserModule", "AnwserModule")
                        .WithMany("anwsers")
                        .HasForeignKey("AnwserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.SComp", "Comp")
                        .WithMany()
                        .HasForeignKey("CompId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AnwserModule");

                    b.Navigation("Comp");
                });

            modelBuilder.Entity("Models.AnwserModule", b =>
                {
                    b.HasOne("Models.Survey", "Survey")
                        .WithMany("AnwserModules")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("Models.CompModule", b =>
                {
                    b.HasOne("Models.SComp", "CompMulti")
                        .WithMany("MultiAnwsers")
                        .HasForeignKey("CompMultiId");

                    b.HasOne("Models.SComp", "CompSingle")
                        .WithMany("SingleAnwser")
                        .HasForeignKey("CompSingleId");

                    b.Navigation("CompMulti");

                    b.Navigation("CompSingle");
                });

            modelBuilder.Entity("Models.SComp", b =>
                {
                    b.HasOne("Models.Survey", "Survey")
                        .WithMany("SComps")
                        .HasForeignKey("SurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Survey");
                });

            modelBuilder.Entity("Models.AnwserModule", b =>
                {
                    b.Navigation("anwsers");
                });

            modelBuilder.Entity("Models.SComp", b =>
                {
                    b.Navigation("MultiAnwsers");

                    b.Navigation("SingleAnwser");
                });

            modelBuilder.Entity("Models.Survey", b =>
                {
                    b.Navigation("AnwserModules");

                    b.Navigation("SComps");
                });
#pragma warning restore 612, 618
        }
    }
}