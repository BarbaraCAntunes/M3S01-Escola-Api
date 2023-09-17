using System.Collections.Generic;

namespace Escola.API.Model
{
    public class Boletim
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public List<NotasMateria> NotasMaterias { get; set; }
    }
}
