using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.DTOs.Topic;
using DeveloperManagement.Application.DTOs.TopicContent;
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
        public DbSet<TrainingTopicContent> TrainingTopicContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<TopicResponseDto>()
                .HasNoKey();

            modelBuilder.Entity<TopicResponseDto>()
            .Property(x => x.TopicId)
            .HasColumnName("topic_id");

            modelBuilder.Entity<TopicResponseDto>()
                .Property(x => x.TopicName)
                .HasColumnName("topic_name");

            modelBuilder.Entity<TopicResponseDto>()
                .Property(x => x.PublicFolderId)
                .HasColumnName("publicfolderid");

            modelBuilder.Entity<TrainingCourse>(entity =>
            {
                entity.ToTable("tbltraining_courses", "erpsystem");

                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id");

                entity.Property(e => e.CourseName)
                    .HasColumnName("course_name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.FeesAmount)
                    .HasColumnName("fees_amount");

                entity.Property(e => e.FeesChangeDate)
                    .HasColumnName("fees_change_date");

                entity.Property(e => e.InstallmentPercentage)
                    .HasColumnName("installment_percentage");
            });

            modelBuilder.Entity<TrainingCourseResponseDto>()
            .HasNoKey();

            modelBuilder.Entity<TrainingCourseResponseDto>()
                .Property(x => x.CourseId)
                .HasColumnName("course_id");

            modelBuilder.Entity<TrainingCourseResponseDto>()
                .Property(x => x.CourseName)
                .HasColumnName("course_name");

            modelBuilder.Entity<TrainingCourseResponseDto>()
                .Property(x => x.FeesAmount)
                .HasColumnName("fees_amount");

            modelBuilder.Entity<TrainingCourseResponseDto>()
                .Property(x => x.FeesChangeDate)
                .HasColumnName("fees_change_date");

            modelBuilder.Entity<TrainingCourseResponseDto>()
                .Property(x => x.InstallmentPercentage)
                .HasColumnName("installment_percentage");

            modelBuilder.Entity<CheckNameResponseDto>()
               .HasNoKey();

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
              .HasNoKey();

            modelBuilder.Entity<TrainingTopicContent>(entity =>
            {
                entity.ToTable("tbltopic_contents", "erpsystem");

                entity.HasKey(e => e.ContentId);

                entity.Property(e => e.ContentId)
                    .HasColumnName("content_id");

                entity.Property(e => e.ContentName)
                    .HasColumnName("content_name");

                entity.Property(e => e.TopicId)
                    .HasColumnName("topic_id");

                entity.Property(e => e.Slides)
                    .HasColumnName("slides");

                entity.Property(e => e.VideoName)
                    .HasColumnName("video_name");
            });
        }
    }
}

