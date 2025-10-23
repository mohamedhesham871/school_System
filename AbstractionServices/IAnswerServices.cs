using Domain.Contract;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public interface IAnswerServices 
    {
        Task<string> createAnswer(AnswerCreateOrUpdateDto create,string QuestionCode);
        Task<string> UpdateAnswer(AnswerCreateOrUpdateDto update,string AnswerCode);
        Task<string> DeleteAnswer(string AnswerCode);
        Task<List<AnswerDtoResponse>> GetAllAnswersOfQuestion(string QuestionCode);
        Task<AnswerDtoResponse> GetAnswerByCode(string AnswerCode);
    }
}
