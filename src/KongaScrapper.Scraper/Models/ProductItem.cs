using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KongaScrapper.Scraper.Models
{
    public class ProductItem
    {
        public string ProductUrl { get; set; }
        public string Title { get; set; }
        public uint Price { get; set; }
    }
}
