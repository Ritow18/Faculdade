  struct Node {
    int key;
    Node* left;
    Node* right;
    Node(int value) : key(value), left(nullptr), right(nullptr) {}
};
Node* insertNode(Node* root, int key) {
    if (root == nullptr) {
        return new Node(key);
    }
    if (key < root->key) {
        root->left = insertNode(root->left, key);
    }
    else if (key > root->key) {
        root->right = insertNode(root->right, key);
    }
    return root;
}
Node* findMinValueNode(Node* node) {
    Node* current = node;
    while (current && current->left != nullptr) {
        current = current->left;
    }
    return current;
}
Node* deleteNode(Node* root, int key) {
    if (root == nullptr) {
        return root;
    }
    if (key < root->key) {
        root->left = deleteNode(root->left, key);
    } else if (key > root->key) {
        root->right = deleteNode(root->right, key);
    } else {
        if (root->left == nullptr) {
            Node* temp = root->right;
            delete root;
            return temp;
        } else if (root->right == nullptr) {
            Node* temp = root->left;
            delete root;
            return temp;
        }

        Node* temp = findMinValueNode(root->right);

        root->key = temp->key;

        root->right = deleteNode(root->right, temp->key);
    }
    return root;
}

void inorderTraversal(Node* root) {
    if (root != nullptr) {
        inorderTraversal(root->left);
        cout << root->key << " ";
        inorderTraversal(root->right);
    }
}

int main() {
    Node* root = nullptr;

    root = insertNode(root, 10);
    root = insertNode(root, 5);
    root = insertNode(root, 15);
    root = insertNode(root, 2);
    root = insertNode(root, 7);
    root = insertNode(root, 20);

    cout << "Arvore original (percurso em ordem): ";
    inorderTraversal(root);
    cout << endl;

    cout << "\nInserindo o valor 12..." << endl;
    root = insertNode(root, 12);
    cout << "Arvore apos a insercao de 12 (percurso em ordem): ";
    inorderTraversal(root);
    cout << endl;

    cout << "\nExcluindo o no com valor 15..." << endl;
    root = deleteNode(root, 15);
    cout << "Arvore apos a exclusao de 15 (percurso em ordem): ";
    inorderTraversal(root);
    cout << endl;

    return 0;
}
