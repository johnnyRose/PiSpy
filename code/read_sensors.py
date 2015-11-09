import subprocess
import os
sensor_directory = '/var/pispy/'

class Sensor(object):
    process_dict = {}

    def __init__(self, name, reading_program, args=None):
        self.name = name
        self.reading_program = reading_program
        self.args = args if args else []
        self.process = None

    def collect_data(self):
        call_list = [self.reading_program] + self.args
        self.process = subprocess.Popen(call_list)
        self.process_dict[self.process.pid] = self

    def collect_callback(self):
        pass

def read_camera():
    f = open(camera_filename, 'rb')
    return f.read()

sensors = [
    Sensor('humiture', 'humiture', ['11', '18']),
    # we don't have a microphone on this pi
    # microphone = Sensor('microphone', '')
    Sensor('camera', 'python', ['split_stream.py']),
]

for s in sensors:
    s.collect_data()

while Sensor.process_dict:
    finished_process = os.wait()
    pid = finished_process[0]
    Sensor.process_dict[pid].collect_callback()
    del Sensor.process_dict[pid]
