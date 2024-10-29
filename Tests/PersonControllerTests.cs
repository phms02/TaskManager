using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Controllers;
using Xunit;

namespace TaskManager.Tests;

public class PersonControllerTests {
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase {
        private static List<Person> _people = new List<Person>();

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person) {
            if(person == null) {
                return BadRequest("Person cannot be null.");
            }

            person.Id = _people.Count + 1; // Simula a geração de ID
            _people.Add(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id) {
            var person = _people.FirstOrDefault(p => p.Id == id);
            if(person == null) {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePerson(int id) {
            var person = _people.FirstOrDefault(p => p.Id == id);
            if(person == null) {
                return NotFound();
            }

            // Simula a verificação de tarefas pendentes
            // Você pode adaptar isso com uma lógica real de verificação de tarefas associadas
            if(false) // Substitua por uma condição real
            {
                return BadRequest("Cannot delete person with pending tasks.");
            }

            _people.Remove(person);
            return NoContent();
        }
    }
}