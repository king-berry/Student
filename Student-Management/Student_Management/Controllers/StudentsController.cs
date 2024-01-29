using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Student_Management.Data;
using Student_Management.Models;

namespace Student_Management.Controllers
{
    public class StudentsController : Controller
    {
        private readonly Student_ManagementContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StudentsController(Student_ManagementContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var student_ManagementContext = _context.Student.Include(s => s.Class);
            return View(await student_ManagementContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            PopulateClassDropDownList();
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Status,Comment,ClassId")] Student student, List<IFormFile> files, string status)
        {
            long size = files.Sum(f => f.Length);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                //Check if the file has a valid extension
                var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file extension. Allowed extensions are: " + string.Join(",", allowedExtensions));
                }

                if (formFile.Length > 0)
                {
                    //change the folder path to where you want to store the upload files
                    var uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadFolderPath);

                    var fileName = Path.GetRandomFileName() + fileExtension;
                    var filePath = Path.Combine(uploadFolderPath, fileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            try
            {
                if (true)
                {
                    if (filePaths.Count > 0)
                    {
                        student.Image = "/uploads/" + Path.GetFileName(filePaths[0]);
                    }
                    if (status == "TookAttendance")
                    {
                        student.Status = true;
                    }
                    else if (status == "NoAttendanceYet")
                    {
                        student.Status = false;
                    }
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
        }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
    PopulateClassDropDownList(student.ClassId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            PopulateClassDropDownList(student.ClassId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, List<IFormFile> files, string status)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentToUpdate = await _context.Student
                .Include(p => p.Class)
                .FirstOrDefaultAsync(p => p.Id == id);
            if(studentToUpdate != null)
            {
            
                if (files != null && files.Count > 0)
                {
                    long size = files.Sum(f => f.Length);

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    var filePaths = new List<string>();
                    foreach (var formFile in files)
                    {
                        //Check if the file has a valid extension
                        var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                        if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                        {
                            return BadRequest("Invalid file extension. Allowed extensions are: " + string.Join(",", allowedExtensions));
                        }

                        if (formFile.Length > 0)
                        {
                            //change the folder path to where you want to store the upload files
                            var uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                            Directory.CreateDirectory(uploadFolderPath);

                            var fileName = Path.GetRandomFileName() + fileExtension;
                            var filePath = Path.Combine(uploadFolderPath, fileName);
                            filePaths.Add(filePath);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                    studentToUpdate.Image = "/uploads/" + Path.GetFileName(filePaths[0]);
                    if (status == "TookAttendance")
                    {
                        studentToUpdate.Status = true;
                    }
                    else if (status == "NoAttendanceYet")
                    {
                        studentToUpdate.Status = false;
                    }
                }
                if (await TryUpdateModelAsync<Student>(studentToUpdate,
                "",
                p => p.Id, p => p.Name, p => p.Image, p => p.Status, p => p.Comment, p => p.ClassId))
                {
                }
                try
                {
                    //_context.Update(studentToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                
            }
            PopulateClassDropDownList(studentToUpdate.ClassId);
            return View(studentToUpdate);
        }

        private void PopulateClassDropDownList(object selectedClass = null)
        {
            var classQuery = from d in _context.Class
                                  orderby d.Name
                                  select d;
            ViewBag.ClassId = new SelectList(classQuery.AsNoTracking(), "Id", "Name", selectedClass);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Class)
                .AsNoTracking()
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
