using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WorkOrders.Data;
using WorkOrders.Models;

namespace WorkOrders.Pages.WorkOrders;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public IndexModel(AppDbContext db) => _db = db;

    public List<WorkOrder> Items { get; private set; } = new();
    public List<string> Departments { get; private set; } = new();

    [TempData] public string? Message { get; set; }

    // FILTERS (from query string)
    [BindProperty(SupportsGet = true)] public WorkOrderStatus? Status { get; set; }
    [BindProperty(SupportsGet = true)] public WorkOrderPriority? Priority { get; set; }
    [BindProperty(SupportsGet = true)] public string? Department { get; set; }
    [BindProperty(SupportsGet = true)] public string? q { get; set; }

    // NEW: sorting + pagination inputs
    [BindProperty(SupportsGet = true)] public string? Sort { get; set; }   // title, dept, priority, status, updated
    [BindProperty(SupportsGet = true)] public string? Dir { get; set; }    // asc or desc
    [BindProperty(SupportsGet = true)] public int page { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public int pageSize { get; set; } = 10;

    public int Total { get; private set; }
    public int TotalPages => (int)Math.Ceiling(Total / (double)pageSize);
    public int StartItem => Total == 0 ? 0 : ((page - 1) * pageSize) + 1;
    public int EndItem => Total == 0 ? 0 : ((page - 1) * pageSize) + Items.Count;

    public async Task OnGetAsync()
    {
        // Build Department dropdown
        Departments = await _db.WorkOrders
            .AsNoTracking()
            .Select(w => w.Department)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync();

        // Base query
        var query = _db.WorkOrders.AsNoTracking().AsQueryable();

        // Filters
        if (Status.HasValue)   query = query.Where(w => w.Status == Status.Value);
        if (Priority.HasValue) query = query.Where(w => w.Priority == Priority.Value);
        if (!string.IsNullOrWhiteSpace(Department)) query = query.Where(w => w.Department == Department);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(w => w.Title.Contains(q) || w.Description.Contains(q));

        // Defaults
        Sort ??= "updated";
        Dir ??= "desc";

        // Sorting
        (string s, string d) key = (Sort.ToLower(), Dir.ToLower());
        query = key switch
        {
            ("title", "asc")      => query.OrderBy(w => w.Title),
            ("title", "desc")     => query.OrderByDescending(w => w.Title),
            ("dept", "asc")       => query.OrderBy(w => w.Department),
            ("dept", "desc")      => query.OrderByDescending(w => w.Department),
            ("priority", "asc")   => query.OrderBy(w => w.Priority).ThenByDescending(w => w.UpdatedAt),
            ("priority", "desc")  => query.OrderByDescending(w => w.Priority).ThenByDescending(w => w.UpdatedAt),
            ("status", "asc")     => query.OrderBy(w => w.Status).ThenByDescending(w => w.UpdatedAt),
            ("status", "desc")    => query.OrderByDescending(w => w.Status).ThenByDescending(w => w.UpdatedAt),
            ("updated", "asc")    => query.OrderBy(w => w.UpdatedAt),
            ("updated", "desc")   => query.OrderByDescending(w => w.UpdatedAt),
            ("created", "asc")    => query.OrderBy(w => w.CreatedAt),
            ("created", "desc")   => query.OrderByDescending(w => w.CreatedAt),
            _                      => query.OrderByDescending(w => w.UpdatedAt)
        };

        // Total count BEFORE pagination
        Total = await query.CountAsync();

        // Clamp page values
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 200) pageSize = 200;
        if (page < 1) page = 1;
        var maxPage = TotalPages;
        if (page > maxPage && maxPage > 0) page = maxPage;

        // Pagination
        Items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
