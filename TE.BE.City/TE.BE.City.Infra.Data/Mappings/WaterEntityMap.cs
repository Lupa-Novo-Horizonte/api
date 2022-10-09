using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class WaterEntityMap : IEntityTypeConfiguration<WaterEntity>
    {
        public void Configure(EntityTypeBuilder<WaterEntity> builder)
        {
            builder.ToTable("water");

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

            builder.Property(c => c.HomeWithWater)
                .IsRequired()
                .HasColumnName("homeWithWater")
                .HasColumnType("bool");

            builder.Property(c => c.WaterMissedInAWeek)
                .IsRequired()
                .HasColumnName("waterMissedInAWeek")
                .HasColumnType("int");

            builder.Property(c => c.HasWell)
                .IsRequired()
                .HasColumnName("hasWell")
                .HasColumnType("bool");

            builder.Property(c => c.HasSanitationProject)
                .HasColumnName("hasSanitationProject")
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