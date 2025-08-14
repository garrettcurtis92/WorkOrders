using System.ComponentModel.DataAnnotations;

namespace WorkOrders.Models
{
    public enum WorkOrderStatus { New = 0, InProgress = 1, Done = 2 }
    public enum WorkOrderPriority { Low = 0, Medium = 1, High = 2, Critical = 3 }

    public class WorkOrder
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string Department { get; set; } = "General";

        [Required]
        public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Medium;

        [Required]
        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.New;

        [Required, StringLength(80)]
        public string RequestedBy { get; set; } = "Unknown";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
