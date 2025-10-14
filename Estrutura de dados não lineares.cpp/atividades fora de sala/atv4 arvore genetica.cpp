#include <iostream>
#include <vector>
using namespace std; 

struct Pessoa {
	string nome;
	Pessoa* filhoEsquerdo;
	Pessoa* filhoDireito;
	
	Pessoa(string nome){
		this->nome = nome;
		this->filhoDireito = nullptr;
		this->filhoEsquerdo = nullptr;
	}
};

void imprimirArvoreOrganizada(Pessoa* no, int nivel=0){
	if(no!=nullptr){
		//exibir o nó atual que esta
		for(int i=0;i<nivel;i++){
			cout << "   ";
		}
		cout << "| " << no->nome << " |" << endl;
		
		imprimirArvoreOrganizada(no->filhoEsquerdo,nivel+1);
		imprimirArvoreOrganizada(no->filhoDireito,nivel+1);
	}
};
void deletarArvore(Pessoa* no){
	if(no!=nullptr){
		deletarArvore(no->filhoEsquerdo);
		deletarArvore(no->filhoDireito);
		
		delete no;
	}
}

int main(int argc, char** argv){
	//parte do pai
	Pessoa* avoPaterno = new Pessoa("Avo parte de pai");
	Pessoa* avohPaterno = new Pessoa("Avoh parte de pai");
	
	Pessoa* pai = new Pessoa("pai");
	Pessoa* tioPai = new Pessoa("tio parte de pai");
	
	avoPaterno->filhoEsquerdo = pai;
	avoPaterno->filhoDireito = tioPai;
	avohPaterno->filhoEsquerdo = pai;
	avohPaterno->filhoDireito = tioPai;
	
	//parte da mãe
	Pessoa* avoMaterna = new Pessoa("avo parte da mae");
	Pessoa* avohMaterna = new Pessoa("avoh parte da mae");
	
	Pessoa* mae = new Pessoa("mae");
	Pessoa* tioMae = new Pessoa("Tio parte da mae");
	
	avoMaterna->filhoEsquerdo = mae;
	avoMaterna->filhoDireito = tioMae;
	avohMaterna->filhoEsquerdo = mae;
	avohMaterna->filhoDireito = tioMae;
	//eu e meus irmãos 
	
	Pessoa* eu = new Pessoa("Eu");
	Pessoa* irmao1 = new Pessoa("Irmao por parte de pai");
	Pessoa* irmao2 = new Pessoa("Irmao por parte de mae");
	
	pai->filhoEsquerdo = irmao1;
	pai->filhoDireito = eu;
	mae->filhoEsquerdo = eu;
	mae->filhoDireito = irmao2;
	
	cout << "( arvore binaria por parte de pai )" << endl;
	imprimirArvoreOrganizada(avoPaterno);
	imprimirArvoreOrganizada(avohPaterno);
	cout << endl << "( arvore binaria por parte da mae )" << endl;
	imprimirArvoreOrganizada(avoMaterna);
	imprimirArvoreOrganizada(avohMaterna);
	
	deletarArvore(avoPaterno);
	deletarArvore(avohPaterno);
	deletarArvore(avoMaterna);
	deletarArvore(avohMaterna);
	
	return 0;
}
