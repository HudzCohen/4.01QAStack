using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._01QAStack.Data
{
    public class QuestionRepo
    {
        private string _connectionString;

        public QuestionRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

       //Questions:
       public List<Question> GetQuestions()
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Questions
               .Include(q => q.Answers)
               .Include(q => q.User)
               .Include(q => q.QuestionTags)
               .ThenInclude(qt => qt.Tag)
               .ToList();
        }

        public void AddQuestion(Question question, List<string> tags)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            ctx.Questions.Add(question);
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
                ctx.QuestionTags.Add(new QuestionTag
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            ctx.SaveChanges();
        }

        public Question GetQuestionById(int id)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Questions
                .Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.User)
                .Include(q => q.Answers)
                .ThenInclude(q => q.User)
                .FirstOrDefault(q => q.Id == id);
              
                
        }

        public List<Question> GetByTags(string name)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Questions
                    .Where(c => c.QuestionTags.Any(t => t.Tag.Name.ToLower() == name.ToLower()))
                    .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .ToList();
        }

        //Tags:
        private Tag GetTag(string name)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.Name == name);
        }

        private int AddTag(string name)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            var tag = new Tag { Name = name };
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.Id;
        }

        public List<Question> GetQuestionsForTag(string name)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            return ctx.Questions
                    .Where(c => c.QuestionTags.Any(t => t.Tag.Name == name))
                    .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .ToList();
        }

        //Answers:
        public void AddAnswer(Answer answer)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            ctx.Answers.Add(answer);
            ctx.SaveChanges();
             
        }


        //Likes:
        public void IncrementLikes(int id)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"UPDATE Questions SET Likes = Likes + 1 WHERE Id = {id}");
        }

        public int GetLikesById(int id)
        {
            using var ctx = new QAStackDataContext(_connectionString);
            var question = ctx.Questions.FirstOrDefault(i => i.Id == id);
            if (question == null)
            {
                return 0;
            }
            else
            {
                return question.Likes;
            }
        }
    }
}


