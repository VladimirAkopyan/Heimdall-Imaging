using System;
using System.Runtime.Serialization;
using System.Threading;

namespace HighSpecter.NetworkData
{
    /* class allows creating a Data item and incrementally updating it, and keep everythign timestamped
     * should be used as static. No need for threadlocks, and there should never be two threads updating same value*/
    [DataContract]
    public class TelemetryR : Command
    {

        /*Timestamped Variables - public Interfaces create a new timestamp every time a value is changed*/
        [DataMember]
        private double [] _telemetry = new double [4]; 
        [DataMember]
        private DateTime _timestamp;
        [DataMember]
        private short _nOfcameras;
        [DataMember]
        public int ImagesTaken; 
        [DataMember]
        public int[] PertTime = new int[6]; 



        /*Constructor, sets everything*/
        public TelemetryR(double Lsensor1, double Lsensor2, double Longitude, double Latitude, bool active, double fps, int exposure)
        {
            _timestamp = DateTime.Now;
            _telemetry[0] = Lsensor1;
            _telemetry[1] = Lsensor2;
            _telemetry[2] = Longitude;
            _telemetry[3] = Latitude;
            Active = active;
            Exposureμs = exposure;
            FPS = fps;
        }

        /* Status-only constructor */
        public TelemetryR(bool imaging, int exposure, double fps)
        {
            _telemetry = new double []{-1,-1,-1,-1};
            Active = imaging;
            Exposureμs = exposure;
            FPS = fps;
        }

        /* Empty Constructor*/
        public TelemetryR()
        {

        }

        [DataMember]
        public short Cameras { get { return _nOfcameras; } set { _nOfcameras = value; _timestamp = DateTime.Now; } }

        [DataMember]
        /*only set this as an array!*/
        public double[] Lsensors
        {
            get
            {
                var temp = new double[2];
                temp[0] = _telemetry[0];
                temp[1] = _telemetry[1];
                return temp;
            }
            set
            {
                _telemetry[0] = value[0];
                _telemetry[1] = value[1];
                _timestamp = DateTime.Now;
            }
        }

        [DataMember]
        public double Longitude
        {
            get { return _telemetry[2]; }
            set { _telemetry[2] = value; _timestamp = DateTime.Now; }
        }

        [DataMember]
        public double Latitude
        {
            get
            {
                return _telemetry[3];
            }
            set
            {
                _telemetry[3] = value;
                _timestamp = DateTime.Now;
            }
            
        }

        [DataMember]
        public DateTime Timestap
        {
            get { return _timestamp; }
            set { _timestamp = DateTime.Now; }
        }

    }
}
