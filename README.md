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
# Performance 
### 1. PKIHashFunctions
- Windows 11 
- TimeCounter: mili second

| Hash function | 1MB    | 10MB   | 50MB   | 100MB  | 1000MB  |
|---------------|--------|--------|--------|--------|---------|
| SHA224        | 1.42   | 12.06  | 66.89  | 125.57 | 1734.67 |
| SHA256        | 1.22   | 12.93  | 76.13  | 117.98 | 1551.27 |
| SHA384        | 1.94   | 23.03  | 117.52 | 218.71 | 2026.76 |
| SHA512        | 1.99   | 19.83  | 100.16 | 210.90 | 1991.28 |
| SHA3-224      | 2.70   | 32.72  | 148.71 | 415.95 | 2935.61 |
| SHA3-256      | 2.74   | 30.91  | 167.38 | 327.86 | 3044.60 |
| SHA3-384      | 4.55   | 37.08  | 192.49 | 365.60 | 3798.19 |
| SHA3-512      | 5.49   | 52.88  | 250.12 | 528.39 | 6663.68 |
SHAKE128	|2.5	|25.62	|124.78	|264.04	|3749.73
SHAKE256|	2.73	|31.64|	165.59|	319.68|	3752.67

- Ubuntu 22.04
- TimeCounter: mili second

| Hash function | 1MB       | 10MB      | 50MB      | 100MB     | 1000MB    |
|---------------|-----------|-----------|-----------|-----------|-----------|
| SHA224        | 0.788311  | 7.1848    | 30.2826   | 60.9747   | 605.985   |
| SHA256        | 0.649772  | 6.83955   | 31.3967   | 61.5561   | 607.117   |
| SHA384        | 1.51447   | 14.0291   | 68.9668   | 138.205   | 1370.05   |
| SHA512        | 1.4882    | 14.082    | 68.9973   | 137.608   | 1369.09   |
| SHA3-224      | 2.38281   | 21.4823   | 105.774   | 211.16    | 2108.34   |
| SHA3-256      | 2.45782   | 23.0742   | 112.867   | 225.468   | 2252.18   |
| SHA3-384      | 3.13962   | 29.6363   | 147.123   | 293.206   | 2932.3    |
| SHA3-512      | 4.41077   | 41.9686   | 208.239   | 416.219   | 4163.59   |
| SHAKE128      | 2.01453   | 18.5726   | 91.0495   | 184.23    | 1813.99   |
| SHAKE256      | 2.4694    | 23.3074   | 113.109   | 231.785   | 2253.2    |
### 2. ECCDigitalSignature
- TimeCounter: mili second

| File | Sign (Window) | Verify (Window) | Sign (Linux) | Verify (Linux) |
|-------------|---------------|-----------------|--------------|----------------|
| 1 KB        | 3.42          | 0.44            | 0.588103     | 0.492876       |
| 5 KB        | 4.97          | 0.93            | 0.56734      | 0.508007       |
| 10 KB       | 5.43          | 1.52            | 0.573572     | 0.51834        |
| 20 KB       | 6.42          | 2.96            | 0.525403     | 0.525403       |
| 30 KB       | 5.75          | 3.97            | 0.640234     | 0.590785       |

# Installation
### PKIHashFunctions and ECCDigitalSignature
- Ensure you have a C++ compiler installed (GCC for Linux, MSVC for Windows).
- Install OpenSSL library.
