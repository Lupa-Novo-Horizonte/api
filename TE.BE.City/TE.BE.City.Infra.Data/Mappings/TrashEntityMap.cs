using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class TrashEntityMap : IEntityTypeConfiguration<TrashEntity>
    {
        public void Configure(EntityTypeBuilder<TrashEntity> builder)
        {
            builder.ToTable("trash");

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

            builder.Property(c => c.HasRoadcleanUp)
                .IsRequired()
                .HasColumnName("hasRoadcleanUp")
                .HasColumnType("bool");

            builder.Property(c => c.HowManyTimes)
                .IsRequired()
                .HasColumnName("howManyTimes")
                .HasColumnType("int");

            builder.Property(c => c.HasAccumulatedTrash)
                .IsRequired()
                .HasColumnName("hasAccumulatedTrash")
                .HasColumnType("bool");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            builder.Property(c => c.StatusId)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("int");

            builder.Property(c => c.UserId)
                .IsRequired()
                .HasColumnName("user")
                .HasColumnType("int");

            builder.Property(c => c.EndDate)
                .HasColumnName("endDate")
                .HasColumnType("datetime");

            builder.Ignore(c => c.Error);
            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
        }
    }
}
