using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Contracts.Dtos;
using TaskManager.Core.Domain.Entities;

namespace TaskManager.Core.Contracts.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskItemDto>> GetAllAsync();
        Task AddAsync(TaskItemDto newTask);
        Task MarkAsDoneAsync(int id);
        Task CancelAsync(int id);
    }
}
