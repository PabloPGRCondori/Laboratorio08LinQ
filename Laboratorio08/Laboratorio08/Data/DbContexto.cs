using System;
using Laboratorio08.Models;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio08.Data
{
    public partial class DbContexto : DbContext
    {
        public DbContexto()
        {
        }

        public DbContexto(DbContextOptions<DbContexto> options)
            : base(options)
        {
        }

        // Tablas del modelo
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Orderdetail> Orderdetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // ⚠️ Mueve esta cadena a appsettings.json o variable de entorno para mayor seguridad
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=linqexample;Username=postgres;Password=1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabla Clients
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Clientid).HasName("clients_pkey");

                entity.ToTable("clients");

                entity.Property(e => e.Clientid).HasColumnName("clientid");
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            // Tabla Orders
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Orderid).HasName("orders_pkey");

                entity.ToTable("orders");

                entity.Property(e => e.Orderid).HasColumnName("orderid");
                entity.Property(e => e.Clientid).HasColumnName("clientid");
                entity.Property(e => e.Orderdate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("orderdate");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Clientid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orders_clientid_fkey");
            });

            // Tabla Orderdetails
            modelBuilder.Entity<Orderdetail>(entity =>
            {
                entity.HasKey(e => e.Orderdetailid).HasName("orderdetails_pkey");

                entity.ToTable("orderdetails");

                entity.Property(e => e.Orderdetailid).HasColumnName("orderdetailid");
                entity.Property(e => e.Orderid).HasColumnName("orderid");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderdetails)
                    .HasForeignKey(d => d.Orderid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderdetails_orderid_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orderdetails)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderdetails_productid_fkey");
            });

            // Tabla Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Productid).HasName("products_pkey");

                entity.ToTable("products");

                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
