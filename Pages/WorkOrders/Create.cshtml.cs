using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;
    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public WorkOrder WorkOrder { get; set; } = new();

    [TempData] public string? Message { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page(); // show validation errors
        }

        _db.WorkOrders.Add(WorkOrder);
        await _db.SaveChangesAsync();
        Message = "Work order created.";
        return RedirectToPage("Index");
    }
}
