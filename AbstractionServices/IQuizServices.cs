using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractionServices
{
    public  interface IQuizServices
    {
   
    //Part One In this Interface Will BE For Teacher :
        //1- Create a new quiz
        Task<string> CreateQuizAsync(CreateOrUpdateQuizDto quizDto ,string LessonCode);
       
        //2- Delete a quiz by Code
        Task<string> DeleteQuizAsync(string QuizCode ,string email); //emial teacher
        
        //3- Update a quiz by Code
        Task<string> UpdateQuizAsync(string QuizCode, CreateOrUpdateQuizDto quizDto,string email);
        
        //4- Get a quiz by Code [Teacher and Student]
        Task<QuizResponseDto> GetQuizByCodeAsync(string QuizCode);

        //5- Get All Quizes in Subject By Subject Code [Teacher and Student]
        Task<List<QuizShort>> GetAllQuizesInSubjectBySubjectCodeAsync(string SubjectCode);

        //6- Get All Question in Quize by Quiz Code [Teacher and Student]
        Task<List<QuestionDto>> GetAllQuestionsInQuizByQuizCodeAsync(string QuizCode);

        //7- Get Quiz Results by Quiz Code [Teacher]  will return  List Of Sutdent and Marks 
        Task<List<QuizStudentResultDetailsForSubjectDto>> GetAllQuizResultsOfSubjectByCodeAsync(string SubjectCode,string email);

        //8- Get Quiz Statistics by Quiz Code [Teacher]  will return  List Of Sutdent and Marks in Lesson
        Task<List<QuizStudentResultInLesson>> GetAllQuizStatisticsInLessonByCodeAsync(string LessonCode, string email);

        //Part Two In this Interface Will BE For Student :
    
        //1- Submit a quiz attempt
        Task<QuizStudentResultInLesson> SubmitQuizAttemptAsync(string QuizCode, string StudentUserName, List<AnswerSubmissionDto> answers);

        //2- Get All Student Attempts
        Task<List<QuizStudentResultInLesson>> GetAllStudentAttemptsAsync(string StudentUserName ,string QuizCode);


    }
}
