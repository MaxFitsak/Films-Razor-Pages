using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FilmMVC.Models;

namespace FilmMVC.Pages
{
    public class CreateModel(FilmContext context, IWebHostEnvironment webHostEnvironment) : PageModel
    {
        private readonly FilmContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        [BindProperty]
        public Film Film { get; set; } = default!;

        [BindProperty]
        public IFormFile? PhotoFile { get; set; }

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Film.Photo");

            if (!ModelState.IsValid) return Page();

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

            _context.Films.Add(Film);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}