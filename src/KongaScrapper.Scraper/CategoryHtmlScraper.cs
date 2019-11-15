using AngleSharp;
using KongaScrapper.Scraper.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KongaScrapper.Scraper
{
    public class CategoryHtmlScraper
    {
        private readonly IBrowsingContext _browsingContext;
        private readonly String websiteUrl = "https://www.konga.com";
        private readonly ILogger _logger;

        public CategoryHtmlScraper(IBrowsingContext browsingContext, ILogger<CategoryHtmlScraper> logger)
        {
            this._browsingContext = browsingContext;
            _logger = logger;
        }

        private async Task<List<ProductItem>> GetPageData(string url, int treasurePrice, List<ProductItem> results)
        {
            var pager = 1;
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);

            var productItems = document.QuerySelectorAll("#mainContent > section._9cac3_2I9l4 > section > section > section > section > ul > li");

            foreach (var item in productItems)
            {
                var productItem = new ProductItem();

                MatchCollection regexMatches = Regex.Matches(item.QuerySelector("span.d7c0f_sJAqi").TextContent, @"^[-+]?(?:[0-9]+,)*[0-9]+(?:\.[0-9]+)?$");
                uint.TryParse(string.Join("", regexMatches), out uint price);
                productItem.Price = price;

                var title = item.QuerySelector("div.af885_1iPzH > h3").TextContent;
                productItem.Title = title;

                productItem.ProductUrl = websiteUrl + item.QuerySelector("div._4941f_1HCZm > a").GetAttribute("Href");

                results.Add(productItem);

                if (productItem.Price == treasurePrice)
                {
                    _logger.LogInformation("Treasure Hunt Found! " + productItem.ProductUrl);
                    return results;
                }
            }


            string nextPageUrl = "";
            var nextPageLink = document.QuerySelector("div._30a64_UM2-5 > a._08932_1bhTj._4c4d8_1SOeS._37aeb_13AdS");
            if (nextPageLink != null)
            {
                pager = pager++;
                if (nextPageLink.GetAttribute("Href").Contains(pager.ToString()))
                    nextPageUrl = websiteUrl + nextPageLink.GetAttribute("Href");
            }

            // If next page link is present recursively call the function again with the new url
            if (!String.IsNullOrEmpty(nextPageUrl))
            {
                return await GetPageData(nextPageUrl, treasurePrice, results);
            }

            results.RemoveAt(results.Count - 1);
            return results;
        }

        public async Task<List<ProductItem>> ScrapeCategoryPage(string url, int treasurePrice)
        {
            List<ProductItem> productItems = new List<ProductItem>();

            var results = await GetPageData(url, treasurePrice, productItems);
            return results;
        }
    }
}
