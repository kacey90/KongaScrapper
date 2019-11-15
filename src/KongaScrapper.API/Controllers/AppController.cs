using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KongaScrapper.Scraper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KongaScrapper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private readonly CategoryHtmlScraper _categoryHtmlScraper;

        public AppController(CategoryHtmlScraper categoryHtmlScraper)
        {
            _categoryHtmlScraper = categoryHtmlScraper;
        }
        [HttpGet]
        [Route("kongascrapper")]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery]int treasurePrice)
        {
            var url = "https://www.konga.com/" + path;
            var result = await _categoryHtmlScraper.ScrapeCategoryPage(url, treasurePrice);
            return Ok(result);
        }
    }
}