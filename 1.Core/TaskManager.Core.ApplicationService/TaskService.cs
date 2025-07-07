using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Contracts.Dtos;
using TaskManager.Core.Contracts.Interfaces;
using TaskManager.Core.Domain.Entities;
using TaskManager.Core.Domain.Enums;
using TaskManager.Core.Domain.Repositories;

namespace TaskManager.Core.ApplicationService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<TaskItemDto>> GetAllAsync()
        {
            var tasks = await _repository.GetAllAsync();
            return tasks.Select(task => new TaskItemDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString()
            }).ToList();
        }

        public async Task AddAsync(TaskItemDto newTask)
        {
            var task = new TaskItem
            {
                Title = newTask.Title,
                Description = newTask.Description,
                DueDate = newTask.DueDate,
                Status = TaskStatusEnum.New
            };
            await _repository.AddAsync(task);
        }

        public async Task MarkAsDoneAsync(int id) => await _repository.UpdateStatusAsync(id, TaskStatusEnum.Done);
        public async Task CancelAsync(int id) => await _repository.UpdateStatusAsync(id, TaskStatusEnum.Cancelled);
    }
}
