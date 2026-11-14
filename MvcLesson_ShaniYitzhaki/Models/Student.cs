using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GradeLevel { get; set; }

        public int? ClassRoomId { get; set; }

        [ValidateNever]
        public ClassRoom ClassRoom { get; set; }

        //public int? TeacherId { get; set; }
        //public Teacher Teacher { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
