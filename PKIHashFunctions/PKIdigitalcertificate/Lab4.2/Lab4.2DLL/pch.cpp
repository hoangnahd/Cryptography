#include "pch.h"

extern "C" {
    __declspec(dllexport) bool verify_certificate(const char* cert_file, bool is_pem) {
        X509* cert = nullptr;
        EVP_PKEY* pub_key = nullptr;
        BIO* bio_cert = nullptr;

        // Create a new BIO object to handle the certificate file
        bio_cert = BIO_new_file(cert_file, "rb");
        if (!bio_cert) {
            fprintf(stderr, "Error opening certificate file\n");
            return false;
        }

        // Read X509 certificate from BIO
        if (is_pem) {
            cert = PEM_read_bio_X509(bio_cert, nullptr, nullptr, nullptr);
        }
        else {
            cert = d2i_X509_bio(bio_cert, nullptr);
        }

        // Cleanup BIO
        BIO_free(bio_cert);

        if (!cert) {
            fprintf(stderr, "Error reading X509 certificate\n");
            return false;
        }

        // Get public key from certificate
        pub_key = X509_get_pubkey(cert);
        if (!pub_key) {
            fprintf(stderr, "Error getting public key\n");
            X509_free(cert); // Free X509 structure if needed
            return false;
        }

        // Verify certificate signature
        if (X509_verify(cert, pub_key) != 1) {
            fprintf(stderr, "Signature verification failed\n");
            EVP_PKEY_free(pub_key);
            X509_free(cert); // Free X509 structure if needed
            return false;
        }

        // Cleanup resources
        EVP_PKEY_free(pub_key);
        X509_free(cert); // Free X509 structure if needed
        return true;
    }
}