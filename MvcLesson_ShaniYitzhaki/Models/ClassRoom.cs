using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public string ClassName { get; set; }

        //[ValidateNever]
        public ICollection<Student> Students { get; set; }
    }
}