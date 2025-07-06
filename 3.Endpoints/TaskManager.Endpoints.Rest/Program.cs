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
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync($"{context.Request.Method}\r\n");
                        await context.Response.WriteAsync($"{context.Request.Path}\r\n");
                        foreach (var item in context.Request.Headers)
                        {
                            await context.Response.WriteAsync($"Key:{item.Key} Value:{item.Value}\r\n");
                        }
                    }
                    else if (context.Request.Path.StartsWithSegments("/tasks"))
                    {
                        context.Response.ContentType = "text/html";
                        await context.Response.WriteAsync($"{context.Request.Method}<br/>");
                        await context.Response.WriteAsync($"{context.Request.Path}<br/>");
                        var tasks = await taskService.GetAllAsync();
                        foreach (var task in tasks)
                        {
                            await context.Response.WriteAsync($"Id:{task.Id}");

                        }
                        context.Response.StatusCode = 200;
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
                                taskService.AddAsync(task);
                                context.Response.StatusCode = 200;
                            }
                            else
                            {
                                context.Response.StatusCode = 400;
                            }
                        }
                    }
                    else if (context.Request.Method == "PUT")
                    {
                        if (context.Request.Path.StartsWithSegments("/cancel-task"))
                        {
                            if (context.Request.Query.Keys.Contains("Id"))
                            {
                                int id = int.Parse(context.Request.Query["Id"]);
                                taskService.CancelAsync(id);
                                context.Response.StatusCode = 200;
                            }
                            else
                            {
                                context.Response.StatusCode = 400;
                            }
                        }
                        if (context.Request.Path.StartsWithSegments("/done-task"))
                        {
                            if (context.Request.Query.Keys.Contains("Id"))
                            {
                                int id = int.Parse(context.Request.Query["Id"]);
                                taskService.MarkAsDoneAsync(id);
                                context.Response.StatusCode = 200;
                            }
                            else
                            {
                                context.Response.StatusCode = 400;
                            }
                        }
                    }
                }
            });

            app.Run();
        }
    }
}
