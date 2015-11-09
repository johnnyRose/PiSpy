import picamera
import io

resolution = (640, 480)
framerate = 10
last_read = -1

def write_video(stream):
    global last_read
    with stream.lock:
        first = True
        begin = -1
        last = -1
        for f in stream.frames:
            if f.header and not first:
                last = f.position
            if f.header and first and f.position > last_read:
                first = False
                begin = f.position
        if last > 0 and begin > last_read:
            stream.seek(begin)
            with io.open('/var/pispy/camera/data', 'wb') as output:
                print('writing')
                contents = stream.read(last - first)
                content_length = len(contents)
                print(content_length)
                contents = content_length.to_bytes(4, 'little') + contents
                output.write(contents)
            last_read = last

with picamera.PiCamera() as camera:
    stream = picamera.PiCameraCircularIO(camera, seconds=20)
    camera.resolution = resolution
    camera.framerate = framerate
    camera.start_recording(stream, format='h264')
    try:
        for i in range(100):
            camera.wait_recording(10)
            write_video(stream)
    finally:
        camera.stop_recording()
