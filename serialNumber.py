# returns the raspberry pi's serial number
def getSerialNumber():
	serial = "0000000000000000"
	
	f = open('/proc/cpuinfo', 'r')
	for line in f:
		if line[0:6] == 'Serial':
			serial = line[10:26]
	f.close()
	
	return serial
