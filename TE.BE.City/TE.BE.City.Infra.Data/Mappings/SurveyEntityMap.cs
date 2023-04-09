using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class SurveyEntityMap : IEntityTypeConfiguration<SurveyEntity>
    {
        public void Configure(EntityTypeBuilder<SurveyEntity> builder)
        {
            builder.ToTable("survey");

            builder.HasKey(c => c.Id)
                .HasName("id");

            builder.Property(c => c.Question01)
               .HasColumnName("question01")
               .HasColumnType("varchar(1)");

            builder.Property(c => c.Question02)
               .HasColumnName("question02")
               .HasColumnType("varchar(1)");

            builder.Property(c => c.Question03)
               .HasColumnName("question03")
               .HasColumnType("varchar(1)");

            builder.Property(c => c.Question04)
               .HasColumnName("question04")
               .HasColumnType("varchar(1)");

            builder.Property(c => c.Question05)
               .HasColumnName("question05")
               .HasColumnType("varchar(1)");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            builder.Ignore(c => c.Latitude);
            builder.Ignore(c => c.Longitude);
            builder.Ignore(c => c.StatusId);
            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
            builder.Ignore(c => c.UserId);
            builder.Ignore(c => c.EndDate);
            builder.Ignore(c => c.Error);
        }
    }
}
