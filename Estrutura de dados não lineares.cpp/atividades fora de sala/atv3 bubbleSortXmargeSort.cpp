#include <iostream>
#include <vector>
using namespace std; 

long long bubbleSort(long long n) {
    long long comparar = 0;
    for (long long i = 0; i < n; i++) {
        for (long long j = 0; j < n - i - 1; j++) {
            comparar++;
        }
    }
    return comparar;
}
long long mergeSort(long long n) {
    long long comparar = n * log2(n);
    return comparar;
}

int main(int argc, char** argv) {
    long long n_grande;
    cout << "insira um valor para comparar o bubbleSort com o MergeSort: ";
	cin >> n_grande;
	
    cout << "\n comparacoes para n = " << n_grande << endl;
    cout << "Bubble Sort (O(n^2)): " << bubbleSort(n_grande) << endl;
    cout << "Merge Sort (O(n log n)): " << mergeSort(n_grande) << endl;
    return 0;
    // não vou tentar denovo bubbleSort 10 a sexta...... demorou muito e não deu resultado
    // bubbleSort caso o numero seja 5000 o bubbleSort faz 12497500 comparações e o MergeSort faz 61438 depois de 2.591 segundos
    // caso o numero sej arealmente 10 a sexta, o mergeSort fez em 4.926 segundos (excluindo o bubbleSort que não rodou com um numero tão grande)
}
-----------------------------------------------------------------
// atv2
long long f1(long long n) {
    long long cont = 0;
    for (long long i = 0; i < n; i++) {
        cont++;
    }
    return cont;
}

long long f2(long long n) {
    long long cont = 0;
    for (long long i = 0; i < n; i++) {
        for (long long j = 0; j < n; j++) {
            cont++;
        }
    }
    return cont;
}

long long f3(long long n) {
    long long cont = 0;
    for (long long i = 0; i < n; i++) {
        long long temp = n;
        while (temp > 1) {
            temp /= 2;
            cont++;
        }
    }
    return cont;
}

int main() {
    cout << "Calculando o numero de operacoes...\n\n";

    long long n1 = 1000;
    long long n2 = 10000;
    long long n3 = 100000;

    cout << n1 << " f1(n) para n = " << n1 << ": " << f1(n1) << " operacoes\n";
    cout << "f2(n) para n = " << n1 << ": " << f2(n1) << " operacoes\n";
    cout << "f3(n) para n = " << n1 << ": " << f3(n1) << " operacoes\n" << endl;

    cout << n2 << " f1(n) para n = " << n2 << ": " << f1(n2) << " operacoes\n";
    cout << "f2(n) para n = " << n2 << ": " << f2(n2) << " operacoes\n";
    cout << "f3(n) para n = " << n2 << ": " << f3(n2) << " operacoes\n" << endl;

    cout << n3 << " f1(n) para n = " << n3 << ": " << f1(n3) << " operacoes\n";
    cout << "f2(n) para n = " << n3 << ": " << f2(n3) << " operacoes\n";
    cout << "f3(n) para n = " << n3 << ": " << f3(n3) << " operacoes\n";

    return 0;
}
