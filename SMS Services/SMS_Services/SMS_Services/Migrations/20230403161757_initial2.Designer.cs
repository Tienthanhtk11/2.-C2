﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SMS_Services.Model;

#nullable disable

namespace SMS_Services.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230403161757_initial2")]
    partial class initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SMS_Services.Model.Admin_User", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Admin_User");
                });

            modelBuilder.Entity("SMS_Services.Model.Customer", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<bool>("active")
                        .HasColumnType("bit");

                    b.Property<int>("cash")
                        .HasColumnType("int");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("last_active")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("license_exp")
                        .HasColumnType("datetime2");

                    b.Property<string>("license_key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("SMS_Services.Model.Message_Receive", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("date_receive")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_receive")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_send")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("Message_Receive");
                });

            modelBuilder.Entity("SMS_Services.Model.Order", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("count_sms")
                        .HasColumnType("int");

                    b.Property<long>("customer_id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<DateTime>("timeSend")
                        .HasColumnType("datetime2");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("SMS_Services.Model.Port", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<int>("cash")
                        .HasColumnType("int");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("Port");
                });

            modelBuilder.Entity("SMS_Services.Model.SendSMSHistory", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("order_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("order_id")
                        .HasColumnType("bigint");

                    b.Property<string>("phone_receive")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone_send")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int>("sum_sms")
                        .HasColumnType("int");

                    b.Property<string>("system_response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("SendSMSHistory");
                });

            modelBuilder.Entity("SMS_Services.OrderDetails", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<long?>("Orderid")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("order_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("order_id")
                        .HasColumnType("bigint");

                    b.Property<string>("phone_receive")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.Property<int>("sum_sms")
                        .HasColumnType("int");

                    b.Property<string>("telco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.HasIndex("Orderid");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("SMS_Services.OrderDetails", b =>
                {
                    b.HasOne("SMS_Services.Model.Order", null)
                        .WithMany("details")
                        .HasForeignKey("Orderid");
                });

            modelBuilder.Entity("SMS_Services.Model.Order", b =>
                {
                    b.Navigation("details");
                });
#pragma warning restore 612, 618
        }
    }
}
