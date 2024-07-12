# Overview
This project, developed as part of a school assignment, covers various cryptographic concepts and implementations, including PKI, hash functions, and digital signatures. The project is divided into two main folders:
#### 1. PKIHashFunctions
#### 2. ECCDigitalSignature
# PKIHashFunctions
## HashFunctions
  - Implement the following hash functions: SHA224, SHA256, SHA384, SHA512, SHA3-224, SHA3-256, SHA3-384, SHA3-512, SHAKE128, SHAKE256.
  - Support UTF-8 encoded Vietnamese text.
## Digest Output:
  - For SHAKE128 and SHAKE256, allow selection of digest output length from command-line arguments.
  - Encode the digest as a hex string and save it to a file or print it on the screen.

## PKIdigitalcertificates
  - Validate the signature.
  - Accept certificate input in PEM (Base64) or DER (binary) formats.
# CollisionLengthAttacks
## 1. MD5 Collision Attacks:
  - Generate two collision messages with the same prefix string using the [hashclash](https://github.com/cr-marcstevens/hashclash) tool.
  - Create two different C++ programs with the same MD5 hash.
## 2. Length Extension Attacks on MAC:
  - Demonstrate length extension attacks on MAC using SHA1, SHA256, and SHA512 with the [hashpump](https://github.com/mheistermann/HashPump-partialhash) tool.
# ECCDigitalSignature
## Key Generation:
- Generate public and private keys.
- Save keys to files in BER and PEM formats.
## Signing Function:
- Input message from file(supporting UTF-8 Vietnamese text).
- Secret key input from file.
## Verify Function:
- Input message and signature from files (supporting UTF-8 Vietnamese text).
- Public key input from file.
## ECC Curve:
- Select from standard curves.
- Public keys should be at least 256 bits.

# Installation
### PKIHashFunctions and ECCDigitalSignature
- Ensure you have a C++ compiler installed (GCC for Linux, MSVC for Windows).
- Install OpenSSL library.
