#include <wiringPi.h>
#include <stdio.h>
#include <string.h>
#include <errno.h>
#include <stdlib.h>

#define     ADC_CS    0
#define     ADC_CLK   1
#define     ADC_DIO   2

#define  MIC_DO_Pin   3

typedef unsigned char uchar;
typedef unsigned int uint;

char numbers[100000];
int current_number = 0;


uchar get_ADC_Result(void)
{
	uchar i;
	uchar dat1=0, dat2=0;

	digitalWrite(ADC_CS, 0);
	digitalWrite(ADC_CLK,0);
	digitalWrite(ADC_DIO,1);	delayMicroseconds(2);
	digitalWrite(ADC_CLK,1);	delayMicroseconds(2);

	digitalWrite(ADC_CLK,0);	
	digitalWrite(ADC_DIO,1);    delayMicroseconds(2);
	digitalWrite(ADC_CLK,1);	delayMicroseconds(2);

	digitalWrite(ADC_CLK,0);	
	digitalWrite(ADC_DIO,0);	delayMicroseconds(2);
	digitalWrite(ADC_CLK,1);	
	digitalWrite(ADC_DIO,1);    delayMicroseconds(2);
	digitalWrite(ADC_CLK,0);	
	digitalWrite(ADC_DIO,1);    delayMicroseconds(2);
	
	for(i=0;i<8;i++)
	{
		digitalWrite(ADC_CLK,1);	delayMicroseconds(2);
		digitalWrite(ADC_CLK,0);    delayMicroseconds(2);

		pinMode(ADC_DIO, INPUT);
		dat1=dat1<<1 | digitalRead(ADC_DIO);
	}
	
	for(i=0;i<8;i++)
	{
		dat2 = dat2 | ((uchar)(digitalRead(ADC_DIO))<<i);
		digitalWrite(ADC_CLK,1); 	delayMicroseconds(2);
		digitalWrite(ADC_CLK,0);    delayMicroseconds(2);
	}

	digitalWrite(ADC_CS,1);
	
	return(dat1==dat2) ? dat1 : 0;
}

void micISR(void)
{
	uchar analogVal;

	//printf("voice in \n");
	
	pinMode(ADC_DIO, OUTPUT);

	analogVal = get_ADC_Result();
	if (current_number == sizeof(numbers) - 1) {
		FILE* f = fopen("a.wav", "wb");
		fwrite(numbers, 1, sizeof(numbers), f);
		fclose(f);
		exit(0);
	}
	numbers[current_number] = analogVal;
	//printf("%i", current_number);
	//printf("%i \n", analogVal);
	current_number++;
	//printf("Current analog : %d\n", analogVal);
}

int main(void)
{

	if(wiringPiSetup() == -1){ //when initialize wiring failed,print messageto screen
		printf("setup wiringPi failed !");
		return 1; 
	}
    
	//if(wiringPiISR(MIC_DO_Pin, INT_EDGE_RISING, &micISR) < 0){
	//	fprintf(stderr, "Unable to init ISR\n",strerror(errno));	
	//	return 1;
	//}

	pinMode(ADC_CS,  OUTPUT);
	pinMode(ADC_CLK, OUTPUT);

	printf("Please speaking...\n");

	while(1){
		//delayMicroseconds(100);
		micISR();
	}

	return 0;
}

