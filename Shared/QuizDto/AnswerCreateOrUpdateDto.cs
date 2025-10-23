using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.QuizDto
{
    public class AnswerCreateOrUpdateDto
    {
        public required string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
