﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Point_of_Sale.Entities;

#nullable disable

namespace Point_of_Sale.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220519181531_initial3")]
    partial class initial3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Point_of_Sale.Entities.Admin_Group", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("admin_group");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Admin_Group_User", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("group_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<long>("user_id")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("admin_group_user");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Admin_Role", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("admin_role");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Admin_Role_Group", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("group_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<long>("role_id")
                        .HasColumnType("bigint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("admin_role_group");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Admin_User", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("address")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("district_id")
                        .HasColumnType("bigint");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("full_name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("pass_code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<long>("province_id")
                        .HasColumnType("bigint");

                    b.Property<byte>("sex")
                        .HasColumnType("tinyint");

                    b.Property<byte>("type")
                        .HasColumnType("tinyint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("ward_id")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("admin_user");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Category_Packing", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("ecom_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<int>("order")
                        .HasColumnType("int");

                    b.Property<byte>("status_id")
                        .HasColumnType("tinyint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("category_packing");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Category_Product", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("ecom_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<int>("order")
                        .HasColumnType("int");

                    b.Property<long>("parent_id")
                        .HasColumnType("bigint");

                    b.Property<byte>("status_id")
                        .HasColumnType("tinyint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("category_product");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Category_Unit", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("ecom_id")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("note")
                        .IsRequired()
                        .HasMaxLength(1500)
                        .HasColumnType("nvarchar(1500)");

                    b.Property<int>("order")
                        .HasColumnType("int");

                    b.Property<byte>("status_id")
                        .HasColumnType("tinyint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("category_unit");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Order", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("customer_id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<byte>("payment_type")
                        .HasColumnType("tinyint");

                    b.Property<long>("pos_id")
                        .HasColumnType("bigint");

                    b.Property<decimal>("product_total_cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("sales_session_id")
                        .HasColumnType("bigint");

                    b.Property<byte>("status_id")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("total_amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<long>("warehouse_id")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("order");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Order_Detail", b =>
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

                    b.Property<long>("order_id")
                        .HasColumnType("bigint");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<long>("product_id")
                        .HasColumnType("bigint");

                    b.Property<long>("product_packing_id")
                        .HasColumnType("bigint");

                    b.Property<long>("product_unit_id")
                        .HasColumnType("bigint");

                    b.Property<int>("quatity")
                        .HasColumnType("int");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<long>("warehouse_id")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("order_detail");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Partner", b =>
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("id_ecom")
                        .HasColumnType("bigint");

                    b.Property<string>("introduce")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("taxcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<string>("website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("partner");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Product", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("category_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<long>("ecom_prartner_id")
                        .HasColumnType("bigint");

                    b.Property<long>("id_ecom")
                        .HasColumnType("bigint");

                    b.Property<bool>("is_active")
                        .HasColumnType("bit");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("prouct");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Product_Warehouse", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<string>("barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("batch_number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("exp_date")
                        .HasColumnType("datetime2");

                    b.Property<double>("import_price")
                        .HasColumnType("float");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<string>("packing_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<long>("product_id")
                        .HasColumnType("bigint");

                    b.Property<int>("quantity_sold")
                        .HasColumnType("int");

                    b.Property<int>("quantity_stock")
                        .HasColumnType("int");

                    b.Property<string>("unit_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.Property<long>("warehouse_id")
                        .HasColumnType("bigint");

                    b.Property<int>("warning_date")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("prouct_warehouse");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Sales_Session", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<double>("closing_cash")
                        .HasColumnType("float");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<double>("opening_cash")
                        .HasColumnType("float");

                    b.Property<long>("staff_id")
                        .HasColumnType("bigint");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("sale_session");
                });

            modelBuilder.Entity("Point_of_Sale.Entities.Voucher", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"), 1L, 1);

                    b.Property<DateTime>("active_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("dateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("dateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("end_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_delete")
                        .HasColumnType("bit");

                    b.Property<double>("maxium_reduction")
                        .HasColumnType("float");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("reduction_price")
                        .HasColumnType("float");

                    b.Property<int>("reduction_rate")
                        .HasColumnType("int");

                    b.Property<byte>("status_id")
                        .HasColumnType("tinyint");

                    b.Property<byte>("type")
                        .HasColumnType("tinyint");

                    b.Property<int>("used_quantity")
                        .HasColumnType("int");

                    b.Property<long>("userAdded")
                        .HasColumnType("bigint");

                    b.Property<long?>("userUpdated")
                        .HasColumnType("bigint");

                    b.HasKey("id");

                    b.ToTable("voucher");
                });
#pragma warning restore 612, 618
        }
    }
}
