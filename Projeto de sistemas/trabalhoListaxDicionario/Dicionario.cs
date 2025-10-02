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
public class Pedido
{
    public int Numero { get; set; }
    public int Mesa { get; set; }
    public List<string> Itens { get; set; } = new List<string>();
    public string Status { get; set; } = "Aberto";
    public override string ToString()
    {
        string itensStr = Itens.Count > 0 ? string.Join(", ", Itens) : "Nenhum item";
        return $"Pedido Nº: {Numero} | Mesa: {Mesa} | Itens: {itensStr} | Status: {Status}";
    }
}
public class GerenciadorPedidos
{
    private readonly List<Pedido> _listaPedidos = new List<Pedido>();
    private readonly Dictionary<int, Pedido> _dicionarioPedidos = new Dictionary<int, Pedido>();
    public void Cadastrar(Pedido pedido)
    {
        if (_dicionarioPedidos.ContainsKey(pedido.Numero))
            throw new InvalidOperationException("Número do pedido já cadastrado!");
        _listaPedidos.Add(pedido);
        _dicionarioPedidos.Add(pedido.Numero, pedido);
    }
    public List<Pedido> ListarTodos() => new List<Pedido>(_listaPedidos);
    public Pedido BuscarPorNumero(int numero) =>
        _dicionarioPedidos.TryGetValue(numero, out var pedido) ? pedido : null;
    public List<Pedido> BuscarPorMesa(int mesa) =>
        _listaPedidos.Where(p => p.Mesa == mesa).ToList();
    public void AtualizarStatus(int numero, string novoStatus)
    {
        var pedido = BuscarPorNumero(numero);
        if (pedido == null)
            throw new KeyNotFoundException("Pedido não encontrado!");
        pedido.Status = novoStatus;
    }
    public void Remover(int numero)
    {
        var pedido = BuscarPorNumero(numero);
        if (pedido == null)
            throw new KeyNotFoundException("Pedido não encontrado!");
        _listaPedidos.Remove(pedido);
        _dicionarioPedidos.Remove(numero);
    }
}
class Program
{
    static GerenciadorPedidos gerenciador = new GerenciadorPedidos();
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nGERENCIADOR DE PEDIDOS");
            Console.WriteLine("1. Cadastrar pedido");
            Console.WriteLine("2. Listar pedidos");
            Console.WriteLine("3. Buscar pedido por número");
            Console.WriteLine("4. Buscar pedidos por mesa");
            Console.WriteLine("5. Atualizar status do pedido");
            Console.WriteLine("6. Remover pedido");
            Console.WriteLine("7. Sair");
            Console.Write("Escolha uma opção: ");
            switch (Console.ReadLine())
            {
                case "1": CadastrarPedido(); 
                    break;
                case "2": ListarPedidos(); 
                    break;
                case "3": BuscarPorNumero(); 
                    break;
                case "4": BuscarPorMesa(); 
                    break;
                case "5": AtualizarStatus(); 
                    break;
                case "6": RemoverPedido(); 
                    break;
                case "7": return;
                default: Console.WriteLine("Opção inválida!");
                    break;
            }
        }
    }
    static void CadastrarPedido()
    {
        try
        {
            var pedido = new Pedido();
            Console.Write("Número do pedido: ");
            pedido.Numero = int.Parse(Console.ReadLine());
            Console.Write("Número da mesa: ");
            pedido.Mesa = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite os itens do pedido (digite vazio para terminar):");
            while (true)
            {
                Console.Write("Item: ");
                string item = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(item)) break;
                pedido.Itens.Add(item);
            }
            gerenciador.Cadastrar(pedido);
            Console.WriteLine("Pedido cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
    static void ListarPedidos()
    {
        var pedidos = gerenciador.ListarTodos();
        if (pedidos.Count == 0)
            Console.WriteLine("Nenhum pedido cadastrado.");
        else
            pedidos.ForEach(p => Console.WriteLine(p));
    }
    static void BuscarPorNumero()
    {
        Console.Write("Número do pedido: ");
        if (int.TryParse(Console.ReadLine(), out int numero))
        {
            var pedido = gerenciador.BuscarPorNumero(numero);
            Console.WriteLine(pedido != null ? pedido.ToString() : "Pedido não encontrado.");
        }
        else
        {
            Console.WriteLine("Número inválido.");
        }
    }
    static void BuscarPorMesa()
    {
        Console.Write("Número da mesa: ");
        if (int.TryParse(Console.ReadLine(), out int mesa))
        {
            var pedidos = gerenciador.BuscarPorMesa(mesa);
            if (pedidos.Count == 0)
                Console.WriteLine("Nenhum pedido encontrado para essa mesa.");
            else
                pedidos.ForEach(p => Console.WriteLine(p));
        }
        else
        {
            Console.WriteLine("Número inválido.");
        }
    }
    static void AtualizarStatus()
    {
        try
        {
            Console.Write("Número do pedido: ");
            int numero = int.Parse(Console.ReadLine());
            Console.Write("Novo status: ");
            string status = Console.ReadLine();
            gerenciador.AtualizarStatus(numero, status);
            Console.WriteLine("Status atualizado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
    static void RemoverPedido()
    {
        try
        {
            Console.Write("Número do pedido: ");
            int numero = int.Parse(Console.ReadLine());
            gerenciador.Remover(numero);
            Console.WriteLine("Pedido removido com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }
}
