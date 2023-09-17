namespace Escola.API.DTO
{
    public class BoletimDTO
    {
        public int Id { get; set; }
        public int MateriaId { get; set; }
        public decimal Nota { get; set; }

        public int AlunoId { get; set; }
    }
}
