using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //[ValidateNever]
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}