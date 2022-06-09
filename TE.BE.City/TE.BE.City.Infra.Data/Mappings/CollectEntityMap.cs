using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class CollectEntityMap : IEntityTypeConfiguration<CollectEntity>
    {
        public void Configure(EntityTypeBuilder<CollectEntity> builder)
        {
            builder.ToTable("collect");
                
            builder.HasKey(c => c.Id)
                .HasName("id");

            builder.Property(c => c.Latitude)
                .IsRequired()
                .HasColumnName("latitude")
                .HasColumnType("varchar(24)");

            builder.Property(c => c.Longitude)
                .IsRequired()
                .HasColumnName("longitude")
                .HasColumnType("varchar(24)");

            builder.Property(c => c.HasCollect)
                .IsRequired()
                .HasColumnName("hasCollect")
                .HasColumnType("bool");
            
            builder.Property(c => c.HowManyTimes)
                .IsRequired()
                .HasColumnName("howManyTimes")
                .HasColumnType("int");

            builder.Property(c => c.HasSelectiveCollect)
                .IsRequired()
                .HasColumnName("hasSelectiveCollect")
                .HasColumnType("bool");

            builder.Property(c => c.StatusId)
                 .IsRequired()
                 .HasColumnName("status")
                 .HasColumnType("int");

            builder.Property(c => c.UserId)
                .IsRequired()
                .HasColumnName("user")
                .HasColumnType("int");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            builder.Property(c => c.EndDate)
                .HasColumnName("endDate")
                .HasColumnType("datetime");

            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
            builder.Ignore(c => c.Error);

        }
    }
}
