using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class SewerEntityMap : IEntityTypeConfiguration<SewerEntity>
    {
        public void Configure(EntityTypeBuilder<SewerEntity> builder)
        {
            builder.ToTable("sewer");

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

            builder.Property(c => c.HasHomeSewer)
                .HasColumnName("hasHomeSewer")
                .HasColumnType("bool");

            builder.Property(c => c.HasHomeCesspool)
                .HasColumnName("hasHomeCesspool")
                .HasColumnType("bool");

            builder.Property(c => c.DoesCityHallCleanTheSewer)
                .HasColumnName("doesCityHallCleanTheSewer")
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
