import subprocess, time

# returns [temperature, humidity, timeCaptured]
def getHumiture():
	cmd = ['sudo', 'humiture', '11', '18']

	sthp = subprocess.check_output(cmd)
	sth = str(sthp).split(' ')
	status = sth[0]
	while status != "b'OK":
		print("Failed to read from sensor, trying again.")
		time.sleep(1)
		sthp = subprocess.check_output(cmd)
		sth = str(sthp).split(' ')
		status = sth[0]

	sth.append(str(round(time.time())))
	sth[2] = sth[2][:len(sth[2]) - 1]
	sth = sth[1:]
	return sth
	    
