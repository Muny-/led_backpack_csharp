using System;
using RJCP.IO.Ports;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace LEDBackpackController
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Available ports:");
            Console.WriteLine(String.Join(", ", SerialPortStream.GetPortNames()));

            Console.WriteLine("Using first...");

            Backpack pack = new Backpack(SerialPortStream.GetPortNames()[0]);

            Bitmap bmp = (Bitmap)Bitmap.FromFile("/home/kevin/Downloads/unnamed.gif");

            for (int i = 0; i < bmp.GetFrameCount(FrameDimension.Time); i++)
            {
                bmp.SelectActiveFrame(FrameDimension.Time, i);

                pack.InitializeFrame();

                pack.FillStrips(Color.Black);

                pack.FillMatrix(bmp);

                if (i == bmp.GetFrameCount(FrameDimension.Time)-1)
                    i = 0;
            }

            

            Console.ReadLine();
        }
    }
}
