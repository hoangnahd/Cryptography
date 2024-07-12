#!/bin/bash

# Function to perform length extension attack for a given hash function
perform_length_extension_attack() {
    local hash_algo="$1"
    local original_mac="$2"
    local original_message="$3"
    local extend_data="$4"
    local key_length="$5"

    echo "=== Performing Length Extension Attack for $hash_algo ==="
    echo

    # Perform length extension attack using hashpump
    extended_message=$(echo -n "$original_message"$(printf "$extend_data" | xxd -p -c 256) | xxd -r -p)
    hashpump -s "$original_mac" -d "$original_message" -a "$extend_data" -k "$key_length" | tee attack_output.txt

    # Display the output
    echo "Extended Message: $extended_message"
    echo "Attack Output:"
    cat attack_output.txt
    echo
}

# Example parameters
hash_algo_sha1="sha1"
hash_algo_sha256="sha256"
hash_algo_sha512="sha512"

message="hello"
extend_data=" world!"
key_length=4  # Example key length, adjust as per your scenario

# Original MACs for each hash algorithm (example values)
original_mac_sha1="04b9c48680b1610b0ac9025e0c96d67e6f8a2e9a"
original_mac_sha256="d5f1f28ffd555a9e145a9922c91537c778c60571bb0c2407c1ea897506190f46"
original_mac_sha512="f0a1f0e669742f0a0f5c070c5b197da32402a94d07259ec2dab50997e1e4ccdeab95c02e432199187d1777be2c86fa843d72006f48a1b0ac9856a792f8440c0b"

echo "Original Message: $message"
echo

# Perform length extension attack for each hash algorithm
perform_length_extension_attack "$hash_algo_sha1" "$original_mac_sha1" "$message" "$extend_data" "$key_length"
perform_length_extension_attack "$hash_algo_sha256" "$original_mac_sha256" "$message" "$extend_data" "$key_length"
perform_length_extension_attack "$hash_algo_sha512" "$original_mac_sha512" "$message" "$extend_data" "$key_length"

exit 0
