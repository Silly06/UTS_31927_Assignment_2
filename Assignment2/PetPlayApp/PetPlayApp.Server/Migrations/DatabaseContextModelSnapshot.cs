﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetPlayApp.Server.Db;

#nullable disable

namespace PetPlayApp.Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("PetPlayApp.Server.Models.Like", b =>
                {
                    b.Property<Guid>("PostId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("PostId", "Id");

                    b.HasIndex("Id");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.Match", b =>
                {
                    b.Property<Guid>("User1Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("User2Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("OverallStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("User1Response")
                        .HasColumnType("INTEGER");

                    b.Property<int>("User2Response")
                        .HasColumnType("INTEGER");

                    b.HasKey("User1Id", "User2Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTimePosted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("BLOB");

                    b.Property<Guid?>("PostCreatorId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PostCreatorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bio")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Interest")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserStatus")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.Like", b =>
                {
                    b.HasOne("PetPlayApp.Server.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetPlayApp.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.Match", b =>
                {
                    b.HasOne("PetPlayApp.Server.Models.User", "User1")
                        .WithMany("MatchesInitiated")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PetPlayApp.Server.Models.User", "User2")
                        .WithMany("MatchesReceived")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.Post", b =>
                {
                    b.HasOne("PetPlayApp.Server.Models.User", "PostCreator")
                        .WithMany("CreatedPosts")
                        .HasForeignKey("PostCreatorId");

                    b.Navigation("PostCreator");
                });

            modelBuilder.Entity("PetPlayApp.Server.Models.User", b =>
                {
                    b.Navigation("CreatedPosts");

                    b.Navigation("MatchesInitiated");

                    b.Navigation("MatchesReceived");
                });
#pragma warning restore 612, 618
        }
    }
}
