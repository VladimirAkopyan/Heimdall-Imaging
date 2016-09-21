using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using xiApi.NET;

namespace HighSpecter.Imaging
{
    class CameraXimea : CameraBase

    {

        private static readonly xiCam detector = new xiCam();
        private xiCam Camera = new xiCam(); 

        
        /*Constructors
         */
        
        public CameraXimea(int i)
        {
            nowCameraType = CameraType.XiCamera; 
            nowState = State.Closed;

            Open(i);

        }

        public CameraXimea(string serial)
        {
            nowCameraType = CameraType.XiCamera;
            nowState = State.Closed;
            Open(serial); 
        }

        /*Static Public methods*/

        public static int getNOfXimeaCameras()
        {
            int NofCameras;
            detector.GetNumberDevices(out NofCameras);
            return NofCameras;
        }
              
        public static CameraXimea[] InitAllXimeaCameras()
        {
            int NofXimeaCameras = getNOfXimeaCameras(); 
            CameraXimea[] allCameras = new CameraXimea[NofXimeaCameras];
            try
            {
                for (int i = 0; i < NofXimeaCameras; i++)
                {
                    allCameras[i] = new CameraXimea(i); 
                }
                                               
            }
            catch (Exception exception)
            {
                Logging.Add(exception.ToString() + exception.Message);
            }
            return allCameras;
        }
         


        /*returns 0 if the camera is in the wrong state, -1 for exceptionand +1 for success */
        public override int UpdateSettings(int exposureUs, float gainDb = 0, float gammaY = 1, float gammaC = 1)
        {
            int toreturn = 0;
            if (nowState == State.OpenIdle)
            {
                try
                {////EDIT THIS NEEDS to BE CHANGED TO AFFECT BOTH GAIN AND EXPOSURE!!!!!!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    _exposure = (int)(exposureUs * expCorrection + 0.5);
                    Camera.SetParam(PRM.EXPOSURE, _exposure);
                    
                    Camera.SetParam(PRM.GAIN, gainDb);
                    _gain = gainDb;

                    Camera.SetParam(PRM.GAMMAY, gammaY);
                    _gammaY = gammaY;
                    Camera.SetParam(PRM.GAMMAC, gammaC);
                    _gammaC = gammaC;
                    toreturn = 1;
                    nowState = State.OpenIdle;
                }
                catch (Exception exception)
                {
                    toreturn = -1; 
                    Logging.Add("Expection in CameraXimea when trying to update settings :" + exception.Message); 
                }
            }
            else
            {
                toreturn = 0;
            }

            return toreturn;
        }

        /*returns 0 if the camera is in the wrong state, -1 for exceptionand +1 for success */
        public override int DirectlyUpdateSettings(int exposureUs, float gainDb = 0)
        {
            int toreturn = 0;
            if (nowState == State.OpenIdle)
            {
                try
                { ////EDIT THIS NEEDS to BE CHANGED TO AFFECT BOTH GAIN AND EXPOSURE!!!!!!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    Camera.SetParam(PRM.EXPOSURE_DIRECT_UPDATE, (int)(exposureUs * expCorrection +0.5));
                    _exposure = exposureUs;
                    Camera.SetParam(PRM.GAIN_DIRECT_UPDATE, gainDb);
                    _gain = gainDb;
                }
                catch (Exception exception)
                {
                    toreturn = -1;
                    Logging.Add("Expection in CameraXimea when trying to update settings :" + exception.Message);
                }
            }
            else
            {
                toreturn = 0;
            }

            return toreturn;
        }

        protected override int Initialise()
        {
            int toreturn; 
            if (nowState != State.OpenIdle)
            {
                Logging.Add("Can't initialise the camera with SN " + SerialNumber + "because the state should be OpenIdle and it is " + nowState);
                toreturn = 0; 
            }
            else
            {
                nowState = State.OpenUpdating;
                try
                {
                    /*get static camera parameters */
                    Camera.GetParam(PRM.DEVICE_SN, out SerialNumber);

                    //Resolution
                    // image width must be divisible by 4
                    Camera.GetParam(PRM.WIDTH, out _width);
                    _width = _width - (_width % 4);
                    Camera.SetParam(PRM.WIDTH, _width);
                    Camera.GetParam(PRM.HEIGHT, out _height);
                    _height = _height - (_height%4); 
                    Camera.SetParam(PRM.HEIGHT, _height);
                    
                    Camera.SetParam(PRM.IMAGE_DATA_FORMAT, IMG_FORMAT.RAW16);
                    Camera.SetParam(PRM.OUTPUT_DATA_BIT_DEPTH, _bitdepth);
                    
                    //control
                    Camera.SetParam(PRM.BUFFER_POLICY, BUFF_POLICY.UNSAFE);
                    Camera.SetParam(PRM.TRG_SOURCE, TRG_SOURCE.SOFTWARE);
                    nowTrigger = Trigger.Software;
                    toreturn = 1;


                    nowState = State.OpenIdle;
                }
                catch (Exception exception)
                {
                    toreturn = -1; 
                    Logging.Add("Updating camera settings failed: " + exception.Message);
                }
            }
            return 0;
        }

        /*If the camera is closed, opens a camera using an index. Finished*/
        public override int Open(int i)
        {
            int toreturn;
            if (nowState == State.Closed)
            {
                try
                {
                    Camera.OpenDevice(i);
                    nowState = State.OpenIdle;
                    Initialise();
                    toreturn = 1;
                }
                catch (Exception exception)
                {
                    Logging.Add("Exception occured in opening camera with index " + i +
                                " in CameraXimea, exception is: " + exception.Message);
                    toreturn = -1; 
                }
            }
            else
            {
                Logging.Add("Failed to open Ximea camera number" + i + "camera device should be clsoed but is in state" + nowState);
                toreturn = 0;
            }

            return toreturn;
        }

        /*if the camera is closed, opens a camera using a serial number. Finished*/
        public override int Open(string serial)
        {
            int toreturn;
            if (nowState == State.Closed)
            {
                try
                {
                    Camera.OpenDevice(xiCam.OpenDevBy.SerialNumber, serial);
                    nowState = State.OpenIdle;
                    Initialise();
                    toreturn = 1; 
                }
                catch (Exception exception)
                {
                    Logging.Add("Exception occured in opening camera with serial " + serial +
                                " in CameraXimea, exception is: " + exception.Message);
                    toreturn = -1; 
                }
                
            }
            else
            {
                toreturn = 0;
                Logging.Add("Failed to open camera by serial" + serial + "Camera should be closed, but is in state" + nowState);
            }

            return toreturn;
        }

        /*if the camera is OpenIdle, it will put it in AquidleMode. Finished*/
        public override int StartAquisition()
        {
            int toreturn; 
            if(nowState == State.OpenIdle)
            {
                try
                {
                    Camera.StartAcquisition();
                    nowState = State.AquiIdle;
                    toreturn = 1;
                }
                catch (Exception exception)
                {
                    toreturn = -1; 
                    Logging.Add("Exception occured when starting aquisition in camera with serial " + SerialNumber +
                                " in CameraXimea, exception is: " + exception.Message);
                }
            }
            else if (nowState == State.AquiIdle)
            {
                toreturn = 1;
                Logging.Add("Tried to start aquisition for camera with serial number " + SerialNumber + "but the state is already AquiIdle, continuing");
            }
            else
            {
                Logging.Add("Starting aquisition failed for camera with serial number " + SerialNumber + "State should be OpenIdle, but it is " + nowState);
                toreturn = 0; 
            }
          return toreturn;
        }

        /*if the camera is AquiIdle, it will put it in OpenleMode. Finished*/
        public override int StopAquisition()
        {
            int toreturn;
            if (nowState == State.AquiIdle)
            {
                try
                {
                    Camera.StopAcquisition();
                    nowState = State.OpenIdle;
                    toreturn = 1;
                }
                catch (Exception exception)
                {
                    Logging.Add("Exception occured when trying to stop aquisition in Ximea camera with serial number " + SerialNumber+ "Exception is " + exception.Message);
                    toreturn = -1; 
                }
            }
            else
            {
                toreturn = 0; 
                Logging.Add("Failed to Stop aquisition, camera should be in a state AquiIdle, but it is" + nowState);
            }
            return toreturn;
        }

        /*Returns null if taking the image fails*/
        protected override Bitmap TakeImage()
        {
            
            Bitmap tempImage= null;
            
            if (nowState != State.AquiIdle)
            {
                Logging.Add("Taking Image failed on camera with SN" + SerialNumber + "Mode should be AquiIdle, but is " + nowState);
                
            }
            else
            {
                int timeout = _exposure*2 + 200;
                
                try
                {
                    nowState = State.AquiExposure;
                    Camera.SetParam(PRM.TRG_SOFTWARE, 1);
                    nowState = State.AquiReading;
                    Camera.GetImage(out tempImage, timeout);
                    nowState = State.AquiIdle;

                }
                catch (Exception exception)
                {
                    

                    nowState = State.AquiIdle;

                    if (exception.Message.Equals("Error 1", StringComparison.OrdinalIgnoreCase))
                    {
                        Logging.Add("Camera SN " + SerialNumber + "Handle became invalid. Did you unplug it?");
                        nowState = State.Closed;
                        Dispose();
                    }
                    else if (exception.Message.Contains("Error 10"))
                    {
                        Logging.Add("Camera SN " + SerialNumber + "Experienced timeout and failed to aquire a frame");

                    }
                    else
                    {
                        Logging.Add("Exception occured when trying to image, camera SN " + SerialNumber +
                                    "Exception message " + exception.Message);
                    }
                }
                
                
            }

            return tempImage;
        }


        protected override int Close()
        {
            int toreturn = 0;
            if (nowState != State.Closed)
            {
                try
                {
                    Camera.CloseDevice();
                }
                catch (Exception exception)
                {
                    
                    Logging.Add(exception.Message + "in" + nowCameraType.ToString() + "when trying to close device");
                    toreturn = -1; 
                }
                finally
                {
                    nowState = State.Closed;
                }
            }
            else
            {
                Logging.Add("Closing camera failed, Ximea camera with serial number" + SerialNumber + "is already closed");
                toreturn = 0; 
            }

            return toreturn;
        }
    }

    /** @name Error codes
         Definitions of the error codes used in API
         @note Most functions return MM40_OK on success, an error code otherwise
   */
   /*
#define MM40_OK 						0	//!< Function call succeeded
#define MM40_INVALID_HANDLE				1	//!< Invalid handle
#define MM40_READREG					2	//!< Register read error
#define MM40_WRITEREG					3	//!< Register write error
#define	MM40_FREE_RESOURCES				4	//!< Freeing resiurces error
#define	MM40_FREE_CHANNEL				5	//!< Freeing channel error
#define	MM40_FREE_BANDWIDTH				6	//!< Freeing bandwith error
#define	MM40_READBLK					7	//!< Read block error
#define	MM40_WRITEBLK					8	//!< Write block error
#define	MM40_NO_IMAGE					9	//!< No image
#define	MM40_TIMEOUT					10	//!< Timeout
#define	MM40_INVALID_ARG				11	//!< Invalid arguments supplied
#define	MM40_NOT_SUPPORTED				12	//!< Not supported
#define	MM40_ISOCH_ATTACH_BUFFERS		13	//!< Attach buffers error
#define	MM40_GET_OVERLAPPED_RESULT		14	//!< Overlapped result
#define	MM40_MEMORY_ALLOCATION			15	//!< Memory allocation error
#define	MM40_DLLCONTEXTISNULL			16	//!< DLL context is NULL
#define	MM40_DLLCONTEXTISNONZERO		17	//!< DLL context is non zero
#define	MM40_DLLCONTEXTEXIST			18	//!< DLL context exists
#define	MM40_TOOMANYDEVICES				19	//!< Too many devices connected
#define	MM40_ERRORCAMCONTEXT			20	//!< Camera context error
#define MM40_UNKNOWN_HARDWARE			21	//!< Unknown hardware
#define	MM40_INVALID_TM_FILE			22	//!< Invalid TM file
#define	MM40_INVALID_TM_TAG				23	//!< Invalid TM tag
#define	MM40_INCOMPLETE_TM				24	//!< Incomplete TM
#define	MM40_BUS_RESET_FAILED			25	//!< Bus reset error
#define	MM40_NOT_IMPLEMENTED			26	//!< Not implemented
#define	MM40_SHADING_TOOBRIGHT			27	//!< Shading too bright
#define	MM40_SHADING_TOODARK			28	//!< Shading too dark
#define	MM40_TOO_LOW_GAIN				29	//!< Gain is too low
#define	MM40_INVALID_BPL				30	//!< Invalid bad pixel list
#define	MM40_BPL_REALLOC				31	//!< Bad pixel list realloc error
#define	MM40_INVALID_PIXEL_LIST			32	//!< Invalid pixel list
#define	MM40_INVALID_FFS				33	//!< Invalid Flash File System
#define	MM40_INVALID_PROFILE			34	//!< Invalid profile
#define	MM40_INVALID_CALIBRATION		35	//!< Invalid calibration
#define	MM40_INVALID_BUFFER				36	//!< Invalid buffer
#define	MM40_INVALID_DATA				38	//!< Invalid data
#define MM40_TGBUSY						39	//!< Timing generator is busy
#define MM40_IO_WRONG					40	//!< Wrong operation open/write/read/close
#define	MM40_ACQUISITION_ALREADY_UP		41	//!< Acquisition already started
#define	MM40_OLD_DRIVER_VERSION			42	//!< Old version of device driver installed to the system.
#define	MM40_GET_LAST_ERROR				43	//!< To get error code please call GetLastError function.
#define MM40_CANT_PROCESS				44	//!< Data can't be processed
#define MM40_ACQUISITION_STOPED			45	//!< Error occured and acquisition has been stoped or didn't start.
#define MM40_ACQUISITION_STOPED_WERR	46	//!< Acquisition has been stoped with error.
#define	MM40_INVALID_INPUT_ICC_PROFILE	47  //!< Input ICC profile missed or corrupted 
#define	MM40_INVALID_OUTPUT_ICC_PROFILE	48  //!< Output ICC profile missed or corrupted
#define MM40_DEVICE_NOT_READY			49	//!< Device not ready to operate
#define	MM40_SHADING_TOOCONTRAST		50	//!< Shading too contrast
#define	MM40_ALREADY_INITIALIZED		51	//!< Modile already initialized
#define MM40_NOT_ENOUGH_PRIVILEGES		52	//!< Application doesn't enough privileges(one or more applications opened)
#define MM40_NOT_COMPATIBLE_DRIVER		53	//!< Installed driver not compatible with current software
#define MM40_TM_INVALID_RESOURCE	    54	//!< TM file was not loaded successfully from resources
/** @} */ 


    
}
