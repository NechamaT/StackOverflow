using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using QASite.Data.Migrations;

namespace QASite.Data
{
    public class QuestionsRepository
    {
        private readonly string _connectionString;

        public QuestionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private Tag GetTag(string name)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.Name == name);
        }

        private int AddTag(string name)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            var tag = new Tag {Name = name};
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.Id;
        }

        public void AddQuestion(Question q, List<string> tags)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
      
            ctx.Questions.Add(q);
            ctx.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }

                ctx.QuestionsTags.Add(new QuestionTags
                {
                    QuestionId = q.Id,
                    TagId = tagId
                });

            }
        }

        public List<Question> GetAllQuestions()
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            List<Question> questions = ctx.Questions.OrderByDescending(q => q.DatePosted).Include(q => q.Likes)
                .Include(q => q.Answers)
                .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag).ToList();
            return questions;
        }

        public List<Question> GetQuestionsForTag(string name)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            return ctx.Questions.Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                .Where(a => a.QuestionTags.Any((t => t.Tag.Name == name))).ToList();
        }

      
        public Question GetQuestionById(int id)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            return ctx.Questions
                .Include(q => q.Likes)
                .ThenInclude(l => l.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.User)
                .FirstOrDefault(q => q.Id == id);
        }

        public void AddAnswer(Answer answer)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            ctx.Answers.Add(answer);
            ctx.SaveChanges();
        }

        public void UpdateLike(int id, User user)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            var like = new Likes
            {
                QuestionId = id,
                UserId = user.ID
            };
            ctx.Likes.Add(like);
            ctx.SaveChanges();
        }

        public int GetLikes(int id)
        {
            using var ctx = new QuestionTagsContext(_connectionString);
            var question = GetQuestionById(id);
            return question.Likes.Count;
        }

      
    }
}
