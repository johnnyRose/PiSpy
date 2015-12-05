import threading
import io
import socket
import struct
import re

data_path = '/var/pispy/'
i = 0
serial = ''
socket_info = ('104.208.29.179', 27008)

def transmit(data, timestamp):
    header_size = 24 # in bytes
    args = ['iid16b', len(data), header_size, timestamp]
    args += serial
    header = struct.pack(*args)
    try:
        client = socket.socket()
        client.connect(socket_info)
        client.send(header)
        client.send(data)
        client.shutdown(socket.SHUT_RDWR)
        client.close()
    except Exception as e:
        print(e)
        print('could not connect to network')
        pass # TODO deal with saving things if we can't talk

def read_camera(path):
    print(path)
    with io.open(path, 'rb') as f:
        while True:
            print('reading ' + path)
            struct_format = 'id'
            header = f.read(struct.calcsize(struct_format))
            header = struct.unpack(struct_format, header)
            length = header[0]
            time = header[1]
            transmit(f.read(length), time)

def read_microphone(path):
    print(path)

def get_serial():
    # don't check for errors because this needs to work or we need to know it didn't
    f = open('/proc/cpuinfo')
    match = re.search('Serial\s*: ([0-9a-f]{16})', f.read())
    global serial
    serial = bytes(match.groups()[0], 'utf-8')



get_serial()

data_pipes = [
    (data_path + 'camera/data', read_camera),
    (data_path + 'microphone/data', read_microphone),
]

for part, function in data_pipes:
    threading.Thread(target=read, args=(f,)).start()

