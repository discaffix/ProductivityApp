﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductivityApp.DataAccess;

namespace ProductivityApp.DataAccess.Migrations
{
    [DbContext(typeof(ProductivityContext))]
    partial class ProductivityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProductivityApp.Model.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ProductivityApp.Model.Project", b =>
                {
                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int?>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("ProjectId");

                    b.HasIndex("ClientId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProductivityApp.Model.Session", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SessionId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("ProductivityApp.Model.SessionTag", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("SessionId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("SessionTag");
                });

            modelBuilder.Entity("ProductivityApp.Model.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ProductivityApp.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProductivityApp.Model.Workspace", b =>
                {
                    b.Property<int>("WorkspaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CreatedByUserUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkspaceId");

                    b.HasIndex("CreatedByUserUserId");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("ProductivityApp.Model.Project", b =>
                {
                    b.HasOne("ProductivityApp.Model.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientId");

                    b.HasOne("ProductivityApp.Model.Workspace", "Workspace")
                        .WithMany("Projects")
                        .HasForeignKey("WorkspaceId");
                });

            modelBuilder.Entity("ProductivityApp.Model.Session", b =>
                {
                    b.HasOne("ProductivityApp.Model.Project", "Project")
                        .WithMany("Sessions")
                        .HasForeignKey("ProjectId");

                    b.HasOne("ProductivityApp.Model.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ProductivityApp.Model.SessionTag", b =>
                {
                    b.HasOne("ProductivityApp.Model.Session", "Session")
                        .WithMany("Tags")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductivityApp.Model.Tag", "Tag")
                        .WithMany("Sessions")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductivityApp.Model.Workspace", b =>
                {
                    b.HasOne("ProductivityApp.Model.User", "CreatedByUser")
                        .WithMany("Workspaces")
                        .HasForeignKey("CreatedByUserUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
