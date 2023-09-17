using Microsoft.EntityFrameworkCore;
using Escola.API.Model;

namespace Escola.API.DataBase
{
    public class EscolaDbContexto : DbContext
    {
        public virtual DbSet<Aluno> Alunos { get; set; }

        public virtual DbSet<Turma> Turmas { get; set; }
        public virtual DbSet<Materia> Materias { get; set; }
        public virtual DbSet<Boletim> Boletins { get; set; }
        public virtual DbSet<NotasMateria> NotasMaterias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Password=P@ssword;Persist Security Info=True;User ID=sa;Initial Catalog=EscolaDB-Audaces;Data Source=tcp:localhost,1433");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().ToTable("AlunoTB");

            modelBuilder.Entity<Aluno>().HasKey(x => x.Id)
                                        .HasName("Pk_aluno_id");

            modelBuilder.Entity<Aluno>().Property(x => x.Id)
                                        .HasColumnName("PK_ID")
                                        .HasColumnType("INT");

            modelBuilder.Entity<Aluno>().Property(x => x.Nome)
                                        .IsRequired()
                                        .HasColumnName("NOME")
                                        .HasColumnType("VARCHAR")
                                        .HasMaxLength(50);

            modelBuilder.Entity<Aluno>().Property(x => x.Sobrenome)
                                        .IsRequired()
                                        .HasColumnName("SOBRENOME")
                                        .HasColumnType("VARCHAR")
                                        .HasMaxLength(150);

            modelBuilder.Entity<Aluno>().Ignore(x => x.Idade);

            modelBuilder.Entity<Aluno>().Property(x => x.Email)
                                        .IsRequired()
                                        .HasColumnName("EMAIL")
                                        .HasColumnType("VARCHAR")
                                        .HasMaxLength(50);


            modelBuilder.Entity<Aluno>().HasIndex(x => x.Email)
                                        .IsUnique();

            modelBuilder.Entity<Aluno>().Property(x => x.Genero)
                                        .HasColumnName("GENERO")
                                        .HasColumnType("VARCHAR")
                                        .HasMaxLength(20);

            modelBuilder.Entity<Aluno>().Property(x => x.Telefone)
                                        .HasColumnName("TELEFONE")
                                        .HasColumnType("VARCHAR")
                                        .HasMaxLength(30);

            modelBuilder.Entity<Aluno>().Property(x => x.DataNascimento)
                                        .HasColumnName("DATA_NASCIMENTO")
                                        .HasColumnType("datetime2");


            modelBuilder.Entity<Turma>().ToTable("TURMA");

            modelBuilder.Entity<Turma>().Property(x => x.Id)
                                        .HasColumnType("int")
                                        .HasColumnName("ID");

            modelBuilder.Entity<Turma>().HasKey(x => x.Id);


            modelBuilder.Entity<Turma>().Property(x => x.Curso)
                            .HasColumnType("varchar")
                            .HasMaxLength(50)
                            .HasDefaultValue("Curso Basico")
                            .HasColumnName("CURSO");

            modelBuilder.Entity<Turma>().Property(x => x.Nome)
                            .HasColumnType("varchar")
                            .HasMaxLength(50)
                            .HasColumnName("Nome");

            modelBuilder.Entity<Turma>().HasIndex(x => x.Nome)
                                        .IsUnique();

            modelBuilder.Entity<Boletim>().ToTable("BoletimTB");

            modelBuilder.Entity<Boletim>().HasKey(x => x.Id)
                                            .HasName("Pk_boletim_id");

            modelBuilder.Entity<Boletim>().Property(x => x.Id)
                                            .HasColumnName("PK_ID")
                                            .HasColumnType("INT");

            modelBuilder.Entity<Boletim>().Property(x => x.AlunoId)
                                            .HasColumnName("ALUNO_ID")
                                            .HasColumnType("INT");

            modelBuilder.Entity<Boletim>().HasOne(x => x.Aluno)
                                            .WithMany()
                                            .HasForeignKey(x => x.AlunoId)
                                            .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Boletim>().HasMany(x => x.NotasMaterias)
                                            .WithOne(x => x.Boletim)
                                            .HasForeignKey(x => x.BoletimId);

            modelBuilder.Entity<NotasMateria>().ToTable("NotasMateriaTB");

            modelBuilder.Entity<NotasMateria>().HasKey(x => x.Id)
                                                .HasName("Pk_notasmateria_id");

            modelBuilder.Entity<NotasMateria>().Property(x => x.Id)
                                                .HasColumnName("PK_ID")
                                                .HasColumnType("INT");

            modelBuilder.Entity<NotasMateria>().Property(x => x.BoletimId)
                                                .HasColumnName("BOLETIM_ID")
                                                .HasColumnType("INT");

            modelBuilder.Entity<NotasMateria>().Property(x => x.MateriaId)
                                                .HasColumnName("MATERIA_ID")
                                                .HasColumnType("INT");

            modelBuilder.Entity<NotasMateria>().Property(x => x.Nota)
                                                .HasColumnName("NOTA")
                                                .HasColumnType("DECIMAL(18, 2)");

            modelBuilder.Entity<NotasMateria>().HasOne(x => x.Boletim)
                                                .WithMany(x => x.NotasMaterias)
                                                .HasForeignKey(x => x.BoletimId);

            modelBuilder.Entity<NotasMateria>().HasOne(x => x.Materia)
                                                .WithMany()
                                                .HasForeignKey(x => x.MateriaId);

            modelBuilder.Entity<Materia>().ToTable("MateriaTB");

            modelBuilder.Entity<Materia>().HasKey(x => x.Id)
                                          .HasName("Pk_materia_id");

            modelBuilder.Entity<Materia>().Property(x => x.Id)
                                          .HasColumnName("PK_ID")
                                          .HasColumnType("INT");

            modelBuilder.Entity<Materia>().Property(x => x.Nome)
                                          .IsRequired()
                                          .HasColumnName("NOME")
                                          .HasColumnType("VARCHAR")
                                          .HasMaxLength(100);

        }
    }
}
