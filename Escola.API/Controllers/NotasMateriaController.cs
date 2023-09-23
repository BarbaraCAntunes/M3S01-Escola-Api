using Escola.API.DataBase;
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
    }
}
