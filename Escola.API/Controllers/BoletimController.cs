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
    [Route("api/boletins")]
    [ApiController]
    public class BoletimController : ControllerBase
    {
        private readonly EscolaDbContexto _context;
        public BoletimController(EscolaDbContexto context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Boletim>> GetBoletim(int id)
        {
            var boletim = await _context.Boletins.FindAsync(id);

            if (boletim == null)
            {
                return NotFound();
            }

            return boletim;
        }

        // GET: api/alunos/{idaluno}/boletins
        [HttpGet("~/api/alunos/{idaluno}/boletins")]
        public async Task<ActionResult<IEnumerable<Boletim>>> GetBoletinsPorAluno(int idaluno)
        {
            var boletins = await _context.Boletins.Where(b => b.AlunoId == idaluno).ToListAsync();

            if (boletins.Count == 0)
            {
                return NotFound();
            }

            return boletins;
        }

        [HttpPost]
        public async Task<ActionResult<Boletim>> PostBoletim(BoletimDTO boletimDTO)
        {
            var aluno = await _context.Alunos.FindAsync(boletimDTO.AlunoId);

            if (aluno == null)
            {
                return BadRequest("O aluno associado não foi encontrado.");
            }
            var boletim = new Boletim
            {
                AlunoId = boletimDTO.AlunoId,
            };

            _context.Boletins.Add(boletim);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoletim), new { id = boletim.Id }, boletim);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoletim(int id, BoletimDTO boletimDTO)
        {
            if (id != boletimDTO.Id)
            {
                return BadRequest("O ID no corpo da solicitação não coincide com o ID na URL."); // Retorna 400 Bad Request se o ID não coincidir
            }

            var boletim = await _context.Boletins.FindAsync(id);

            if (boletim == null)
            {
                return NotFound();
            }

            boletim.AlunoId = boletimDTO.AlunoId;

            _context.Entry(boletim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoletimExists(id))
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

        private bool BoletimExists(int id)
        {
            return _context.Boletins.Any(e => e.Id == id);
        }

        // DELETE: api/boletins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Boletim>> DeleteBoletim(int id)
        {
            var boletim = await _context.Boletins.FindAsync(id);

            if (boletim == null)
            {
                return NotFound();
            }

            var notasMaterias = await _context.NotasMaterias.Where(n => n.BoletimId == id).ToListAsync();
            foreach (var notaMateria in notasMaterias)
            {
                _context.NotasMaterias.Remove(notaMateria);
            }

            _context.Boletins.Remove(boletim);
            await _context.SaveChangesAsync();

            return boletim;
        }
    }
}
