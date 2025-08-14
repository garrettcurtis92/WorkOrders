using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;
    public DeleteModel(AppDbContext db) => _db = db;

    public WorkOrder? WorkOrder { get; private set; }

    [TempData] public string? Message { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        WorkOrder = await _db.WorkOrders.FindAsync(id);
        if (WorkOrder == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var item = await _db.WorkOrders.FindAsync(id);
        if (item == null) return NotFound();

        _db.WorkOrders.Remove(item);
        await _db.SaveChangesAsync();

        Message = "Work order deleted.";
        return RedirectToPage("Index");
    }
}
