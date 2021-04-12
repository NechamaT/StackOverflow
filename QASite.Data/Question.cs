using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace QASite.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public User User { get; set; }
        public List<Likes> Likes { get; set; }
        public List<Answer> Answers { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
    }
}
