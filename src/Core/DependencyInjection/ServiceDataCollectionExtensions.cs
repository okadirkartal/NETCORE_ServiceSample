using DataSource;
using DataSource.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DependencyInjection
{
    public static class ServiceDataCollectionExtensions
    {
        public static IServiceCollection AddServiceRetrieverServiceCollection(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IDataRetriever<City>),
                typeof(CityDataRetriever)); // open generic registration
            services.TryAddSingleton(typeof(IDataRetriever<Weather>), typeof(WheatherDataRetriever));
            services.TryAddSingleton(typeof(IDataRetriever<GeoLocation>), typeof(GeoLocationDataRetriever));
            return services;
        }
    }
}