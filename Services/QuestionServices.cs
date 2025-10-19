using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.subject_Lesson;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class QuestionServices(IUnitOfWork unitOfWork ,ILogger<Question> logger,UserManager<AppUsers> userManager) : IQuestionServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<Question> _logger = logger;
        private readonly UserManager<AppUsers> _userManager = userManager;
        //Create Question
        public async Task<string> CreateQuestion(QuestionCreateOrUpdateDto questionCreateDto, string QuizCode,string email)
        {
            try
            {
                //check on eamil 
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null) throw new NotFoundException("User not found");
                //check on quiz code
                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz is null) throw new NotFoundException("Quiz not found");
                //check if the user is the owner of the quiz
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetByIdAsync(quiz.Lesson.SubjectId);
               
                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You are not authorized to add questions to this quiz");
                //create question
                var question = new Question
                {
                    QuestionText = questionCreateDto.QuestionText,
                    QuestionType = questionCreateDto.QuestionType.ToString(),
                    Points = questionCreateDto.Points,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    QuizId = quiz.QuizId,
                };
                 _unitOfWork.GetRepository<Question, int>().AddAsync(question);
                //Update quiz total points
                quiz.TotalMarks +=(int) question.Points;
                _unitOfWork.GetRepository<Quiz, int>().UpdateAsync(quiz);
                //save changes
                var res =await _unitOfWork.SaveChanges();
                
                return (res <= 0)? throw new BadRequestException("Failed to create question"): "Question created successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating question");
                throw new Exception(ex.Message);
            }
        }

        //Delete Question
        public async Task<string> DeleteQuestion(string questionCode, string QuizCode, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null) throw new NotFoundException("User not found");
                //check on quiz code
                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz is null) throw new NotFoundException("Quiz not found");
                //check if the user is the owner of the quiz
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetByIdAsync(quiz.Lesson.SubjectId);

                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You are not authorized to add questions to this quiz");

                //delete question
                var question = await _unitOfWork.GetRepository<Question, int>().GetEntityWithCode<Question>(questionCode);
                if (question is null) throw new NotFoundException("Question not found");
                _unitOfWork.GetRepository<Question, int>().DeleteAsync(question);
                //Update quiz total points
                quiz.TotalMarks -= (int)question.Points;
                _unitOfWork.GetRepository<Quiz, int>().UpdateAsync(quiz);
                //save changes
                var res = await _unitOfWork.SaveChanges();
                return (res <= 0) ? throw new BadRequestException("Failed to Delete question") : "Question Deleted successfully";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting question");
                throw new Exception(ex.Message);
            }
        }
        //Update Question
        public async Task<string> UpdateQuestion(string questionCode,string QuizCode, QuestionCreateOrUpdateDto questionUpdateDto, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null) throw new NotFoundException("User not found");
                //check on quiz code
                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz is null) throw new NotFoundException("Quiz not found");
                //check if the user is the owner of the quiz
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetByIdAsync(quiz.Lesson.SubjectId);

                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You are not authorized to add questions to this quiz");
                // i want to get total points before update 
                var totalPointsBeforeUpdate = quiz.TotalMarks;
                //Get question
                if (string.IsNullOrEmpty(questionCode)) throw new NullReferenceException("Question code is null or empty");
                var question = await _unitOfWork.GetRepository<Question, int>().GetEntityWithCode<Question>(questionCode);
                if (question is null) throw new NotFoundException("Question not found");
                //Update question
                question.QuestionText = questionUpdateDto.QuestionText ?? question.QuestionText;
                question.QuestionType = questionUpdateDto.QuestionType.ToString() ?? question.QuestionType;
                question.Points = questionUpdateDto.Points ??question.Points;

                question.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<Question, int>().UpdateAsync(question);
                //update quiz total points
                quiz.TotalMarks = quiz.TotalMarks - (int)totalPointsBeforeUpdate + (int)(questionUpdateDto.Points ?? totalPointsBeforeUpdate);
                 _unitOfWork.GetRepository<Quiz, int>().UpdateAsync(quiz);

                return (await _unitOfWork.SaveChanges() <= 0) ? throw new BadRequestException("Failed to update question") : "Question updated successfully";
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while updating question");
                throw new Exception(ex.Message);
            }

        }
        //Get Question By Code
        public Task<QuestionDto> GetQuestionByCode(string questionCode)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<QuestionDto>> GetAllQuestions(string QuizCode)
        {
            throw new NotImplementedException();
        }

       
      
    }
}
