using Microsoft.EntityFrameworkCore;
using Task = TasksApp.Server.Models.Task;

namespace TasksApp.Server.Data
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options) { }
        public virtual DbSet<Task> Tasks { get; set; }
    }
}
