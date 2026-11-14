using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcLesson_ShaniYitzhaki.Models;
using NuGet.Protocol;

namespace MvcLesson_ShaniYitzhaki.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.ClassRoom)
                .Include(s => s.Teachers)
                .Include(s => s.Courses)
                .ToListAsync();

            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.ClassRoom)
                .Include(s => s.Teachers)
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AllClassRooms = await _context.ClassRooms.ToListAsync();
            ViewBag.AllTeachers = await _context.Teachers.ToListAsync();
            ViewBag.AllCourses = await _context.Courses.ToListAsync();
            return View();
        }
        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,GradeLevel")] Student student, int classRoomId, int[] selectedTeachers, int[] selectedCourses)
        {
            if (classRoomId > 0)
            {
                student.ClassRoomId = classRoomId;
            }

            if (selectedCourses != null && selectedCourses.Length > 0)
            {
                student.Courses = new List<Course>();
                foreach (var courseId in selectedCourses)
                {
                    var course = await _context.Courses.FindAsync(courseId);
                    if (course != null)
                    {
                        student.Courses.Add(course);
                    }
                }
            }


            if (selectedTeachers != null && selectedTeachers.Length > 0)
            {
                student.Teachers = new List<Teacher>();
                foreach (var TeacherId in selectedTeachers)
                {
                    var teacher = await _context.Teachers.FindAsync(TeacherId);
                    if (teacher != null)
                    {
                        student.Teachers.Add(teacher);
                    }
                }
            }

            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.ClassRoom)
                .Include(s => s.Teachers)
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.AllClassRooms = await _context.ClassRooms.ToListAsync();
            ViewBag.AllTeachers = await _context.Teachers.ToListAsync();
            ViewBag.AllCourses = await _context.Courses.ToListAsync();
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,GradeLevel,ClassRoomId")] Student student, int classRoomId, int[] selectedTeachers, int[] selectedCourses)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var studentToUpdate = await _context.Students
                        .Include(s => s.ClassRoom)
                        .Include(s => s.Teachers)
                        .Include(s => s.Courses)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (studentToUpdate == null)
                    {
                        return NotFound();
                    }

                    studentToUpdate.FirstName = student.FirstName;
                    studentToUpdate.LastName = student.LastName;
                    studentToUpdate.GradeLevel = student.GradeLevel;
                    studentToUpdate.ClassRoomId = classRoomId > 0 ? classRoomId : (int?)null;

                    studentToUpdate.Teachers.Clear();
                    if (selectedTeachers != null && selectedTeachers.Length > 0)
                    {
                        foreach (var teacherId in selectedTeachers)
                        {
                            var teacher = await _context.Teachers.FindAsync(teacherId);
                            if (teacher != null)
                            {
                                studentToUpdate.Teachers.Add(teacher);
                            }
                        }
                    }

                    studentToUpdate.Courses.Clear();
                    if (selectedCourses != null && selectedCourses.Length > 0)
                    {
                        foreach (var courseId in selectedCourses)
                        {
                            var course = await _context.Courses.FindAsync(courseId);
                            if (course != null)
                            {
                                studentToUpdate.Courses.Add(course);
                            }
                        }
                    }

                    _context.Update(studentToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllClassRooms = await _context.ClassRooms.ToListAsync();
            ViewBag.AllTeachers = await _context.Teachers.ToListAsync();
            ViewBag.AllCourses = await _context.Courses.ToListAsync();
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.ClassRoom)
                .Include(s => s.Teachers)
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
