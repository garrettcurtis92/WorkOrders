using Microsoft.EntityFrameworkCore;
using WorkOrders.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));



var app = builder.Build();
// Create DB and seed sample data at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (!db.WorkOrders.Any())
    {
        db.WorkOrders.AddRange(new[]
        {
            new WorkOrders.Models.WorkOrder { Title = "Replace light bulbs in hallway", Description = "3 bulbs out on 2nd floor", Department = "Facilities", Priority = WorkOrders.Models.WorkOrderPriority.Medium, Status = WorkOrders.Models.WorkOrderStatus.New, RequestedBy = "J. Smith" },
            new WorkOrders.Models.WorkOrder { Title = "Laptop won’t boot", Description = "Dell XPS stuck on logo", Department = "IT", Priority = WorkOrders.Models.WorkOrderPriority.High, Status = WorkOrders.Models.WorkOrderStatus.New, RequestedBy = "A. Nguyen" },
            new WorkOrders.Models.WorkOrder { Title = "Printer jam in Admin", Description = "Xerox C8145 jams every 3 pages", Department = "Admin", Priority = WorkOrders.Models.WorkOrderPriority.Medium, Status = WorkOrders.Models.WorkOrderStatus.InProgress, RequestedBy = "R. Lee" },
            new WorkOrders.Models.WorkOrder { Title = "Door access card issue", Description = "Badge not opening west entrance", Department = "Security", Priority = WorkOrders.Models.WorkOrderPriority.High, Status = WorkOrders.Models.WorkOrderStatus.New, RequestedBy = "M. Patel" },
            new WorkOrders.Models.WorkOrder { Title = "Request extra recycling bins", Description = "Need 2 more on 4th floor", Department = "Facilities", Priority = WorkOrders.Models.WorkOrderPriority.Low, Status = WorkOrders.Models.WorkOrderStatus.Done, RequestedBy = "T. Garcia" },
            new WorkOrders.Models.WorkOrder { Title = "Software install: ArcGIS", Description = "License approved, please install", Department = "Planning", Priority = WorkOrders.Models.WorkOrderPriority.Medium, Status = WorkOrders.Models.WorkOrderStatus.New, RequestedBy = "K. Wilson" },
            new WorkOrders.Models.WorkOrder { Title = "Email phishing report", Description = "Suspicious message sent to 20 users", Department = "IT Security", Priority = WorkOrders.Models.WorkOrderPriority.Critical, Status = WorkOrders.Models.WorkOrderStatus.InProgress, RequestedBy = "SOC" },
            new WorkOrders.Models.WorkOrder { Title = "Conference room A/V issue", Description = "HDMI input not detected", Department = "IT", Priority = WorkOrders.Models.WorkOrderPriority.Medium, Status = WorkOrders.Models.WorkOrderStatus.New, RequestedBy = "Front Desk" }
        });

        await db.SaveChangesAsync();
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
