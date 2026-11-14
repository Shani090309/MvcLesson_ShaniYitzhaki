using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }

        //[ValidateNever] 
        public ICollection<Student> Students { get; set; }
    }
}