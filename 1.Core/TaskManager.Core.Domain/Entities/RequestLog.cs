using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Domain.Entities
{
    public class RequestLog
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
    }
}
