using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class AsphaltEntityMap : IEntityTypeConfiguration<AsphaltEntity>
    {
        public void Configure(EntityTypeBuilder<AsphaltEntity> builder)
        {
            builder.ToTable("asphalt");
                
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

            builder.Property(c => c.IsPaved)
                .IsRequired()
                .HasColumnName("IsPaved")
                .HasColumnType("bool");

            builder.Property(c => c.HasHoles)
                .IsRequired()
                .HasColumnName("hasHoles")
                .HasColumnType("bool");

            builder.Property(c => c.HasPavedSidewalks)
                .IsRequired()
                .HasColumnName("hasPavedSidewalks")
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
