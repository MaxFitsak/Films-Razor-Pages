using FilmMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FIlmsRazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly FilmContext _context;

    public IndexModel(FilmContext context)
    {
        _context = context;
    }
    
    public IList<Film> Films { get;set; }
    
    public async Task OnGetAsync()
    {
        Films = await _context.Films.AsNoTracking().ToListAsync();
    }
}