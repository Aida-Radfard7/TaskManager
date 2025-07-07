using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManager.Core.ApplicationService;
using TaskManager.Core.Contracts.Interfaces;
using TaskManager.Core.Domain.Entities;
using TaskManager.Core.Domain.Repositories;
using TaskManager.Infra.Data.EF.SqlServer;

namespace TaskManager.Endpoints.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            builder.Services.AddDbContext<TaskDbContext>(options => options.UseSqlServer("Server=AIDA\\SQLEXPRESS;Database=TaskManagerNikamooz;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"));

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();


            app.Run(async (context) =>
            {
                var taskService = context.RequestServices.GetRequiredService<ITaskService>();

                if (context.Request.Method == "GET")
                {
                    if (context.Request.Path.StartsWithSegments("/"))
                    {
                        context.Response.ContentType = "text/html";
                        var tasks = await taskService.GetAllAsync();
                        foreach (var task in tasks)
                        {
                            await context.Response.WriteAsync($"{task.Id} : Title:{task.Title} - Description:{task.Description}<br/><br/>");

                        }
                    }
                }
                else if (context.Request.Method == "POST")
                {
                    if (context.Request.Path.StartsWithSegments("/tasks"))
                    {
                        StreamReader streamReader = new StreamReader(context.Request.Body);
                        string body = await streamReader.ReadToEndAsync();
                        TaskItem task = JsonSerializer.Deserialize<TaskItem>(body);
                        if (task != null)
                        {
                            await taskService.AddAsync(task);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    }
                }
                else if (context.Request.Method == "PUT")
                {
                    if (context.Request.Path.StartsWithSegments("/cancel"))
                    {
                        if (context.Request.Query.Keys.Contains("Id"))
                        {
                            int id = int.Parse(context.Request.Query["Id"]);
                            await taskService.CancelAsync(id);
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                        }
                    }
                    if (context.Request.Path.StartsWithSegments("/done"))
                    {
                        if (context.Request.Query.Keys.Contains("Id"))
                        {
                            int id = int.Parse(context.Request.Query["Id"]);
                            await taskService.MarkAsDoneAsync(id);
                        }
                        else
                        {
                           context.Response.StatusCode = 400;
                        }
                    }
                }
                
            });

            app.Run();
        }
    }
}
