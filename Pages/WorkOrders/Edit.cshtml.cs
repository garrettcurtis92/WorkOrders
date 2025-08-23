using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;
    public EditModel(AppDbContext db) => _db = db;

    [BindProperty]
    public WorkOrder WorkOrder { get; set; } = new();

    [TempData] public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // Load existing entity for editing; 404 if not found (prevents misleading blank form)
        var item = await _db.WorkOrders.FindAsync(id);
        if (item == null) return NotFound();
        WorkOrder = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Validate input first
        if (!ModelState.IsValid) return Page();

        // Ensure entity still exists (handles race where it was deleted between GET + POST)
        var exists = await _db.WorkOrders.AnyAsync(w => w.Id == WorkOrder.Id);
        if (!exists) return NotFound();

        // Attach and mark modified so EF updates all scalar properties
        _db.Attach(WorkOrder).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        Message = "Work order updated.";
        return RedirectToPage("Index");
    }
}
