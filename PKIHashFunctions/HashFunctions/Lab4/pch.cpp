#include "pch.h"

// Function to compute hash using OpenSSL
extern "C" {
    __declspec(dllexport) const char* hash_data(const char* data, int data_len, const char* algorithm, int digest_length_bits) {
        // Check if the digest_length_bits is a multiple of 8
        if (digest_length_bits % 8 != 0) {
            std::cerr << "Digest length must be a multiple of 8." << std::endl;
            return "";
        }

        EVP_MD_CTX* ctx = EVP_MD_CTX_new();
        const EVP_MD* md = EVP_get_digestbyname(algorithm);

        if (!md) {
            std::cerr << "Unknown message digest " << algorithm << std::endl;
            EVP_MD_CTX_free(ctx);
            return "";
        }

        EVP_DigestInit_ex(ctx, md, nullptr);
        EVP_DigestUpdate(ctx, data, data_len);

        // Calculate the number of bytes needed for the given digest length in bits
        int digest_length_bytes = digest_length_bits / 8;
        std::vector<unsigned char> digest(digest_length_bytes);

        if (std::strcmp(algorithm, "SHAKE128") == 0 || std::strcmp(algorithm, "SHAKE256") == 0) {
            // SHAKE algorithms use different finalization
            EVP_DigestFinalXOF(ctx, digest.data(), digest_length_bytes);
        }
        else {
            unsigned int len;
            digest.resize(EVP_MD_size(md));
            digest_length_bytes = EVP_MD_size(md);
            EVP_DigestFinal_ex(ctx, digest.data(), &len);
        }

        EVP_MD_CTX_free(ctx);

        // Convert digest to hex string using std::ostringstream
        std::ostringstream oss;
        oss << std::hex << std::setfill('0');
        for (int i = 0; i < digest_length_bytes; ++i) {
            oss << std::setw(2) << static_cast<int>(digest[i]);
        }
        std::string hex_digest = oss.str();

        // Allocate memory for the C-style string
        char* result = new char[hex_digest.length() + 1];

        // Use strcpy_s for safer string copying
        strcpy_s(result, hex_digest.length() + 1, hex_digest.c_str());

        return result;
    }
    __declspec(dllexport) char* read_file(const char* file_path) {
        std::ifstream file(file_path, std::ios::binary | std::ios::ate);
        if (!file.is_open()) {
            return nullptr;
        }

        std::streamsize size = file.tellg();
        file.seekg(0, std::ios::beg);

        char* buffer = new char[size + 1];
        if (file.read(buffer, size)) {
            buffer[size] = '\0';
            return buffer;
        }
        else {
            delete[] buffer;
            return nullptr;
        }
    }
}
