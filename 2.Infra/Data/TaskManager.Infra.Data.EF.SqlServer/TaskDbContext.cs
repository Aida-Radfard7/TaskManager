using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain.Entities;

namespace TaskManager.Infra.Data.EF.SqlServer
{
    public class TaskDbContext : DbContext 
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options):base(options) {}

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.Property(entity => entity.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(entity => entity.Status).IsRequired();
            });
        }
    }
}
