using DeveloperManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeveloperManagement.Infrastructure.Persistence.Context
{
    public class DeveloperDbContext : DbContext
    {
        public DeveloperDbContext(DbContextOptions<DeveloperDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingTopic> TrainingTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingTopic>(entity =>
            {
                entity.ToTable("tbltraining_topics", "erpsystem");

                entity.HasKey(e => e.TopicId);

                entity.Property(e => e.TopicId)
                    .HasColumnName("topic_id");

                entity.Property(e => e.TopicName)
                    .HasColumnName("topic_name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.PublicFolderId)
                    .HasColumnName("publicfolderid");
            });
        }
    }
}

