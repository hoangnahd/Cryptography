#include <openssl/x509.h>
#include <openssl/pem.h>
#include <openssl/evp.h>
#include <openssl/bio.h>
#include <iostream>

bool verify_certificate(const char* cert_file, bool is_pem) {
    X509* cert = nullptr;
    EVP_PKEY* pub_key = nullptr;
    BIO* bio_cert = nullptr;

    // Create a new BIO object to handle the certificate file
    bio_cert = BIO_new_file(cert_file, "rb");
    if (!bio_cert) {
        std::cerr << "Error opening certificate file" << std::endl;
        return false;
    }

    // Read X509 certificate from BIO
    if (is_pem) {
        cert = PEM_read_bio_X509(bio_cert, nullptr, nullptr, nullptr);
    } else {
        cert = d2i_X509_bio(bio_cert, nullptr);
    }

    // Cleanup BIO
    BIO_free(bio_cert);

    if (!cert) {
        std::cerr << "Error reading X509 certificate" << std::endl;
        return false;
    }

    // Print certificate information
    BIO* bio_out = BIO_new_fp(stdout, BIO_NOCLOSE);
    if (bio_out) {
        X509_print(bio_out, cert);
        BIO_free(bio_out);
    } else {
        std::cerr << "Error creating BIO for stdout" << std::endl;
        X509_free(cert);
        return false;
    }

    // Get public key from certificate
    pub_key = X509_get_pubkey(cert);
    if (!pub_key) {
        std::cerr << "Error getting public key" << std::endl;
        X509_free(cert); // Free X509 structure if needed
        return false;
    }

    // Verify certificate signature
    if (X509_verify(cert, pub_key) != 1) {
        std::cerr << "Signature verification failed" << std::endl;
        EVP_PKEY_free(pub_key);
        X509_free(cert); // Free X509 structure if needed
        return false;
    }

    // Cleanup resources
    EVP_PKEY_free(pub_key);
    X509_free(cert); // Free X509 structure if needed
    return true;
}

int main(int argc, char* argv[]) {
    if (argc != 3) {
        std::cerr << "Usage: " << argv[0] << " <certificate_file> <is_pem>" << std::endl;
        return 1;
    }

    const char* cert_file = argv[1];
    bool is_pem = std::stoi(argv[2]);

    std::cout << "Verifying certificate: " << cert_file << std::endl;
    if (verify_certificate(cert_file, is_pem)) {
        std::cout << "Certificate verification succeeded" << std::endl;
    } else {
        std::cout << "Certificate verification failed" << std::endl;
    }

    return 0;
}