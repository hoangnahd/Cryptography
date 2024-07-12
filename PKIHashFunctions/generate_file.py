import os

def generate_random_file(file_path, size_in_mb):
    """Generate a file with random data of a specified size in MB."""
    size_in_bytes = size_in_mb * 1024 * 1024
    with open(file_path, 'wb') as f:
        f.write(os.urandom(size_in_bytes))

def main():
    # Define the sizes in MB
    sizes_in_mb = [1, 10, 50, 100, 1000]  # 10MB to 10GB

    # Directory to store generated files
    output_dir = 'test_files'
    os.makedirs(output_dir, exist_ok=True)

    # Generate the files
    for size in sizes_in_mb:
        file_path = os.path.join(output_dir, f'file_{size}MB.bin')
        print(f'Generating file of size {size}MB...')
        generate_random_file(file_path, size)
        print(f'File {file_path} generated.')

if __name__ == '__main__':
    main()
