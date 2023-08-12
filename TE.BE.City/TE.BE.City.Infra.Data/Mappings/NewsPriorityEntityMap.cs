using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Infra.Data.Mappings
{
    public class NewsPriorityEntityMap : IEntityTypeConfiguration<NewsPriorityEntity>
    {
        public void Configure(EntityTypeBuilder<NewsPriorityEntity> builder)
        {
            builder.ToTable("newsPriority");

            builder.HasKey(c => c.Id)
                .HasName("id");

            builder.Property(c => c.OccurrenceId)
                .HasColumnName("occurrenceId")
                .HasColumnType("int");

            builder.Property(c => c.OccurrenceType)
                .HasColumnName("occurrenceType")
                .HasColumnType("varchar(24)");

            builder.Property(c => c.Weight)
                .HasColumnName("weight")
                .HasColumnType("int");

            builder.Ignore(c => c.Address);
            builder.Ignore(c => c.Longitude);
            builder.Ignore(c => c.Latitude);
            builder.Ignore(c => c.CreatedAt);
            builder.Ignore(c => c.StatusId);
            builder.Ignore(c => c.UserId);
            builder.Ignore(c => c.EndDate);
            builder.Ignore(c => c.Status);
            builder.Ignore(c => c.User);
            builder.Ignore(c => c.Error);
            builder.Ignore(c => c.Score);
        }
    }
}
