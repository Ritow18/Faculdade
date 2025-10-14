#include <iostream>
#include <vector>
using namespace std; 

class queue {
private:
    vector<int> heap;
    int parent(int i) {
        return (i - 1) / 2;
    }

    int left(int i) {
        return (2 * i + 1);
    }

    int right(int i) {
        return (2 * i + 2);
    }

    void maxHeapify(int i) {
        int l = left(i);
        int r = right(i);
        int largest = i;
        int n = heap.size();

        if (l < n && heap[l] > heap[largest]) {
            largest = l;
        }

        if (r < n && heap[r] > heap[largest]) {
            largest = r;
        }

        if (largest != i) {
            swap(heap[i], heap[largest]);
            maxHeapify(largest);
        }
    }

public:
    void insert(int val) {
        heap.push_back(val);
        int i = heap.size() - 1;

        while (i != 0 && heap[parent(i)] < heap[i]) {
            swap(heap[i], heap[parent(i)]);
            i = parent(i);
        }
    }

    int getMax() {
        if (heap.empty()) {
            cout << "A fila esta vazia." << endl;
            return -1; 
        }
        return heap[0];
    }

    int deleteMax() {
        if (heap.empty()) {
            cout << "Erro: A fila esta vazia. Nao e possivel remover." << endl;
            return -1; 
        }

        if (heap.size() == 1) {
            int max_val = heap[0];
            heap.pop_back();
            cout << "Elemento " << max_val << " removido (era o unico)." << endl;
            return max_val;
        }

        int max_val = heap[0];

        heap[0] = heap.back();
        heap.pop_back();

        maxHeapify(0);

        cout << "Elemento " << max_val << " removido." << endl;
        return max_val;
    }

    void showQueue() {
        if (heap.empty()) {
            cout << "Fila de Prioridade: Vazia" << endl;
            return;
        }

        cout << "Estado Atual da Fila de Prioridade (Max-Heap): [";
        for (size_t i = 0; i < heap.size(); ++i) {
            cout << heap[i];
            if (i < heap.size() - 1) {
                cout << ", ";
            }
        }
        cout << "]" << endl;
    }
};

int main() {
    queue fila;

    fila.insert(30);
    fila.insert(10);
    fila.insert(50);
    fila.insert(40);
    fila.insert(20);
    fila.showQueue(); 

    cout << "\nMaior elemento (getMax): " << fila.getMax() << endl;

    fila.deleteMax(); 
    fila.showQueue();

    fila.deleteMax(); 
    fila.showQueue();

    fila.insert(50);
    fila.showQueue(); 

    cout << "\nRemovendo todos os elementos restantes:" << endl;
    while (fila.getMax() != -1) {
        fila.deleteMax();
    }

    fila.showQueue();

    fila.getMax();
    fila.deleteMax();

    return 0;
}
