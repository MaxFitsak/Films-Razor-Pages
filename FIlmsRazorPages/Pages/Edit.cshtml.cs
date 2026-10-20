using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmMVC.Models;
using System.IO;

namespace FilmMVC.Pages
{
    public class EditModel(FilmContext context, IWebHostEnvironment webHostEnvironment) : PageModel
    {
        private readonly FilmContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [BindProperty]
        public Film Film { get; set; } = default!;

        [BindProperty]
        public IFormFile? PhotoFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null) return NotFound();

            var film = await _context.Films.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (film is null) return NotFound();

            Film = film;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id != Film.Id) return NotFound();

            ModelState.Remove("Film.Photo");
            if (!ModelState.IsValid) return Page();

            try
            {
                if (PhotoFile != null && PhotoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photos");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(PhotoFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoFile.CopyToAsync(fileStream);
                    }

                    Film.Photo = uniqueFileName;
                }

                _context.Update(Film);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Films.AnyAsync(e => e.Id == Film.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}