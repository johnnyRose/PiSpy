#include <stdio.h>
#include <unistd.h>
#include "camera.h"
#include <cv.h>
#include <highgui.h>
#include <fstream>
bool start = false;

void CameraCallback(CCamera* cam, const void* buffer, int buffer_length)
{
    std::cout << "called" << std::endl;
    start = false;
    if (start) {
        //cv::Mat image(720, 1280, CV_8UC1, (char*) buffer);
        //cv::imwrite("image.png", image);
        std::fstream f("raw.dat", std::ios::out | std::ios::binary);
        f.write((char*) buffer, buffer_length);
        f.flush();
        f.close();
    }
}

int main(int argc, const char **argv)
{
    printf("PI Cam api tester\n");
    StartCamera(2592,1944,30,CameraCallback);
    sleep(1);
    start = true;
    sleep(10);
    StopCamera();
}
