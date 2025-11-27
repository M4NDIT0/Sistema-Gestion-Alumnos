using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBe.Data;
using PruebaTecnicaBe.Models;
using PruebaTecnicaBe.DTOs;

namespace PruebaTecnicaBe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly PruebaTecnicaBeDbContext _context;

        public AlumnosController(PruebaTecnicaBeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            return await _context.Alumnos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alumno>> GetAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);

            if (alumno == null)
            {
                return NotFound();
            }

            return alumno;
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno(AlumnoCreateDTO alumnoDto)
        {
            var alumno = new Alumno
            {
                Nombre = alumnoDto.Nombre,
                Apellido = alumnoDto.Apellido,
                Email = alumnoDto.Email,
                FechaNacimiento = alumnoDto.FechaNacimiento
            };

            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumno", new { id = alumno.Id }, alumno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumno(int id, AlumnoCreateDTO alumnoDto)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }

            alumno.Nombre = alumnoDto.Nombre;
            alumno.Apellido = alumnoDto.Apellido;
            alumno.Email = alumnoDto.Email;
            alumno.FechaNacimiento = alumnoDto.FechaNacimiento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AlumnoExists(id))
            {
                return NotFound();
            }
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.Id == id);
        }
    }
}