using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using HighSpecter.NetworkData;
using System.IO;

using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Converters;
using System.Windows.Media.Media3D;
using HighSpecter.Imaging;
using xiApi.NET;

namespace HighSpecter
{
    public static class Logic
    {
        /*counter variables*/ 
        public const String StaticPath = "C:/SMapping/";
        public static String CurrentPath;


        public static ConcurrentQueue<ushort[][][]> CQ = new ConcurrentQueue<ushort[][][]>();

        private static Task _TcaptueImage =  new Task(CaptueImage);
        private static Stopwatch stopwatchCapture = new Stopwatch();
        private static Stopwatch stopwatchWrite = new Stopwatch();


        private static volatile Int32 _updateDue = 1; //Keep this at true! It's important that first initialisation happens!
   
        private volatile static Int32 _FilesWriting= 0; //set to 0 when not writing, and set to 1 when writing
        public static TelemetryR Status =new TelemetryR(false, 1, 10);


        //Holds all current cameras
        private static Imaging.CameraBase[] _cameras; 

        
        public static void Initialise_Cameras()
        {
            Logging.Add("initialising cameras");
            _cameras = new Imaging.CameraBase[Imaging.CameraBase.GetNOfAllCameras()]; 
            Status.Cameras = (short)_cameras.Length;
            if (_cameras.Length > 0)
                _cameras[0] = new CameraXimea("21402542");
            if (_cameras.Length > 1)
                _cameras[1] = new CameraXimea("09403750");

        }

        public static void Reinitialise()
        {
            if (_TcaptueImage.Status !=  System.Threading.Tasks.TaskStatus.Running )
            {
                Logging.Add("Reinitialising cameras");
                Close_Cameras();
                Initialise_Cameras();
            }
            else
            {
                Logging.Add("Can't Reinitialise Cameras While Running");
            }
            

        }

        private static void Close_Cameras()
        {
            Logging.Add("Closing Cameras");

            foreach (Imaging.CameraBase cam in _cameras)
            {
                cam.Dispose();
            }
        }

        /*Read  The Camera*/

        private static void CaptueImage()
        {

            while (Status.Active && _cameras.Length > 0)
            {
                stopwatchCapture.Reset();
                stopwatchCapture.Start();
                
                if (Interlocked.CompareExchange(ref _updateDue, 0, 1) == 1)
                    //checks if there is an update due, if it is then replaces the _updatedue with 0, and enters the loop
                {
                    
                    //updates camera settings
                    foreach (CameraBase cam in _cameras)
                    {
                        if (cam.GetCurrentState() == CameraBase.State.AquiIdle)
                            //stops aquisition on each camera is they are idle
                        {
                            cam.StopAquisition();
                            
                        }
                        cam.UpdateSettings(Status.Exposureμs, (float) Status.Gain, (float) Status.GammaY,
                            (float) Status.GammaC);
                    }
                    Status.PertTime[0] = (int)stopwatchCapture.ElapsedMilliseconds;
                    stopwatchCapture.Stop();
                }
                else
                {

                    //calculating sensor driven exposure
                                      
                    if (Status.SensorDrivenExposure)
                    {
                        if (Status.Lsensors[0] > 0 && Status.Lsensors[1] > 0)
                        {
                            double brightness = (Status.Lsensors[0] + Status.Lsensors[1]) / 2d;//estimate the brightness by averaging the two sensors
                            //calculate nessesary exposure by dividing coefficient by brightness, multiply by a million to get milliseconds
                            Status.Exposureμs = (int)((Status.SensorResponce / brightness) * 1000000d);
                        }
                        else
                            Logging.Add("Sensor Driven Exposure set, but Light sensors are not connected");
                    }
                    

                    //take a picture
                    foreach (CameraBase cam in _cameras)
                    {

                        

                        if (cam.GetCurrentState() == CameraBase.State.OpenIdle)
                            //start aquisition on each camera is they are not in aqusition state
                        {
                            if(Status.SensorDrivenExposure)
                            {
                                cam.DirectlyUpdateSettings(Status.Exposureμs, (float)Status.Gain);                                
                                    
                            }
                            cam.StartAquisition();
                        }
 /////////////Check for closed cameras and handle it!!!!
                    }
                    ushort[][][] temp = new ushort[_cameras.Length][][];

                
                    Parallel.For(0, _cameras.Length, i =>
                    {
                        temp[i] = _cameras[i].TakeImageUshort2D(stopwatchCapture);
                    });
                
                   // CQ.Enqueue(temp); //enque onto a concurrent queue, that serves to pass information to the task
                    bool imagingSucceded = true;

                    foreach(var image in temp)
                        if (image == null)
                            imagingSucceded = false; 

                    if (! imagingSucceded)
                    {
                        //camera failed ot aquire an image, do not procced
                        
                    }
                    else if (Interlocked.Increment(ref _FilesWriting) < 3)
                    {

                        Task _writeToDisk = new Task(() =>
                        {
                            WriteToDisk(temp);
                        });
                        _writeToDisk.Start(); 
                    }
                    else
                    {
                        //skip writing a file
                    }


                    stopwatchCapture.Stop();
                    int delay = (int)(( 1000/Status.FPS) - stopwatchCapture.ElapsedMilliseconds);

                    if (delay > 0)
                    {
                        System.Threading.Thread.Sleep(delay);
                    }
                    else
                    {
                    
                    }
                Status.PertTime[3] = (int)stopwatchCapture.ElapsedMilliseconds;
                }

               
            }
        }


        public static void Start()
        {
            //If the task is waiting to run, run it!
            if (_TcaptueImage.Status == System.Threading.Tasks.TaskStatus.Created ||
                _TcaptueImage.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation ||
                _TcaptueImage.Status == System.Threading.Tasks.TaskStatus.WaitingToRun)
            {
                _TcaptueImage.Start();

            }//If the task is has finished, start it again!
            else if(_TcaptueImage.Status ==  System.Threading.Tasks.TaskStatus.RanToCompletion)         
            {
                _TcaptueImage.ContinueWith((continuation) =>
                {
                    CaptueImage();
                });
            }//If the task is has a fault, then reinitialise cameras and continue!
            else if (_TcaptueImage.Status == System.Threading.Tasks.TaskStatus.Faulted)
            {
                Reinitialise();
                _TcaptueImage.ContinueWith((continuation) =>
                {
                    CaptueImage();
                });
            }//If the task is running fine!, leave it alone!
            else if (_TcaptueImage.Status == System.Threading.Tasks.TaskStatus.Running)
            {
                //Everything is well
            }
        }

        public static void ChangeShift()
        {
            for(int i = 0; i < _cameras.Length; i++)
            {
                 _cameras[i].Xoffset = Status.Xshift[i];
                 _cameras[i].Yoffset = Status.Yshift[i];
            }
            
        }

        public static void ChangeExpCorr()
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].digitCorrection = (float)Status.DigitCorrection[i];
                _cameras[i].expCorrection = (float)Status.ExpCorrection[i];
            }

        }


        private static void WriteToDisk(ushort[][][] allImages)
        {
            stopwatchWrite.Reset();
            stopwatchWrite.Start();
            int bpp =16;

            
            allImages = imageshift(allImages);

            Status.PertTime[4] = (int) stopwatchWrite.ElapsedMilliseconds; 

            if (Status.curBitDepth == Command.BitDepth.Mono8bpp)
            {
                bpp = 8;
            }
            else if (Status.curBitDepth == Command.BitDepth.Mono16Bpp)
            {
                bpp = 16;
            }
            
            
            if (Status.CurImageImageFormat == TelemetryR.ImageFormat.Seperate)
            {
                int i = 1;
                foreach (ushort[][] image in allImages)
                {
                    String path = CurrentPath + "/" + "Camera" + (i).ToString() + " frame" + Status.ImagesTaken.ToString() + ".tiff";
                    Saving.WriteOnechannel(image, path, allImages[0][0].Length, allImages[0].Length, bpp);
                    i++;
                }
                
            }
            else if (Status.CurImageImageFormat == TelemetryR.ImageFormat.Joined)
            {
                String path = CurrentPath + "/" + "Combined frame" + Status.ImagesTaken.ToString() + ".tiff";
                Saving.WriteAllChannelsInterleaved(allImages, path, allImages[0][0].Length, allImages[0].Length, bpp);
            } 
            else if (Status.CurImageImageFormat == TelemetryR.ImageFormat.Three)
            {
                String path = CurrentPath + "/" + "Combined frame" + Status.ImagesTaken.ToString() + ".tiff";
                Saving.WriteThreeChannelsInterleaved(allImages, path, bpp);
            }
            else
                throw new Exception();

            Interlocked.Decrement(ref _FilesWriting);

            Status.PertTime[5] = (int)stopwatchWrite.ElapsedMilliseconds - Status.PertTime[4];
            stopwatchWrite.Stop();

            Status.ImagesTaken++;
        }

        private static ushort[][][] imageshift(ushort[][][] allImages)
        {
            int XshiftMax = 0; //maximum positive shift
            int YshiftMax = 0;

            foreach (CameraBase cam in  _cameras)
            {
                if (cam.Xoffset > XshiftMax)
                    XshiftMax = cam.Xoffset;

                if (cam.Yoffset > YshiftMax)
                    YshiftMax = cam.Yoffset; 

            }


            //Reduces the resolution by the number of bits we must shift the image
            int NewYres = allImages[0].Length  - Math.Abs(YshiftMax);
            int NewXres = allImages[0][0].Length  - Math.Abs(XshiftMax);
            
            ushort[][][] corrected = new ushort[allImages.Length][][];

            for (int cam = 0; cam < allImages.Length; cam++)
            {
                corrected[cam] = new ushort[NewYres][];
                for (int Y = 0; Y < NewYres; Y++)
                {
                    corrected[cam][Y] = new ushort[NewXres];
                    for (int X = 0; X < NewXres; X++)
                    {
                        corrected[cam][Y][X] = allImages[cam][Y + _cameras[cam].Yoffset][X + _cameras[cam].Xoffset];
                    }
                }
            }

            return corrected; 
        }


        public static void QueUpdate()
        {
            _updateDue = 1;
        }
  
     }
 }


