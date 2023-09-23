using Escola.API.DataBase;
using Escola.API.DTO;
using Escola.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escola.API.Controllers
{
    public class NotasMateriaController : Controller
    {
        private readonly EscolaDbContexto _context;

        public NotasMateriaController(EscolaDbContexto context)
        {
            _context = context;
        }


        // GET: api/notasmateria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotasMateria>> GetNotasMateria(int id)
        {
            var notasMateria = await _context.NotasMaterias.FindAsync(id);

            if (notasMateria == null)
            {
                return NotFound();
            }

            return notasMateria;
        }

        // GET: api/alunos/{idaluno}/boletins/{IdBoletim}/notasmateria
        [HttpGet("~/api/alunos/{idaluno}/boletins/{IdBoletim}/notasmateria")]
        public async Task<ActionResult<IEnumerable<NotasMateria>>> GetNotasMateriaPorBoletim(int idaluno, int IdBoletim, [FromQuery] int? idmateria)
        {
            var notasMateriasQuery = _context.NotasMaterias
                .Where(n => n.BoletimId == IdBoletim);

            if (idmateria.HasValue)
            {
                notasMateriasQuery = notasMateriasQuery.Where(n => n.MateriaId == idmateria);
            }

            var notasMaterias = await notasMateriasQuery.ToListAsync();

            if (notasMaterias.Count == 0)
            {
                return NotFound();
            }

            return notasMaterias;
        }

        // POST: api/notasmateria
        [HttpPost]
        public async Task<ActionResult<NotasMateria>> PostNotasMateria(NotasMateriaDTO notasMateriaDTO)
        {
            var boletim = await _context.Boletins.FindAsync(notasMateriaDTO.BoletimId);

            if (boletim == null)
            {
                return BadRequest("O boletim associado não foi encontrado.");
            }

            var materia = await _context.Materias.FindAsync(notasMateriaDTO.MateriaId);

            if (materia == null)
            {
                return BadRequest("A matéria associada não foi encontrada.");
            }

            var notasMateria = new NotasMateria
            {
                BoletimId = notasMateriaDTO.BoletimId,
                MateriaId = notasMateriaDTO.MateriaId,
                Nota = notasMateriaDTO.Nota,
            };

            _context.NotasMaterias.Add(notasMateria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotasMateria), new { id = notasMateria.Id }, notasMateria);
        }

        // PUT: api/notasmateria/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotasMateria(int id, NotasMateriaDTO notasMateriaDTO)
        {
            if (id != notasMateriaDTO.Id)
            {
                return BadRequest("O ID no corpo da solicitação não coincide com o ID na URL."); // Retorna 400 Bad Request se o ID não coincidir
            }

            var notasMateria = await _context.NotasMaterias.FindAsync(id);

            if (notasMateria == null)
            {
                return NotFound();
            }

            notasMateria.Nota = notasMateriaDTO.Nota;

            _context.Entry(notasMateria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotasMateriaExists(id))
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

        private bool NotasMateriaExists(int id)
        {
            return _context.NotasMaterias.Any(e => e.Id == id);
        }

        // DELETE: api/notasmateria/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotasMateria(int id)
        {
            var notasMateria = await _context.NotasMaterias.FindAsync(id);

            if (notasMateria == null)
            {
                return NotFound();
            }

            _context.NotasMaterias.Remove(notasMateria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


