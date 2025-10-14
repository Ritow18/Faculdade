
#include <iostream>
#include <bits/stdc++.h>
using namespace std;

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
    } else if (key > root->key) {
        root->right = insertNode(root->right, key);
    }

    return root;
}
void inorderTraversal(Node* root) {
    if (root != nullptr) {
        inorderTraversal(root->left);
        std::cout << root->key << " ";
        inorderTraversal(root->right);
    }
}
void preorderTraversal(Node* root) {
    if (root != nullptr) {
        std::cout << root->key << " ";
        preorderTraversal(root->left);
        preorderTraversal(root->right);
    }
}
void postorderTraversal(Node* root) {
    if (root != nullptr) {
        postorderTraversal(root->left);
        postorderTraversal(root->right);
        std::cout << root->key << " ";
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
    
    cout << "(In-Order):" << endl;
    inorderTraversal(root);
    cout << endl << "-----------------------------------" << endl;
	cout << "(Pre-Order):" << endl;
    preorderTraversal(root);
    cout << endl << "-----------------------------------" << endl;
    cout << "(Post-Order):" << endl;
    postorderTraversal(root);
    cout << endl << "-----------------------------------" << endl;

    return 0;
}
