﻿//WebStoreContext.cs
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebStore.Entities;
using MyECommerce.Console.Entities;

namespace WebStore.Entities;

public partial class WebStoreContext : DbContext
{
    public WebStoreContext()
    {
    }

    public WebStoreContext(DbContextOptions<WebStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Carrier> Carriers { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebStore;Username=postgres\n;Password=learnpassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<DiscountType>(
        schema: "public",       // or whatever schema you want
        name: "discount_type"   // the name to use in PostgreSQL
        );

        modelBuilder.Entity<DiscountCode>(entity =>
        {
        entity.ToTable("discount_codes");

        entity.HasKey(dc => dc.DiscountCodeId);

        entity.Property(dc => dc.DiscountType)
            .HasColumnType("discount_type");  // must match the name above

        entity.Property(dc => dc.Code)
            .HasMaxLength(50)
            .IsRequired();
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("addresses_pkey");

            entity.ToTable("addresses");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.AddressType)
                .HasMaxLength(20)
                .HasColumnName("address_type");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");

            entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_addresses_customer");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_categories_parent");

            entity.HasMany(d => d.Products).WithMany(p => p.Categories)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_pc_product"),
                    l => l.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_pc_category"),
                    j =>
                    {
                        j.HasKey("CategoryId", "ProductId").HasName("pk_product_categories");
                        j.ToTable("product_categories");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                    });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(20)
                .HasColumnName("order_status");
            entity.Property(e => e.ShippingAddressId).HasColumnName("shipping_address_id");

            entity.HasOne(d => d.BillingAddress).WithMany(p => p.OrderBillingAddresses)
                .HasForeignKey(d => d.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_billing_address");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_customer");

            entity.HasOne(d => d.ShippingAddress).WithMany(p => p.OrderShippingAddresses)
                .HasForeignKey(d => d.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_orders_shipping_address");
            
            entity.Property(o => o.TrackingNumber)
              .HasColumnName("tracking_number")
              .HasMaxLength(50);

            entity.Property(o => o.ShippedDate)
                .HasColumnName("shipped_date");

            entity.Property(o => o.DeliveredDate)
                .HasColumnName("delivered_date");

            entity.HasOne(o => o.DiscountCode)
              .WithMany(dc => dc.Orders)
              .HasForeignKey(o => o.DiscountCodeId)
              .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Carrier>(entity =>
        {
            entity.HasKey(e => e.CarrierId).HasName("carriers_pkey");

            entity.ToTable("carriers");

            entity.Property(e => e.CarrierId).HasColumnName("carrier_id");

            entity.Property(e => e.CarrierName)
                .HasMaxLength(50)
                .HasColumnName("carrier_name");

            entity.Property(e => e.ContactUrl)
                .HasMaxLength(50)
                .HasColumnName("contact_url");

            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .HasColumnName("contact_phone");

            entity.HasMany(e => e.Orders)
                .WithOne(o => o.Carrier)
                .HasForeignKey(o => o.CarrierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_orders_carrier");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("pk_order_items");

            entity.ToTable("order_items");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Discount)
                .HasPrecision(10, 2)
                .HasColumnName("discount");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_oi_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_oi_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("product_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("staff_pkey");

            entity.ToTable("staff");

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.StoreId).HasColumnName("store_id");

            entity.HasOne(d => d.Store).WithMany(p => p.Staff)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("fk_staff_store");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProductId }).HasName("pk_stocks");

            entity.ToTable("stocks");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QuantityInStock)
                .HasDefaultValue(0)
                .HasColumnName("quantity_in_stock");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_stocks_product");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("fk_stocks_store");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("stores_pkey");

            entity.ToTable("stores");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.StoreName)
                .HasMaxLength(100)
                .HasColumnName("store_name");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
