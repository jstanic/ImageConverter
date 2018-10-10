using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ImageConverter.Services;
using Newtonsoft.Json;
using ImageConverter.Models;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ImageConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageConverterController : ControllerBase
    {
        /// <summary>
        /// Post method for accepting files and returning converted tiff file.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post()
        {               
            try
            {
                var files = Request.Form.Files.ToList();
                ConversionData conversionData = new ConversionData();
                conversionData.Images = files;
                
                ImageConverterService.ConvertData(conversionData);
                var returnStream = new MemoryStream();
                using (var stream = new FileStream(conversionData.FileName, FileMode.Open))
                {                    
                    stream.CopyTo(returnStream);
                }
                returnStream.Position = 0;
                return File(returnStream, "image/tiff", Path.GetFileName(conversionData.FileName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
    }
}
