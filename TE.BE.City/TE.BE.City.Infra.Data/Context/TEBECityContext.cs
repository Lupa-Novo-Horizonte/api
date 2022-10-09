using Microsoft.EntityFrameworkCore;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Infra.Data.Mappings;

namespace TE.BE.City.Infra.Data
{
    public class TEBECityContext : DbContext
    {
        public TEBECityContext(DbContextOptions options) : base(options)
        {
            //Empty
        }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<RoleEntity> Role { get; set; }
        public DbSet<WaterEntity> Water { get; set; }
        public DbSet<CollectEntity> Collect { get; set; }
        public DbSet<AsphaltEntity> Asphalt { get; set; }
        public DbSet<LightEntity> Light { get; set; }
        public DbSet<SewerEntity> Sewer { get; set; }
        public DbSet<TrashEntity> Trash { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<PublicServiceEntity> PublicService { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(new UserEntityMap().Configure);
            modelBuilder.Entity<RoleEntity>(new RoleEntityMap().Configure);
            modelBuilder.Entity<WaterEntity>(new WaterEntityMap().Configure);
            modelBuilder.Entity<CollectEntity>(new CollectEntityMap().Configure);
            modelBuilder.Entity<AsphaltEntity>(new AsphaltEntityMap().Configure);
            modelBuilder.Entity<LightEntity>(new LightEntityMap().Configure);
            modelBuilder.Entity<SewerEntity>(new SewerEntityMap().Configure);
            modelBuilder.Entity<TrashEntity>(new TrashEntityMap().Configure);
            modelBuilder.Entity<StatusEntity>(new StatusEntityMap().Configure);
            modelBuilder.Entity<PublicServiceEntity>(new PublicServiceEntityMap().Configure);
            base.OnModelCreating(modelBuilder);
        }
    }
}
