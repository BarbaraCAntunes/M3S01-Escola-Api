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
    }

}
