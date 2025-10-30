using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestaurantGorRahsa.Models;

namespace RestaurantGorRahsa.Models
{
    public class RMScontext: IdentityDbContext<ApplicationUser>
    {

        public RMScontext(DbContextOptions<RMScontext> options)
            : base(options)
        {
        }

        public DbSet<ModelType> TbType { get; set; }
        public DbSet<ModelMeal> TbMeal { get; set; }

        public DbSet<ModelItem> TbItem { get; set; }

        public DbSet<ModelCart> TbCart { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

      

            modelBuilder.Entity<ModelMeal>(entity =>
            {
                entity.HasKey(e=>e.ID);

                entity.HasOne(e=>e.type).WithMany(j=>j.lstMeal).HasForeignKey(e=>e.TypeID).OnDelete(DeleteBehavior.NoAction);
   
                entity.HasMany(e=>e.lstItems).WithOne(j=>j.meal).HasForeignKey(j=>j.MealID).OnDelete(DeleteBehavior.NoAction);
            });



            modelBuilder.Entity<ModelType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e=>e.lstMeal).WithOne(j=>j.type).HasForeignKey(j=>j.TypeID).OnDelete(DeleteBehavior.Cascade);
           

            });


            modelBuilder.Entity<ModelItem>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasOne(e => e.meal).WithMany(j => j.lstItems).HasForeignKey(e=>e.MealID).OnDelete(DeleteBehavior.NoAction);
                    entity.HasOne(e=>e.cart).WithMany(j=>j.lstItems).HasForeignKey(e=>e.CartID).OnDelete(DeleteBehavior.NoAction);
                }
                );

            modelBuilder.Entity<ModelCart>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasMany(e => e.lstItems).WithOne(j => j.cart).HasForeignKey( j => j.CartID).OnDelete(DeleteBehavior.NoAction);
                    entity.HasOne(c => c.User)
                     .WithMany(u => u.Carts)
                    .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
                }
                );

           

        }



        }
}

    