using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lesson_Dto
{
    public  class UploadFileDto
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Matrial { set; get; } = null!;


    }
}
