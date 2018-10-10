using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace ImageConverter.Models
{
    public class ImageConverterDbContext : DbContext
    {
        public ImageConverterDbContext(DbContextOptions<ImageConverterDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=SFO-PC\SQL2014;Database=ImageConverterDB;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<ConversionData> ConversionData { get; set; }
    }
}
