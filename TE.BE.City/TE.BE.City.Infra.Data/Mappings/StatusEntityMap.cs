using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class StatusEntityMap : IEntityTypeConfiguration<StatusEntity>
    {
        public void Configure(EntityTypeBuilder<StatusEntity> builder)
        {
            builder.ToTable("status");

            builder.HasKey(c => c.Id)
                .HasName("id");

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Closed)
               .IsRequired()
               .HasColumnName("closed")
               .HasColumnType("boolean");


            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("createdAt")
                .HasColumnType("datetime");

            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
            builder.Ignore(c => c.Error);
            builder.Ignore(c => c.EndDate);
            builder.Ignore(c => c.Latitude);
            builder.Ignore(c => c.Longitude);
            builder.Ignore(c => c.StatusId);
            builder.Ignore(c => c.UserId);
        }
    }
}
