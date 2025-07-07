using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Contracts.Dtos
{
    public class TaskItemDto
    {
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "New";
    }
}
