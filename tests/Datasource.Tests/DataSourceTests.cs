using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using DataSource;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using CKeys = Common.Common;

namespace Datasource.Tests
{
    [Order(2)]
    public class DataSourceTests : BaseTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            //Get solution path
            _serviceDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent
                ?.FullName;
            //Copy appSettings.json file from Service project 
            CopyJsonFiles(_serviceDirectory, "appSettings.json");

            _configuration = GetApplicationConfiguration();

            Directory.CreateDirectory("Data");
            CopyJsonFiles(_serviceDirectory, _configuration[CKeys.CityDataConfigKey]);

            CopyJsonFiles(_serviceDirectory, _configuration[CKeys.GeoLocationDataConfigKey]);
        }

        [TestCase(CKeys.CityDataConfigKey)]
        [TestCase(CKeys.GeoLocationDataConfigKey)]
        public void CheckJsonFiles_WhenExists_ReturnsTrue(string parameter)
        {
            var filePath = _configuration[parameter];
            string dataPath = Path.Combine(_serviceDirectory, $@"src{Path.DirectorySeparatorChar}Services\");
            Assert.IsTrue(File.Exists(Path.Combine(dataPath, filePath)));
        }

        [Test]
        public async Task CheckCityData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {CityDataFilePath = _configuration[CKeys.CityDataConfigKey]});

            var result = await new CityDataRetriever(mockedConfiguration).GetDataFromFile("745044");
            Assert.Greater(result.Count, 0);
        }

        #region GeoLocationDataRetriever Tests

        [Test]
        public async Task CheckGeoLocationDataFromFile_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {GeolocationDataFilePath = _configuration[CKeys.GeoLocationDataConfigKey]});

            var mockedHttpClient = Substitute.For<IHttpClientFactory>();
            mockedHttpClient.CreateClient("test");


            var logger = Substitute.For<ILogger<GeoLocationDataRetriever>>();

            var result = await new GeoLocationDataRetriever(mockedConfiguration, mockedHttpClient, logger)
                .GetDataFromFile(null);
            Assert.Greater(result.Count, 0);
        }

        [Test]
        public async Task CheckGeoLocationData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {GeolocationDataFilePath = _configuration[CKeys.GeoLocationDataConfigKey]});

            var mockedHttpClient = Substitute.For<IHttpClientFactory>();
            mockedHttpClient.CreateClient("test");


            var logger = Substitute.For<ILogger<GeoLocationDataRetriever>>();

            var result = await new GeoLocationDataRetriever(mockedConfiguration, mockedHttpClient, logger)
                .GetDataFromFile("60610");
            Assert.AreEqual(result.Count, 5);
        }

        [Test]
        public async Task CheckGeoLocationRandomData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {GeolocationDataFilePath = _configuration[CKeys.GeoLocationDataConfigKey]});

            var mockedHttpClient = Substitute.For<IHttpClientFactory>();
            mockedHttpClient.CreateClient("test");


            var logger = Substitute.For<ILogger<GeoLocationDataRetriever>>();

            var result = await new GeoLocationDataRetriever(mockedConfiguration, mockedHttpClient, logger)
                .GetRandomData();
            Assert.IsNotNull(result.acceptable_city_names);
        }

        #endregion


        #region  WeatherDataRetriever Tests

        [Test]
        public async Task CheckWeatherData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {WeatherApiBaseUrl = _configuration[CKeys.WeatherApiBaseUrl]});

            var mockedHttpClient = Substitute.For<IHttpClientFactory>();
            mockedHttpClient.CreateClient("test");


            var logger = Substitute.For<ILogger<WeatherDataRetriever>>();

            var result = await new WeatherDataRetriever(mockedConfiguration, mockedHttpClient, logger)
                .GetData("745044");
            Assert.Greater(result.Count, 0);
        }

        #endregion

        //Delete configuration and json data files after all test finished
        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(),
                "appSettings.json"));
            Directory.Delete("Data", true);
        }
    }
}