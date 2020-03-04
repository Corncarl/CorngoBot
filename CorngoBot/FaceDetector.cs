using DlibDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace CorngoBot
{
    class FaceDetector
    {
        private const string inputFilePath = "./external_resources/facialIinput.jpg";
        private const string outputFilePath = "./external_resources/facialOutput.jpg";

        public int detectFaces(String url)
        {
            var numFaces = 0;

            // set up Dlib facedetector
            using (var fd = Dlib.GetFrontalFaceDetector())
            {

                //Save image locally
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, inputFilePath);

                // load input image
                var img = Dlib.LoadImage<RgbPixel>(inputFilePath);



                // find all faces in the image
                var faces = fd.Operator(img);

                try 
                {
                    foreach (var face in faces)
                    {
                        // draw a rectangle for each face
                        Dlib.DrawRectangle(img, face, color: new RgbPixel(0, 255, 255), thickness: 4);
                        numFaces += 1;
                    }

                    Dlib.SaveJpeg(img, outputFilePath);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    numFaces = -1;
                }
                webClient.Dispose();
            } 
            return numFaces;
        }
    }
}
