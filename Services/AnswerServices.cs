using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models.subject_Lesson;
using Microsoft.Extensions.Logging;
using Services.SpecificationsFile;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AnswerServices(IUnitOfWork unitOfWork,ILogger<Answer> logger) : IAnswerServices
    {
        private readonly IUnitOfWork _unitOfWork=unitOfWork;
        private readonly ILogger<Answer> _logger = logger;
        public async Task<string> createAnswer(AnswerCreateOrUpdateDto create ,string QuestionCode)
        {
            try
            {
                //check if question exists
                if(string.IsNullOrEmpty(QuestionCode) ||create is null)
                {
                    throw new NullReferenceException("Question code Or create Data  is Invalid . ");
                }
                var question =await _unitOfWork.GetRepository<Question,int>().GetEntityWithCode<Question>(QuestionCode);
                if (question is null) throw new NotFoundException("Question Is Invalid,try Valid Qustion");
                var answer = new Answer
                {
                    AnswerText = create.AnswerText,
                    IsCorrect = create.IsCorrect,
                    QuestionId = question.QuestionId
                };
                 _unitOfWork.GetRepository<Answer, int>().AddAsync(answer);
                return await _unitOfWork.SaveChanges() > 0 ? "Add New Answer Successfully": throw new BadRequestException("Failed to create answer.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an answer.");
                throw;
            }
                
        }

        public async Task<string> DeleteAnswer(string AnswerCode)
        {
            try
            {
                //check if answer exists
                if (string.IsNullOrEmpty(AnswerCode))
                {
                    throw new NullReferenceException("Answer code is Invalid . ");
                }
                var answer =await  _unitOfWork.GetRepository<Answer, int>().GetEntityWithCode<Answer>(AnswerCode);
                if (answer is null) throw new NotFoundException("Answer Is Invalid,try Valid Answer");
               
                _unitOfWork.GetRepository<Answer, int>().DeleteAsync(answer);
                return await _unitOfWork.SaveChanges() > 0 ? "Delete Answer Successfully" : throw new BadRequestException("Failed to delete answer.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an answer.");
                throw;
            }
        }

        public async  Task<List<AnswerDtoResponse>> GetAllAnswersOfQuestion(string QuestionCode)
        {
            try
            {
                if (string.IsNullOrEmpty(QuestionCode))
                {
                    throw new NullReferenceException("Question code is Invalid . ");
                }
                
                var question = await _unitOfWork.GetRepository<Question, int>().GetEntityWithCode<Question>(QuestionCode);
                if (question is null) throw new NotFoundException("Question Is Invalid,try Valid Qustion");

                var answers = await _unitOfWork.GetRepository<Answer, int>().GetByConditionAsync(new Specifications<Answer>( a=>a.QuestionId == question.QuestionId));

                var answerDtos = answers.Select(a => new AnswerDtoResponse
                {
                    AnswerCode = a.Code,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                }).ToList();

                return answerDtos;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving answers for question ");
                throw;
            }
        }


        public async Task<AnswerDtoResponse> GetAnswerByCode(string AnswerCode)
        {
            try
            {
                //check On Answer Code 
                if (string.IsNullOrEmpty(AnswerCode))
                    throw new NullReferenceException("Answer code is Invalid . ");

                var answer = await _unitOfWork.GetRepository<Answer, int>().GetEntityWithCode<Answer>(AnswerCode);
                if (answer is null) throw new NotFoundException("Answer Is Invalid,try Valid Answer");
                var answerDto = new AnswerDtoResponse
                {
                    AnswerCode = answer.Code,
                    AnswerText = answer.AnswerText,
                    IsCorrect = answer.IsCorrect
                };
                return answerDto;


            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving answer by code.");
                throw;
            }
        }

        public async  Task<string> UpdateAnswer(AnswerCreateOrUpdateDto update, string AnswerCode)
        {
            try
            {
                //check On Answer Code
                if (string.IsNullOrEmpty(AnswerCode) || update is null)
                    throw new NullReferenceException("Answer code Or update Data  is Invalid . ");
                var answer = await _unitOfWork.GetRepository<Answer, int>().GetEntityWithCode<Answer>(AnswerCode);
                if (answer is null) throw new NotFoundException("Answer Is Invalid,try Valid Answer");
                answer.AnswerText = update.AnswerText;
                answer.IsCorrect = update.IsCorrect;
                _unitOfWork.GetRepository<Answer, int>().UpdateAsync(answer);
                return await _unitOfWork.SaveChanges() > 0 ? "Update Answer Successfully" : throw new BadRequestException("Failed to update answer.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an answer.");
                throw;
            }
        }
    }
}
