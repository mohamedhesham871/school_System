using Domain.Contract;
using Domain.Models.subject_Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SpecificationsFile.Questions
{
    public class QuestionSpec : Specifications<Question>
    {
        public QuestionSpec(string QuestionCode) : base(c => c.Code == QuestionCode)
        {
            AddInclude(a => a.Answers);
        }
        // For filteringv[filter will Just Take Index page number] and pagination
        public QuestionSpec(int QuizId , int PageIndex) :base(q=>q.QuizId == QuizId)
        {
            AddInclude(a => a.Answers);
          

            if (PageIndex > 0)
            {
                ApplyPaging(PageIndex, 5);
            }
            else
            {
                ApplyPaging(1, 5);
            }

        }





}
}
