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
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Students)
                .ToListAsync();

            return View(courses);
        }


        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Students) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewBag.Students = _context.Students.ToList();
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Course course, int[] selectedStudents)
        {
            if (ModelState.IsValid)
            {
                if (selectedStudents != null)
                {
                    foreach (var studentId in selectedStudents)
                    {
                        var student = await _context.Students.FindAsync(studentId);
                        if (student != null)
                        {
                            course.Students.Add(student);
                        }
                    }
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Students = _context.Students.ToList();
            return View(course);
        }

        // GET: Courses/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            ViewBag.AllStudents = await _context.Students.ToListAsync();
            return View(course);
        }
        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Course course, int[] selectedStudents)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var courseToUpdate = await _context.Courses
                        .Include(c => c.Students)
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (courseToUpdate == null)
                    {
                        return NotFound();
                    }

                    courseToUpdate.Title = course.Title;

                    courseToUpdate.Students.Clear();
                    if (selectedStudents != null)
                    {
                        foreach (var studentId in selectedStudents)
                        {
                            var student = await _context.Students.FindAsync(studentId);
                            if (student != null)
                            {
                                courseToUpdate.Students.Add(student);
                            }
                        }
                    }

                    _context.Update(courseToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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

            ViewBag.AllStudents = await _context.Students.ToListAsync();
            return View(course);
        }

        private bool CourseExists(int id)
        {
            throw new NotImplementedException();
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
