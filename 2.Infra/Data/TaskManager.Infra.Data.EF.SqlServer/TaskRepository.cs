using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Contracts.Dtos;
using TaskManager.Core.Domain.Entities;
using TaskManager.Core.Domain.Enums;
using TaskManager.Core.Domain.Repositories;

namespace TaskManager.Infra.Data.EF.SqlServer
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllAsync() {
            List<TaskItem> taskLst =  await _context.Tasks.OrderBy(t => t.Id).ToListAsync();
            return taskLst;
        }

        public async Task AddAsync(TaskItem newTask) {
            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();
        }
       
        public async Task UpdateStatusAsync(int id, TaskStatusEnum status) {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return;

            task.Status = status;
            await _context.SaveChangesAsync();
        }
    }
}
