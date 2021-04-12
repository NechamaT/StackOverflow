using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace QASite.Data
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public List<Question> Questions { get; set; }
        public List<Likes> Likes { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
