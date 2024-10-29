using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Controllers;

[Route("api/[controller]")]
[ApiController]
internal class TaskController : ControllerBase {
    private readonly TaskManagerContext _context;

    internal TaskController(TaskManagerContext context) {
        _context = context;
    }

    // GET: api/task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Task>>> GetTasks() {
        return await _context.Tasks.Include(t => t.Person).ToListAsync();
    }

    // GET: api/task/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Task>> GetTask(int id) {
        var task = await _context.Tasks.Include(t => t.Person).FirstOrDefaultAsync(t => t.Id == id);

        if(task == null) {
            return NotFound();
        }

        return task;
    }

    // POST: api/task
    [HttpPost]
    public async Task<ActionResult<Task>> PostTask(Task task) {
        if(!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: api/task/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id, Task task) {
        if(id != task.Id || !ModelState.IsValid) {
            return BadRequest();
        }

        _context.Entry(task).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException) {
            if(!TaskExists(id)) {
                return NotFound();
            }
            else {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/task/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id) {
        var task = await _context.Tasks.FindAsync(id);

        if(task == null) {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Associate a person with a task
    [HttpPost("{taskId}/person/{personId}")]
    public async Task<IActionResult> AssignPerson(int taskId, int personId) {
        var task = await _context.Tasks.FindAsync(taskId);
        var person = await _context.Persons.FindAsync(personId);

        if(task == null || person == null) {
            return NotFound();
        }

        task.PersonId = personId; // Assign person to the task
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Remove the person assignment from a task
    [HttpDelete("{taskId}/person")]
    public async Task<IActionResult> RemovePerson(int taskId) {
        var task = await _context.Tasks.FindAsync(taskId);

        if(task == null) {
            return NotFound();
        }

        task.PersonId = null; // Remove the person assignment
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskExists(int id) {
        return _context.Tasks.Any(e => e.Id == id);
    }
}