using System;
using System.Linq;
using System.Collections.Generic;
using RJCP.IO.Ports;
using System.Drawing;
using System.Drawing.Imaging;

namespace LEDBackpackController
{
    public class Backpack
    {
        private SerialPortStream SerialPort;

        private static byte mas = 10;

        public Brightnesses MasterBrightnesses = new Brightnesses(mas, mas, mas, mas, mas, mas);
        //public Brightnesses MasterBrightnesses = new Brightnesses(190, 190, 190, 190, 190, 190);

        public Backpack(string serialPortName)
        {
            SerialPort = new SerialPortStream(serialPortName, 4000000);

            SerialPort.DataReceived += SerialPort_DataReceieved;

            SerialPort.Open();
        }

        private void SerialPort_DataReceieved(object sender, SerialDataReceivedEventArgs e)
        {
            Console.Write(SerialPort.ReadExisting());
        }

        public void WriteMagicBytes()
        {
            SerialPort.Write(Consts.MagicBytes, 0, Consts.MagicBytes.Length);
        }

        public void WriteBrightnesses(Brightnesses brightnesses)
        {
            SerialPort.Write(brightnesses.Values, 0, brightnesses.Values.Length);
        }

        public void InitializeFrame()
        {
            WriteMagicBytes();
            WriteBrightnesses(MasterBrightnesses);
        }

        public void FillPixels(int n, Color c)
        {
            byte[] arr = new byte[n*3];
            for (int i = 0; i < arr.Length; i += 3)
            {
                arr[i] = c.R;
                arr[i + 1] = c.G;
                arr[i + 2] = c.B;
            }

            SerialPort.Write(arr, 0, arr.Length);
        }

        public void SendPixel(Color c)
        {
            byte[] arr = new byte[3];

            arr[0] = Consts.GAMMA_CORRECTED(c.R);
            arr[1] = Consts.GAMMA_CORRECTED(c.G);
            arr[2] = Consts.GAMMA_CORRECTED(c.B);

            SerialPort.Write(arr, 0, arr.Length);
        }

        public void FillAllPixels(Color c)
        {
            FillPixels(Consts.NUM_PIXELS_TOTAL, c);
        }

        public void FillStrips(Color c)
        {
            FillPixels(Consts.NUM_PIXELS_STRIPS, c);
        }

        public void FillMatrix(Color c)
        {
            FillPixels(Consts.NUM_PIXELS_MATRIX, c);
        }

        public void FillMatrix(Color f, int n, Color b)
        {
            FillPixels(n, f);

            FillPixels(Consts.NUM_PIXELS_MATRIX - n, b);
        }

        public void FillMatrix(Bitmap bmp)
        {
            int y_mode = 0;

            int delta = 1;

            for (int y = 15; y >= 0; y--)
            {
                for
                (
                    int x = (delta == 1) ? 0 : 15;
                    (delta == 1) ? x < 16 : x >= 0;
                    x+= delta
                )
                {

                    SendPixel(bmp.GetPixel(x + y_mode, y));

                    if (delta == 1 && x == 15)
                    {
                        delta = -1;
                        break;
                    }
                    else if (delta == -1 && x == 0)
                    {
                        delta = 1;
                        break;
                    }
                }

                if (y == 0 && y_mode == 0)
                {
                    y_mode = 16;
                    y = 16;
                }
            }
        }

        public void Flush()
        {
            SerialPort.Flush();
        }
    }
}