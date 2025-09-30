struct Raiz {
	string valor;
	Raiz* rigth;
	Raiz* left;
};

void imprimirArvore(Raiz* no, int nivel = 0) {
    if (no == nullptr) return;

    for (int i = 0; i < nivel; i++) {
        cout << "   ";
    }

    cout << "|-- " << no->valor << endl;

    imprimirArvore(no->left, nivel + 1);
    imprimirArvore(no->rigth, nivel + 1);
}

int main(){
    int n;
    cin >> n;
    Raiz* raiz = nullptr;

    for (int i = 0; i < n; i++) {
        string valor;
        cin >> valor;
        Raiz* novoNo = new Raiz{valor, nullptr, nullptr};
        if (raiz == nullptr) {
            raiz = novoNo;
        } else {
            Raiz* atual = raiz;
            Raiz* pai = nullptr;
            while (atual != nullptr) {
                pai = atual;
                if (valor < atual->valor) {
                    atual = atual->left;
                } else {
                    atual = atual->rigth;
                }
            }
            if (valor < pai->valor) {
                pai->left = novoNo;
            } else {
                pai->rigth = novoNo;
            }
        }
    }

    imprimirArvore(raiz);

    return 0;
}
