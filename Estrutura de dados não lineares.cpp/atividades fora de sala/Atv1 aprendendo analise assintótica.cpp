#include <iostream>
#include <vector>
using namespace std; 

int exemplo(int n){
	int cont=0;
	for(int i=0;i<n;i++){
		for(int j=0;j<n;j++){
			cont++;
		}
	}
	return cont;
}

int main(int argc, char** argv) {
	int numero;
	cin >> numero;
	int resultado = exemplo(numero);
	cout << "o valor do cont eh: " << resultado << endl;
	return 0;
	
//	1. a estimativa é n² (o numero digitado vezes ele mesmo)
//	2. Big(O) de n ao quadrado, ou O(n²)
//	3. if(n=10){
//		cout << "100";
//		} else if(n=100){
//			cout << "10.000";
//			}else if(n=1000){
//				cout << "1.000.000";

}
