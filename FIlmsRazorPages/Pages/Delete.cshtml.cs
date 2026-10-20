using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmMVC.Models;

namespace FilmMVC.Pages
{
    public class DeleteModel(FilmContext context) : PageModel
    {
        private readonly FilmContext _context = context;

        [BindProperty]
        public Film Film { get; set; } = default!;

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
            var film = await _context.Films.FindAsync(id);
            if (film is not null)
            {
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}