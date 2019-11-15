using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KongaScrapper.Scraper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAngleSharp(this IServiceCollection services) =>
            services.AddSingleton(BrowsingContext.New(
                Configuration.Default.WithDefaultLoader()));
    }
}
