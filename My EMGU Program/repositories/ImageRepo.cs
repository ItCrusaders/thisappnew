using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EQS_2._0.repositories
{
    static class ImageRepo
    {

        public static async Task<List<BitmapImage>> getPdfAsync(String path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            var pdf = await PdfDocument.LoadFromFileAsync(file);
            List<BitmapImage> images = new List<BitmapImage>();
            for (int i = 0; i < pdf.PageCount; i++)
                using (var page = pdf.GetPage((uint)i))
                {
                    Console.WriteLine("sss" + path);
                    BitmapImage img = await PageToBitmapAsync(page);

                    Save(img, path+"-"+i);
                    images.Add(img);
                }

            return images;
        }

        private static async Task<BitmapImage> PageToBitmapAsync(PdfPage page)
        {
            BitmapImage image = new BitmapImage();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream);

                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream.AsStream();
                image.EndInit();
            }

            return image;
        }
        public static void Save(this BitmapImage image, String filePath){
            //image.Save(filePath);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath+".jpg", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }


    }

    
}
