using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;

namespace HighSpecter
{
    class Saving
    {


        public static int WriteOnechannel(ushort[][] imageData, String path, int width, int height, int bitdepth)
        {
            int toreturn=0;

            using (Tiff tif = Tiff.Open(path, "w"))
            {

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                    toreturn = 0;
                }
                else
                {



                    tif.SetField(TiffTag.IMAGEWIDTH, width);
                    tif.SetField(TiffTag.IMAGELENGTH, height);
                    tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                    tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                    tif.SetField(TiffTag.ROWSPERSTRIP, height);
                    tif.SetField(TiffTag.XRESOLUTION, 88.0);
                    tif.SetField(TiffTag.YRESOLUTION, 88.0);
                    tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                    tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                    tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                    tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

                    int bytespersample = (bitdepth/8);




                    int i = 0;
                    if (bytespersample == 2)
                    {
                        foreach (ushort[] stripe in imageData)
                        {
                            byte[] buffer = new byte[width*bytespersample]; //allocate a buffer

                            Buffer.BlockCopy(stripe, 0, buffer, 0, stripe.Length*sizeof (short));
                                //block copy copies the bits directly, 
                            //so the shorts get atuomatically sptil into bytes without a performance penalty
                            tif.WriteScanline(buffer, i);
                            i++;
                        }
                    }
                    else if (bytespersample == 1)
                    {
                        foreach (ushort[] stripe in imageData)
                        {
                            byte[] buffer = new byte[width*bytespersample]; //allocate a buffer

                            int n = 0;

                            foreach (ushort Pixel in stripe)
                            {
                                buffer[n] = BitConverter.GetBytes(Pixel)[1];
                                    //assigns the bit in the buffer as a higher byte of short
                                n++;
                            }

                            tif.WriteScanline(buffer, i);
                            i++;
                        }
                    }
                    else
                    {
                        Logging.Add("In saving, bitdepth should be 8 or 16, and it is " + bitdepth);
                    }
                    tif.FlushData();
                    tif.Close();
                    toreturn = 1;
                }
            }

            return toreturn; 
        }
        
        public static int WriteOnechannel(ushort[] imageData, String path, int width, int height, int bitdepth)
        {
            int toreturn = 0;

            using (Tiff tif = Tiff.Open(path, "w"))
            {

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                    toreturn = 0;
                }


                tif.SetField(TiffTag.IMAGEWIDTH, width);
                tif.SetField(TiffTag.IMAGELENGTH, height);
                tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                tif.SetField(TiffTag.ROWSPERSTRIP, height);
                tif.SetField(TiffTag.XRESOLUTION, 88.0);
                tif.SetField(TiffTag.YRESOLUTION, 88.0);
                tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

                int bytespersample = (bitdepth / 8);




                
                if (bytespersample == 2)
                {
                    for (int i = 0; i < height; i++)
                    {
                        byte[] buffer = new byte[width * bytespersample]; //allocate a buffer, two bytes per short
                        Buffer.BlockCopy(imageData, width * i * bytespersample, buffer, 0, buffer.Length);
                        tif.WriteScanline(buffer, i);
                    }
                }
                else if (bytespersample == 1)
                {
                    for (int i = 0; i < height; i++)
                    {
                        byte[] buffer = new byte[width * bytespersample]; //allocate a buffer, one byte per short

                        for (int n = 0; n < width; n++) //iterate through every short
                        {
                            buffer[n] = BitConverter.GetBytes(imageData[width * i + n])[1]; //
                                //assigns the bit in the buffer as a higher byte of short
                        }

                        tif.WriteScanline(buffer, i);
                        i++;
                        }
                }
                else
                {
                    Logging.Add("In saving, bitdepth should be 8 or 16, and it is " + bitdepth);
                }
                tif.FlushData();
                tif.Close();
                toreturn = 1;
            }

            return toreturn;
        }



        public static void WriteAllChannelsInterleaved(ushort[][][] AllImages, String path, int width, int height, int bitdepth)
        {

            using (Tiff tif = Tiff.Open(path, "w"))
            {
                int toreturn = 0;

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                }
                else
                {



                    tif.SetField(TiffTag.IMAGEWIDTH, width);
                    tif.SetField(TiffTag.IMAGELENGTH, height);
                    tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, AllImages.Length);
                    tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                    tif.SetField(TiffTag.ROWSPERSTRIP, height);
                    tif.SetField(TiffTag.XRESOLUTION, 88.0);
                    tif.SetField(TiffTag.YRESOLUTION, 88.0);
                    tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                    tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                    tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                    tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

                    int bytespersample = (bitdepth/8);





                    //now write to disk
                    for (int i = 0; i < height; i++)
                    {
                        byte[] buffer = new byte[width*bytespersample*AllImages.Length];

                        for (int x = 0; x < width; x++)
                        {
                            int offsetLine = x*bytespersample*AllImages.Length;

                            for (int c = 0; c < AllImages.Length; c++)
                            {
                                int offsetCamera = c*bytespersample;


                                int offsetTotal = offsetCamera + offsetLine;

                                if (bitdepth == 16)
                                {
                                    buffer[offsetTotal] = BitConverter.GetBytes(AllImages[c][i][x])[0];
                                    buffer[offsetTotal + 1] = BitConverter.GetBytes(AllImages[c][i][x])[1];
                                }
                                else
                                {
                                    buffer[offsetTotal] = BitConverter.GetBytes(AllImages[c][i][x])[1];
                                }

                            }
                        }



                        tif.WriteScanline(buffer, i);
                    }

                    tif.FlushData();
                    tif.Close();
                }
            }

            //Conversions.Start(path);

        }
        

        public static void WriteThreeChannelsInterleaved(ushort[][][] AllImages, String path, int bitdepth)
        {

           int toreturn = 0;

            using (Tiff tif = Tiff.Open(path, "w"))
            {

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                    toreturn = 0;
                }
                else
                {
                    int width = AllImages[0][0].Length;
                    int height = AllImages[0].Length;

                    tif.SetField(TiffTag.IMAGEWIDTH, width);
                    tif.SetField(TiffTag.IMAGELENGTH, height);
                    tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                    tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                    tif.SetField(TiffTag.ROWSPERSTRIP, height);
                    tif.SetField(TiffTag.XRESOLUTION, 88.0);
                    tif.SetField(TiffTag.YRESOLUTION, 88.0);
                    tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                    tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                    tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                    tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                    ;

                    int bytespersample = (bitdepth/8);

                    
                         
                        //now write to disk
                        for (int i = 0; i < AllImages[0].Length; i++) // iterate height
                        {

                            byte[] buffer = new byte[width * bytespersample * 3];

                            for (int x = 0; x < AllImages[0][i].Length; x++) //Iterate Width
                                {
                                    int offsetLine = x*bytespersample*3;

                                    for (int c = 0; c < AllImages.Length; c++)
                                    {
                                        int offsetCamera = c*bytespersample;

                                        int offsetTotal = offsetCamera + offsetLine;


                                        if (bitdepth == 16)
                                        {
                                            buffer[offsetTotal] = BitConverter.GetBytes(AllImages[c][i][x])[0];
                                            buffer[offsetTotal + 1] = BitConverter.GetBytes(AllImages[c][i][x])[1];
                                        }
                                        else
                                        {
                                            buffer[offsetTotal] = BitConverter.GetBytes(AllImages[c][i][x])[1];
                                        }

                                    }
                                }
                            

                            tif.WriteScanline(buffer, i);
                        }
                    


                    tif.FlushData();
                    tif.Close();
                }
            }
        }


        //Works like crap
        public static void WriteThreeChannelsPerChannel(ushort[][][] AllImages, String path, int bitdepth)
        {

            int toreturn = 0;

            using (Tiff tif = Tiff.Open(path, "w"))
            {

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                    toreturn = 0;
                }
                else
                {
                    int width = AllImages[0][0].Length;
                    int height = AllImages[0].Length;

                    tif.SetField(TiffTag.IMAGEWIDTH, width);
                    tif.SetField(TiffTag.IMAGELENGTH, height);
                    tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                    tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                    tif.SetField(TiffTag.ROWSPERSTRIP, height);
                    tif.SetField(TiffTag.XRESOLUTION, 88.0);
                    tif.SetField(TiffTag.YRESOLUTION, 88.0);
                    tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                    tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                    tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                    tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                    ;

                    int bytespersample = (bitdepth / 8);

                    unsafe
                    {
                        //now write to disk
                        for (int i = 0; i < AllImages[0].Length; i++) // iterate height
                        {

                            byte[] buffer = new byte[width * bytespersample * 3];

                            Buffer.BlockCopy(AllImages[0][i], 0, buffer, 0, bytespersample * AllImages[0][i].Length);
                            Buffer.BlockCopy(AllImages[1][i], 0, buffer, bytespersample * AllImages[0][i].Length, bytespersample * AllImages[1][i].Length);
                            if (AllImages.Length >= 3)
                            {
                                Buffer.BlockCopy(AllImages[2][i], 0, buffer, bytespersample * AllImages[0][i].Length * 2, bytespersample * AllImages[1][i].Length);
                            }      
            
                            tif.WriteScanline(buffer, i);
                        }
                    }


                    tif.FlushData();
                    tif.Close();
                }
            }
        }

        /*
        private static void writeThreeChannels(ushort[][][] AllImages, String path, int width, int height, int bitdepth)
        {

           int toreturn = 0;

            using (Tiff tif = Tiff.Open(path, "w"))
            {

                if (tif == null)
                {
                    Logging.Add("couldn't create a tiff file in location " + path);
                    toreturn = 0;
                }


                tif.SetField(TiffTag.IMAGEWIDTH, width);
                tif.SetField(TiffTag.IMAGELENGTH, height);
                tif.SetField(TiffTag.BITSPERSAMPLE, bitdepth);
                tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                tif.SetField(TiffTag.ORIENTATION, Orientation.TOPLEFT);
                tif.SetField(TiffTag.ROWSPERSTRIP, height);
                tif.SetField(TiffTag.XRESOLUTION, 88.0);
                tif.SetField(TiffTag.YRESOLUTION, 88.0);
                tif.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                tif.SetField(TiffTag.COMPRESSION, Compression.NONE);
                tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);

                int bytespersample = (bitdepth / 8);

                
                //now write to disk
                 for (int i = 0; i < height; i++)
                 {

                      byte[] buffer = new byte[width * bytespersample * 3];

                      for (int x = 0; x < width; x++)
                      {
                          int offsetLine = x * bytespersample * 3;

                          for (int c = 0; c < 3; c++)
                          {
                              int offsetCamera = c * bytespersample;

                              for (int b = 0; b < bytespersample; b++)
                              {
                                  int offsetBytes = b;
                                  int offsetTotal = offsetBytes + offsetCamera + offsetLine;

                                  if (c < AllImages.Length)
                                      buffer[offsetTotal] = AllImages[c][b + x * bytespersample + i * width * bytespersample];
                                  else
                                      buffer[offsetTotal] = 0;
                              }

                          }
                      }


                      tif.WriteScanline(buffer, i);
                  }

                  tif.FlushData();
                  tif.Close();
              }

             }
    } */
    }
        

}
