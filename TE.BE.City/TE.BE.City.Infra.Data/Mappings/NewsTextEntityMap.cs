using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class NewsTextEntityMap : IEntityTypeConfiguration<NewsTextEntity>
    {
        public void Configure(EntityTypeBuilder<NewsTextEntity> builder)
        {
            builder.ToTable("newsText");

            builder.HasKey(c => c.Id)
                .HasName("id");

            builder.Property(c => c.T1)
                .HasColumnName("t1")
                .HasColumnType("string");

            builder.Property(c => c.T2)
                .HasColumnName("t2")
                .HasColumnType("string");

            builder.Property(c => c.T3)
                .HasColumnName("t3")
                .HasColumnType("string");
            
            builder.Property(c => c.GenerativeTool)
                .HasColumnName("generativeTool")
                .HasColumnType("string");

            builder.Ignore(c => c.Longitude);
            builder.Ignore(c => c.Latitude);
            builder.Ignore(c => c.CreatedAt);
            builder.Ignore(c => c.StatusId);
            builder.Ignore(c => c.UserId);
            builder.Ignore(c => c.EndDate);
            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
            builder.Ignore(c => c.Error);
        }
    }
}
