using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManager.Core.ApplicationService;
using TaskManager.Core.Contracts.Dtos;
using TaskManager.Core.Contracts.Interfaces;
using TaskManager.Core.Domain.Entities;
using TaskManager.Core.Domain.Repositories;
using TaskManager.Endpoints.Rest.Middlewares;
using TaskManager.Infra.Data.EF.SqlServer;
using TaskManager.Infra.Data.EF.SqlServer.Repositories;

namespace TaskManager.Endpoints.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            builder.Services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<IRequestLogRepository, RequestLogRepository>();

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("nationalCode", typeof(CustomConstraints.NationalCodeConstraint));
            });

            var app = builder.Build();

            app.UseMiddleware<RequestLogMiddleware>();
            /*app.Run(async (context) =>
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
                    if (context.Request.Path.StartsWithSegments("/addTask"))
                    {
                        StreamReader streamReader = new StreamReader(context.Request.Body);
                        string body = await streamReader.ReadToEndAsync();
                        TaskItemDto task = JsonSerializer.Deserialize<TaskItemDto>(body);
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
                    if (context.Request.Path.StartsWithSegments("/cancelTask"))
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
                    if (context.Request.Path.StartsWithSegments("/doneTask"))
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
                
            });*/

            app.UseRouting();
            app.MapGet("{nationalCode:nationalCode}", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("national code is valid");
            });

            app.Run();
        }
    }
}
