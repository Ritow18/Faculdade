using System;
using System.Collections.Generic;
using System.Linq;
/*
    Edson Ristow Junior
    Davis Silva de Araujo
    Lucas Martins Ferreira
    Rodrigo Rodrigues
    Vitor Silva Brum
 */
namespace Sistema_de_Gerenciammento_Projeto_Poo
{
    internal class Program
    {
        private Universidade universidade = new Universidade();

        public static void Main(string[] args)
        {
            Program sistema = new Program();
            sistema.ExecutarSistema();
        }

        public void ExecutarSistema()
        {
            while (true)
            {
                Console.WriteLine("Bem-vindo ao Sistema de Gerenciamento de Universidade!");

                Console.WriteLine("\n--- Menu Principal ---");
                Console.WriteLine("1. Cadastrar Curso");
                Console.WriteLine("2. Cadastrar Professor");
                Console.WriteLine("3. Cadastrar Aluno");
                Console.WriteLine("4. Matricular Aluno em Curso");
                Console.WriteLine("5. Listar Cursos");
                Console.WriteLine("6. Listar Professores");
                Console.WriteLine("7. Listar Alunos");
                Console.WriteLine("0. Sair");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarCursoUI();
                        break;
                    case "2":
                        CadastrarProfessorUI();
                        break;
                    case "3":
                        CadastrarAlunoUI();
                        break;
                    case "4":
                        MatricularAlunoEmCursoUI();
                        break;
                    case "5":
                        universidade.ListarCursos();
                        break;
                    case "6":
                        universidade.ListarProfessores();
                        break;
                    case "7":
                        universidade.ListarAlunos();
                        break;
                    case "0":
                        Console.WriteLine("Saindo do sistema. Até mais!");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                        break;
                }
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear(); 
            }
        }
        private void CadastrarCursoUI()
        {
            Console.WriteLine("\n--- Cadastrar Novo Curso ---");
            Console.Write("Nome do Curso: ");
            string nome = Console.ReadLine();

            Console.Write("Código do Curso: ");
            string codigo = Console.ReadLine();

            Console.Write("Descrição do Curso: ");
            string descricao = Console.ReadLine();

            universidade.CadastrarCurso(nome, codigo, descricao);
        }

        private void CadastrarProfessorUI()
        {
            Console.WriteLine("\n--- Cadastrar Novo Professor ---");
            Console.Write("Nome do Professor: ");
            string nome = Console.ReadLine();
            Console.Write("Título do Professor (Ex: Dr., Me.): ");
            string titulo = Console.ReadLine();
            Console.Write("Departamento do Professor: ");
            string departamento = Console.ReadLine();

            universidade.CadastrarProfessor(nome, titulo, departamento);
        }

        private void CadastrarAlunoUI()
        {
            Console.WriteLine("\n--- Cadastrar Novo Aluno ---");
            Console.Write("Nome do Aluno: ");
            string nome = Console.ReadLine();
            Console.Write("Matrícula do Aluno: ");
            string matricula = Console.ReadLine();

            universidade.CadastrarAluno(nome, matricula);
        }

        private void MatricularAlunoEmCursoUI()
        {
            Console.WriteLine("\n--- Matricular Aluno em Curso ---");
            Console.Write("Matrícula do Aluno: ");
            string matriculaAluno = Console.ReadLine();

            universidade.ListarCursos(); 
            Console.Write("Código do Curso para Matricular: ");
            string codigoCurso = Console.ReadLine();

            universidade.MatricularAlunoEmCurso(matriculaAluno, codigoCurso);
        }
        public class Curso
        {
            public string Nome { get; set; }
            public string Codigo { get; set; }
            public string Descricao { get; set; }

            public Curso(string nome, string codigo, string descricao)
            {
                Nome = nome;
                Codigo = codigo;
                Descricao = descricao;
            }

            public override string ToString()
            {
                return $"[Código: {Codigo}] Nome: {Nome} - Descrição: {Descricao}";
            }
        }

        public class Professor
        {
            public string Nome { get; set; }
            public string Titulo { get; set; }
            public string Departamento { get; set; }

            public Professor(string nome, string titulo, string departamento)
            {
                Nome = nome;
                Titulo = titulo;
                Departamento = departamento;
            }

            public override string ToString()
            {
                return $"Nome: {Nome} - Título: {Titulo} - Departamento: {Departamento}";
            }
        }

        public class Aluno
        {
            public string Nome { get; set; }
            public string Matricula { get; set; }
            public Curso CursoAssociado { get; set; } 

            public Aluno(string nome, string matricula, Curso cursoAssociado)
            {
                Nome = nome;
                Matricula = matricula;
                CursoAssociado = cursoAssociado;
            }

            public override string ToString()
            {
                string nomeCurso = CursoAssociado != null ? CursoAssociado.Nome : "Nenhum";
                return $"Nome: {Nome} - Matrícula: {Matricula} - Curso: {nomeCurso}";
            }
        }
        public class Universidade
        {
            private List<Curso> cursos;
            private List<Professor> professores;
            private List<Aluno> alunos;

            public Universidade()
            {
                cursos = new List<Curso>();
                professores = new List<Professor>();
                alunos = new List<Aluno>();
            }

            public void CadastrarCurso(string nome, string codigo, string descricao)
            {
                if (cursos.Any(c => c.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Erro: Já existe um curso com o código '{codigo}'.");
                    return;
                }
                Curso novoCurso = new Curso(nome, codigo, descricao);
                cursos.Add(novoCurso);
                Console.WriteLine($"Curso '{nome}' cadastrado com sucesso!");
            }

            public void CadastrarProfessor(string nome, string titulo, string departamento)
            {
                Professor novoProfessor = new Professor(nome, titulo, departamento);
                professores.Add(novoProfessor);
                Console.WriteLine($"Professor '{nome}' cadastrado com sucesso!");
            }

            public void CadastrarAluno(string nome, string matricula)
            {
                if (alunos.Any(a => a.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Erro: Já existe um aluno com a matrícula '{matricula}'.");
                    return;
                }
                Aluno novoAluno = new Aluno(nome, matricula, null); 
                alunos.Add(novoAluno);
                Console.WriteLine($"Aluno '{nome}' cadastrado com sucesso!");
            }

            public void MatricularAlunoEmCurso(string matriculaAluno, string codigoCurso)
            {
                Aluno aluno = alunos.FirstOrDefault(a => a.Matricula.Equals(matriculaAluno, StringComparison.OrdinalIgnoreCase));
                if (aluno == null)
                {
                    Console.WriteLine($"Erro: Aluno com matrícula '{matriculaAluno}' não encontrado.");
                    return;
                }

                Curso curso = cursos.FirstOrDefault(c => c.Codigo.Equals(codigoCurso, StringComparison.OrdinalIgnoreCase));
                if (curso == null)
                {
                    Console.WriteLine($"Erro: Curso com código '{codigoCurso}' não encontrado.");
                    return;
                }

                if (aluno.CursoAssociado != null && aluno.CursoAssociado.Codigo.Equals(codigoCurso, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Atenção: Aluno '{aluno.Nome}' já está matriculado no curso '{curso.Nome}'.");
                    return;
                }

                aluno.CursoAssociado = curso;
                Console.WriteLine($"Aluno '{aluno.Nome}' matriculado com sucesso no curso '{curso.Nome}'.");
            }

            public void ListarCursos()
            {
                if (cursos.Count == 0)
                {
                    Console.WriteLine("Nenhum curso cadastrado.");
                    return;
                }
                Console.WriteLine("\n--- Cursos Cadastrados ---");
                foreach (var curso in cursos)
                {
                    Console.WriteLine(curso);
                }
            }

            public void ListarProfessores()
            {
                if (professores.Count == 0)
                {
                    Console.WriteLine("Nenhum professor cadastrado.");
                    return;
                }
                Console.WriteLine("\n--- Professores Cadastrados ---");
                foreach (var professor in professores)
                {
                    Console.WriteLine(professor);
                }
            }

            public void ListarAlunos()
            {
                if (alunos.Count == 0)
                {
                    Console.WriteLine("Nenhum aluno cadastrado.");
                    return;
                }
                Console.WriteLine("\n--- Alunos Cadastrados ---");
                foreach (var aluno in alunos)
                {
                    Console.WriteLine(aluno);
                }
            }
        }
    }
}
