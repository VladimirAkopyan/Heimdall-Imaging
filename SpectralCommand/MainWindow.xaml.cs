    using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using HighSpecter.NetworkData;
using SpectralCommand.Properties;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Collections.Generic;

namespace SpectralCommand
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum State
        {
            Idle,
            Connecting,
            ConnectedOff,
            ConnectedOn,
            Error
        }

        /* programm state tracker*/ 
        public static State _gstatus = State.Idle;

        private static NConnection Network;

        private static readonly Command User_input = new Command(false, 10, 1); /*tracks the settings user has set*/

        public string Feedback = ""; //Written along the bottomn of the programm, it's of questionable use

        //Regex are used to validate input for checkboxes
        private Regex _RegexAlphanumeric = new Regex("[a-zA-Z0-9,.]");
        private Regex _RegexNumeric = new Regex("[0-9.]"); 

        /*Used to distinguis between user causing UI events to fire, and code changing the UI*/
        private static bool InternalChange = false;

        private static bool UI_loaded= false; 
        

        private ImageSource IconConnected = (ImageSource)new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources\Connected.png");
        private ImageSource IconDisconnected = (ImageSource)new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources\NoConnection.png");


        public MainWindow()
        {
            InitializeComponent();
            
            Network = new NConnection(this);

            LoadSettings();
            UI_loaded = true; 
        }

        private void LoadIcons()
        {

        }


        /* when Connect is licked, this happens*/
        private void TryToConnect(object sender, RoutedEventArgs e)
        {
            UI_Connect_Button.IsEnabled = false;
            State update;

            switch (_gstatus)
            {
                case State.Idle:
                    try
                    {
                        Network.Connect((UI_IP_Input.Text), ushort.Parse(UI_Port_Input.Text));
                        Feedback = "Attempting to connect";
                        update = State.Connecting;
                        SaveSettings();
                    }
                    catch (Exception exception)
                    {
                        Feedback = exception.Message;
                        update = State.Idle;
                    }

                    break;

                case State.Connecting:
                case State.ConnectedOff:

                    Feedback = "Connection Closed";
                    Network.Reset();
                    update = State.Idle;
                    break;

                default:
                    Feedback = "Error";
                    update = State.Error;
                    break;
            }
            _gstatus = update;
            UpdateUi(Feedback);
            UI_Connect_Button.IsEnabled = true;
        }

        /* Saves user input for next session 
         pretty much useless*/
        private void SaveSettings()
        {
            if (UI_loaded)
            {
                Settings.Default.IP_Address = UI_IP_Input.Text;
                Settings.Default.Port = UI_Port_Input.Text;
                Settings.Default.FolderName = UI_FoldernameInput.Text;
                Settings.Default.BItDepth = UI_selectBitDepth.SelectedIndex;
                Settings.Default.Save_format = UI_SelectFormat.SelectedIndex;
                Settings.Default.SensorResponce = User_input.SensorResponce;
                Settings.Default.SensorDrivenExposure = User_input.SensorDrivenExposure; 

                for (int i = 0; i < 4; i++ )
                {
                    Settings.Default.Xshift[i] = User_input.Xshift[i];
                    Settings.Default.Yshift[i] = User_input.Yshift[i];
                    Settings.Default.ExpCorrection[i] = User_input.ExpCorrection[i];
                    Settings.Default.DigitCorrection[i] = User_input.DigitCorrection[i]; 
                }
                    
                
                Settings.Default.Save();
            }
            else
            {
                //do nothing
            }
        }

        /*obviously loads settings */
        private void LoadSettings()
        {
            UI_IP_Input.Text = Settings.Default.IP_Address;
            UI_Port_Input.Text = Settings.Default.Port;
            UI_FeedbackGain.Text = Settings.Default.Gain.ToString();
            UI_setGain.Value = Settings.Default.Gain;

            UI_FeedbackImageFreq.Text = Settings.Default.FPS.ToString();
            UI_setImagingFreq.Value = Settings.Default.FPS;
            UI_FeedbackExposure.Text = Settings.Default.Exposure.ToString();

            UI_SensorDrivenExposure.IsChecked = Settings.Default.SensorDrivenExposure;
            UI_SensorCoefifcient.Text = Settings.Default.SensorResponce.ToString(); 

            if(Settings.Default.Xshift == null) //if this is the first time porgamm is started on the system, add four 
            {
                Settings.Default.Xshift = new System.Collections.ArrayList(); 
                Settings.Default.Xshift.Add(0);
                Settings.Default.Xshift.Add(0);
                Settings.Default.Xshift.Add(0);
                Settings.Default.Xshift.Add(0);
            }
           
            Xcorrection0.Text = Settings.Default.Xshift[0].ToString();
            Xcorrection1.Text = Settings.Default.Xshift[1].ToString();
            Xcorrection2.Text = Settings.Default.Xshift[2].ToString();
            Xcorrection3.Text = Settings.Default.Xshift[3].ToString();

            if (Settings.Default.Yshift == null) //if this is the first time porgamm is started on the system, add four 
            {
                Settings.Default.Yshift = new System.Collections.ArrayList();
                Settings.Default.Yshift.Add(0);
                Settings.Default.Yshift.Add(0);
                Settings.Default.Yshift.Add(0);
                Settings.Default.Yshift.Add(0);
            }
            Ycorrection0.Text = Settings.Default.Yshift[0].ToString();
            Ycorrection1.Text = Settings.Default.Yshift[1].ToString();
            Ycorrection2.Text = Settings.Default.Yshift[2].ToString();
            Ycorrection3.Text = Settings.Default.Yshift[3].ToString();

            if (Settings.Default.ExpCorrection == null) //if this is the first time porgamm is started on the system, add four 
            {
                Settings.Default.ExpCorrection = new System.Collections.ArrayList();
                Settings.Default.ExpCorrection.Add(1d);
                Settings.Default.ExpCorrection.Add(1d);
                Settings.Default.ExpCorrection.Add(1d);
                Settings.Default.ExpCorrection.Add(1d);
            }
            ExpCorr0.Text = Settings.Default.ExpCorrection[0].ToString();
            ExpCorr1.Text = Settings.Default.ExpCorrection[1].ToString();
            ExpCorr2.Text = Settings.Default.ExpCorrection[2].ToString();
            ExpCorr3.Text = Settings.Default.ExpCorrection[3].ToString();

            if (Settings.Default.DigitCorrection == null) //if this is the first time porgamm is started on the system, add four 
            {
                Settings.Default.DigitCorrection = new System.Collections.ArrayList();
                Settings.Default.DigitCorrection.Add(1d);
                Settings.Default.DigitCorrection.Add(1d);
                Settings.Default.DigitCorrection.Add(1d);
                Settings.Default.DigitCorrection.Add(1d);
            }
            DigCorr0.Text = Settings.Default.DigitCorrection[0].ToString();
            DigCorr1.Text = Settings.Default.DigitCorrection[1].ToString();
            DigCorr2.Text = Settings.Default.DigitCorrection[2].ToString();
            DigCorr3.Text = Settings.Default.DigitCorrection[3].ToString();

            UI_selectBitDepth.SelectedIndex = Settings.Default.BItDepth;
            UI_SelectFormat.SelectedIndex = Settings.Default.Save_format; 
            UI_FoldernameInput.Text = Settings.Default.FolderName ;
            
        }

        /* Triggers when state of the programm is changed
         * updates teh feedback box and Button text
         * Toggles state of the button, based on whether TCPclient is connected*/
        public void UpdateUi(string feedback = null, bool newconnection = false)
        {
            if (newconnection)
                UI_LogTextBlock.Text = "";
            else { }

            string buttontext;
            if(feedback != null)
                UI_Feedback.Content = feedback;


            switch (_gstatus)
            {
                case State.Idle:
                    buttontext = "Connect";
                    ProgressbarAnimation(0, 0.5);
                    UI_Image_Button.IsEnabled = false;
                    UI_Port_Input.IsReadOnly = false;
                    UI_IP_Input.IsReadOnly = false;
                    UI_Apply_Button.IsEnabled = false;
                    UI_image_connection.Source = IconDisconnected;
                    UI__Reinitialise_Button .IsEnabled= false;
                    UI__Shutdown_button.IsEnabled = false;
                    UI__AddtoLog_Button.IsEnabled = false;
                    UI__Reinitialise_Button.IsEnabled = false;
                    UI__Shutdown_button.IsEnabled = false; 
                    break;
                case State.Connecting:
                    ProgressbarAnimation(100, 21);
                    buttontext = "Cancel";
                    UI_Image_Button.IsEnabled = false;
                    UI_Port_Input.IsReadOnly = true;
                    UI_IP_Input.IsReadOnly = true;
                    UI_Apply_Button.IsEnabled = false;
                    UI_image_connection.Source = IconDisconnected; 
                    UI__Reinitialise_Button .IsEnabled= false;
                    UI__Shutdown_button.IsEnabled = false;
                    UI__AddtoLog_Button.IsEnabled = false;
                    UI__Reinitialise_Button.IsEnabled = false;
                    UI__Shutdown_button.IsEnabled = false;

                    break;
                case State.ConnectedOff:
                    UI_Port_Input.IsReadOnly = true;
                    UI_IP_Input.IsReadOnly = true;
                    UI_Progress.Value = 100;
                    buttontext = "Disc.";
                    ProgressbarAnimation(100, 0.5);
                    UI_Image_Button.IsEnabled = true;
                    UI_Apply_Button.IsEnabled = true;
                    UI_image_connection.Source = IconConnected;
                    UI__Reinitialise_Button .IsEnabled= true;
                    UI__Shutdown_button.IsEnabled = true;
                    UI__AddtoLog_Button.IsEnabled = true;
                    UI__Reinitialise_Button.IsEnabled = true;
                    UI__Shutdown_button.IsEnabled = true;
                    break;
                case State.ConnectedOn:
                    UI_Port_Input.IsReadOnly = true;
                    UI_IP_Input.IsReadOnly = true;
                    UI_Progress.Value = 100;
                    buttontext = "Disc.";
                    ProgressbarAnimation(100, 0.5);
                    UI_Image_Button.IsEnabled = true;
                    UI_Apply_Button.IsEnabled = true;
                    UI_image_connection.Source = IconConnected;
                    UI__Reinitialise_Button .IsEnabled= true;
                    UI__Shutdown_button.IsEnabled = true;
                    UI__AddtoLog_Button.IsEnabled = true;
                    UI__Reinitialise_Button.IsEnabled =false;
                    UI__Shutdown_button.IsEnabled = false;
                    break;
                default:
                    buttontext = "Error";
                    UI_Image_Button.IsEnabled = false;
                    break;
            }

            UI_Connect_Button.Content = buttontext;
        }

        //this is called when an update frame is recieved from network
        public void UpdateUi(TelemetryR recieved)
        {
            if (recieved.Lsensors[0] >= 0)
                UI_DisplayLight1.Content = recieved.Lsensors[0];
            else
                UI_DisplayLight1.Content = "N/A";
            if (recieved.Lsensors[1] >= 0)
                UI_DisplayLight2.Content = recieved.Lsensors[1];
            else
                UI_DisplayLight2.Content = "N/A";
            if (recieved.Lsensors[1] >= 0 && recieved.Lsensors[0] >= 0)
            {
                UI_DisplayLightAVG.Content = (recieved.Lsensors[0] + recieved.Lsensors[1]) / 2;
            }
            else
            {
                UI_DisplayLightAVG.Content = "N/A";
            }

            User_input.Active = recieved.Active;
            UI_Image_Button.Content = User_input.Active ? "Stop" : "Start";
            UI_RemoteExposure.Content = Math.Round(recieved.Exposureμs / 1000d, 3) + " ms";
            UI_RemoteFps.Content = recieved.FPS + " fps";
            UI_RemoteGain.Content = recieved.Gain + " db";
            UI_Cameras_Connected.Content = recieved.Cameras.ToString() + " cams";
            UI_ImagesTaken.Content = recieved.ImagesTaken.ToString() + " imgs";
            
            UI_LogTextBlock.Text += recieved.LogChanges;

            //Update the perofrmance tab
            UI_PerfTUpdate.Content = recieved.PertTime[0].ToString() + " ms";
            UI_PerfTaqusition.Content = recieved.PertTime[1].ToString() + " ms";
            UI_PerfCopy.Content = recieved.PertTime[2].ToString() + " ms";
            UI_PerfCaptureTotal.Content = recieved.PertTime[3].ToString() + " ms";
            UI_PerfCorrect.Content = recieved.PertTime[4].ToString() + " ms";
            UI_PerfWrite.Content = recieved.PertTime[5].ToString() + " ms";

            if(recieved.Active)
                _gstatus = State.ConnectedOn;
            else
            {
                _gstatus = State.ConnectedOff;
            }
            UpdateUi();

        }

        /*Executes asyncronously, when TCPconnection results in success or failure */
        private void ProgressbarAnimation(int value, double durationS)
        {
            var duration = new Duration(TimeSpan.FromSeconds(durationS));
            var doubleanimation = new DoubleAnimation(value, duration);
            UI_Progress.BeginAnimation(RangeBase.ValueProperty, doubleanimation);
        }


        /* Send Data! */

        private void Start_Imaging(object sender, RoutedEventArgs e)
        {
            User_input.Active = !User_input.Active; // Toggle Imaging
            User_input.Foldername = UI_FoldernameInput.Text; 

            UI_Image_Button.Content = User_input.Active ? "Stop" : "Start"; //Toggle button state
            
            if (Network != null && (_gstatus == State.ConnectedOff || _gstatus == State.ConnectedOn))
                Network.send_data(User_input);
            else
            { //Networking is not initialised

            }
            //Send the command
        }

        private void FPS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = updateTextboxFromSlider(UI_setImagingFreq, UI_FeedbackImageFreq);
            value = Math.Round(value, 1);
            if (value >= 0)
            {
                Settings.Default.FPS = value;
                Settings.Default.Save();

                User_input.FPS = value;
            }
            else
            { //Invalid Value, do nothing
            }; 
        }

        private void UI_FeedbackFPS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender; //cast the calling object as a textbox
            inputcontrol(textbox, _RegexNumeric);

            double value = updateSliderFromTextbox(UI_FeedbackImageFreq, UI_setImagingFreq, 10d, 0d);
            if (value >= 0)
            {
                Settings.Default.FPS = value;
                Settings.Default.Save();

                User_input.FPS = value;
            }
            else
            { //Invalid Value, do nothing
            }
        }

        private void Gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = updateTextboxFromSlider(UI_setGain, UI_FeedbackGain);
            value = Math.Round(value, 1);
            if (value >= 0)
            {
                Settings.Default.Gain = value;
                Settings.Default.Save();

                User_input.Gain = value;
            }
            else
            { //Invalid Value, do nothing
            }
        }

        private void UI_FeedbackGain_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender; //cast the calling object as a textbox
            inputcontrol(textbox, _RegexNumeric);

            double value = updateSliderFromTextbox(UI_FeedbackGain, UI_setGain, 15d, 0d);
            if (value >= 0)
            {
                Settings.Default.Gain = value;
                Settings.Default.Save(); 

                User_input.Gain = value; 
            }
            else
            { //Invalid Value, do nothing
            }
            
        }

        /*This function rescales exposure slider to exponential scale
 * displays text in UI_FeedbakcExposure and caves the parameter to Command */
        private void Exposure_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (InternalChange == true)
            {
                InternalChange = false;
            }
            else
            {
                var temp = (Math.Pow(10, e.NewValue));
                User_input.Exposureμs = (int) temp;

                var ExposureDisplayMs = Math.Round(temp / 1000, 2);

                if (UI_FeedbackExposure != null)
                {
                    UI_FeedbackExposure.Text = ExposureDisplayMs.ToString();
                    Settings.Default.Exposure = ExposureDisplayMs;
                    Settings.Default.Save();
                }
            }
        }

        private void UI_FeedbackExposure_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            double max = 1000000;
            double min = 0.000001; //Min Max values in ms

            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender; //cast the calling object as a textbox
            inputcontrol(textbox, _RegexNumeric);

            if (InternalChange == true)
            {
                InternalChange = false;
            }
            else if (textbox != null && UI_setExposure != null)
            {
                try
                {
                    double temp = 1000 * Double.Parse(textbox.Text);

                    if (temp > max)
                    { 
                        System.Media.SystemSounds.Exclamation.Play();
                        temp = max;
                    }
                    else if (temp < min)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        temp = min;
                    }


                    double sliderPosition = Math.Log(temp, 10);
                    // update the slider
                    InternalChange = true;
                    UI_setExposure.Value = sliderPosition;
                    InternalChange = false;


                    User_input.Exposureμs = (int)temp; //save to settings

                    Settings.Default.Exposure = Math.Round(temp / 1000, 2);
                    Settings.Default.Save();

                }
                catch (FormatException exception)
                {
                    UI_Feedback.Content = "Invalid ImageFormat";
                }
            }
            else
            {
                //UI is not initialised yet
            }
            
        }

        /* Takes value from textbox and updates the slider and command variable, has no side effect and does not deal with the network, returns -1 for invalid user input */
        private double updateSliderFromTextbox (System.Windows.Controls.TextBox textbox, System.Windows.Controls.Slider slider, Double upperLimit, Double lowerLimit) 
        {
            if(InternalChange == true)
            {
                InternalChange = false; 
                return -1; //this was caused by internal change, no action required
            }                
            else
            {   //check that UI is fully initialised
                if(slider != null && textbox != null)
                {
                    try
                    {
                        Double temp = Double.Parse(textbox.Text);
                        if(temp > upperLimit)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            temp = upperLimit;
                        }
                        else if (temp < lowerLimit)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            temp = lowerLimit;
                        }
                        else
                        {
                            //value withing the bounds, continue happily
                        }



                        // update the slider
                        if (slider.Value != Math.Round(temp, 0))
                        {
                            InternalChange = true;
                            slider.Value = Math.Round(temp, 0);
                        }
                        else
                        {
                            //changed a decimal place only, no need to update the slider
                        }


                        return temp;
                    }
                    catch(FormatException exception)
                    {
                        
                        UI_Feedback.Content = "Invalid ImageFormat";
                         return -1;
                    }                    
                }
                else
                {
                    return -1;
                }
                
            }
        }


        /* Takes value from slider and updates the textbox */
        private double updateTextboxFromSlider(System.Windows.Controls.Slider slider, System.Windows.Controls.TextBox textbox)
        {
            if (InternalChange == true)
            {
                InternalChange = false;
                return -1; //this was caused by internal change, no action required
            }
            else
            {   //check that UI is fully initialised
                if (slider != null && textbox != null)
                {
                    InternalChange = true;
                    double temp = Math.Round(slider.Value, 1);
                    textbox.Text = temp.ToString();


                    return slider.Value;
                }
                else
                {
                    return -1;
                }

            }
        }

        private void UI_BitDepthSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {


            System.Windows.Controls.ComboBoxItem item = (System.Windows.Controls.ComboBoxItem)UI_selectBitDepth.SelectedItem;

            if(item == UI_cItem_8bit)
            {
                User_input.curBitDepth = Command.BitDepth.Mono8bpp;
            }
            else if(item == UI_cItem_16bit)
            {
                User_input.curBitDepth = Command.BitDepth.Mono16Bpp;
            }
            else
            {
                string debug = UI_selectBitDepth.SelectedItem.ToString();
                throw new Exception("Following bit-depth format is not implemented:" + debug);
            }
 
            SaveSettings();
        }

        private void UI_FormatSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBoxItem item = (System.Windows.Controls.ComboBoxItem)UI_SelectFormat.SelectedItem;
            
            if (item == UI_cItem_combined)
            {
                User_input.CurImageImageFormat = Command.ImageFormat.Joined;
            }
            else if (item == UI_cItem_seperate)
            {
                User_input.CurImageImageFormat = Command.ImageFormat.Seperate;               
            }
            else if (item == UI_cItem_ThreeChan)
            {
                User_input.CurImageImageFormat = Command.ImageFormat.Three;
            }
            else
            {
                string debug = UI_SelectFormat.SelectedItem.ToString();
                throw new Exception("Following bit-depth format is not implemented:" + debug);
            }
            
            SaveSettings(); 
        }

        //Compares the text in a textbox to a regex and deletes the character that's invalid + makes a system sound
        private void inputcontrol(System.Windows.Controls.TextBox textbox, Regex regex)
        {
            if (textbox.Text.Length > 0)
            {
                string a = textbox.Text;
                string validated = "";
                int i = 0;

                foreach (Match m in regex.Matches(a))
                {
                    validated += m;
                    i++;
                }

                if (i < a.Length)
                {
                    textbox.Text = validated;
                    System.Media.SystemSounds.Exclamation.Play();
                }
                else
                {
                    //all input is valid
                }
            }
            else
            {
                //Textbox is empty, nothing to do here
            }

        }

        private void AlphanumericTextboxInputCheck(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {            
            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender; //cast the calling object as a textbox
            inputcontrol(textbox, _RegexAlphanumeric);
            SaveSettings();
        }

        private void NumericTextboxInputCheck(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textbox = (System.Windows.Controls.TextBox)sender; //cast the calling object as a textbox
            inputcontrol(textbox, _RegexNumeric);
            SaveSettings();
            
        }

        private void Send_Setting(object sender, RoutedEventArgs e)
        {
            User_input.Foldername = UI_FoldernameInput.Text; //update the folder 
            SaveSettings();

            if (Network != null && (_gstatus == State.ConnectedOff || _gstatus == State.ConnectedOn))
                Network.send_data(User_input);
            else
            { //Networking is not initialised

            }
        }

        private void Shutdown(object sender, RoutedEventArgs e)
        {
            User_input.Shutdown = true;
            Send_Setting(sender, e);
            User_input.Shutdown = false;
        }

        private void Reinit(object sender, RoutedEventArgs e)
        {
            User_input.Reinitialise = true;
            Send_Setting(sender, e);
            User_input.Reinitialise = false;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void AddtoLog(object sender, RoutedEventArgs e)
        {
            lock (User_input.LogChanges)
            {
                User_input.LogChanges += UI_Textbox_AddtoLog.Text;
                UI_Textbox_AddtoLog.Text = "";
                Network.send_data(User_input);
                User_input.LogChanges = "";
            }
        }

        private void UI_LogTextBlock_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void UI_Xcorrection_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                Double temp = Double.Parse(Xcorrection0.Text);
                User_input.Xshift[0] = (int) temp;
                temp = Double.Parse(Xcorrection1.Text);
                User_input.Xshift[1] = (int)temp;
                temp = Double.Parse(Xcorrection2.Text);
                User_input.Xshift[2] = (int)temp;
                temp = Double.Parse(Xcorrection3.Text);
                User_input.Xshift[3] = (int)temp;

                SaveSettings();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void UI_Ycorrection_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                Double temp = Double.Parse(Ycorrection0.Text);
                User_input.Yshift[0] = (int)temp;
                temp = Double.Parse(Ycorrection1.Text);
                User_input.Yshift[1] = (int)temp;
                temp = Double.Parse(Ycorrection2.Text);
                User_input.Yshift[2] = (int)temp;
                temp = Double.Parse(Ycorrection3.Text);
                User_input.Yshift[3] = (int)temp;

                SaveSettings();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void UI_ExpCorr_Textchanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                
                User_input.ExpCorrection[0] = Double.Parse(ExpCorr0.Text);                
                User_input.ExpCorrection[1] = Double.Parse(ExpCorr1.Text);               
                User_input.ExpCorrection[2] = Double.Parse(ExpCorr2.Text);
                User_input.ExpCorrection[3] = Double.Parse(ExpCorr3.Text);

                UI_CorrectTotal0.Text = (User_input.ExpCorrection[0] * User_input.DigitCorrection[0]).ToString();
                UI_CorrectTotal1.Text = (User_input.ExpCorrection[1] * User_input.DigitCorrection[1]).ToString();
                UI_CorrectTotal2.Text = (User_input.ExpCorrection[2] * User_input.DigitCorrection[2]).ToString();
                UI_CorrectTotal3.Text = (User_input.ExpCorrection[3] * User_input.DigitCorrection[3]).ToString();

                SaveSettings();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void UI_DigCorr_Textchanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                User_input.DigitCorrection[0] = Double.Parse(DigCorr0.Text);

                User_input.DigitCorrection[1] = Double.Parse(DigCorr1.Text);

                User_input.DigitCorrection[2] = Double.Parse(DigCorr2.Text);

                User_input.DigitCorrection[3] = Double.Parse(DigCorr3.Text);

                UI_CorrectTotal0.Text = (User_input.ExpCorrection[0] * User_input.DigitCorrection[0]).ToString();
                UI_CorrectTotal1.Text = (User_input.ExpCorrection[1] * User_input.DigitCorrection[1]).ToString();
                UI_CorrectTotal2.Text = (User_input.ExpCorrection[2] * User_input.DigitCorrection[2]).ToString();
                UI_CorrectTotal3.Text = (User_input.ExpCorrection[3] * User_input.DigitCorrection[3]).ToString();
                
                SaveSettings();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            User_input.SensorDrivenExposure = UI_SensorDrivenExposure.IsChecked.Value;
            SaveSettings();
        }

        private void UI_SensorCoefifcient_Textchanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                User_input.SensorResponce = (float)Double.Parse(UI_SensorCoefifcient.Text);

                SaveSettings();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
                

    }
}