using Delivery_app.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
                .HasMany(_ => _.drop_points)
                .WithOne()
                .HasForeignKey(_ => _.order_id);

            modelBuilder.Entity<Orders>()
                .HasOne(_ => _.user)
                .WithMany()
                .HasForeignKey(_ => _.user_id);

            modelBuilder.Entity<Payments>()
                .HasOne(_ => _.order)
                .WithOne()
                .HasForeignKey<Payments>(_ => _.order_id);

            modelBuilder.Entity<Payments>()
                .HasOne(_ => _.courier)
                .WithMany()
                .HasForeignKey(_ => _.courier_id);

            modelBuilder.Entity<Payments>()
                .HasOne(_ => _.user)
                .WithMany()
                .HasForeignKey(_ => _.user_id);

            modelBuilder.Entity<Couriers>()
                .HasMany(_ => _.documents)
                .WithOne()
                .HasForeignKey(_ => _.courier_id);

            modelBuilder.Entity<Couriers>()
                .Property(_ => _.onBoard)
                .HasDefaultValue(true);

            modelBuilder.Entity<Couriers>()
                .Property(_ => _.commission)
                .HasDefaultValue(0.8);
        }

        public DbSet<Users> users { get; set; }
        public DbSet<DropPoints> drop_points { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Couriers> couriers { get; set; }
        public DbSet<Documents> documents { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<PromoCodes> promo_codes { get; set; }
        public DbSet<Payments> payments { get; set; }
    }
}
