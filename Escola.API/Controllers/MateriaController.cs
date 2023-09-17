using Escola.API.DataBase;
using Escola.API.DTO;
using Escola.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Controllers
{
    [Route("api/materias")]
    [ApiController]
    public class MateriaController : ControllerBase
    {

        private readonly EscolaDbContexto _context;

        public MateriaController(EscolaDbContexto context)
        {
            _context = context;
        }

        // GET: api/materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materia>>> GetMaterias()
        {
            return await _context.Materias.ToListAsync();
        }

        // GET: api/materias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materia>> GetMateria(int id)
        {
            var materia = await _context.Materias.FindAsync(id);

            if (materia == null)
            {
                return NotFound();
            }

            return materia;
        }

        // GET: api/materias/nome?nome=Matematica
        [HttpGet("nome")]
        public async Task<ActionResult<IEnumerable<Materia>>> GetMateriaByName([FromQuery] string nome)
        {
            var materias = await _context.Materias.Where(m => m.Nome.Contains(nome)).ToListAsync();

            if (materias.Count == 0)
            {
                return NotFound();
            }

            return materias;
        }

        // POST: api/materias
        [HttpPost]
        public async Task<ActionResult<Materia>> PostMateria(MateriaDTO materiaDTO)
        {
            var existingMateria = await _context.Materias.FirstOrDefaultAsync(m => m.Nome.Equals(materiaDTO.Nome, StringComparison.OrdinalIgnoreCase));

            if (existingMateria != null)
            {
                return Conflict("A matéria já existe.");
            }

            var materia = new Materia
            {
                Nome = materiaDTO.Nome,
            };

            _context.Materias.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMateria), new { id = materia.Id }, materia);
        }

        // DELETE: api/materias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Materia>> DeleteMateria(int id)
        {
            var materia = await _context.Materias.FindAsync(id);

            if (materia == null)
            {
                return NotFound();
            }

            var notasMaterias = await _context.NotasMaterias.Where(n => n.MateriaId == id).ToListAsync();
            foreach (var notaMateria in notasMaterias)
            {
                _context.NotasMaterias.Remove(notaMateria);
            }

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();

            return materia;
        }

    }
}


