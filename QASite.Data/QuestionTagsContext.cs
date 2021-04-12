using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QASite.Data
{
    public class QuestionTagsContext : DbContext
    {
        private readonly string _connectionString;

            public QuestionTagsContext(string connectionString)
            {
                _connectionString = connectionString;
            }

            

            public DbSet<Question> Questions { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<QuestionTags> QuestionsTags { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Likes> Likes { get; set; }
            public DbSet<Answer> Answers { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                //Taken from here:
                //https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration

                //set up composite primary key
                modelBuilder.Entity<QuestionTags>()
                    .HasKey(qt => new { qt.QuestionId, qt.TagId });

                //set up foreign key from QuestionsTags to Questions
                modelBuilder.Entity<QuestionTags>()
                    .HasOne(qt => qt.Question)
                    .WithMany(q => q.QuestionTags)
                    .HasForeignKey(q => q.QuestionId);

                //set up foreign key from QuestionsTags to Tags
                modelBuilder.Entity<QuestionTags>()
                    .HasOne(qt => qt.Tag)
                    .WithMany(t => t.QuestionTags)
                    .HasForeignKey(q => q.TagId);

                modelBuilder.Entity<Likes>().HasKey(l => new { l.QuestionId, l.UserId });
                modelBuilder.Entity<Likes>()
                    .HasOne(l => l.Question)
                    .WithMany(q => q.Likes)
                    .HasForeignKey(l => l.QuestionId);
                modelBuilder.Entity<Likes>()
                    .HasOne(l => l.User)
                    .WithMany(u => u.Likes)
                    .HasForeignKey(l => l.UserId);
        }
        }
       
        }

    

