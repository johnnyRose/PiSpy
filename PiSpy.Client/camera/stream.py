import picamera
import io
import struct
import time

resolution = (640, 480)
framerate = 10
next_header_index = 0
output = None

def write_video(stream, start_time):
    global next_header_index
    with stream.lock:
        first_frame = None
        second_frame = None
        third_frame = None
        for f in stream.frames:
            if f.header and f.index >= next_header_index:
                if second_frame and first_frame:
                    third_frame = f
                    break
                if not second_frame and first_frame:
                    second_frame = f
                if not first_frame:
                    first_frame = f
        if first_frame and second_frame and third_frame:
            print('writing')
            stream.seek(first_frame.position)
            # figure out the exact size needed
            # contents = stream.read(second_frame.position - first_frame.position)
            contents = stream.read()
            content_length = len(contents)
            header = struct.pack('id', content_length, start_time + first_frame.index / framerate)
            contents = header + contents
            output.write(contents)
            output.flush()
            next_header_index = second_frame.index

with picamera.PiCamera() as camera:
    stream = picamera.PiCameraCircularIO(camera, seconds=20)
    camera.resolution = resolution
    camera.framerate = framerate
    camera.start_recording(stream, format='h264')
    start_time = round(time.time(), 1)
    output = io.open('/var/pispy/camera/data', 'wb')
    try:
        while True:
            camera.wait_recording(1)
            write_video(stream, start_time)
    finally:
        camera.stop_recording()
