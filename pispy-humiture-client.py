
import socket, time, random, subprocess, queue

# returns the raspberry pi's serial number
def getSerialNumber():
	serial = "0000000000000000"

	try:
		f = open('/proc/cpuinfo', 'r')
		for line in f:
			if line[0:6] == 'Serial':
				serial = line[10:26]
		f.close()
	except:
		serial = "ERROR"

	return serial


clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
clientsocket.connect(('104.208.29.115', 27007))
clientsocket.settimeout(5)

serial =  getSerialNumber();
q = queue.Queue()

while True:
	sthp = subprocess.check_output(['sudo','humiture','11','18'])
	sth = str(sthp).split(' ')
	status = sth[0]
	while status != "b'OK":
		print("Failed to read from sensor, trying again.")
		time.sleep(1)
		sthp = subprocess.check_output(['sudo','humiture','11','18'])
		sth = str(sthp).split(' ')
		status = sth[0]
	sth.append(str(round(time.time())))
	sth[2] = sth[2][:len(sth[2])-1]
	sth = sth[1:]
	q.put(sth)
	while not q.empty():
		msg = q.get()
		temperature = msg[0]
		humidity = msg[1]
		timeCaptured = msg[2]
		message = (serial + ";" + temperature+ ";" + humidity + ";" + timeCaptured)
		print("Attempting to send to server: " + message)
		try:
			numSent = clientsocket.send(bytes(message, "UTF-8"))
			print("Sent "+str(numSent)+" of " + str(len(message)) + " bytes.")
			if str(numSent) != str(len(message)):
                                print("Message not fully sent requing message now.")
                                q.put(msg)
                                break
                                
		except Exception as e:
			print("Failed to send message with error : " + str(e))
			q.put(msg)
			clientsocket.close()
			clientsocket.connect(('104.208.29.115', 27007))
			break
		#print("Got message '" + clientsocket.recv(1024) + "' back from server.")
			
	time.sleep(60)

clientsocket.close()
