namespace Escola.API.Model
{
    public class NotasMateria
    {
        public int Id { get; set; }
        public int BoletimId { get; set; }
        public Boletim Boletim { get; set; }
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
        public decimal Nota { get; set; }
    }
}
