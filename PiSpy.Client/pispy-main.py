import socket, time, serialNumber, queue, humiture

server = ('104.208.29.115', 27007)
#server = ('104.208.39.124', 27007)

clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
clientsocket.connect(server)
clientsocket.settimeout(5)

serial = serialNumber.getSerialNumber();
q = queue.Queue()

humitureCmd = ['sudo','humiture','11','18']

while True:
	q.put(humiture.getHumiture()) # [temperature, humidity, timeCaptured]
	while not q.empty():
		
		msg = q.get()
		message = (serial + ";" + msg[0] +  ";" + msg[1] + ";" + msg[2])
		print("Attempting to send to server: " + message)
		
		try:
			numSent = clientsocket.send(bytes(message, "UTF-8"))
			print("Sent " + str(numSent) + " of " + str(len(message)) + " bytes.")
			if str(numSent) != str(len(message)):
				print("Message not fully sent, reqeueing message now.")
				q.put(msg)
				break
				
		except Exception as e:
			print("Failed to send message. Error: " + str(e)
			      + "\nAttempting to reestablish connection.")
			q.put(msg)
			try:
				clientsocket.close()
				print("Socket Released.")
			except Exception as e:
				print("Failed to release socket: " + str(e))
			finally:
				try:
					clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
					clientsocket.connect(server)
					print("Connection reestablished.")
				except Exception as e2:
					print("Failed to reestablish connection: " + str(e2))
			break
		
	time.sleep(60)

clientsocket.close()
