using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using System.Windows.Media.Imaging;

namespace ImageRegistrator
{
    class Image
    {
        public ushort[][][] imageArray;
        public volatile bool loaded = false; 

        public volatile String path;

        public int channels = 0;
        public int height = 0;
        public int width = 0;

        public int bytesPerSample = 0; 

        public Image(string filepath)
        {
            path = filepath; 
           
        }

        public void Load()
        {
            
            using (Tiff image = Tiff.Open(@path, "r"))
            {

                //Read all teh tags to know how big the image is
                FieldValue[] value = image.GetField(TiffTag.IMAGEWIDTH);  
                width = value[0].ToInt();

                value = image.GetField(TiffTag.IMAGELENGTH);
                height = value[0].ToInt();

                value = image.GetField(TiffTag.SAMPLESPERPIXEL);
                channels = value[0].ToInt();

                value = image.GetField(TiffTag.BITSPERSAMPLE);
                bytesPerSample = value[0].ToInt();
                bytesPerSample = bytesPerSample / 8;

                

                //allocates buffer and fills it up 
                byte[] buffer = new byte[width * height * channels * bytesPerSample];
                

                for (int h = 0; h < height; h++)
                {
                    image.ReadScanline(buffer, h * width * channels * bytesPerSample, h, 0);
                }


                //move the buffer to an array of ushorts
                ushort[] wholeBuffer = ByteToUshort(buffer, width, height, channels, bytesPerSample);

                //move it to permanent storage
                imageArray = Reshape1Dto3D(wholeBuffer, width, height, channels);
                loaded = true; 
            }

        }

        public void Unload()
        {
            imageArray = null;
            loaded = false; 
        }

        public static ushort[][][] Reshape1Dto3D (ushort[] wholeBuffer, int width, int height, int channels)
        {
             ushort[][][] image= createImageStorage(width, height, channels);

             Parallel.For(0, image.Length, h => //h is the index
             {
                int hOffset = h * width * channels;
                for (int w = 0; w < image[h].Length; w++)
                {
                    int wOffset = w * channels;

                    for (int c = 0; c < image[h][w].Length; c++)
                    {
                        image[h][w][c] = wholeBuffer[hOffset + wOffset + c];
                    }
                }
             });

            return image; 

        }

        /*
        public static ushort[] Reshape3Dto1D (ushort[][][] image, int width, int height, int channels)
        {
            ushort[] wholeBuffer = new ushort[width * height * channels];

            //move it into image storage
            for (int h = 0; h < image.Length; h++)
            {
                int hOffset = h * width * channels;

                for (int w = 0; w < image[h].Length; w++)
                {
                    int wOffset = w * channels;

                    for (int c = 0; c < image[h][w].Length; c++)
                    {
                        wholeBuffer[hOffset + wOffset + c] = image[h][w][c];
                    }
                }
            }

            return wholeBuffer;

        } */


        public static ushort[] ByteToUshort(byte[] buffer, int width, int height, int channels, int bytesPerSample)
        {
            ushort[] wholeBuffer = new ushort[width * height * channels];
            if (bytesPerSample == 2)
            {
                Buffer.BlockCopy(buffer, 0, wholeBuffer, 0, buffer.Length);
            }
            else if(bytesPerSample == 1)
            {
                Array.Copy(buffer, wholeBuffer, buffer.Length);
                
            }
            return wholeBuffer; 
        }

        /*creates a ushort array for storing the image with given parameters*/
        public static ushort[][][] createImageStorage(int width, int height, int channels)
        {
            //Allocate image to have correct amount of storage 
            ushort[][][] ushortArray = new ushort[height][][];

            Parallel.For(0, ushortArray.Length, h => //h is the index
            {
                ushortArray[h] = new ushort[width][];
                for (int w = 0; w < ushortArray[h].Length; w++)
                {
                    ushortArray[h][w] = new ushort[channels];
                }
            });
            return ushortArray; 
        }
        
       public static BitmapSource ArrayToBitmap(ushort[][][] image3D, int channel1, int channel2, int bytesPerSample)
        {
            int width = image3D[0].Length;
            int height = image3D.Length;
            int channels = image3D[0][0].Length;

            int stride = width * 3;


            byte[] localArray = new byte[width * height * 4];


            if (bytesPerSample == 2)
            {
                Parallel.For(0, height, h => //h is the index
                {
                    for (int w = 0; w < width; w++)
                    {
                        int offset = h * width * 3 + w * 3;
                        localArray[offset + 2] = (byte)(image3D[h][w][channel1] / 256); //red
                        localArray[offset + 1] = (byte)(image3D[h][w][channel2] / 256); //green
                        localArray[offset ] = 0;

                    }
                });
            }
            else if(bytesPerSample == 1)
            {
                Parallel.For(0, height, h => //h is the index
                {
                    for (int w = 0; w < width; w++)
                    {
                        int offset = h * width * 3 + w * 3;
                        localArray[offset + 2] = (byte)image3D[h][w][channel1];  //red
                        localArray[offset + 1] = (byte)image3D[h][w][channel2]; //green
                        localArray[offset ] = 0;

                    }
                });
            }


            BitmapSource image = BitmapSource.Create(

                width,
                height,
                96,
                96,
                System.Windows.Media.PixelFormats.Bgr24,
                null,
                localArray,
                stride
            );

            return image;


            // ushort[] image1D = Reshape3Dto1D(image3D,  image3D[0].Length, image3D.Length, image3D[0][0].Length);
            // return ArrayToBitmap(image1D, image3D.Length, image3D[0].Length, image3D[0][0].Length, colour1, colour2);            
        }


        /*
        public static BitmapSource ArrayToBitmap(ushort[] image1D, int height, int width, int channels, int colour1, int colour2)
        {
            int stride = width * 2;

            ushort[] localArray = new ushort
            for 



        }*/

    }
}
