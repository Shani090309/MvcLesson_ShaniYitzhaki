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
    public class ClassRoomsController : Controller
    {
        private readonly AppDbContext _context;

        public ClassRoomsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ClassRooms
        public async Task<IActionResult> Index()
        {
            var classRooms = await _context.ClassRooms
            .Include(c => c.Students)
            .ToListAsync();
            return View(classRooms);
        }

        // GET: ClassRooms/Details/5
        // GET: ClassRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _context.ClassRooms
                .Include(c => c.Students) // <-- חשוב מאוד לכלול את התלמידים כאן!
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // GET: ClassRooms/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AllStudents = await _context.Students.ToListAsync();
            return View();
        }

        // POST: ClassRooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassName")] ClassRoom classRoom, int[] selectedStudents)
        {
            classRoom.Students = new List<Student>();

            if (selectedStudents != null && selectedStudents.Length > 0)
            {
                foreach (var id in selectedStudents)
                {
                    var student = await _context.Students.FindAsync(id);
                    if (student != null)
                    {
                        classRoom.Students.Add(student);
                    }
                }
            }

            _context.Add(classRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ClassRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _context.ClassRooms
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classRoom == null)
            {
                return NotFound();
            }

            ViewBag.AllStudents = await _context.Students.ToListAsync();
            return View(classRoom);
        }

        // POST: ClassRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassName")] ClassRoom classRoom, int[] selectedStudents)
        {
            if (id != classRoom.Id)
            {
                return NotFound();
            }

            var classToUpdate = await _context.ClassRooms
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classToUpdate == null)
            {
                return NotFound();
            }

            classToUpdate.ClassName = classRoom.ClassName;

            foreach (var student in _context.Students.ToList())
            {
                if (selectedStudents != null && selectedStudents.Contains(student.Id))
                {
                    if (!classToUpdate.Students.Any(s => s.Id == student.Id))
                    {
                        classToUpdate.Students.Add(student);
                    }
                }
                else
                {
                    if (classToUpdate.Students.Any(s => s.Id == student.Id))
                    {
                        classToUpdate.Students.Remove(student);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassRoomExists(classRoom.Id))
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

        // GET: ClassRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _context.ClassRooms
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // POST: ClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classRoom = await _context.ClassRooms
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classRoom != null)
            {
                classRoom.Students.Clear();
                _context.ClassRooms.Remove(classRoom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassRoomExists(int id)
        {
            return _context.ClassRooms.Any(e => e.Id == id);
        }
    }
}
