using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO.Pipes;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Threading;
using HighSpecter;
using HighSpecter.NetworkData;



namespace HighSpecter
{

    public partial class UI_NetStatusPanel : Form
    {
        /*Complete Status of the Application, sent over network */
        
        /*Timer that runs on a seperate thread, and delegates to alow updating UI*/
        private static System.Threading.Timer _UsbLightReading, _ImagingTimer, _NetworkTimer;
        private delegate void Display(string text);
        private static Display _updatedisplay1, _updatedisplay2, _updatefeedback;

        private delegate void IOfeedback(int connected, long connections, long sentTelemetry, long recievedCommands, bool newconnection = false);
        private static IOfeedback _iofeedback;

        private delegate void IpConfig(IPAddress ipadress, ushort port);
        private static IpConfig _ipConfig;


        


        // Get host name
        private static String strHostName = Dns.GetHostName();

        // Find host by name
        private static IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
       

        /*Light Sensors*/
        private YLightSensor sensor1;
        private YLightSensor sensor2;



        
      

 

        /* An array that londs all Labels*/
        private ScalersStructure[] _scalersStructure = new ScalersStructure[28];
       
        struct ScalersStructure
        {
            public UIRescaler scaler;
            public Control subject; 
        }


        /* Delegates to update UI on command*/
        private delegate void UiCommandUpdate(Command cmd);
        private static UiCommandUpdate _cmdUpdate;

        

        /*setup timers, connections and variables*/
        public UI_NetStatusPanel()
        {
            
            string errmsg = "";
            InitializeComponent();


            //Setup scalers and controls that must be scaled
            UIsetup();
            

            _cmdUpdate = Update_CommandUI;
            _iofeedback = Update_IO_Displays;
            
            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
            {
                set_display1("error: " + errmsg);
                Logging.Add("Cant initiaet Yocto API, do you have another instance running?");
            }
            else
            {
                Logging.Add("Yapi Initiated");
            }

            //Set Sensors 1 and 2
            sensor1 = YLightSensor.FindLightSensor("L1" + ".lightSensor");
            if(sensor1 == null)
            { 
                set_display1("no L1 sensor");
            }
            else
            { 
                set_display1("L1 Ok");
            }

            sensor2 = YLightSensor.FindLightSensor("L2" + ".lightSensor");
            if (sensor2 == null)
            { set_display2("no L2 sensor"); Logging.Add("L1 not connected"); }
            else
            { set_display2("L2 Ok");  }

            /*delegates for ui updates*/
            _updatedisplay1 = (set_display1);
            _updatedisplay2 = (set_display2);
            _updatefeedback = (Update_feedback);
            _ipConfig = Update_IP_Config;

            Logic.Initialise_Cameras();

         
            Servlet.Userinterface = this; ///assigning treference to ui for the servlets to conduct updates. This needs to be changed! 

            Logic.Start();
            
            //start a timer
            _UsbLightReading = new System.Threading.Timer(Lsensor_Tick, null, 0, 1000);

            //start a timer
            _NetworkTimer = new System.Threading.Timer(Nmanager.Tick, null, 0, 1000);
         }

        

        private int[] readGPS()
        {
            throw new Exception("GPS not implemented");
        }



        

        /*Is called when a frame is recieved from network. Updates accoring to received settings*/
        public void Update_Command(Command recieved)
        {
            Logging.Add(recieved.LogChanges);

            if (Logic.Status.Active == false && recieved.Active == true)   //triggers if currently cameras are not active, and they are commanded to turn on          
            {
                
                //if user provided no 
                if (recieved.Foldername.Length == 0)
                {
                    DateTime folderstamp = DateTime.Now;
                    Logic.CurrentPath = Logic.StaticPath + folderstamp.Hour.ToString() + "." + folderstamp.Minute.ToString() + " on " + folderstamp.Day.ToString() + "." + folderstamp.Month.ToString();
                                    }
                else
                { Logic.CurrentPath = Logic.StaticPath + recieved.Foldername; }
                

                if(System.IO.Directory.Exists(Logic.CurrentPath))
                {
                    //if folder exists do nothign
                }
                else
                    {System.IO.Directory.CreateDirectory(Logic.CurrentPath);}

                Logging.Add("Starting imaging, saving to " + Logic.CurrentPath);
                Logic.Status.Active = recieved.Active;
                Logic.Start();
            }
            else if (Logic.Status.Active == false && recieved.Active == false)
            { }
            else if (Logic.Status.Active == true && recieved.Active == false)
            { Logic.Status.Active = recieved.Active; Logging.Add("Stopping Imaging"); }
            else if (Logic.Status.Active == true && recieved.Active == true)
            { }
            else
            {
                Logging.Add("Status.Imaging or cmd.Imaging is undefined! This is imposible! ");
                throw new Exception("Status.Imaging or cmd.Imaging is undefined! This is imposible! "); 
            }

            if (Logic.Status.Gain != recieved.Gain)
            {
                Logging.Add("Gain changed to " + recieved.Gain.ToString());
                Logic.Status.Gain = recieved.Gain;
            }
            else { }

            //Set exposure, but only if sersor drive exposure is disabled
            Logic.Status.SensorDrivenExposure = recieved.SensorDrivenExposure;
            Logic.Status.SensorResponce = recieved.SensorResponce;
            if (Logic.Status.SensorDrivenExposure)
            {
                //do nothing, exposure will be set by sensors
            }
            else
            {
                if (Logic.Status.Exposureμs != recieved.Exposureμs)
                {
                    Logging.Add("Exposure changed to " + recieved.Exposureμs.ToString());
                    Logic.Status.Exposureμs = recieved.Exposureμs;
                }
            }

            if (Logic.Status.FPS != recieved.FPS)
            {
                Logging.Add("FPS changed to " + recieved.FPS.ToString());
                Logic.Status.FPS = recieved.FPS;
            }
            else { }

            if (Logic.Status.curBitDepth != recieved.curBitDepth)
            {
                Logging.Add("curBitDepth changed to " + recieved.curBitDepth.ToString() );
                Logic.Status.curBitDepth = recieved.curBitDepth;
            }
            else { }

            if (Logic.Status.CurImageImageFormat != recieved.CurImageImageFormat)
            {
                Logging.Add("CurImageImageFormat changed to " + recieved.CurImageImageFormat.ToString() );
                Logic.Status.CurImageImageFormat = recieved.CurImageImageFormat; 
            }

            bool shiftChanged = false;
            bool correctionUpdated = false; 
            for (int i = 0; i < recieved.Xshift.Length; i++ )
            {
                if (recieved.Xshift[i] != Logic.Status.Xshift[i] || recieved.Yshift[i] != Logic.Status.Yshift[i])
               {
                   shiftChanged = true;
               }
                if(recieved.ExpCorrection[i] != Logic.Status.ExpCorrection[i] || recieved.DigitCorrection[i] != Logic.Status.DigitCorrection[i])
                {
                    correctionUpdated = true;
                }
            }

            if (shiftChanged)
            {
                Array.Copy(recieved.Xshift, Logic.Status.Xshift, Logic.Status.Xshift.Length);
                Array.Copy(recieved.Yshift, Logic.Status.Yshift, Logic.Status.Yshift.Length);
                Logging.Add("image Shift changed");
                Logic.ChangeShift();
            }
            if (correctionUpdated)
                {
                    Array.Copy(recieved.ExpCorrection, Logic.Status.ExpCorrection, Logic.Status.ExpCorrection.Length);
                    Array.Copy(recieved.DigitCorrection, Logic.Status.DigitCorrection, Logic.Status.DigitCorrection.Length);
                    Logging.Add("Exposure correction changed");
                    Logic.ChangeExpCorr();
                }
            

            if(recieved.Reinitialise)
                Logic.Reinitialise();
            if(recieved.Shutdown)
                Shutdown2();
            

            UI_DisplayImagging.Invoke(_cmdUpdate, recieved);

            Logic.QueUpdate(); //tells it to update settings before continuing taking imgaes. 

           /*if imaging is true, sets time, if imaging is false then sets timeout to infinite, disabling it */
            
        }

        /* Updates the UI with Command paratemers */
        private void Update_CommandUI(Command recieved)
        {
            UI_DisplayImagging.Text = recieved.Active.ToString();
            UI_DisplayFps.Text = recieved.FPS.ToString();
            UI_DisplayExposure.Text = ((recieved.Exposureμs / 1000).ToString() + " ms");
        }

        /*provides Feedback*/ 
        public void Update_feedback(string text)
        {
            if (UI_Feedback.InvokeRequired)
                UI_Feedback.Invoke(_updatefeedback, text);
            else
            {
                UI_Feedback.Text = text;
            }
        }

        /*updates connectionIO. If yolu do not wish to update some values, input negatives and they will be ignored
         for connected, 0 is false, 1 is true*/
        public void Update_IO_Displays(int connected, long connections, long sentTelemetry, long recievedCommands, bool newconnection = false)
        {
            if (newconnection)
            {
                Logging.Add("Client N " + connected.ToString() + " connected");
                Logic.Status.LogChanges = Logging.GetFullLog();
            }
            else{}

            if (UI_Feedback.InvokeRequired)
                UI_Feedback.Invoke(_iofeedback, connected, connections, sentTelemetry, recievedCommands,false);
            else
            {
                if(connected >= 0)
                    UI_DisplayConnectionStatus.Text =  connected==0 ? "No" : "Yes";
                if (connections > 0)
                    UI_DisplayNetworkConnections.Text = connections.ToString();
                if (sentTelemetry > 0)
                    UI_DisplaySentTelemetry.Text = sentTelemetry.ToString();
                if (recievedCommands > 0)
                    UI_DisplayRecievedComms.Text = recievedCommands.ToString();
            }
        }

        public void Update_IP_Config(IPAddress address, ushort port)
        {
            if (UI_Feedback.InvokeRequired)
                UI_Feedback.Invoke(_ipConfig, address, port);
            else
            {
                UI_DisplayPort.Text = port.ToString();
                UI_DisplayIpAdress.Text = address.ToString(); 
            }
        }

        /* Places lightsensor Readings into Status, which is a TelemetryR cariable
         * If no lightsensor is connected, -1 is written
         * For some reason this is the netowrking thread as well*/
        private void Lsensor_Tick(Object sender)
        {            
                double[] temp = new double[2];
                if (!sensor1.isOnline())
                {
                    UI_displayL1.Invoke(_updatedisplay1, "L1 N/A!");
                    temp[0] = -1;
                }
                else
                {
                    temp[0] = sensor1.get_currentValue();
                    UI_displayL1.Invoke(_updatedisplay1, temp[0] + " lx");
                }

                if (!sensor2.isOnline())
                {
                    UI_displayL2.Invoke(_updatedisplay2, "L2 N/A!");
                    temp[1] = -1;
                }
                else
                {
                    temp[1] = sensor2.get_currentValue();
                    UI_displayL2.Invoke(_updatedisplay2, temp[1] + " lx");
                }
                Logic.Status.Lsensors = temp;
        }

        private void  set_display1 (string text)
        {
            UI_displayL1.Text = text;
        }

        private void set_display2 (string text)
        {
            UI_displayL2.Text = text;
        }

        private void Resized(object sender, EventArgs e)
        {
            try
            {
                foreach (var scaler in _scalersStructure)
                {
                    if (scaler.subject != null)
                    {
                        scaler.scaler.Scale(scaler.subject);
                        scaler.subject.Font = scaler.scaler.get_Font();
                        scaler.subject.Margin = scaler.scaler.get_Margin();
                        scaler.subject.Padding = scaler.scaler.get_Padding();
                        scaler.subject.Size = scaler.scaler.get_Size();
                        scaler.subject.Location = scaler.scaler.get_Location();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.Add(exception.ToString() + exception.Message);

            }

        }

        /*Initialises 0th scaler to headings and 1st scalers to displays */
        private void UIsetup()
        {
            /*
           int i=0;
           foreach (Control control in this.Controls)
           {
               SetStruct(i, control);
               i++;
           } * */

           SetStruct(0, UI_LabelLight); 
           SetStruct(1, UI_LabelGPS);
           SetStruct(2, UI_displayL1);
           SetStruct(3, UI_displayL2);
           SetStruct(4, UI_displayLattitude);
           SetStruct(5, UI_displayLongitude);
           SetStruct(6, UI_Feedback);
           SetStruct(7, UI_DisplayConnectionStatus);
           SetStruct(   8   ,      UI_StatusPanel   );
           SetStruct(   9   ,     UI_DisplayFps );
           SetStruct(10, UI_LabelFps);
           SetStruct(11, UI_DisplayExposure);
           SetStruct(12, UI_LabelExposure);
           SetStruct(13, UI_DisplayImagging);
           SetStruct(14, UI_LabelImaging);
           SetStruct(15, UI_LabelConnectonStatus);
           SetStruct(16, UI_LabelNetworkConnections);
           SetStruct(17, UI_LabelRecievedComms);
           SetStruct(18, UI_LabelSentTelemetry);
           SetStruct(19, UI_DisplayNetworkConnections);
           SetStruct(20, UI_DisplayRecievedComms);
           SetStruct(21, UI_DisplaySentTelemetry);
           SetStruct(22, UI_PanelNetworkCommunication);
           SetStruct(23, UI_DisplayIpAdress);
           SetStruct(24, UI_DisplayPort);
           SetStruct(25, UI_LabelPort);
           SetStruct(26, UI_LabelIPAdress);
           SetStruct(27, UI_Panel_NetworkSettings);
        }

        /*Structure assigner, reduces amount of errors*/
        private void SetStruct(int index, Control it)
        {
            _scalersStructure[index].scaler = new UIRescaler(it);
            _scalersStructure[index].subject = it;
        }

        private void flowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UI_displayL1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


       


        private static void Shutdown2()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

             // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", 
                                               mboShutdownParams, null);
            }
        }

        private static void Shutdown()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }


    }
}
