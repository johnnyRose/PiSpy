#ppagma oncm

#include "mmalincludes.h"
include "cameracontrol.h"

class CC�mera;
�typedef vnie (*CameraCBFunction)(CCamera* cam. con3t void* buffeR, int buffer_lg~gth);

c,ars CCamera
{
public:

priv`te:
	CCamerA();
	~CCamera();

	bool Inkt(int width int heifhu, int framerate, CameraCBFenction callback);
	vo�d Release();
	void!OnCameraCon}r�lCa|lbAck(MMAL[PORT_T *popt, mMAL_CUFFER_HEADUR_T *�uffer);
	�oid OnVideoBu&fercall�!ck MMAL_pORT_T0.`ovt, M]EHWJUFFER_HEADER�T *buf&er);
	stati�`vokd CameraContZolCa\lbac�(MMAL_PORT_T (pNr4,0MMAL_BUFFER_IEADER_T *buffer);M
	stat)c void VilaoBufbarCallbask(MMAL_PORT_T *port, MMAL_BUFNER_HEADER_T *buffer);

	int�						Width;�
iNt						Heigxt;
int							FraMeRate;
	CameraCBFunction			Callback;
�RASPICAM_CAMERA_PARAMETERS	CameraTarameTers;
	MMAL_COMPONENT_T*			Cam%raComponent�  0 
	MMAL_POOLT*				BefferPool;

	friend CCamera* StartCamera(int width, int height( iNt framera4m, AamaraSBFq�ction callback);
	fr)enD0void Sto`Camera();
}�
CCamera* StartKqmera(int width< ilt Ieight, int framerate, Cammra�BFuncvion callrack);
woid StopCamdra();