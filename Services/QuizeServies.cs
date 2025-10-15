using AbstractionServices;
using Domain.Contract;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.subject_Lesson;
using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.SpecificationsFile;
using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class QuizeServies(IUnitOfWork unitOfWork, ILogger<Quiz> logger, UserManager<AppUsers> userManager) : IQuizServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<Quiz> _logger = logger;
        private readonly UserManager<AppUsers> _userManager = userManager;
        //Create a New Quiz By Teacher
        public async Task<string> CreateQuizAsync(CreateOrUpdateQuizDto quizDto, string LessonCode)
        {
            try
            {
                if (quizDto == null) throw new NullRefrenceException("Quiz data cannot be null.");

                // Check if the lesson exists
                var lesson = await _unitOfWork.GetRepository<Lesson, int>().GetEntityWithCode<Lesson>(LessonCode);

                if (string.IsNullOrEmpty(LessonCode) || lesson == null)
                    throw new NullRefrenceException($"Lesson with code {LessonCode} not found.");

                //Cheack if Lesson Already Has Quiz
                var ExitingQuiz = lesson.quiz.QuizId;
                if (ExitingQuiz != 0)
                    throw new BadRequestException($"Lesson with  already has a quiz.,Please update the existing quiz instead");

                // Create a new Quiz entity
                var Quiz = new Quiz()
                {
                    Title = quizDto.Title,
                    Description = quizDto.Description,
                    IsActive = quizDto.IsActive,
                    LessonId = lesson.LessonId,
                    TotalMarks = 0, // Will be calculated when questions are added
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow

                };
                // Add the new quiz to the repository
                _unitOfWork.GetRepository<Quiz, int>().AddAsync(Quiz);
                var res = await _unitOfWork.SaveChanges();
                return res > 0 ? "Quiz created successfully." : throw new DatabaseException("Error creating quiz");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a quiz.");
                throw;
            }
        }

        //Delete Quiz By Teacher Who Own Subject
        public async Task<string> DeleteQuizAsync(string QuizCode, string email)
        {
            try
            {
                // cheak if QuizCode is null or empty
                if (string.IsNullOrEmpty(QuizCode))
                    throw new NullRefrenceException("Quiz code cannot be null or empty.");
                // Check if the quiz exists
                var quiz = _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode).Result;
                if (quiz == null)
                    throw new NotFoundException($"Quiz with code {QuizCode} not found.");

                // Check if the user is a teacher
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !await _userManager.IsInRoleAsync(user, "Teacher"))
                    throw new UnauthorizedAccessException("Only teachers can delete quizzes.");

                // cheack that Teacher is Owner of Subject
                var lesson = await _unitOfWork.GetRepository<Lesson, int>().GetByIdAsync(quiz.LessonId);
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetByIdAsync(lesson.SubjectId);
                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You do not have permission to delete this quiz.");
                // Delete the quiz
                _unitOfWork.GetRepository<Quiz, int>().DeleteAsync(quiz);
                var res = await _unitOfWork.SaveChanges();
                return res > 0 ? "Quiz deleted successfully." : throw new DatabaseException("Error deleting quiz");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a quiz.");
                throw;
            }
        }

        //Update Quiz By Teacher Who Own Subject
        public async Task<string> UpdateQuizAsync(string QuizCode, CreateOrUpdateQuizDto quizDto, string email)
        {

            try
            {
                // cheak if QuizCode is null or empty
                if (string.IsNullOrEmpty(QuizCode) || quizDto is null)
                    throw new NullRefrenceException("Quiz  cannot be null or empty.");
                // Check if the quiz exists
                var quiz = _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode).Result;
                if (quiz == null)
                    throw new NotFoundException($"Quiz with code {QuizCode} not found.");

                // Check if the user is a teacher
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !await _userManager.IsInRoleAsync(user, "Teacher"))
                    throw new UnauthorizedAccessException("Only teachers can Update quizzes.");

                // cheack that Teacher is Owner of Subject
                var lesson = await _unitOfWork.GetRepository<Lesson, int>().GetByIdAsync(quiz.LessonId);
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetByIdAsync(lesson.SubjectId);
                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You do not have permission to Update this quiz.");
                // Update the quiz
                quiz.Title = quizDto.Title ?? quiz.Title;
                quiz.Description = quizDto.Description ?? quiz.Description;
                quiz.IsActive = quizDto.IsActive;
                quiz.UpdatedAt = DateTime.UtcNow.ToLocalTime();

                // Update
                _unitOfWork.GetRepository<Quiz, int>().UpdateAsync(quiz);
                var res = await _unitOfWork.SaveChanges();
                return res > 0 ? "Quiz update successfully." : throw new DatabaseException("Error updating quiz");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a quiz.");
                throw;
            }
        }

        //Get All Questions In Quiz By Quiz Code
        public async Task<List<QuestionDto>> GetAllQuestionsInQuizByQuizCodeAsync(string QuizCode)
        {
            try
            {
                //check if QuizCode is null or empty
                if (string.IsNullOrEmpty(QuizCode))
                    throw new NullRefrenceException("Quiz code cannot be null or empty.");
                // Check if the quiz exists
                var quiz = _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode).Result;
                if (quiz == null)
                    throw new NotFoundException($"Quiz not found. Please try valid Quiz");

                // Get all questions associated with the quiz
                var Questions = await _unitOfWork.GetRepository<Question, int>().GetByConditionAsync(new Specifications<Question>(q => q.QuizId == quiz.QuizId));

                if (Questions == null)
                    throw new NotFoundException("No questions found for this quiz.");
                // Map to QuestionDto
                var QuestionDtos = Questions.Select(q => new QuestionDto
                {
                    QuestionText = q.QuestionText,
                    QuestionCode = q.Code,
                    Points = q.Points,
                    QuestionType = q.QuestionType,
                    Answers = q.Answers.Select(c => new AnswerDtoResponse
                    {
                        AnswerCode = c.Code,
                        AnswerText = c.AnswerText,
                        IsCorrect = c.IsCorrect,
                    }).ToList()
                }).ToList();
                return QuestionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all questions in quiz .");
                throw;
            }
        }


        //Get All Quizes In Subject By Subject Code [As Each Lesson may have Quizz]
        public async Task<List<QuizShort>> GetAllQuizesInSubjectBySubjectCodeAsync(string SubjectCode)
        {
            try
            {
                // check if subject code is null or no data 
                if (string.IsNullOrEmpty(SubjectCode) || await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode) is null)
                    throw new NotFoundException("Subject code Or Subject Doesn't Exist Try Again .");

                // Get all lessons associated with the subject
                var lessons = await _unitOfWork.GetRepository<Lesson, int>().GetByConditionAsync(new Specifications<Lesson>(l => l.Subject.Code == SubjectCode));

                var quizzes = lessons.Select(l => new QuizShort()
                {
                    QuizCode = l.quiz.Code,
                    Title = l.quiz.Title,
                    LessonTitle = l.Title,
                    TotalMarks = l.quiz.TotalMarks
                }).ToList();

                return quizzes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all quizes in subject .");
                throw;
            }
        }

       
        //Get All Quiz Results Of Subject By Subject Code [Teacher]
        public async Task<List<QuizStudentResultDetailsForSubjectDto>> GetAllQuizResultsOfSubjectByCodeAsync(string SubjectCode, string email)
        {
            try
            {
                var subject = await _unitOfWork.GetRepository<Subject, int>().GetEntityWithCode<Subject>(SubjectCode);
                if (string.IsNullOrEmpty(SubjectCode) || subject is null)
                    throw new NotFoundException("Subject code Or Subject Doesn't Exist Try Again .");
                // Check if the user is a teacher
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !await _userManager.IsInRoleAsync(user, "Teacher"))
                    throw new UnauthorizedAccessException("Only teachers can view quiz results.");
                // cheack that Teacher is Owner of Subject
                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You do not have permission to view quiz results for this subject.");
                // Get all lessons associated with the subject
                var lessons = await _unitOfWork.GetRepository<Lesson, int>().GetByConditionAsync(new Specifications<Lesson>(l => l.SubjectId == subject.SubjectID));

                //Get all Quiz In Lessons
                var Quizes = await _unitOfWork.GetRepository<Quiz, int>().GetByConditionAsync(new Specifications<Quiz>(q => lessons.Select(l => l.LessonId).Contains(q.LessonId)));
                if (Quizes is null)
                    throw new NotFoundException("No Quizes Found In This Subject");

                //Getting All Ids in Quizes
                var QuizIds = Quizes.Select(q => q.QuizId).ToList();

                //Get From Table QuizStudentResult
                var quizStudentResults = await _unitOfWork.GetRepository<QuizStudent, int>().GetByConditionAsync(new Specifications<QuizStudent>(qs => QuizIds.Contains(qs.QuizId)));

                if (quizStudentResults is null)
                    throw new NotFoundException("No Results Found For This Subject");

                //Groping By StudentId And QuizId to get the latest Attempt
                var results = quizStudentResults.GroupBy(qs => new { qs.StudentId, qs.QuizId })
                    .Select(group => new QuizStudentResultDetailsForSubjectDto
                    {
                        StudentName = group.First().Student.FirstName + " " + group.First().Student.LastName,
                        TotalQuizzes = group.Count(),
                        QuizTitle = group.First().Quiz.Title,
                        LessonTitle = group.First().Quiz.Lesson.Title,
                        PassedQuizzes = group.Count(qs => qs.IsPassed),
                        AverageScore = group.Average(qs => qs.Score),
                        TotalScore = group.Sum(qs => qs.Score)

                    }).ToList();

                //Get all Result Form calss QuizStudent
                return results;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all quiz results of subject .");
                throw;
            }
        }

        //Get All Quiz Statistics In Lesson By Lesson Code [Teacher]
        public async Task<List<QuizStudentResultInLesson>> GetAllQuizStatisticsInLessonByCodeAsync(string LessonCode, string email)
        {
            try
            {
                //caheck if LessonCode is null or empty
                if (string.IsNullOrEmpty(LessonCode))
                    throw new NullRefrenceException("Lesson code cannot be null or empty.");

                // Check if the user is a teacher
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !await _userManager.IsInRoleAsync(user, "Teacher"))
                    throw new UnauthorizedAccessException("Only teachers can view quiz results.");
                //Get Subject By LessonCode
                var subject = _unitOfWork.GetRepository<Subject, int>().GetByConditionAsync(new Specifications<Subject>(s => s.Lessons.Any(l => l.Code == LessonCode))).Result.FirstOrDefault();

                //check if Subject Exist 
                if (subject is null)
                    throw new NotFoundException("Lesson code Or Lesson Doesn't Exist Try Again .");

                // cheack that Teacher is Owner of Subject
                if (subject.TeacherId != user.Id)
                    throw new UnauthorizedAccessException("You do not have permission to view quiz results for this subject.");
                // Get all lessons associated with the subject
                var lessons = await _unitOfWork.GetRepository<Lesson, int>().GetEntityWithCode<Lesson>(LessonCode);


                //Getting Id Of Quizes
                var QuizId = lessons.quiz.QuizId;

                //Get From Table QuizStudentResult
                var quizStudentResults = await _unitOfWork.GetRepository<QuizStudent, int>().GetByConditionAsync(new Specifications<QuizStudent>(qs => QuizId == qs.QuizId));

                if (quizStudentResults is null)
                    throw new NotFoundException("No Results Found For This Lesson");

                //Groping By StudentId And QuizId to get the latest Attempt
                var results = quizStudentResults
                    .Select(res => new QuizStudentResultInLesson
                    {
                        StudentName = res.Student.FirstName + " " + res.Student.LastName,
                        TakeTime = res.TakenAt,
                        Score = res.Score,
                        IsPassed = res.IsPassed,
                    }).ToList();

                //Get all Result Form calss QuizStudent
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all quiz results of subject .");
                throw;
            }

        }

        //Get Quiz Data And QuestionWith Answers [For Teacher And Student]
        public async Task<QuizResponseDto> GetQuizByCodeAsync(string QuizCode)
        {
            try
            {
                if (string.IsNullOrEmpty(QuizCode))
                    throw new NullReferenceException("Quiz code cannot be null or empty");

                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz == null)
                    throw new NotFoundException("Quiz not found. Please try a valid quiz code.");

                // Load questions with answers
                var questions = await _unitOfWork.GetRepository<Question, int>()
                    .GetAllAsync();

                var questionDtos = questions.Select(q => new QuestionDto
                {
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    QuestionCode = q.Code,
                    Points = q.Points,
                    Answers = q.Answers.Select(c => new AnswerDtoResponse
                    {
                        AnswerCode = c.Code,
                        AnswerText = c.AnswerText,
                        IsCorrect = c.IsCorrect,
                    }).ToList(),

                    Quiz = q.Quiz
                }).ToList();

                var quizResponse = new QuizResponseDto
                {
                    QuizCode = quiz.Code,
                    Title = quiz.Title,
                    Description = quiz.Description,
                    TotalMarks = quiz.TotalMarks,
                    LessonId = quiz.LessonId,
                    Questions = questionDtos
                };

                return quizResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz {QuizCode}", QuizCode);
                throw;
            }
        }

        //Get All Student Attempts [Student]
        public async Task<List<QuizStudentResultInLesson>> GetAllStudentAttemptsAsync(string StudentUserName, string QuizCode)
        {
            try
            {
                if (string.IsNullOrEmpty(StudentUserName) || string.IsNullOrEmpty(QuizCode))
                    throw new NullReferenceException("Student username or quiz code cannot be null or empty");

                // Get student
                var student = await _userManager.FindByEmailAsync(StudentUserName); //check by email
                if (student == null)
                    throw new NotFoundException("Student not found. Please try a valid username.");

                // Get quiz
                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz == null)
                    throw new NotFoundException("Quiz not found. Please try a valid quiz code.");

                // Get all attempts for this student and quiz
                var quizStudents = await _unitOfWork.GetRepository<QuizStudent, string>()
                    .GetByConditionAsync(new Specifications<QuizStudent>(qs => qs.StudentId == student.Id && qs.QuizId == quiz.QuizId));

                var results = quizStudents.Select(qs => new QuizStudentResultInLesson
                {
                    StudentName = student.FirstName + " " + student.LastName,
                    TakeTime = qs.TakenAt,
                    Score = qs.Score,
                    IsPassed = qs.IsPassed
                }).ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student attempts for {StudentUserName} on quiz {QuizCode}", StudentUserName, QuizCode);
                throw;
            }
        }

        //Submit a quiz attempt
        public async Task<QuizStudentResultInLesson> SubmitQuizAttemptAsync(string QuizCode, string StudentUserName, List<AnswerSubmissionDto> answers)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(QuizCode) || string.IsNullOrEmpty(StudentUserName) || answers == null || !answers.Any())
                    throw new NullReferenceException("Quiz code, student username, and answers cannot be null or empty");
                // Get student
                var student = await _userManager.FindByEmailAsync(StudentUserName);
                if (student == null)
                    throw new NotFoundException("Student not found. Please try a valid username.");
                // Get quiz
                var quiz = await _unitOfWork.GetRepository<Quiz, int>().GetEntityWithCode<Quiz>(QuizCode);
                if (quiz == null)
                    throw new NotFoundException("Quiz not found. Please try a valid quiz code.");
                // Get questions for the quiz
                var questions = await _unitOfWork.GetRepository<Question, int>()
                    .GetByConditionAsync(new Specifications<Question>(q => q.QuizId == quiz.QuizId));
                if (questions == null || !questions.Any())
                    throw new NotFoundException("No questions found for this quiz.");
                // Calculate score
                int totalScore = 0;
                foreach(var question in questions)
                {
                    var submittedAnswer = answers.FirstOrDefault(a => a.QuestionCode == question.Code);
                    if(submittedAnswer is not null)
                    {
                        var correctAnswer = question.Answers.Where(a => a.IsCorrect).Select(a=>a.Code).FirstOrDefault();
                        if (correctAnswer is not null && submittedAnswer.SelectedAnswerCode is not null && submittedAnswer.SelectedAnswerCode==correctAnswer)
                        {
                            totalScore += (int)question.Points!;
                        }

                    }
                }
                // Determine if passed (assuming passing score is 50%)
                bool isPassed = totalScore >= (quiz.TotalMarks / 2);
                // Create a new QuizStudent 
                var quizStudent = new QuizStudent
                {
                    QuizId = quiz.QuizId,
                    StudentId = student.Id,
                    Score = totalScore,
                    IsPassed = isPassed,
                    TakenAt = DateTime.UtcNow.ToLocalTime()
                };

                _unitOfWork.GetRepository<QuizStudent, int>().AddAsync(quizStudent);
                var res = await _unitOfWork.SaveChanges();
                if (res <= 0)
                    throw new DatabaseException("Error submitting quiz attempt");
                // Return result
                return new QuizStudentResultInLesson
                {
                    StudentName = student.FirstName + " " + student.LastName,
                    TakeTime = quizStudent.TakenAt,
                    Score = quizStudent.Score,
                    IsPassed = quizStudent.IsPassed
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting quiz attempt for {StudentUserName} on quiz {QuizCode}", StudentUserName, QuizCode);
                throw;
            }
        }
    }
}
