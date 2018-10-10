using Newtonsoft.Json;
using ImageConverter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ImageConverter.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace ImageConverter.Services
{
    public static class ImageConverterService
    {
        /// <summary>
        /// Starts the conversion of the data - defines conversion details and converts images
        /// </summary>
        /// <param name="conversionData">Conversion data object with image list to be converted</param>
        /// <returns></returns>
        public static ConversionData ConvertData(ConversionData conversionData)
        {
            DateTime now = DateTime.Now;
            conversionData.ConversionName = "Conversion " + now.ToString();
            conversionData.ConversionTime = now;

            ConvertImageData(conversionData);

            return conversionData;
        }

        /// <summary>
        /// Method that saves files to server root folder, reads them as image files and calls for conversion to Tiff
        /// </summary>
        /// <param name="conversionData"></param>
        private static void ConvertImageData(ConversionData conversionData)
        {
            #region ByteStream to Image
            List<Image> images = new List<Image>();
            if (conversionData.Images != null)
            {
                foreach (var image in conversionData.Images)
                {
                    if (image.Length > 0)
                    {
                        using (FileStream fs = new FileStream(image.FileName, FileMode.Create, FileAccess.ReadWrite))
                        {
                            image.CopyTo(fs);
                            fs.Close();
                        }
                        images.Add(Image.FromFile(image.FileName));
                    }
                }
            }
            #endregion

            #region TIFF creation

            conversionData.FileName = CreateTIFF(images);

            if (conversionData.ConversionDetails == null)
            {
                conversionData.Images.ForEach(item => conversionData.ConversionDetails += item.FileName.ToString() + Environment.NewLine);
            }

            ImageConverterRepository.SaveConversionData(conversionData);

            #endregion
        }
        
        /// <summary>
        /// Creates TIFF files and returns the full file name of the created image
        /// </summary>
        /// <param name="images">List of images to convert to TIFF</param>
        /// <returns></returns>
        private static string CreateTIFF(List<Image> images)
        {
            try
            {
                if (images == null || images.Count == 0)
                    return ""; 
                string tempFilename = "temp.tiff";

                int n = 1;
                while (File.Exists(tempFilename))
                {
                    tempFilename = string.Format("temp_{0}.tiff", n++);
                }

                Encoder encoder = Encoder.SaveFlag;
                ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);

                // Save the first frame of the multi page tiff
                Bitmap tiff = (Bitmap)images.First();
                tiff.Save(tempFilename, encoderInfo, encoderParameters);

                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);

                // Add the remining images to the tiff
                for (int i = 1; i < images.Count; i++)
                {
                    Bitmap img = (Bitmap)images[i];
                    tiff.SaveAdd(img, encoderParameters);
                }

                // Close out the file
                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                tiff.SaveAdd(encoderParameters);
                return tempFilename;
            }
            catch
            {
                throw;
            }
        }
    }
}
