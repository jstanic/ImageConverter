using ImageConverterClient.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageConverterClient.Services
{
    public static class ImageConverterClientService
    {
        /// <summary>
        /// Read files from disk into image wrapper objects
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public static List<ImageWrapper> GetImageWrappers(List<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0)
            {
                throw new Exception("No file has been selected!");
            }

            List<ImageWrapper> imageWrappers = new List<ImageWrapper>();
            
            foreach (var fileName in fileNames)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {                        
                        fs.Position = 0;
                        fs.CopyTo(ms);

                        ImageWrapper iw = new ImageWrapper();
                        iw.Name = fileName;
                        iw.Format = Path.GetExtension(fileName);
                        iw.Size = (uint)fs.Length;
                        imageWrappers.Add(iw);
                    }
                }
            }

            return imageWrappers;
        }

        /// <summary>
        /// Method that sends files to the server and saves the returned TIFF file
        /// </summary>
        /// <param name="images">List of images in image wrappers to convert</param>
        /// <param name="fileName">Name of the tiff file we want to save once we get it from the server</param>
        public static bool ConvertData(List<ImageWrapper> images, string fileName = "")
        {
            var client = new RestClient(@"http://localhost:1508");
            var request = new RestRequest("api/ImageConverter/", Method.POST);

            foreach (var wrapper in images)
            {
                request.AddFile(Path.GetFileName(wrapper.Name), wrapper.Name);
            }


            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var bytes = response.RawBytes;

                if (!string.IsNullOrEmpty(fileName))
                    bytes.SaveAs(fileName + ".tiff");
                else
                    bytes.SaveAs("file.tiff"); ;
            }
            else
            {
                if (response.ErrorException != null)
                    throw response.ErrorException;
                else
                    throw new Exception("Error in converting files!");
            }

            return true;
        }
    }
}
