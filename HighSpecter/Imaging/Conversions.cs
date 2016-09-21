using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HighSpecter.Imaging
{
    public static class Conversions
    {
        /*these are variables shared across all camera types*/
        private const ushort Image10To16bit_scaler = 64;
        
        
        
        public static byte[] BitmapToByteArray(Bitmap image)
        {
            byte[] imageByte;
            
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int length = Math.Abs(bmpData.Stride) * image.Height;
            imageByte = new byte[length];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, imageByte, 0, length);

            return imageByte;
        }


        /*ONly use this for copying data from monochrome 16BPP bitmaps. bitsPerPixel will apply bitshift to the */
        public static ushort[] BitmapToUShortArray(Bitmap image, int truebitPerPixel, float correction)
        {
            
            UInt16[] destination = null;
            if ((truebitPerPixel <= 16 ) && (truebitPerPixel <=8))
            {
                ushort shift = (ushort)(16 - truebitPerPixel);
                // Lock the bitmap's bits.
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. The returned value is in bytes, and we are storing ushort, so we need to divide by 2
                int length = (Math.Abs(bmpData.Stride) * image.Height) / 2;
                

                destination = new ushort[length];
                if (image.PixelFormat == PixelFormat.Format16bppGrayScale)
                {
                    unsafe
                    {
                        var sourcePtr = (ushort*) ptr;
                        for (int i = 0; i < length; i++)
                        {
                            destination[i] = (ushort) ((*sourcePtr << shift)*correction);
                            sourcePtr++;
                        }
                    }
                }
                else
                {
                    Logging.Add("BitmapToUShortArray failed because Pixelformat should be 16bpp grayscale, but it is " +
                                image.PixelFormat);
                }

            }
            else
            {
                Logging.Add("BitmapToUShortArray failed because truebitPerPixel value is " + truebitPerPixel + " but it should be between 8 and 16");
            }

            return destination;
        }

        public static ushort[][] BitmapToUShort2DArray(Bitmap image, int truebitPerPixel, float correction)
        {

            UInt16[][] destination = null; 
            if (image == null)
            {
                Logging.Add("BitmapToUShortArray Bitmap is null");
            }
            else if ((truebitPerPixel <= 16) && (truebitPerPixel >= 8))
            {
                destination = new UInt16[image.Height][];
                for (int i=0; i< destination.Length; i++)
                {
                    destination[i] = new ushort[image.Width];
                }

                ushort shift = (ushort)(16 - truebitPerPixel);
                // Lock the bitmap's bits.
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

                // Get the address of the first line.
                IntPtr ptr = bmpData.Scan0;

                // Declare an array to hold the bytes of the bitmap. The returned value is in bytes, and we are storing ushort, so we need to divide by 2
                int length = (Math.Abs(bmpData.Stride) * image.Height) / 2;

                if (image.PixelFormat == PixelFormat.Format16bppGrayScale)
                {
                    unsafe
                    {
                        var sourcePtr = (ushort*) ptr;
                        for (int i = 0; i < destination.Length; i++)
                        {
                            for (int n = 0; n < destination[i].Length; n++)
                            {
                                destination[i][n] = (ushort) ((*sourcePtr << shift)*correction);
                                sourcePtr++;
                            }
                        }
                    }
                }
                else
                {
                    Logging.Add("BitmapToUShortArray failed because Pixelformat should be 16bpp grayscale, but it is " +
                                image.PixelFormat);
                }
            }
            else
            {
                Logging.Add("BitmapToUShortArray failed because truebitPerPixel value is " + truebitPerPixel + " but it should be between 8 and 16");
            }

            return destination;
        }


        /*returns a 2D arra., in thue format array [y][x], if the parameters are wrong it returns arrays full of zeros */
        public static ushort[][] ByteToUshort2D(byte [] data, int width, int height)
        {
            ushort[][] scaledImage = new ushort[height][];

            //allocate space for data in the 2D array
            for (int i = 0; i < height; i++)
            { scaledImage[i] = new ushort[width]; }


            if(data.Length == (height * width * 2))
            {
                
               
                for (int i = 0; i < data.Length/2; i++)
                {
                    int lowernumber = data[2*i];
                    int highernumber = data[((2*i) + 1)];
                    scaledImage[i / width][i % width] = (ushort)(((lowernumber + highernumber * 256) * Image10To16bit_scaler));
                }
            }
            else
            {
                Logging.Add("Can't perform ByteToUshort2D, dimentions are wrong, image is" + width + ":" + height + "but the data is of length" + data.Length);
            }

            return scaledImage;
        }

        public static ushort[] ByteToUshort1D(byte[] data)
        {
            ushort[] scaledImage = new ushort[data.Length/2];

                for (int i = 0; i < data.Length / 2; i++)
                {
                    ushort lowernumber = data[2 * i];
                    ushort highernumber = data[((2 * i) + 1)];
                    scaledImage[i] = (ushort)(((lowernumber + highernumber * 256) * Image10To16bit_scaler));
                }

            return scaledImage;
        }
        
        public static byte[] Scale1DByteArray10To16Bit(byte [] data, int width, int height)
        {
            for (int i = 0; i < data.Length / 2; i++)
            {
                int lowernumber = data[2 * i];
                int highernumber = data[((2 * i) + 1)];
                ushort scaledImage = (ushort)(((lowernumber + highernumber * 256) * Image10To16bit_scaler));

                data[((2 * i) + 1)] = (byte)(scaledImage / 256);
                data[2 * i] = (byte)(scaledImage % 256);
             }
            return data; 
        }

        public static byte[] UshortToByte1D(ushort[] data)
        {
            byte[] image = new byte[data.Length*2];

            for (int i = 0; i < data.Length; i++)
            {
                image[2*i] = (byte) (data[i] % 256);
                image[2*i + 1] = (byte) (data[i]/256);
            }

            return image;
        }
    }
}

/*attempts to scale the image. The cameras are 10 bit, so supply values between 0 and 1023
 * the format we are saving in is 16 bit, so it takes values between 0 and 65535
 *a 16 bit Image comes as a 1D array of bytes. Bytes come in pairs, each pair forms numerical value of a pixel
 * First byte is the lower part of a short, second byte is the upper part of a short
 * Suppose we got a value of 756 fro mteh camera, taht would split into 244 in lower byte and 2 in upper byte. 
 * to recover original value we must multiply upper byte by 256 and add the lower byte
 * to scale the 10 bit number to 16 bit number we must multiply it by (2^16 / 2^10) = 64
 * once we have the ushort integer value, we must split it into two bits again. Upper bit is produced by integer division between ushort and 256
 * Lower bit is the remainder of the same division
 * Effect of this method on image of maximum brightness:
 *                         Before:   0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 
 *                          After:   1 1 1 1 1 1 1 1 1 1 0 0 0 0 0*/
