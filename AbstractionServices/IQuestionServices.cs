using Shared;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public  interface IQuestionServices
    {
        Task<string> CreateQuestion(QuestionCreateOrUpdateDto questionCreateDto , string QuizCode,string email);
        Task<string> UpdateQuestion(string questionCode,string QuizCode, QuestionCreateOrUpdateDto questionUpdateDto ,string email);
        Task<string> DeleteQuestion(string questionCode,string QuizCode, string email);
        Task<QuestionDto> GetQuestionByCode(string questionCode);
        Task<PaginationResponse<QuestionDto>> GetAllQuestions(string QuizCode,int Index);



    }
}