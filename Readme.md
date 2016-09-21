#Heimdall Imaging#
This project lets you use machine vision cameras for infrared / multispectral imaging on UAVs/Drones.
That is accomplished by combining them into a Multi-Camera array — that is placing a spectral filter in front of every camera and carefully aligning them.
The software combines monochrome frames from any number of cameras into a multi-channel tiff, that can be later post-prossed like any 'normal' image. 
You will need Machine vision cameras (this software is designed for Ximea), light sensors from http://www.yoctopuce.com/, and a single-board computer such as Intel NUC. 
The solution requires LibTiff.Net and Ximea's library to build. 
When using the software, it is desirable to configure DHCP on the single-board computer. 

##Documents##
There is a PDF user manual and a broshure, they describe the workflow and some key characteristics in more detail.  

##3D models##
Included are 3D models for an enclosure. The enclosure is for 4 Ximea MQ013MG cameras each with a Narrowbard filter and an intel NUC. It can be mounted on a large Quadcopter. The models are in Autodesk Inventor and STL file formats. 

##Code##
There are three projects in the solution, each produces an executable for a different purpose.

###Spectral Command###
End user application written in C# and WPF. It connects to the camera over Wi-Fi, and allows the user to align channels,
set exposure and timing, preview the image, and start/stop the imaging process.  


###HighSpecter###
C# & Winforms application that is meant to run on startup on the SBC that controls the cameras. It will create a Wifi Hospot, and wait for a connection from Spectral Command.

###ImageRegistrator###
A stand-alone desktop application meant for registering images if you found that you've taken them out of alignment. We found it quite usefull. 

##Issues and limitations##
###High Specter###
1. The program must be set to run on startup with admin rights, so that it can start the hotspot. 
2. DHCP setver must be installed and run speerately, to use the system conveniently. 
3. Program is not designed to connect to more than one Spectral Command, doing so may lead to spontaneous resonance cascade and detruction of the planet. 

###Spectral Command###

1. The program will report successfull connection as long as it can open a TCP connection, regardsless of what is recieved and what it managed to connect to. It could be your router, Google.com, or a fridge - it will report succesfull conenction jsut the same. 
2. Ordinary disconnect is reported as an eror in the status bar
3. Calibration button is not implemented, and hence does nothing. 
