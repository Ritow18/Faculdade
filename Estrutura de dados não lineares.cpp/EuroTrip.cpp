#include <bits/stdc++.h>
using namespace std;

// Estrutura para representar os pesos (Custo, Distância, Tempo)
struct PesoAresta {
    int custo;    // Euros 
    int distancia;  // km
    int tempo;    // horas
};

// Estrutura para mostrar uma aresta
struct Aresta {
    int destino;
    PesoAresta peso;
};

// Estrutura para mostrarum vertice
struct Vertice {
    string nome;
    string pais;
    vector<Aresta> adjacentes; // Lista de Adjacência
};

// Estrutura para uso na Fila de Prioridade do Dijkstra: {peso_acumulado, id_do_vertice}
typedef pair<int, int> ParPrioridade;

// Estrutura para o Kruskal
struct ArestaKruskal {
    int origem;
    int destino;
    int custo;
};
//kruskal
bool compArestas(const ArestaKruskal& a1, const ArestaKruskal& a2) {
    return a1.custo < a2.custo;
}

//kruskal
struct UnionFind {
    vector<int> pai;
    UnionFind(int n) {
        pai.resize(n);
        iota(pai.begin(), pai.end(), 0);
    }
    int find(int i) {
        if (pai[i] == i)
            return i;
        return pai[i] = find(pai[i]);
    }
    void unite(int i, int j) {
        int root_i = find(i);
        int root_j = find(j);
        if (root_i != root_j) {
            pai[root_i] = root_j;
        }
    }
};

// classe princioal do grafo

class GrafoEurotrip {
public:
    vector<Vertice> vertices;

    void addVertice(const string& nome, const string& pais) {
        Vertice v;
        v.nome = nome;
        v.pais = pais;
        vertices.push_back(v);
    }

    void addAresta(int origem_id, int destino_id, int custo, int distancia, int tempo, bool bidirecional = false) {
        // Aresta de Origem -> Destino
        PesoAresta p = {custo, distancia, tempo};
        Aresta a = {destino_id, p};
        vertices[origem_id].adjacentes.push_back(a);
        
        // Se for bidirecional
        if (bidirecional) {
             Aresta a_inversa = {origem_id, p};
             vertices[destino_id].adjacentes.push_back(a_inversa);
        }
    }

    // Algoritmo de Dijkstra (Caminho Mínimo)
    void dijkstra(int origem_id, int destino_id, int tipo_peso) {
        vector<int> dist(vertices.size(), numeric_limits<int>::max());
        vector<int> pai(vertices.size(), -1);
        priority_queue<ParPrioridade, vector<ParPrioridade>, greater<ParPrioridade>> pq;

        dist[origem_id] = 0;
        pq.push({0, origem_id});

        while (!pq.empty()) {
            int d_atual = pq.top().first;
            int u = pq.top().second;
            pq.pop();

            if (d_atual > dist[u]) continue;
            
            for (const auto& aresta : vertices[u].adjacentes) {
                int v = aresta.destino;
                int peso_aresta = 0;

                if (tipo_peso == 0) peso_aresta = aresta.peso.custo;
                else if (tipo_peso == 1) peso_aresta = aresta.peso.distancia;
                else if (tipo_peso == 2) peso_aresta = aresta.peso.tempo;

                if (dist[u] + peso_aresta < dist[v]) {
                    dist[v] = dist[u] + peso_aresta;
                    pai[v] = u;
                    pq.push({dist[v], v});
                }
            }
        }
        imprimirCaminho(origem_id, destino_id, dist, pai, tipo_peso);
    }
	//algoritimo kruskal agm
    void kruskal() {
        vector<ArestaKruskal> todas_arestas;
        
        // Coleta todas as arestas
        for (size_t i = 0; i < vertices.size(); ++i) {
            for (const auto& aresta : vertices[i].adjacentes) {
                ArestaKruskal ak;
                ak.origem = i;
                ak.destino = aresta.destino;
                ak.custo = aresta.peso.custo;
                todas_arestas.push_back(ak);
            }
        }

        // Ordena por custo
        sort(todas_arestas.begin(), todas_arestas.end(), compArestas);

        UnionFind dsu(vertices.size());
        vector<ArestaKruskal> agm;
        int custo_total_agm = 0;

        // Itera sobre as arestas e constrói oagm
        for (const auto& aresta : todas_arestas) {
            if (dsu.find(aresta.origem) != dsu.find(aresta.destino)) {
                agm.push_back(aresta);
                custo_total_agm += aresta.custo;
                dsu.unite(aresta.origem, aresta.destino);
                
                if (agm.size() == vertices.size() - 1) break;
            }
        }
        //mostra o resultado em agm
        cout << "\n======================================================\n";
        cout << "|--|arvore geradora minima|--| \n";
        cout << "Rota de menor custo para CONECTAR TODOS os destinos:\n";
        
        for (const auto& aresta : agm) {
            cout << "  " << vertices[aresta.origem].nome << " --[" << aresta.custo << "Euros]--> " 
                      << vertices[aresta.destino].nome << "\n";
        }
        cout << "\nCusto Total Minimo da Conexao agm: " << custo_total_agm << "Euros\n";
    }

    //Geração e Impressão das 3 Matrizes
    void gerarEImprimirMatriz(int tipo_peso) {
        int N = vertices.size();
        vector<vector<int>> matriz(N, vector<int>(N, 0));

        for (int i = 0; i < N; ++i) {
            for (const auto& aresta : vertices[i].adjacentes) {
                int peso = 0;
                if (tipo_peso == 0) peso = aresta.peso.custo;
                else if (tipo_peso == 1) peso = aresta.peso.distancia;
                else if (tipo_peso == 2) peso = aresta.peso.tempo;
                
                matriz[i][aresta.destino] = peso;
            }
        }

        string titulo;
        if (tipo_peso == 0) titulo = "Matriz de custo (Euro)";
        else if (tipo_peso == 1) titulo = "Matriz de distancia (km)";
        else titulo = "Matriz de tempo (horas)";

        cout << "\n======================================================\n";
        cout << titulo << endl;
        
        // Imprime cabeçalho
        cout << setw(6) << "ID\\Dest";
        for (int i = 0; i < N; ++i) {
            cout << setw(6) << vertices[i].nome.substr(0, 5); // Usa 5 primeiras letras
        }
        cout << endl;

        // Imprime linhas
        for (int i = 0; i < N; ++i) {
            cout << setw(6) << (to_string(i) + " " + vertices[i].nome.substr(0, 5));
            for (int j = 0; j < N; ++j) {
                cout << setw(6) << matriz[i][j];
            }
            cout << endl;
        }
    }

private:
    // Função pra reconstruir e mostrar o caminho com Dijkstra
    void imprimirCaminho(int origem, int destino, const vector<int>& dist, const vector<int>& pai, int tipo_peso) {
        string tipo_str;
        if (tipo_peso == 0) tipo_str = "custo em euro";
        else if (tipo_peso == 1) tipo_str = "distancia (km)";
        else tipo_str = "tempo (horas)";
        
        cout << "\n======================================================\n";
        cout << "|--| caminho minimo por " << tipo_str << endl;
        
        if (dist[destino] == numeric_limits<int>::max()) {
            cout << "Não há caminho de " << vertices[origem].nome << " para " << vertices[destino].nome << endl;
            return;
        }

        vector<int> caminho;
        int atual = destino;
        while (atual != -1) {
            caminho.push_back(atual);
            atual = pai[atual];
        }
        reverse(caminho.begin(), caminho.end());

        cout << "Trajeto: ";
        for (size_t i = 0; i < caminho.size(); ++i) {
            cout << vertices[caminho[i]].nome;
            if (i < caminho.size() - 1) {
                cout << " -> ";
            }
        }
        cout << "\nTotal (" << tipo_str << "): " << dist[destino] << "\n";
    }
};

// --- FUNÇÃO PRINCIPAL ---

int main() {
    // Definindo o idioma para evitar problemas de acentuação na saída
    setlocale(LC_ALL, "Portuguese"); 

    GrafoEurotrip grafo;

    grafo.addVertice("S.Paulo", "Brasil"); 
    grafo.addVertice("Lisboa", "Portugal");  
    grafo.addVertice("Madrid", "Espanha");   
    grafo.addVertice("Barcelo", "Espanha");  
    grafo.addVertice("Paris", "França");     
    grafo.addVertice("Roma", "Itália");      
    grafo.addVertice("Milao", "Itália");     
    grafo.addVertice("Berlim", "Alemanha");  // (destino final!!)
    grafo.addVertice("Frankf", "Alemanha");  
    grafo.addVertice("Amster", "Holanda");   
    grafo.addVertice("Bruxel", "Bélgica");   
    grafo.addVertice("Praga", "R. Tcheca");  

    // definição das arestas
    
    // Adiciona Aresta (Origem, Destino, Custo, Distância, Tempo)

    grafo.addAresta(0, 1, 550, 7800, 10); // S. Paulo -> Lisboa voo

    // V1: Lisboa
    grafo.addAresta(1, 2, 90, 630, 8); // Trem / Voo para Madrid
    grafo.addAresta(1, 4, 150, 1450, 2); // Voo para Paris
    grafo.addAresta(1, 3, 120, 1000, 2); // Voo para Barcelona

    // V2: Madrid
    grafo.addAresta(2, 3, 60, 620, 3); // Trem para Barcelona
    grafo.addAresta(2, 4, 90, 1050, 2); // Voo para Paris
    grafo.addAresta(2, 5, 110, 1360, 2); // Voo para Roma

    // V3: Barcelona
    grafo.addAresta(3, 4, 70, 830, 7); // Trem para Paris
    grafo.addAresta(3, 6, 80, 780, 10); // Trem/Voo para Milão
    grafo.addAresta(3, 5, 100, 860, 1.5); // Voo para Roma

    // V4: Paris
    grafo.addAresta(4, 9, 75, 430, 3.5); // Trem para Amsterdã
    grafo.addAresta(4, 5, 120, 1100, 2); // Voo para Roma
    grafo.addAresta(4, 10, 45, 300, 1.5); // Trem para Bruxelas

    // V5: Roma
    grafo.addAresta(5, 6, 40, 570, 3.5); // Trem para Milão
    grafo.addAresta(5, 7, 100, 1200, 2); // Voo para Berlim
    grafo.addAresta(5, 11, 85, 1250, 2); // Voo para Praga

    // V6: Milão
    grafo.addAresta(6, 8, 70, 650, 1.5); // Voo pra Frankfurt
    grafo.addAresta(6, 4, 95, 850, 7); // Trem pra Paris
    grafo.addAresta(6, 11, 90, 880, 1.5); // Voo para Praga

    // V7: Berlim (DESTINO)
    grafo.addAresta(7, 8, 50, 550, 4); // Trem pra Frankfurt
    grafo.addAresta(7, 9, 80, 600, 6); // Trem pra Amsterdã
    grafo.addAresta(7, 11, 35, 350, 4.5); // Trem pra Praga

    // V8: Frankfurt
    grafo.addAresta(8, 9, 60, 450, 4); // Trem pra Amsterdã
    grafo.addAresta(8, 10, 55, 400, 3); // Trem pra Bruxelas

    // V9: Amsterdã
    grafo.addAresta(9, 10, 35, 210, 1.8); // Trem pra Bruxelas

    // V10: Bruxelas
    grafo.addAresta(10, 8, 55, 400, 3); // Trem para Frankfurt

    // V11: Praga
    grafo.addAresta(11, 8, 70, 520, 6); // Trem pra Frankfurt

    // apresenta os dadso originais em Matrizes 
    cout << "======================================================\n";
    cout << "inicio do projeto da eurotrip - Analise de grafo \n";
    
    grafo.gerarEImprimirMatriz(0); // Matriz de Custo
    grafo.gerarEImprimirMatriz(1); // Matriz de Distância
    grafo.gerarEImprimirMatriz(2); // Matriz de Tempo
    
    // calculo dos caminhos minimos usando Dijkstra 
    
    int origem = 0; // V0: São Paulo 
    int destino = 7; // V7: Berlim 

    // Caminho Mínimo por custo
    grafo.dijkstra(origem, destino, 0); 

    // Caminho Mínimo por distancia
    grafo.dijkstra(origem, destino, 1);

    // Caminho Mínimo por tempo
    grafo.dijkstra(origem, destino, 2);

    // arvore minima (Kruskal)
    grafo.kruskal();

    cout << "fim!!\n";

    return 0;
}










































































































































































































































































































































































































































































/*:D*/
