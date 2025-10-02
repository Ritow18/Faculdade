using System;
public struct Retangulo
{
    public double Largura;public double Altura;
    public Retangulo(double largura, double altura){ Largura = largura;Altura = altura;}
    public double CalcArea(){ return Largura * Altura;}
    public double CalcPerim(){ return 2 * (Largura + Altura);}
}
public struct Data
{
    public int Dia;public int Mes;public int Ano;
    public Data(int dia, int mes, int ano){ Dia = dia;Mes = mes;Ano = ano; }
    public bool EhValida()
    {
        if (Ano < 1 || Mes < 1 || Mes > 12 || Dia < 1 || Dia > 31) { return false; }
        int diasNoMes;
        if (Mes == 2) // Fevereiro
            diasNoMes = 29;
        else if (Mes == 4 || Mes == 6 || Mes == 9 || Mes == 11) // Abril, Junho, Setembro, Novembro
            diasNoMes = 30;
        else 
            diasNoMes = 31;

        if (Dia > diasNoMes)
            return false;

        return true;
    }
    public string ObterDataForm()
    {
        string diaStr = Dia.ToString();
        if (Dia < 10)
        {
            diaStr = "0" + diaStr;
        }
        string mesStr = Mes.ToString();
        if (Mes < 10)
        {
            mesStr = "0" + mesStr;
        }
        return diaStr + "/" + mesStr + "/" + Ano.ToString();
    }
}
public struct Cor
{
    public byte R;
    public byte G; 
    public byte B; 
    public Cor(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }
    public string ObterHex()
    {
        string hexR = ConvBytePHex(R);
        string hexG = ConvBytePHex(G);
        string hexB = ConvBytePHex(B);

        return "#" + hexR + hexG + hexB;
    }
    private string ConvBytePHex(byte valor)
    {
        char[] caracHex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        string resultado = "";
        resultado += caracHex[valor / 16];
        resultado += caracHex[valor % 16];

        return resultado;
    }
}
public struct Produto
{
    public string Nome;
    public decimal Preco;
    public int Quant;
    public Produto(string nome, decimal preco, int quant)
    {
        Nome = nome;
        Preco = preco;
        Quant = quant;
    }
    public decimal CalcTotalEstoque()
    {
        return Preco * Quant;
    }
}
public class StructsInterativo
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Cálculo de Retângulo");
        Console.Write("Digite a largura do retângulo: ");
        double largRet = double.Parse(Console.ReadLine());

        Console.Write("Digite a altura do retângulo: ");
        double altuRet = double.Parse(Console.ReadLine());

        Retangulo meuRetangulo = new Retangulo(largRet, altuRet);
        Console.WriteLine("Largura: " + meuRetangulo.Largura + ", Altura: " + meuRetangulo.Altura);
        Console.WriteLine("Área do Retângulo: " + meuRetangulo.CalcArea());
        Console.WriteLine("Perímetro do Retângulo: " + meuRetangulo.CalcPerim());
        Console.WriteLine();

        Console.WriteLine("Verificação de Data");
        Console.Write("Digite o dia: ");
        int diaData = int.Parse(Console.ReadLine());

        Console.Write("Digite o mês: ");
        int mesData = int.Parse(Console.ReadLine());

        Console.Write("Digite o ano: ");
        int anoData = int.Parse(Console.ReadLine());

        Data data = new Data(diaData, mesData, anoData);
        Console.WriteLine("Data " + data.ObterDataForm() + ": É válida? " + data.EhValida());
        Console.WriteLine();

        Console.WriteLine("Conversão de Cor para valor Hexadecimal");
        Console.Write("Digite o valor para o componente Vermelho (R) [0-255]: ");
        byte rCor = byte.Parse(Console.ReadLine());

        Console.Write("Digite o valor para o componente Verde (G) [0-255]: ");
        byte gCor = byte.Parse(Console.ReadLine());

        Console.Write("Digite o valor para o componente Azul (B) [0-255]: ");
        byte bCor = byte.Parse(Console.ReadLine());

        Cor cor = new Cor(rCor, gCor, bCor);
        Console.WriteLine("Cor (RGB: " + cor.R + "," + cor.G + "," + cor.B + "): Hexadecimal: " + cor.ObterHex());
        Console.WriteLine();

        Console.WriteLine("Cálculo de Estoque de Produto");
        Console.Write("Digite o nome do produto: ");
        string nomeProd = Console.ReadLine();

        Console.Write("Digite o preço do produto: ");
        decimal precoProd = decimal.Parse(Console.ReadLine());

        Console.Write("Digite a quantidade em estoque: ");
        int quantProd = int.Parse(Console.ReadLine());

        Produto Produto = new Produto(nomeProd, precoProd, quantProd);
        Console.WriteLine("Produto: " + Produto.Nome + ", Preço: " + Produto.Preco + ", Quantidade: " + Produto.Quant);
        Console.WriteLine("Valor Total em Estoque de " + Produto.Nome + ": " + Produto.CalcTotalEstoque());
    }
}
