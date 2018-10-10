using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConverter.Models
{
    public class ConversionData
    {
        #region Properties

        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Name of the conversion
        /// </summary>
        public string ConversionName { get; set; }
        /// <summary>
        /// Time of the conversion
        /// </summary>
        public DateTime ConversionTime { get; set; }
        /// <summary>
        /// Details about the conversion - specifically, all the file names used in conversion
        /// </summary>
        public string ConversionDetails { get; set; }
        /// <summary>
        /// Files received from client to be used in conversion
        /// </summary>
        [NotMapped]
        public List<IFormFile> Images { get; set; }

        [NotMapped]
        public string FileName { get; set; }
        #endregion

    }
}
