using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;
    public DetailsModel(AppDbContext db) => _db = db;

    public WorkOrder? WorkOrder { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // Find by PK; 404 if missing to align with REST semantics
        WorkOrder = await _db.WorkOrders.FindAsync(id);
        if (WorkOrder == null) return NotFound();
        return Page();
    }
}
