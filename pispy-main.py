import socket, time, serialNumber, random

clientsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
clientsocket.connect(('104.208.29.115', 27007))

serial = serialNumber.getSerialNumber();

while True:
	try:
		temperature = random.randrange(20.0, 25.0)
		humidity = random.randrange(20.0, 40.0)

		message = (serial + ";"
                           + str(temperature)+ ";"
                           + str(humidity) + ";"
                           + str(round(time.time())))
		
		print("Sending to server: " + message)
		clientsocket.send(bytes(message, "UTF-8"))

	except Exception as e:
		print(e)
		while True:
			try:
				clientsocket.connect(('104.208.29.115', 27007)) # try reconnecting
				break
			except:
				time.sleep(10)
	time.sleep(60)
