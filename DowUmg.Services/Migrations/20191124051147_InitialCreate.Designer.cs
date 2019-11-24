﻿// <auto-generated />
using System;
using DowUmg.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DowUmg.Services.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20191124051147_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1");

            modelBuilder.Entity("DowUmg.Services.Models.Alliance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("Alliance");
                });

            modelBuilder.Entity("DowUmg.Services.Models.Army", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AllianceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Race")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AllianceId");

                    b.HasIndex("CampaignId");

                    b.ToTable("Army");
                });

            modelBuilder.Entity("DowUmg.Services.Models.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Campaign");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Campaign");
                });

            modelBuilder.Entity("DowUmg.Services.Models.DowMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ModId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Players")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ModId");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("DowUmg.Services.Models.DowMod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Mods");
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("RandomPosition")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("RandomTeams")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ResourceRate")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Sharing")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Speed")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StartingResources")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameInfo");
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameInfoRule", b =>
                {
                    b.Property<int>("InfoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RuleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("InfoId", "RuleId");

                    b.HasIndex("RuleId");

                    b.ToTable("GameInfoRule");
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsWinCondition")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ModId");

                    b.ToTable("GameRules");
                });

            modelBuilder.Entity("DowUmg.Services.Models.Scenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("InfoId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InheritedCustomRules")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InheritedGameOptions")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InheritedGameRules")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InheritedWinConditions")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ManyMatchupCampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MapId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InfoId");

                    b.HasIndex("ManyMatchupCampaignId");

                    b.HasIndex("MapId");

                    b.ToTable("Scenario");
                });

            modelBuilder.Entity("DowUmg.Services.Models.ScenarioPlayers", b =>
                {
                    b.Property<int>("ArmyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("ArmyId", "ScenarioId");

                    b.HasIndex("ScenarioId");

                    b.ToTable("ScenarioPlayers");
                });

            modelBuilder.Entity("DowUmg.Services.Models.ManyMatchupCampaign", b =>
                {
                    b.HasBaseType("DowUmg.Services.Models.Campaign");

                    b.HasDiscriminator().HasValue("ManyMatchupCampaign");
                });

            modelBuilder.Entity("DowUmg.Services.Models.SingleMatchupCampaign", b =>
                {
                    b.HasBaseType("DowUmg.Services.Models.Campaign");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("ScenarioId");

                    b.HasDiscriminator().HasValue("SingleMatchupCampaign");
                });

            modelBuilder.Entity("DowUmg.Services.Models.Alliance", b =>
                {
                    b.HasOne("DowUmg.Services.Models.Campaign", null)
                        .WithMany("Alliances")
                        .HasForeignKey("CampaignId");
                });

            modelBuilder.Entity("DowUmg.Services.Models.Army", b =>
                {
                    b.HasOne("DowUmg.Services.Models.Alliance", "Alliance")
                        .WithMany("Armies")
                        .HasForeignKey("AllianceId");

                    b.HasOne("DowUmg.Services.Models.Campaign", "Campaign")
                        .WithMany("Armies")
                        .HasForeignKey("CampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.DowMap", b =>
                {
                    b.HasOne("DowUmg.Services.Models.DowMod", "Mod")
                        .WithMany("Maps")
                        .HasForeignKey("ModId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameInfo", b =>
                {
                    b.OwnsMany("DowUmg.Services.Data.Entities.CustomRule", "CustomRules", b1 =>
                        {
                            b1.Property<int>("GameInfoId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Details")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("GameInfoId", "Id");

                            b1.ToTable("CustomRule");

                            b1.WithOwner()
                                .HasForeignKey("GameInfoId");
                        });
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameInfoRule", b =>
                {
                    b.HasOne("DowUmg.Services.Models.GameInfo", "Info")
                        .WithMany("Rules")
                        .HasForeignKey("InfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DowUmg.Services.Models.GameRule", "Rule")
                        .WithMany("Infos")
                        .HasForeignKey("RuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.GameRule", b =>
                {
                    b.HasOne("DowUmg.Services.Models.DowMod", "Mod")
                        .WithMany("Rules")
                        .HasForeignKey("ModId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.Scenario", b =>
                {
                    b.HasOne("DowUmg.Services.Models.GameInfo", "Info")
                        .WithMany()
                        .HasForeignKey("InfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DowUmg.Services.Models.ManyMatchupCampaign", null)
                        .WithMany("Scenarios")
                        .HasForeignKey("ManyMatchupCampaignId");

                    b.HasOne("DowUmg.Services.Models.DowMap", "Map")
                        .WithMany()
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.ScenarioPlayers", b =>
                {
                    b.HasOne("DowUmg.Services.Models.Army", "Army")
                        .WithMany("Scenarios")
                        .HasForeignKey("ArmyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DowUmg.Services.Models.Scenario", "Scenario")
                        .WithMany("Players")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DowUmg.Services.Models.SingleMatchupCampaign", b =>
                {
                    b.HasOne("DowUmg.Services.Models.Scenario", "Scenario")
                        .WithMany()
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
