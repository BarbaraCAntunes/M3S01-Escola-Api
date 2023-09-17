using Escola.API.DataBase;
using Escola.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}


