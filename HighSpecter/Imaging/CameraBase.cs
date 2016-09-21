using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighSpecter.Imaging
{

    /*Generic base that abstracts away cameras of different APIs.
     * There is a set of overridable classes at the back
     * Static classes are not suppose to be overrriden
     * 
     * Every inheriting camera class should provide implementations for instantiaded methods
     * 
     * Every inheriting camera class should provide a static class for getting the number of it's own cameras, and the generic classes here need to accomodate it. 
     * */
    

    abstract class CameraBase :IDisposable
    {
        /*Instantaneous settings of the camera, that may be changed by calling Update function, and are accessible form outside only for reading.*/
        protected volatile int _width;
        protected volatile int _height;
        protected volatile float _gammaY = 1;
        protected volatile float _gammaC = 1;
        protected volatile int _exposure ;
        protected volatile int _bitdepth = 10;
        protected volatile float _gain;
        protected ushort [][] _image;  // final storage, used to return values "up" into the programm

        public volatile int Xoffset=0;
        public volatile int Yoffset=0;
        public volatile float expCorrection = 1;
        public volatile float digitCorrection = 1;

        protected float MAXGAIN;
        protected float MAXWidth;
        protected float MAXHeight;
        protected string SerialNumber;


        private int FilterStartWavelength;
        private int FilterEndWavelength;
        private float FilterTransparency;

        protected CameraType nowCameraType; 

        public enum CameraType
        {
            XiCamera,
            IDSCamera,
            unknown
        }

        protected volatile Trigger nowTrigger = Trigger.Software; 
        
        public enum Trigger
        {
            Software,
            GPIOMaster,
            GPIOSlave
        }

        

        /* These are progressing states od camera activity, they are mutually exclusive
         * Closed camera means that it is not initialised
         * Open camera means that it is active and programm has a hold of it, but nothing else is happening
         * Updating camera means that it is in the process of changing settings, and the process should not be interrupted
         * AquiIdle means that it in Aquisition mode and ready for capture
         * Exposure means that camera has been triggered and integration has started, and the process should not be interrupted
         * AquiReading means that the data is being read and copied form the camera, and the process should not be interrupted
         * */
        public enum State
        {
            Closed,
            OpenIdle,
            OpenUpdating,
            AquiIdle,
            AquiExposure,
            AquiReading
        }

        protected volatile State nowState;

        /* These are static methods, operable on all cameras without initialising them! */
        //Update these static methods every time a new API implementation is added, to make the numbers correct
        public static int GetNOfAllCameras()
        {
            int numberOfCameras = 0;
            numberOfCameras += CameraXimea.getNOfXimeaCameras();
            numberOfCameras += CameraIDS.getNofIDScameras();
            return numberOfCameras;             
        }

        

        public static CameraBase[] InitAllCameras()
        {
            CameraBase[] allCameras = new CameraBase[GetNOfAllCameras()];

            int index = 0; 
            CameraXimea[] Xcameras = CameraXimea.InitAllXimeaCameras();
            Xcameras.CopyTo(allCameras, index);
            index = Xcameras.Length; 
            //do the same for IDS
            return allCameras;
        }

        
        /*here are getters and setters that are common to all of the inheriting classes*/

        public CameraType GetCameraType()
        {
            return nowCameraType;
        }

        public Trigger GetTrigger()
        {
            return nowTrigger;
        }

        public State GetCurrentState()
        {
            return nowState;
        }

        public String GetSerialNumber()
        {
            return SerialNumber;
        }

        public int GetHeight()
        {
            return _height;
        }

        public int GetWidth()
        {
            return _width;
        }

        public float GetGain()
        {
            return _gain;
        }

        public int GetExposure()
        {
            return _exposure;
        }

        public float GetGammaY()
        {
            return _gammaY;
        }

        public float GetGammaC()
        {
            return _gammaC;
        }

        public float GetMaxHeight()
        {
            return MAXHeight;
        }

        public float GetMaxWidth()
        {
            return MAXWidth;
        }

        public float GetMaxGain()
        {
            return MAXGAIN;
        }

        /* Herea are methods that must be overwritten to be implemented. Software will not compile if they are not!
       */
        //This function updates camera settigns, for this squisition must be stopped
        abstract public int UpdateSettings(int exposureUs, float gainDb = 0, float gammaY = 1, float gammaC = 1);
       
        //This function updates camera settings without interrupting aquisition
        abstract public int DirectlyUpdateSettings(int exposureUs, float gainDb = 0);

        //sets up the variables in the class
        abstract protected int Initialise();

        abstract public int Open(int i);

        abstract public int Open(string serial);
        
        abstract public int StartAquisition();

        abstract public int StopAquisition();

        //Returns a 16 bit image in the format of a 2d array
        public ushort[][] TakeImageUshort2D(Stopwatch stopwatch)
        {
            
            Bitmap bitmap = TakeImage();
            Logic.Status.PertTime[1] = (int)stopwatch.ElapsedMilliseconds;
            /*
            byte[] bytes = Conversions.BitmapToByteArray(bitmap);
            ushort[][] data = Conversions.ByteToUshort2D(bytes, _width, _height);
            * * */
            ushort[][] data = Conversions.BitmapToUShort2DArray(bitmap, _bitdepth, (float)digitCorrection);
             
            Logic.Status.PertTime[2] = (int) stopwatch.ElapsedMilliseconds - Logic.Status.PertTime[1];

            return data;
             
        }

        //Returns a 16 bit image in the format of a 1d array
        public ushort[] TakeImageUShort1D()
        {
            Bitmap bitmap = TakeImage();
            ushort[] image = null;
            if (bitmap!= null)
            {
                image = Conversions.BitmapToUShortArray(bitmap, _bitdepth, (float)digitCorrection);
            }
            return image;
        }

        protected abstract  Bitmap TakeImage();


        /* should release the camera if it is already open, return 1 if succesfull, 0 if it is already closed, and -1 if error has occured*/
        abstract protected int Close();

        public void Dispose()
        {
            Close();
        }


    }


}
