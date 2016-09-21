using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;

namespace HighSpecter.NetworkData
{

    [DataContract]
    public class Command
    {
        /*if the camera is currently imaging, true of false*/
        [DataMember] public volatile bool Active;
        /*Exposure in milliseconds*/
        [DataMember] public int Exposureμs;
        [DataMember] public double FPS;
        [DataMember] public double Gain;
        [DataMember] public double GammaY =1;
        [DataMember] public double GammaC =1;
        [DataMember] public BitDepth curBitDepth;
        [DataMember] public ImageFormat CurImageImageFormat;
        [DataMember] public string Foldername;
        [DataMember] public string LogChanges="";
        [DataMember] public int[] Xshift = new int[4];
        [DataMember] public int[] Yshift = new int[4];
        [DataMember] public double[] ExpCorrection = new double[4];
        [DataMember] public double[] DigitCorrection = new double[4];
        [DataMember] public bool Reinitialise = false;
        [DataMember] public bool Shutdown = false;
        [DataMember] public bool SensorDrivenExposure = false;
        [DataMember] public float SensorResponce= 2; //Exposure (seconds) = Response (coefficient, lux/second to saturation) / Brightness (lux)
        //All in Si units! 


        public enum BitDepth { Mono8bpp, Mono16Bpp};
        public enum ImageFormat { Seperate, Joined, Three };


        public Command(bool active, int exposureμs, double fps, double gain = 0, BitDepth curBitDepth = BitDepth.Mono8bpp, ImageFormat imageformat = ImageFormat.Seperate, string foldername = "")
        {
            Active = active;
            Exposureμs = exposureμs;
            FPS = fps;
            Gain = gain;
            curBitDepth = curBitDepth;
            CurImageImageFormat = imageformat;
            Foldername = foldername;

        }

        

        /*only call from inherited members if you are overwriting the values*/ 
        protected Command()
        {
            

        }
    }
}
