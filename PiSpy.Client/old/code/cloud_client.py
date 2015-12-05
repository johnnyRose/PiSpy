import threading
import io
data_path = '/var/pispy/'

def read(path):
    print(path)
    while True:
        with io.open(path, 'rb') as f:
            # temporarily throw away data
            print('reading ' + path)
            length = f.read(4)
            length = int.from_bytes(length, 'little')
            f.read(length)

data_pipes = [
    data_path + 'camera/data',
    data_path + 'humiture/data',
]

for f in data_pipes:
    threading.Thread(target=read, args=(f,)).start()

