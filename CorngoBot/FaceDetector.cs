using DlibDotNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorngoBot
{
    class FaceDetector
    {
        //private const string inputFilePath = "./input.jpg";

        public String detectFaces(String inputFilePath)
        {
            // set up Dlib facedetector
            using (var fd = Dlib.GetFrontalFaceDetector())
            {
                // load input image
                var img = Dlib.LoadImage<RgbPixel>(inputFilePath);

                // find all faces in the image
                var faces = fd.Operator(img);
                foreach (var face in faces)
                {
                    // draw a rectangle for each face
                    Dlib.DrawRectangle(img, face, color: new RgbPixel(0, 255, 255), thickness: 4);
                }

                Dlib.SaveJpeg(img, "output.jpg");
            }

            return "./output.jpg";
        }
    }
}
