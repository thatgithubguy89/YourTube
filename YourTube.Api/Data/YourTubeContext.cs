using Microsoft.EntityFrameworkCore;
using YourTube.Api.Models;
using YourTube.Api.Models.Common;

namespace YourTube.Api.Data
{
    public class YourTubeContext : DbContext
    {

        public YourTubeContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VideoView> VideoViews { get; set; }
        public DbSet<Video> Videos { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                ((BaseEntity)entity.Entity).LastEditTime = DateTime.Now;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreateTime = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(u => u.Videos)
                .WithOne(v => v.User)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Video>()
                .HasMany(v => v.Comments)
                .WithOne(c => c.Video)
                .HasForeignKey(c => c.VideoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Video>()
                .HasMany(v => v.Likes)
                .WithOne(c => c.Video)
                .HasForeignKey(c => c.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
