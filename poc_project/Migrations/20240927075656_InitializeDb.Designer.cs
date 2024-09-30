﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using poc_project.Data;

#nullable disable

namespace poc_project.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240927075656_InitializeDb")]
    partial class InitializeDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("poc_project.Models.Entity.Draft", b =>
                {
                    b.Property<int>("DraftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DraftId"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaterialIssueId")
                        .HasColumnType("int");

                    b.Property<int?>("RelevanceScore")
                        .HasColumnType("int");

                    b.Property<int>("StakeholderId")
                        .HasColumnType("int");

                    b.HasKey("DraftId");

                    b.HasIndex("MaterialIssueId");

                    b.HasIndex("StakeholderId");

                    b.ToTable("Drafts");
                });

            modelBuilder.Entity("poc_project.Models.Entity.MaterialIssue", b =>
                {
                    b.Property<int>("IssueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IssueId"));

                    b.Property<string>("IssueCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IssueId");

                    b.ToTable("MaterialIssues");
                });

            modelBuilder.Entity("poc_project.Models.Entity.ResponseRelevance", b =>
                {
                    b.Property<int>("ResponseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResponseId"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IssueId")
                        .HasColumnType("int");

                    b.Property<int>("RelevanceScore")
                        .HasColumnType("int");

                    b.Property<int>("StakeholderId")
                        .HasColumnType("int");

                    b.HasKey("ResponseId");

                    b.HasIndex("IssueId");

                    b.HasIndex("StakeholderId");

                    b.ToTable("ResponseRelevances");
                });

            modelBuilder.Entity("poc_project.Models.Entity.Stakeholder", b =>
                {
                    b.Property<int>("StakeholderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StakeholderId"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StakeholderId");

                    b.ToTable("Stakeholders");
                });

            modelBuilder.Entity("poc_project.Models.Entity.Draft", b =>
                {
                    b.HasOne("poc_project.Models.Entity.MaterialIssue", "MaterialIssue")
                        .WithMany("Drafts")
                        .HasForeignKey("MaterialIssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("poc_project.Models.Entity.Stakeholder", "Stakeholder")
                        .WithMany("Drafts")
                        .HasForeignKey("StakeholderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaterialIssue");

                    b.Navigation("Stakeholder");
                });

            modelBuilder.Entity("poc_project.Models.Entity.ResponseRelevance", b =>
                {
                    b.HasOne("poc_project.Models.Entity.MaterialIssue", "Issue")
                        .WithMany("ResponseRelevances")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("poc_project.Models.Entity.Stakeholder", "Stakeholder")
                        .WithMany("ResponseRelevances")
                        .HasForeignKey("StakeholderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");

                    b.Navigation("Stakeholder");
                });

            modelBuilder.Entity("poc_project.Models.Entity.MaterialIssue", b =>
                {
                    b.Navigation("Drafts");

                    b.Navigation("ResponseRelevances");
                });

            modelBuilder.Entity("poc_project.Models.Entity.Stakeholder", b =>
                {
                    b.Navigation("Drafts");

                    b.Navigation("ResponseRelevances");
                });
#pragma warning restore 612, 618
        }
    }
}
