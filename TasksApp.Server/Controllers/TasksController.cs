using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksApp.Server.Data;
using Task = TasksApp.Server.Models.Task;

namespace TasksApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TasksDbContext _tasksDbContext;

        public TasksController(TasksDbContext context)
        {
            _tasksDbContext = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            return await _tasksDbContext.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            var task = await _tasksDbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _tasksDbContext.Entry(task).State = EntityState.Modified;

            try
            {
                await _tasksDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            _tasksDbContext.Tasks.Add(task);
            await _tasksDbContext.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _tasksDbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _tasksDbContext.Tasks.Remove(task);
            await _tasksDbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _tasksDbContext.Tasks.Any(e => e.Id == id);
        }
    }
}
