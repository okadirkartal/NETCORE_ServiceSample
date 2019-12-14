using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Configuration;
using DataSource;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using NUnit.Framework.Internal;
using CKeys = Common.Common;

namespace Datasource.Tests
{
    [Order(2)]
    public class DataSourceTests
    {
        private string _serviceDirectory;

        private IConfiguration _configuration;


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

            CopyJsonFiles(_serviceDirectory, _configuration[CKeys.WheatherDataConfigKey]);

            CopyJsonFiles(_serviceDirectory, _configuration[CKeys.GeoLocationDataConfigKey]);
        }

        [TestCase(CKeys.CityDataConfigKey)]
        [TestCase(CKeys.WheatherDataConfigKey)]
        [TestCase(CKeys.GeoLocationDataConfigKey)]
        public void CheckCityDataFiles_WhenExists_ReturnsTrue(string parameter)
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

            var result = await new CityDataRetriever(mockedConfiguration).GetData(null);
            Assert.AreEqual(result.Count, 9);
        }

        [Test]
        public async Task CheckWeatherData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {WeatherDataFilePath = _configuration[CKeys.WheatherDataConfigKey]});

            var result = await new WheatherDataRetriever(mockedConfiguration).GetData(null);
            Assert.AreEqual(result.Count, 9);
        }

        [Test]
        public async Task CheckGeoLocationData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {GeolocationDataFilePath = _configuration[CKeys.GeoLocationDataConfigKey]});

            var mockedHttpClient = Substitute.For<IHttpClientFactory>();
            mockedHttpClient.CreateClient("test");

            var result = await new GeoLocationDataRetriever(mockedConfiguration, mockedHttpClient).GetGeoLocationList();
            Assert.AreEqual(result.Count, 5);
        }

        #region Non Tests

        static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        static void CopyJsonFiles(string serviceDirectory, string jsonFileName)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), jsonFileName)))
                File.Copy(
                    Path.Combine(serviceDirectory,
                        $@"src{Path.DirectorySeparatorChar}Services{Path.DirectorySeparatorChar}{jsonFileName}"),
                    jsonFileName);
        }

        static IConfiguration GetApplicationConfiguration()
        {
            return GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
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