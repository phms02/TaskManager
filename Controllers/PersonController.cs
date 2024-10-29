using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers;

[Route("api/[controller]")]
[ApiController]
internal class PersonController : ControllerBase {
    private readonly TaskManagerContext _context;

    internal PersonController(TaskManagerContext context) {
        _context = context;
    }

    // GET: api/person
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetPersons() {
        return await _context.Persons.ToListAsync();
    }

    // GET: api/person/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(int id) {
        var person = await _context.Persons.FindAsync(id);

        if(person == null) {
            return NotFound();
        }

        return person;
    }

    // POST: api/person
    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(Person person) {
        if(!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
    }

    // PUT: api/person/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(int id, Person person) {
        if(id != person.Id || !ModelState.IsValid) {
            return BadRequest();
        }

        _context.Entry(person).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException) {
            if(!PersonExists(id)) {
                return NotFound();
            }
            else {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/person/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id) {
        var person = await _context.Persons.FindAsync(id);

        if(person == null) {
            return NotFound();
        }

        // Verifica se há tarefas pendentes associadas a esta pessoa
        if(await _context.Tasks.AnyAsync(t => t.PersonId == id && t.Status == "pending")) {
            return Conflict("Cannot delete person with pending tasks.");
        }

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Associate a task with a person
    [HttpPost("{personId}/tasks/{taskId}")]
    public async Task<IActionResult> AssignTask(int personId, int taskId) {
        var person = await _context.Persons.FindAsync(personId);
        var task = await _context.Tasks.FindAsync(taskId);

        if(person == null || task == null) {
            return NotFound();
        }

        task.PersonId = personId;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Remove a task from a person
    [HttpDelete("{personId}/tasks/{taskId}")]
    public async Task<IActionResult> RemoveTask(int personId, int taskId) {
        var task = await _context.Tasks.FindAsync(taskId);

        if(task == null || task.PersonId != personId) {
            return NotFound();
        }

        task.PersonId = null; // Remove the association
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PersonExists(int id) {
        return _context.Persons.Any(e => e.Id == id);
    }
}