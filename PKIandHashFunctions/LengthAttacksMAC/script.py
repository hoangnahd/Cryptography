import hashlib
import hmac

def compute_hash(message, secret_key):
    """
    Compute the HMAC-SHA256 hash of the given message using the secret key.
    """
    hash_function = hmac.new(secret_key, digestmod=hashlib.sha256)
    hash_function.update(message)
    return hash_function.hexdigest()

def verify_hash(expected_hash, actual_hash):
    """
    Verify the hash value using a secure comparison function to prevent timing attacks.
    """
    return hmac.compare_digest(expected_hash, actual_hash)

# Assume the original message is "hello world!"
original_message = b'hello'

# Generate a random secret key (in a real application, this should be securely stored)
secret_key = b'key'

# Compute the HMAC-SHA256 hash of the original message
original_hash = compute_hash(original_message, secret_key)
print(f"Original Hash: {original_hash}")

# Attempt the length extension attack by appending additional data
attacker_message = original_message + b"hello\x80\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00@world"
attacker_hash = compute_hash(attacker_message, secret_key)
print(f"Attacker's Hash: {attacker_hash}")

# Verify the hash value to check if the length extension attack was successful
if verify_hash(original_hash, "dca2c2c1d1be21c6140efb8110f312d67c182d66f20a8b8a068de6b19a89f1aa"):
    print("Length extension attack successful!")
else:
    print("Length extension attack failed.")