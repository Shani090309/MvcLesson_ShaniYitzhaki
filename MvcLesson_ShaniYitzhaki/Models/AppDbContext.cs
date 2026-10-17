using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MvcLesson_ShaniYitzhaki.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace MvcLesson_ShaniYitzhaki.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MvcLessonDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }


}

