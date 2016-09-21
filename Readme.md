#Heimdall Imaging#
This project lets you use machine vision cameras for infrared / multispectral imaging on UAVs/Drones.
That is accomplished by combining them into a Multi-Camera array — that is placing a spectral filter in front of every camera and carefully aligning them.
The software combines monochrome frames from any number of cameras into a multi-channel tiff, that can be later post-prossed like any 'normal' image. 
You will need Machine vision cameras (this software is designed for Ximea), light sensors from http://www.yoctopuce.com/, and a single-board computer such as Intel NUC. 
The solution requires LibTiff.Net and Ximea's library to build. 

There are three projects in the solution, each produces an executable for a different purpose.  

###Spectral Command###
End user application written in C# and WPF. It connects to the camera over Wi-Fi, and allows the user to align channels,
set exposure and timing, preview the image, and start/stop the imaging process.  


###HighSpecter###
C# & Winforms application that is meant to run on startup on the SBC that controls the cameras. It will create a Wifi Hospot, and wait for a connection from Spectral Command.

###ImageRegistrator###
A stand-alone desktop application meant for registering images if you found that you've taken them out of alignment. We found it quite usefull. 

##Known Issues##
