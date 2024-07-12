// pch.cpp: source file corresponding to the pre-compiled header

#include "pch.h"

extern "C" {
    __declspec(dllexport) bool signFile(const char* privateKeyPath, const char* filePath, const char* signaturePath)
    {
        OpenSSL_add_all_algorithms();
        ERR_load_crypto_strings();
        // Read the private key
        BIO* bio = BIO_new(BIO_s_file());
        BIO_read_filename(bio, privateKeyPath);
        EVP_PKEY* privateKey = PEM_read_bio_PrivateKey(bio, NULL, NULL, NULL);
        BIO_free(bio);

        if (!privateKey) {
            std::cerr << "Error reading private key." << std::endl;
            ERR_print_errors_fp(stderr);
            return false;
        }

        // Create a buffer to hold the document hash
        unsigned char hash[SHA256_DIGEST_LENGTH];
        std::ifstream readFile(filePath, std::ios::binary);
        if (!readFile.is_open()) {
            std::cerr << "Error opening file file." << std::endl;
            return false;
        }
        std::vector<unsigned char> fileContents((std::istreambuf_iterator<char>(readFile)), std::istreambuf_iterator<char>());
        readFile.close();

        // Hash the File
        std::cout << "Hashing the file" << std::endl;
        SHA256(&fileContents[0], fileContents.size(), hash);

        // Sign the hash
        std::cout << "Signing the hash" << std::endl;
        EVP_MD_CTX* mdCtx = EVP_MD_CTX_new();
        EVP_SignInit(mdCtx, EVP_sha256());
        EVP_SignUpdate(mdCtx, hash, SHA256_DIGEST_LENGTH);

        unsigned int signatureLen = EVP_PKEY_size(privateKey);
        std::vector<unsigned char> signature(signatureLen);
        if (!EVP_SignFinal(mdCtx, &signature[0], &signatureLen, privateKey)) {
            std::cerr << "Error signing file." << std::endl;
            EVP_MD_CTX_free(mdCtx);
            EVP_PKEY_free(privateKey);
            return false;
        }
        // Write the signature to a file
        std::cout << "Writing the signature to file: " << signaturePath << std::endl;
        std::ofstream signatureFile(signaturePath, std::ios::binary);
        if (!signatureFile.is_open()) {
            std::cerr << "Error opening signature file." << std::endl;
            return false;
        }
        signatureFile.write(reinterpret_cast<const char*>(&signature[0]), signatureLen);
        signatureFile.close();
        // Clean up
        EVP_MD_CTX_free(mdCtx);
        EVP_PKEY_free(privateKey);
        EVP_cleanup();
        ERR_free_strings();
        return true;
    }

    __declspec(dllexport) bool verifySignature(const char* publicKeyPath, const char* filePath, const char* signaturePath)
    {
        // Load the public key using BIO
        BIO* bio = BIO_new(BIO_s_file());
        if (BIO_read_filename(bio, publicKeyPath) <= 0) {
            std::cerr << "Error opening public key file." << std::endl;
            BIO_free(bio);
            return false;
        }
        EVP_PKEY* publicKey = PEM_read_bio_PUBKEY(bio, NULL, NULL, NULL);
        BIO_free(bio);

        if (!publicKey) {
            std::cerr << "Error loading public key." << std::endl;
            return false;
        }

        // Load the file
        std::ifstream readFile(filePath, std::ios::binary);
        std::vector<unsigned char> fileContents((std::istreambuf_iterator<char>(readFile)), std::istreambuf_iterator<char>());
        readFile.close();

        // Create a buffer to hold the document hash
        unsigned char hash[SHA256_DIGEST_LENGTH];
        SHA256(&fileContents[0], fileContents.size(), hash);

        // Load the signature
        std::ifstream signatureFile(signaturePath, std::ios::binary);
        std::vector<unsigned char> signature(std::istreambuf_iterator<char>(signatureFile), {});
        signatureFile.close();

        // Verify the signature
        EVP_MD_CTX* mdCtx = EVP_MD_CTX_new();
        EVP_DigestVerifyInit(mdCtx, NULL, EVP_sha256(), NULL, publicKey);
        EVP_DigestVerifyUpdate(mdCtx, hash, SHA256_DIGEST_LENGTH);
        int result = EVP_DigestVerifyFinal(mdCtx, &signature[0], signature.size());

        // Clean up
        EVP_MD_CTX_free(mdCtx);
        EVP_PKEY_free(publicKey);

        return result == 1;
    }
}
