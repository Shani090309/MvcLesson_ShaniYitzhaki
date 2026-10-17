using System.Collections.Generic;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}