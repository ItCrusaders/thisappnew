using ImageMagick;
using System.Drawing;
using System.IO;

namespace EQS_2._0.repositories
{
    static class ImageRepo
    {

        

        public static int ConvertToBitmap(string fileName)
        {
            var settings = new MagickReadSettings();
            // Settings the density to 300 dpi will create an image with a better quality
            settings.Density = new Density(300, 300);

                var page = 0;
            using (var images = new MagickImageCollection())
            {
                // Add all the pages of the pdf file to the collection
                images.Read(fileName, settings);

                foreach (var image in images)
                {
                    // Write page to file that contains the page number
                    image.Write(fileName + page + ".png");
                    // Writing to a specific format works the same as for a single image
                    image.Format = MagickFormat.Ptif;
                    image.Write(fileName + page + ".tif");
                    page++;
                }
            }

            return page;
          
        }


    }

    
}
