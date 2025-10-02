    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    /*1. Classes e Construtores
    Obj: Tentar Criar uma classe chamada Animal com um atributo Nome e um
    construtor que inicializa esse atributo. Adicione um método FazerSom() que
    exiba uma mensagem genérica.*/

    public class animal
    { 
        public string Nome {  get; set; }
        public animal(string nome) {
            Nome = nome;
            }
        public void fazerSom()
        {
            Console.WriteLine($"o animal {Nome} fez um som....");
        }
    }
    namespace proj_test
    {
        class Program
        {
            static void Main(string[] args)
            {
                animal animalEscolhido = new animal(Console.ReadLine());
                animalEscolhido.fazerSom();
            }
        }
    }
