﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventPlannerAPI.Migrations
{
    [DbContext(typeof(EventsDBContext))]
    [Migration("20240406021620_IntermediateTable")]
    partial class IntermediateTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventPlannerAPI.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("EventId");

                    b.ToTable("Events");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            Date = new DateTime(2023, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Tech Conference"
                        },
                        new
                        {
                            EventId = 2,
                            Date = new DateTime(2023, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Local Hackathon"
                        });
                });

            modelBuilder.Entity("EventPlannerAPI.Models.EventPerson", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("EventId", "PersonId");

                    b.HasIndex("PersonId");

                    b.ToTable("EventPerson");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            PersonId = 1
                        },
                        new
                        {
                            EventId = 1,
                            PersonId = 2
                        },
                        new
                        {
                            EventId = 2,
                            PersonId = 3
                        },
                        new
                        {
                            EventId = 2,
                            PersonId = 1
                        });
                });

            modelBuilder.Entity("EventPlannerAPI.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PersonId");

                    b.ToTable("People");

                    b.HasData(
                        new
                        {
                            PersonId = 1,
                            Age = 29,
                            Name = "Alice Smith"
                        },
                        new
                        {
                            PersonId = 2,
                            Age = 35,
                            Name = "Bob Jones"
                        },
                        new
                        {
                            PersonId = 3,
                            Age = 22,
                            Name = "Charlie Davis"
                        });
                });

            modelBuilder.Entity("EventPlannerAPI.Models.EventPerson", b =>
                {
                    b.HasOne("EventPlannerAPI.Models.Event", "Event")
                        .WithMany("EventPersons")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventPlannerAPI.Models.Person", "Person")
                        .WithMany("EventPersons")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("EventPlannerAPI.Models.Event", b =>
                {
                    b.Navigation("EventPersons");
                });

            modelBuilder.Entity("EventPlannerAPI.Models.Person", b =>
                {
                    b.Navigation("EventPersons");
                });
#pragma warning restore 612, 618
        }
    }
}
