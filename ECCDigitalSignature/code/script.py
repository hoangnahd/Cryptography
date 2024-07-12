import os
import ctypes
import time

# Load the DLL
dll_path = r'x64\Debug\Dll1.dll'
dll = ctypes.CDLL(dll_path)

# Define parameters
secret_key = b"private-key.pem"
public_key = b"public-key.pem"
test_files_folder = r'test_files'  # Replace with actual path
average_runs = 1000  # Number of times to run each test case for averaging

# Define function prototypes
sign_file_func = dll.signFile
sign_file_func.argtypes = [ctypes.c_char_p, ctypes.c_char_p, ctypes.c_char_p]
sign_file_func.restype = ctypes.c_int

verify_file_func = dll.verifySignature
verify_file_func.argtypes = [ctypes.c_char_p, ctypes.c_char_p, ctypes.c_char_p]
verify_file_func.restype = ctypes.c_int

# Function to measure execution time
def measure_execution_time(func, *args):
    start_time = time.time()
    func(*args)
    return (time.time() - start_time) * 1000  # Convert to milliseconds

# Function to get list of test files
def get_test_files(folder):
    test_files = []
    for filename in os.listdir(folder):
        if filename.endswith(".txt"):  # Adjust file extension if needed
            test_files.append(os.path.join(folder, filename))
    return test_files

# Main script
if __name__ == "__main__":
    test_files = get_test_files(test_files_folder)

    for file_to_sign in test_files:
        signature_path = f"{file_to_sign}.bin"  # Example: Create signature file next to each test file

        total_time = 0
        for _ in range(average_runs):
            execution_time = measure_execution_time(sign_file_func, secret_key, file_to_sign.encode(), signature_path.encode())
            total_time += execution_time

        average_time = total_time / average_runs

        print(f"File: {file_to_sign}")
        print(f"Average Execution Time: {average_time:.2f} ms")
        print()

    for file_to_verify in test_files:
        signature_path = f"{file_to_sign}.bin"  # Example: Create signature file next to each test file

        total_time = 0
        for _ in range(average_runs):
            execution_time = measure_execution_time(verify_file_func, public_key, file_to_verify.encode(), signature_path.encode())
            total_time += execution_time

        average_time = total_time / average_runs

        print(f"File: {file_to_sign}")
        print(f"Average Execution Time: {average_time:.2f} ms")
        print()