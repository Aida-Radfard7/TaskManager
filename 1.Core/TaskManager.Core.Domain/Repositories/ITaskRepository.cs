using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain.Entities;
using TaskManager.Core.Domain.Enums;

namespace TaskManager.Core.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem newTask);
        Task UpdateStatusAsync(int id, TaskStatusEnum status);
    }
}
