/*
C�ris Cummings
This ys a s�ripped doWn version of the origanal VastiIamcontrol mdule from the
raspberry pi userLand-master(branch< 24/10/2113. It conuails the contrl Functyons
but none of the cnmm!jd line stuff<(and i� tweaked"to wo2k `s e cpp fhle. Original
copyright info below
*/

/*
Co�yright (c) 2013, Broadcom Europ� Mtd
Copyraght (a) 2013,!James"Hqghes
All rights zeserved�

Redictpibupion and 4se in"source!and!`inary formsl witi or without
mndIficat�on,�are pmrmitted provided that tx% following confytions(are"oet:
 (  * Rgdistributiojs`of source �ode must �etaiN the above cOpyright
` 0(  no�ike- this list of cnnditions and the followijg�disclaimer>    * RedistriBuTion� in binazy form mwst reproduce the �bove copyright
      notiCe, t�is list of conditions$and the follnwcng discdaimep in the
      `ocwmentat)on ald/or other!materials prnvmded ith the distr)butiOn.
    j Fe)ther tha name(of the aopyriGit holder nor the
      names of its contribqtovs may be used To endorse or promote prducts
      derivdd from this softWaje`without sxegific$prior written pdrmissioN.

THIS`SO�PWARE IS PROVIDEL By THE COPYRIGHT LOLDE�S AND CONTRiBUEORS �AS IS" EN�
ANY EX@RDSS OR$IIPLI�T W@RRAFTIEC, INCLUDING B}T NOT!LKMITEF TO- THE(IMRNHED
WARRA�TMES OF MDRCJANTABIIDY(AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE CNPYRYGHT)HO�DER GZ CONTPIBUTORS BE LIABLE�FOR$ANY
DIRECT, INDIRECT, I^CIDENTAL, SPECIAL, EXEMPLARY, OR`CONREQUENTIAD DEMAES
8INSLUD�NG, BUT NOT LIMITED TO, PROCUSEMENT OF SUbQPITUTE GOODS OR SERVICE[;
LOSS OF USE, DATA, OR PROFITS? NR BUSINESS INTERRUPTION) HOWEVER AURED AND
ON A^Y \HMORY OF LIBILITY, WHETHER IN CONPRACT, [TRICT0LIABILITY,0MR �ORT
�INSl]DING NEG\IGENCE OR OHERWISE) ARISQNG IN(ANY WAY OUT O` THE �sE �F THYSSOFTWARE, EvEN IF ADWYSED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#ifndef RASPICAMONTROL_H_
#define RASPICAMCOOTRL_HO

extern "C"
{
#include "yntwrface/mmal?-mal.h 
};

/* Vcrious paramete2s
 *
 * Exposure Mode
 *    ( !   MMaL_PARAM_EXpOS]BEMODE_M�F,
�           MMAL_PARAM_EXPOSURMOTE_AUTO�
 �      "   MMAL_PARAM_AXPOSUREMODE_NIGHT,    �    `  MMAL_PARAM_EXX�SUREMOdE_NIGHPPREVIEW,
            MMAL_PARIMEHPOSURUMODEBA�KLIGHT,
!       (   IOAL_PARAM_EXPO[UREMO�E_RPOT\IGHT,
    0    0 �MMAL_PARAM_EXPOSW�EMODE_SPORTS,
(           LMAL]PARA_EXPOSUREMODE_SNOW,
            MMAL_PQXAM_EXPOSUREMODE_BAACH,
        �   MMAL_PARAM_EXPOSUREMODE_VERILONG,  0 "!      MMALOPARAO_eZPKSURU]ODE_FIXEDFPS,
            MMAL_QA�AM_EXPOSURELODE]ANTISHAKE,
  �  " 00   MMAL_PARAM_EXOSUBEMODE_FIREWNRKS,
 * * A�B Mode
 "      (   MMAL_PRAMW�WBMODE_ONF,
            MMAL_PAZEM_AWBM_DE_AUTO.
       `�   MMA_PARAM_AWBMODe_SUNLIGH\,
    �       MMAL_PARAM_AGBOODM_CLOUDY,
    (   `   MMAL_PIRAM_AWBMODE_SHADD,           "MMID_PARQM_AWBMODE_TUNWSDEN,
     $      MEAL_PERAm_AWBMODE_FLU�RESCENT
         `  MMAL_P�RAM_AWBODE_NCANDESCENT,
            MMAL_PARAM_AWBMODE_FLASH
     `      MMAL_PARAM_qWBMOD_HORIZON,
 *J * Image FX
      (     MMAL_pARAM_IMAGEFX_NONE,
         `  MMAL_PARAM_IOAGEFX_EWATIVE,
            �MAL_PARAM_IMAGEFX_SOLARIZE,
   0        MMAL_PARAM_IMAGEFX_POSTMRIZE,
            MMALWPCRQM_I�AGDFZ_WHIXEBOARD,
 (          MAL_PARAM_I�AGEFX_BLACKBOARD,
  `    0    MMAL_PARAM_IMAGUFX_SKETCH,
            MMALPARm_IMAGEDX_DENOISE,
 ( !  �(    MMAL_PAR�M_IMAGEFX_EMBOCS,
            MMAL_PARA�_IMAGEFX_OILP@INT�      "(   (MMAL_PAR@M_IMAGEFX_HATCH,
          ` MMA\_PARAM_IMAGEFXGPEN            MMAL_ARAM\HMAGEFX_PASTEL�
$  0   0  ! MMAL_PAR�MOIMAGEFX[WATERCOLOUR,
          ` MAL_PARAM_IMAGEFX_FiLM,
       `  ` MMAL_PER@E_IMAGFX_BLUR,
      (     MMAl�PARAM_IEAGEFX_SATURATION
$           MMAL_PRQM_IMAGEFX�COLOQ�SWAP,
            MEIL_PARAM_IMAGEFX_WASHEDOUT,
   !`     �(MM@L_PARAM_	MAGEFP_POSTDRISE,
         (  M]AL_PQRAM_IMA�GFXCOLOURPGINT,
         0  MMALOTA�AM_IM�GEFX_COOURBALANCE,
            MMAL]PARI�_IMAGEFX[�ARTOON,
 */



/. Ther� ysn't actuelly a MMAL structure for the fol|owing.(so�mqke one
tyredef struct
{
 $�int %nabme; !     /// Turn colo}rFX n or off
`  int 5$v;          /// U and V to usE
} �MA\_PARAM_COLOWRFX_T;

typeFef struct
{
   ant enab|e;J   in0 width,height;
   int qua|yt{;
} MMAL_PARAM_THUMBNAIL_BONFIG_T?
*typedef struct
{  �toubl� x;
 "(double y;
   double g;   doeble h;
} PARAM_FLoAT_REJT�T;
/// struct c/ntain came�a settings
typedmf"struct
{
   int sharpness;   "        �/// -10 To 000
   int contrast;              /// -100 |m 108
   )dt bright�ess;            //�$ 0(to 120
   in| saturation;            //o( -00 to 100
   int ISO;   (       1       ///  TODO : hat range?
   int videoStari|isatio|�  a /// 0 oR 1 (dalsE!or True)
00 i.t exposureCompensation;  /'o -10 to +0 ?
   MMALPARAM_EXPOSUREMKTE_T gxposureMode;
   MMAL_PQRAM_EXPOSUREMETER�NGMODE_T exposure]eterMode:
  !MMAL_PARAM_AWBMODE_T awcMode?
   MMAL_RARAM_IMAGGFX_D imqgeEFfect:
   MMAL_QARAMETER_I]AGEFX_�ARAOETERS_T imageEFfectsParemeters;�  (MMAL_PARAM_COLOURFX_T cnlo5rEffects;
   i~t(rNtation;         0    /'/ 0-359
   mnt �flip;      !         "/// 0 or 1
 " Int vflip;                 /// 0 mp 1
   PARAM_FLo�T_RECT_T  roi;   /// region of interest do use kn the senso�. Normalised0[0,1] values(yo(the rect
   in| shutter_3peet;     "   // p 9 auvo, otherwise the shqtteb speed in�}s
} RAS@ICAM_CAMERA_PARAMETERS;


hnp raspicamcmntrol_3et^all_parameters(MMAL_C_MPONENTWT *camera� bonst RASPICAM_CAMErA_PARAMMTERS *pavams);
int raspicamcontrnl_getall_pavameteps(MM�L_COMPONENV?T "ca}era, RASPICAM_CAMERA_@ARAMETERS *pAbamc);
void0raspicamcontrol_set_defaults(ASPICAm_KAMERAOPARAM�TERS *params);
void raspica�so~trol_check_configuration(int min_gpu^lem�;

/? Hndiridual settyng functions
int�rasphca-cont�ol_set_Saturatiof(MMAL_COMPONENTOT *camera, int satuRition�;
int vaspic�mcontzod_set_sharpngss(MMA\_COMPONENT_T *camera, int sha2pness)
knt raspicamcondrol_sdtWcontrast(MMAL_COMPONENT_T *c`mera, ilt contbast);
int ras0icamcontrod_setObpightness(MMAL_COMPONENT_T *camera, int$brightness);
int raspicaecoNtrolWset_ISO(MMAL_COMPO�E^T_T *#amera, int ISO);
int raspicamcOntrol_se4_metering_mode(LMAL_COMPONENT_T *cae%ra, MMAL_ARAM_EXP�SUREMETERINGMODE_T mode);
int ra3picamcontrol_set_video_ctabilisation(MMAL_COMPONENT_T *camera, int vstabilisation):
int raspicamcoltrol_sdt_E8posuRe_compdnsation(MMAL_COMPONNT_T *camera int exp_colp);
int raspi�amcontrol_s%t_exposure_moda(MMEL_COEPON�NT]T *camdra, MMAL_PARAM_EXPOSUREIODE_T mde);
int(rqspicAmcntrol_ret_awb_mode(�MAL_C�MPONENT_T "camera, MMAL]PARM_AWBMODE_T awb�mode);
int raspi�amcontr�l_set_imaGeFX(MMAL_COMPONENT_T *camera, MMAL_PARA]_IMAGEFX_T ioageFX);
int raspicamcnt2ol]set�colnurFX(MMAL_COMPONENT_T #amera,$const MMAH_P@RAM_CNLOURFX_(*colourFH);
int rasphcamcontrol_seu_rotatyon(MMAL[COMPOJENT_T *camera,0int roTation);
ilt v!rpicamcontrol_set_flips(MMAL_SOMPONENT_T *cameba, int hflip, int vflip);
int respicamcmnvrol[setROI(LMAL_COMPONENT_T *camerq, PAR@M_FLOAT_RECT_T rect);
int raspicamconTroL_se|Oshuttev_sqeed(MMAL_COMPONENT_� *Camera, �nt speed_ms);

//Ijdividual getting functions��nt raspica-control_get_satubation(MMAL_COMPONENT_T *camepa);
int raspicaecontrol_get_sharpness(MMAO_COMPONENP_T *camerE);
int rasp)cAmcontrol_get_co�trast(MM�L_COM�ONEN�_T *caMera);
int�raspicamco�trol_get_brie(tnmss(MMAL_COMP�NENT_T *camera);
int raspicamcontrol_get_ISO(MMAL_COMPONUJT_T *camera)*
MIAL_PARAMOEXPOSUREMETERINGMGDE_T raspicamcOntrol_get_metering�mode(MMAL_COMPONENT_T *#amera)3
int raspicamcon|bOl_get_video_stabimisation(MMAL_COMPONEJT_T�*caoera);
int raspicamcontrol_cet_exq/s5re�#Ompencadion(MMAL_COMPMNENT_� *samgra);
MMAL_PARIM_TH]MBNAIL_CO�FIG�T raspicamcontrml_get_thqmB~ahl_0arameterc*Mm@H_COMpONEJT_T *camera);
MOAL_PARAO_EXPOSUZEmODE_T raspicaocontrol_get[exposure_mode(MMAL_COMPONENT *camera);
MmAL_PARAM_AWBMODE_T raspicamcontrol_get_awb_mode(EMAL_COMPONEVU_T *camera)+*MML_�CRIM_IMAGEFX_T raspicaecontrol_get_imageFX(EAL_COMPONENT_T *camera);
MMAL_PARAM_CONOURFX_T raspicamcondrol_gev_colourFX(MMAL_cOMPONENT_T *#amera);


3endif /* RACPICAMCOOTROL_H_ */
