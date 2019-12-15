using DataSource;
using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;

namespace DependencyInjection
{
    public static class ServiceDataCollectionExtensions
    {
        public static IServiceCollection AddServiceRetrieverServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IOfflineDataRetriever<City>, CityDataRetriever>();
         
            services.TryAddScoped(typeof(IDataRetriever<WeatherInfo>), typeof(WeatherDataRetriever));

            services.TryAddScoped(typeof(IDataRetriever<GeoLocation>), typeof(GeoLocationDataRetriever));
            return services;
        }
    }
}