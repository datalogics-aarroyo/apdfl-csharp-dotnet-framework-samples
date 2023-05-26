using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Datalogics.PDFL;

/*
 * 
 * This sample program searches through the PDF file that you select and identifies
 * raster drawings, diagrams and photographs among the text. Then, it extracts these
 * images from the PDF file and copies them to a separate set of graphics files in the
 * same directory. Vector images, such as clip art, will not be exported.
 * 
 * 
 * Copyright (c) 2007-2023, Datalogics, Inc. All rights reserved.
 *
 */

namespace ImageExtraction
{
    class ImageExtraction
    {
        static int next = 0;

        static void ExtractImages(Content content)
        {
            for (int i = 0; i < content.NumElements; i++)
            {
                Element e = content.GetElement(i);
                if (e is Datalogics.PDFL.Image)
                {
                    Console.WriteLine("Saving an image");
                    Datalogics.PDFL.Image img = (Datalogics.PDFL.Image)e;
                    Bitmap bitmap = img.Bitmap;
                    bitmap.Save("ImageExtraction-extract-out" + (next) + ".bmp", ImageFormat.Bmp);

                    Datalogics.PDFL.Image newimg = img.ChangeResolution(500);
                    bitmap = newimg.Bitmap;
                    bitmap.Save("ImageExtraction-extracted-out" + (next) + ".bmp", ImageFormat.Bmp);
                    next++;

                    // The bitmap may be saved in any supported ImageFormat, e.g.:
                    //bitmap.Save("extract" + i + ".gif", ImageFormat.Gif);
                    //bitmap.Save("extract" + i + ".png", ImageFormat.Png);
                }
                else if (e is Container)
                {
                    ExtractImages((e as Container).Content);
                }
                else if (e is Group)
                {
                    ExtractImages((e as Group).Content);
                }
                else if (e is Form)
                {
                    ExtractImages((e as Form).Content);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ImageExtraction Sample:");

            using (Library lib = new Library())
            {
                Console.WriteLine("Initialized the library.");

                String sInput = Library.ResourceDirectory + "Sample_Input/ducky.pdf";

                if (args.Length > 0)
                    sInput = args[0];

                Console.WriteLine("Input file: " + sInput);

                Document doc = new Document(sInput);

                Page pg = doc.GetPage(0);
                Content content = pg.Content;
                ExtractImages(content);
            }
        }
    }
}

