using ImageConverter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConverter.Repositories
{
    public class ImageConverterRepository
    {
        /// <summary>
        /// Saves conversion information to database. Conversion information is generated here on server.
        /// </summary>
        /// <param name="conversionData">Conversion data on what we're saving.</param>
        public static void SaveConversionData(ConversionData conversionData)
        {
            try
            {                
                using (ImageConverterDbContext context = new ImageConverterDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ImageConverterDbContext>()))
                {
                    
                    context.ConversionData.Add(conversionData);
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
