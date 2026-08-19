using DeveloperManagement.Application.DTOs.ContentInterviewQuestion;
using DeveloperManagement.Application.DTOs.ContentQuestion;
using DeveloperManagement.Application.DTOs.Course;
using DeveloperManagement.Application.DTOs.CourseTopic;
using DeveloperManagement.Application.DTOs.Details;
using DeveloperManagement.Application.DTOs.ProgramAnswer;
using DeveloperManagement.Application.DTOs.ProgramQuestion;
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
        public DbSet<TrainingCourseTopic> TrainingCourseTopics { get; set; }
        public DbSet<ProgramQuestionResponseDto> ProgramQuestionResponses { get; set; }
        public DbSet<ProgramAnswerResponseDto> ProgramAnswerResponseDtos { get; set; }
        public DbSet<ContentQuestionResponseDto> ContentQuestionResponseDtos { get; set; }
        public DbSet<ContentInterviewQuestionResponseDto> ContentInterviewQuestionResponseDtos { get; set; }

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

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.ContentId)
            .HasColumnName("content_id");

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.ContentName)
            .HasColumnName("content_name");

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.TopicId)
            .HasColumnName("topic_id");

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.TopicName)
            .HasColumnName("topic_name");

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.Slides)
            .HasColumnName("slides");

            modelBuilder.Entity<TrainingTopicContentResponseDto>()
            .Property(x => x.VideoName)
            .HasColumnName("video_name");

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

            modelBuilder.Entity<TrainingCourseTopic>(entity =>
            {
                entity.ToTable("tbltraining_course_topics", "erpsystem");

                entity.HasKey(e => e.CourseTopicId);

                entity.Property(e => e.CourseTopicId)
                    .HasColumnName("course_topic_id");

                entity.Property(e => e.CourseId)
                    .HasColumnName("course_id");

                entity.Property(e => e.TopicId)
                    .HasColumnName("topic_id");
            });


            modelBuilder.Entity<CourseTopicResponseDto>()
                .HasNoKey();

            modelBuilder.Entity<CourseTopicResponseDto>()
                .Property(x => x.CourseTopicId)
                .HasColumnName("course_topic_id");

            modelBuilder.Entity<CourseTopicResponseDto>()
                .Property(x => x.CourseId)
                .HasColumnName("course_id");

            modelBuilder.Entity<CourseTopicResponseDto>()
                .Property(x => x.CourseName)
                .HasColumnName("course_name");

            modelBuilder.Entity<CourseTopicResponseDto>()
                .Property(x => x.TopicId)
                .HasColumnName("topic_id");

            modelBuilder.Entity<CourseTopicResponseDto>()
                .Property(x => x.TopicName)
                .HasColumnName("topic_name");


            modelBuilder.Entity<CourseDetailsFlatDto>()
            .HasNoKey();

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.CourseId)
                .HasColumnName("course_id");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.CourseName)
                .HasColumnName("course_name");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.FeesAmount)
                .HasColumnName("fees_amount");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.FeesChangeDate)
                .HasColumnName("fees_change_date");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.InstallmentPercentage)
                .HasColumnName("installment_percentage");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.TopicId)
                .HasColumnName("topic_id");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.TopicName)
                .HasColumnName("topic_name");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.ContentId)
                .HasColumnName("content_id");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.ContentName)
                .HasColumnName("content_name");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.Slides)
                .HasColumnName("slides");

            modelBuilder.Entity<CourseDetailsFlatDto>()
                .Property(x => x.VideoName)
                .HasColumnName("video_name");

            modelBuilder.Entity<ProgramQuestionResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.ProgramQuestionId)
                    .HasColumnName("program_question_id");

                entity.Property(x => x.ContentId)
                    .HasColumnName("content_id");

                entity.Property(x => x.ContentName)
                    .HasColumnName("content_name");

                entity.Property(x => x.QuestionTitle)
                    .HasColumnName("question_title");

                entity.Property(x => x.QuestionDescription)
                    .HasColumnName("question_description");
            });

            modelBuilder.Entity<ProgramAnswerResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.ProgramAnswerId)
                    .HasColumnName("program_answer_id");

                entity.Property(x => x.ProgramQuestionId)
                    .HasColumnName("program_question_id");

                entity.Property(x => x.QuestionTitle)
                    .HasColumnName("question_title");

                entity.Property(x => x.ProgramAnswer)
                    .HasColumnName("program_answer");

                entity.Property(x => x.ProgramDescription)
                    .HasColumnName("program_description");
            });

            modelBuilder.Entity<ContentQuestionResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.QuestionId)
                    .HasColumnName("question_id");

                entity.Property(x => x.ContentId)
                    .HasColumnName("content_id");

                entity.Property(x => x.ContentName)
                    .HasColumnName("content_name");

                entity.Property(x => x.Question)
                    .HasColumnName("question");

                entity.Property(x => x.Option1)
                    .HasColumnName("option1");

                entity.Property(x => x.Option2)
                    .HasColumnName("option2");

                entity.Property(x => x.Option3)
                    .HasColumnName("option3");

                entity.Property(x => x.Option4)
                    .HasColumnName("option4");

                entity.Property(x => x.CorrectOptionNumber)
                    .HasColumnName("correct_option_number");
            });

            modelBuilder.Entity<ContentInterviewQuestionResponseDto>(entity =>
            {
                entity.HasNoKey();

                entity.Property(x => x.QuestionId)
                    .HasColumnName("question_id");

                entity.Property(x => x.ContentId)
                    .HasColumnName("content_id");

                entity.Property(x => x.ContentName)
                    .HasColumnName("content_name");

                entity.Property(x => x.Question)
                    .HasColumnName("question");

                entity.Property(x => x.Answer)
                    .HasColumnName("answer");
            });


        }



    }
}

