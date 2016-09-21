using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using System.IO;


namespace ImageRegistrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> FilesToOpen = new List<string>();
        private List<Image> Images = new List<Image>();
        private int currentimage = 0;


        private int channel1=0;
        private int channel2=1; 

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            FilesToOpen.Clear();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "tiff files (*.tif; *.tiff)|*.tif; *.tiff|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    FilesToOpen.Clear();
                    foreach (string filename in openFileDialog.FileNames)
                        FilesToOpen.Add(System.IO.Path.GetFullPath(filename));

                    UI_filenumberCounter.Content = FilesToOpen.Count; 
                    Images.Clear();
                    foreach (var file in FilesToOpen)
                    {
                        Images.Add(new Image(file));
                    }
                    currentimage = 0; 

                    Images[currentimage].Load();

                    UI_Viewcounter.Content = currentimage + 1; 
                    UI_displayImage.Source = Image.ArrayToBitmap(Images[currentimage].imageArray, channel1, channel2, Images[currentimage].bytesPerSample);

                    UI_C1Selector.Items.Clear();
                    UI_C2Selector.Items.Clear();

                    for (int i = 0; i < Images[currentimage].channels; i++)
                    {
                        UI_C1Selector.Items.Add("Chan " + (i+1).ToString());
                        UI_C2Selector.Items.Add("Chan " + (i + 1).ToString());
                    }

                    UI_C1Selector.SelectedIndex = 0;
                    UI_C2Selector.SelectedIndex = 1;
                }
            }
            
            
            
        }

        private void NextImage(object sender, MouseButtonEventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        { 
        
            if ((currentimage + 1) < Images.Count)
            {
                currentimage++;

                if (!Images[currentimage].loaded)
                    Images[currentimage].Load();
                else
                {
                    //nothing
                }

                UI_displayImage.Source= Image.ArrayToBitmap(Images[currentimage].imageArray, channel1, channel2, Images[currentimage].bytesPerSample);

                UI_Viewcounter.Content = currentimage +1;

                if(currentimage - 10 >= 0)
                    Images[currentimage - 10].Unload();
            }
            else
            {
                
            }
        }


        private void PrevImage(object sender, MouseButtonEventArgs e)
        {
            PrevImage();
        }

        private void PrevImage()
        {
            if ((currentimage - 1) >= 0)
            {
                currentimage--;
                if (!Images[currentimage].loaded)
                    Images[currentimage].Load();
                else
                {
                    //do nothing
                }

                UI_Viewcounter.Content = currentimage + 1;
                UI_displayImage.Source = Image.ArrayToBitmap(Images[currentimage].imageArray, channel1, channel2, Images[currentimage].bytesPerSample);

                if (currentimage + 10 < Images.Count)
                    Images[currentimage + 10].Unload();
            }
            else
            {

            }
        }

        private void ChannelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(UI_C1Selector.SelectedIndex >= 0)
                channel1 = UI_C1Selector.SelectedIndex;
            if (UI_C2Selector.SelectedIndex >= 0)
                channel2 = UI_C2Selector.SelectedIndex;
            UI_displayImage.Source = Image.ArrayToBitmap(Images[currentimage].imageArray, channel1, channel2, Images[currentimage].bytesPerSample);

        }

        private void PrevImage(object sender, RoutedEventArgs e)
        {
            PrevImage();
        }

        private void NextImage(object sender, RoutedEventArgs e)
        {
            NextImage();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
