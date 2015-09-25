import socket, time, serialNumber, subprocess, queue

server = ('104.208.29.115', 27007)

clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
clientsocket.connect(server)
clientsocket.settimeout(5)

serial = serialNumber.getSerialNumber();
q = queue.Queue()

humitureCmd = ['sudo','humiture','11','18']

while True:
	sthp = subprocess.check_output(humitureCmd)
	sth = str(sthp).split(' ')
	status = sth[0]
	while status != "b'OK":
		print("Failed to read from sensor, trying again.")
		time.sleep(1)
		sthp = subprocess.check_output(humitureCmd)
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
			print("Sent " + str(numSent) + " of " + str(len(message)) + " bytes.")
			if str(numSent) != str(len(message)):
				print("Message not fully sent, requing message now.")
				q.put(msg)
				break
				
		except Exception as e:
			print("Failed to send message. Error: " + str(e))
			q.put(msg)
			clientsocket.close()
			clientsocket.connect(server)
			break
		
	time.sleep(60)

clientsocket.close()
