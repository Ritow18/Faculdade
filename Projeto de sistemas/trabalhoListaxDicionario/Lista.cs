using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
Edson Ristow Junior
Davis Silva de Araujo
Rodrigo Rodrigues
Vitor Silva Brum
Lucas Martins
*/
public class Program
{
    public class Veiculo
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public Veiculo(string placa, string modelo, int ano)
        {
            Placa = placa;
            Modelo = modelo;
            Ano = ano;
        }
        public void ExibirDetalhes()
        {
            Console.WriteLine($"Placa: {Placa}, Modelo: {Modelo}, Ano: {Ano}");
        }
    }
    public class GerenciadorFrota
    {
        private Dictionary<string, Veiculo> veiculosPorPlaca; 
        private List<Veiculo> todosOsVeiculos; 
        public GerenciadorFrota()
        {
            veiculosPorPlaca = new Dictionary<string, Veiculo>();
            todosOsVeiculos = new List<Veiculo>();
        }
        public void CadastrarVeiculo(string placa, string modelo, int ano)
        {
            if (veiculosPorPlaca.ContainsKey(placa.ToUpper())) 
            {
                Console.WriteLine("Erro: Veículo com esta placa já cadastrado.");
                return;
            }
            Veiculo novoVeiculo = new Veiculo(placa.ToUpper(), modelo, ano); 
            veiculosPorPlaca.Add(placa.ToUpper(), novoVeiculo);
            todosOsVeiculos.Add(novoVeiculo);
            Console.WriteLine("Veículo cadastrado com sucesso!");
        }
        public void ListarVeiculos()
        {
            if (todosOsVeiculos.Count == 0)
            {
                Console.WriteLine("Nenhum veículo cadastrado na frota.");
                return;
            }
            Console.WriteLine("\nVeículos na Frota");
            foreach (var veiculo in todosOsVeiculos.OrderBy(v => v.Placa)) 
            {
                veiculo.ExibirDetalhes();
            }
        }
        public Veiculo BuscarVeiculo(string termoBusca, bool porPlaca)
        {
            if (porPlaca)
            {
                if (veiculosPorPlaca.TryGetValue(termoBusca.ToUpper(), out Veiculo veiculoEncontrado))
                {
                    Console.WriteLine("Veículo encontrado por placa:");
                    veiculoEncontrado.ExibirDetalhes();
                    return veiculoEncontrado;
                }
                else
                {
                    Console.WriteLine($"Nenhum veículo encontrado com a placa: {termoBusca}");
                    return null;
                }
            }
            else 
            {
                var veiculosEncontrados = todosOsVeiculos.Where(v => v.Modelo.IndexOf(termoBusca, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                if (veiculosEncontrados.Any())
                {
                    Console.WriteLine($"\n Veículos encontrados por modelo: {termoBusca}");
                    foreach (var v in veiculosEncontrados.OrderBy(v => v.Modelo))
                    {
                        v.ExibirDetalhes();
                    }
                    return veiculosEncontrados.First();
                }
                else
                {
                    Console.WriteLine($"Nenhum veículo encontrado com o modelo: {termoBusca}");
                    return null;
                }
            }
        }
        public void AtualizarVeiculo(string placa, string novoModelo, int novoAno)
        {
            string placaNormalizada = placa.ToUpper();
            if (veiculosPorPlaca.TryGetValue(placaNormalizada, out Veiculo veiculoParaAtualizar))
            {
                veiculoParaAtualizar.Modelo = novoModelo;
                veiculoParaAtualizar.Ano = novoAno;
                Console.WriteLine($"Informações do veículo com placa {placaNormalizada} atualizadas com sucesso!");
                veiculoParaAtualizar.ExibirDetalhes();
            }
            else
            {
                Console.WriteLine($"Erro: Veículo com placa {placaNormalizada} não encontrado para atualização.");
            }
        }
        public void RemoverVeiculo(string placa)
        {
            string placaNormalizada = placa.ToUpper();
            if (veiculosPorPlaca.ContainsKey(placaNormalizada))
            {
                Veiculo veiculoParaRemover = veiculosPorPlaca[placaNormalizada];
                veiculosPorPlaca.Remove(placaNormalizada);
                todosOsVeiculos.Remove(veiculoParaRemover);
                Console.WriteLine($"Veículo com placa {placaNormalizada} removido com sucesso da frota.");
            }
            else
            {
                Console.WriteLine($"Erro: Veículo com placa {placaNormalizada} não encontrado para remoção.");
            }
        }
    }
    private static GerenciadorFrota gerenciador = new GerenciadorFrota();
    public static void Main(string[] args)
    {
        Console.WriteLine("Bem-vindo ao Sistema de Gerenciamento de Frota!");
        int opcao;
        do
        {
            ExibirMenu();
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        CadastrarNovoVeiculo();
                        break;
                    case 2:
                        gerenciador.ListarVeiculos();
                        break;
                    case 3:
                        BuscarVeiculo();
                        break;
                    case 4:
                        AtualizarDetalhesVeiculo();
                        break;
                    case 5:
                        RemoverVeiculoDaFrota();
                        break;
                    case 0:
                        Console.WriteLine("Saindo do sistema. Até mais!");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Por favor, escolha uma opção entre 0 e 5.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Por favor, digite um número.");
            }
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
            Console.Clear(); 
        } while (opcao != 0);
    }
    private static void ExibirMenu()
    {
        Console.WriteLine("\nMENU PRINCIPAL");
        Console.WriteLine("1. Cadastrar Veículo");
        Console.WriteLine("2. Listar Veículos");
        Console.WriteLine("3. Buscar Veículo");
        Console.WriteLine("4. Atualizar Informações do Veículo");
        Console.WriteLine("5. Remover Veículo");
        Console.WriteLine("0. Sair");
        Console.Write("Escolha uma opção: ");
    }
    private static void CadastrarNovoVeiculo()
    {
        Console.WriteLine("\nCADASTRO DE VEÍCULO");
        Console.Write("Digite a placa do veículo (ex: ABC1234): ");
        string placa = Console.ReadLine(); 
        Console.Write("Digite o modelo do veículo: ");
        string modelo = Console.ReadLine();
        Console.Write("Digite o ano do veículo: ");
        int ano;
        while (!int.TryParse(Console.ReadLine(), out ano) || ano < 1900 || ano > DateTime.Now.Year + 5)
        {
            Console.WriteLine("Ano inválido. Por favor, digite um ano válido (entre o ano 1900 e o seu ano atual com no maximo + 5 anos).");
            Console.Write("Digite o ano do veículo: ");
        }
        gerenciador.CadastrarVeiculo(placa, modelo, ano);
    }
    private static void BuscarVeiculo()
    {
        Console.WriteLine("\nBUSCAR VEÍCULO");
        Console.WriteLine("Buscar por: ");
        Console.WriteLine("1. Placa");
        Console.WriteLine("2. Modelo");
        Console.Write("Escolha uma opção: ");
        int tipoBusca;
        if (int.TryParse(Console.ReadLine(), out tipoBusca))
        {
            Console.Write("Digite o termo de busca: ");
            string termo = Console.ReadLine();
            if (tipoBusca == 1)
            {
                gerenciador.BuscarVeiculo(termo, true); 
            }
            else if (tipoBusca == 2)
            {
                gerenciador.BuscarVeiculo(termo, false); 
            }
            else
            {
                Console.WriteLine("Opção de busca inválida.");
            }
        }
        else
        {
            Console.WriteLine("Entrada inválida.");
        }
    }
    private static void AtualizarDetalhesVeiculo()
    {
        Console.WriteLine("\nATUALIZAR INFORMAÇÕES DO VEÍCULO");
        Console.Write("Digite a placa do veículo que deseja atualizar: ");
        string placa = Console.ReadLine();
        Veiculo veiculoAtual = gerenciador.BuscarVeiculo(placa, true);
        if (veiculoAtual == null)
        {
            return;
        }
        Console.Write($"Digite o novo modelo (atual: {veiculoAtual.Modelo}). Deixe em branco para manter: ");
        string novoModelo = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(novoModelo))
        {
            novoModelo = veiculoAtual.Modelo;
        }
        Console.Write($"Digite o novo ano (atual: {veiculoAtual.Ano}). Digite 0 para manter: ");
        int novoAno;
        while (!int.TryParse(Console.ReadLine(), out novoAno) || (novoAno != 0 && (novoAno < 1900 || novoAno > DateTime.Now.Year + 5)))
        {
            Console.WriteLine("Ano inválido. Por favor, digite um ano válido ou 0 para manter o atual.");
            Console.Write("Digite o novo ano: ");
        } 
        if (novoAno == 0)
        {
            novoAno = veiculoAtual.Ano;
        }
        gerenciador.AtualizarVeiculo(placa, novoModelo, novoAno);
    }
    private static void RemoverVeiculoDaFrota()
    {
        Console.WriteLine("\nREMOVER VEÍCULO");
        Console.Write("Digite a placa do veículo que deseja remover: ");
        string placa = Console.ReadLine();
        gerenciador.RemoverVeiculo(placa);
    }
}
