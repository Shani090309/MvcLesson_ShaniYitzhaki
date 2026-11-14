using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcLesson_ShaniYitzhaki.Models;

namespace MvcLesson_ShaniYitzhaki.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Teachers.Include(t => t.Students);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            ViewBag.AvailableStudents = new SelectList(_context.Students, "Id", "FirstName", "LastName");
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Subject")] Teacher teacher, int[] selectedStudentIds)
        {
            ModelState.Remove("Students");

            if (ModelState.IsValid)
            {
                teacher.Students = new List<Student>();

                foreach (var studentId in selectedStudentIds)
                {
                    var student = await _context.Students.FindAsync(studentId);
                    if (student != null)
                    {
                        teacher.Students.Add(student);
                    }
                }

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableStudents = new SelectList(_context.Students, "Id", "FirstName", "LastName");
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            var selectedIds = teacher.Students.Select(s => s.Id).ToList();

            var studentsList = _context.Students.Select(s => new
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.LastName
            }).ToList();

            ViewBag.AvailableStudents = new MultiSelectList(studentsList, "Id", "FullName", selectedIds);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Subject")] Teacher teacher, int[] selectedStudentIds)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Students");

            if (ModelState.IsValid)
            {
                try
                {
                    var teacherToUpdate = await _context.Teachers
                        .Include(t => t.Students)
                        .FirstOrDefaultAsync(t => t.Id == id);

                    if (teacherToUpdate == null)
                    {
                        return NotFound();
                    }

                    teacherToUpdate.Name = teacher.Name;
                    teacherToUpdate.Subject = teacher.Subject;

                    teacherToUpdate.Students.Clear();
                    if (selectedStudentIds != null)
                    {
                        foreach (var studentId in selectedStudentIds)
                        {
                            var student = await _context.Students.FindAsync(studentId);
                            if (student != null)
                            {
                                teacherToUpdate.Students.Add(student);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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

            ViewBag.AvailableStudents = new MultiSelectList(_context.Students, "Id", "Name", selectedStudentIds);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
