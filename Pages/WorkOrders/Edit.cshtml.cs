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
        var item = await _db.WorkOrders.FindAsync(id);
        if (item == null) return NotFound();
        WorkOrder = item;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var exists = await _db.WorkOrders.AnyAsync(w => w.Id == WorkOrder.Id);
        if (!exists) return NotFound();

        _db.Attach(WorkOrder).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        Message = "Work order updated.";
        return RedirectToPage("Index");
    }
}
