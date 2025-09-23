using Shared.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SubjectDtos
{
    public class LessonDetailsResponseDto
    {
        public string LessonCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? MaterialUrl { get; set; }
        public int Order { get; set; }  //Order of the lesson in the subject
        public QuizShort? Quiz { get; set; } // Navigation property to Quiz
        //Relationships
        public SubjectResponseShortDto Subject { get; set; } = null!;  // Navigation to Subject

    }
}
