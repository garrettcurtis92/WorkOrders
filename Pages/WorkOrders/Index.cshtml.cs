using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class WorkOrdersIndexModel : PageModel
{
    private readonly AppDbContext _db;

    public WorkOrdersIndexModel(AppDbContext db) => _db = db;

    public List<WorkOrder> Items { get; private set; } = new();

    [Microsoft.AspNetCore.Mvc.TempData] public string? Message { get; set; }

    public async Task OnGetAsync()
    {
        Items = await _db.WorkOrders
            .AsNoTracking()
            .OrderByDescending(w => w.UpdatedAt)
            .ToListAsync();
    }
}