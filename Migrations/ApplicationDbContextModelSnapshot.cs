﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quiz_API.Models;

#nullable disable

namespace Quiz_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Quiz_API.Models.AppUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("full_name");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime")
                        .HasColumnName("last_login");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime")
                        .HasColumnName("registration_date");

                    b.HasKey("UserId");

                    b.ToTable("app_user");
                });

            modelBuilder.Entity("Quiz_API.Models.Card", b =>
                {
                    b.Property<Guid>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("card_id");

                    b.Property<string>("Answers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("answers");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at");

                    b.Property<byte[]>("Image")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("image");

                    b.Property<string>("QuizText")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("quiz_text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at");

                    b.HasKey("CardId");

                    b.ToTable("quiz_card");
                });

            modelBuilder.Entity("Quiz_API.Models.DeckCard", b =>
                {
                    b.Property<Guid>("DeckId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deck_id")
                        .HasColumnOrder(0);

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("card_id")
                        .HasColumnOrder(1);

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int")
                        .HasColumnName("order_index");

                    b.HasKey("DeckId", "CardId");

                    b.HasIndex("CardId");

                    b.HasIndex("DeckId", "OrderIndex")
                        .IsUnique();

                    b.ToTable("deck_cards");
                });

            modelBuilder.Entity("Quiz_API.Models.QuizDeck", b =>
                {
                    b.Property<Guid>("DeckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("deck_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("created_at");

                    b.Property<string>("DeckName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("deck_name");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.HasKey("DeckId");

                    b.HasIndex("UserId");

                    b.ToTable("quiz_deck");
                });

            modelBuilder.Entity("Quiz_API.Models.UserAuth", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserPwd")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("user_pwd");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)")
                        .HasColumnName("username");

                    b.HasKey("UserId");

                    b.ToTable("user_auth");
                });

            modelBuilder.Entity("Quiz_API.Models.DeckCard", b =>
                {
                    b.HasOne("Quiz_API.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiz_API.Models.QuizDeck", "QuizDeck")
                        .WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("QuizDeck");
                });

            modelBuilder.Entity("Quiz_API.Models.QuizDeck", b =>
                {
                    b.HasOne("Quiz_API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Quiz_API.Models.UserAuth", b =>
                {
                    b.HasOne("Quiz_API.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });
#pragma warning restore 612, 618
        }
    }
}
