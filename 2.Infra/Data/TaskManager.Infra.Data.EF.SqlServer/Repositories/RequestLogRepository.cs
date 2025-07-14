using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain.Entities;

namespace TaskManager.Infra.Data.EF.SqlServer.Repositories
{
    public interface IRequestLogRepository
    {
        Task SaveLogAsync(RequestLog log);
    }

    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly TaskDbContext _context;

        public RequestLogRepository(TaskDbContext context)
        {
            _context = context;
        }
        public async Task SaveLogAsync(RequestLog log)
        {
            _context.RequestLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
