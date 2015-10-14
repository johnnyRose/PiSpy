import socket, time, serialNumber, queue, random

server = ('104.208.29.115', 27007)

clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
clientsocket.connect(server)
clientsocket.settimeout(5)

serial = serialNumber.getSerialNumber();
q = queue.Queue()

while True: 
	q.put([str(random.randrange(18.0, 25.0)), str(random.randrange(0.0, 100.0)), str(round(time.time()))]) # [temperature, humidity, timeCaptured]
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
			print("Failed to send message. Error: " + str(e))
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

