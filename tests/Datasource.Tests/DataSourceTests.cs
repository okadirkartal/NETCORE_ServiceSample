using System.IO;
using Configuration;
using DataSource;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace Datasource.Tests
{
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
            CopyJsonFiles(_serviceDirectory, _configuration["DataSource:FilePaths:CityDataFilePath"]);

            CopyJsonFiles(_serviceDirectory, _configuration["DataSource:FilePaths:WeatherInformationDataFilePath"]);

            CopyJsonFiles(_serviceDirectory, _configuration["DataSource:FilePaths:GeolocationFilePath"]);
        }

        [TestCase("DataSource:FilePaths:CityDataFilePath")]
        [TestCase("DataSource:FilePaths:WeatherInformationDataFilePath")]
        [TestCase("DataSource:FilePaths:GeolocationFilePath")]
        public void CheckCityDataFiles_WhenExists_ReturnsTrue(string parameter)
        {
            var filePath = _configuration[parameter];
            string dataPath = Path.Combine(_serviceDirectory, $@"src{Path.DirectorySeparatorChar}Services\");
            Assert.IsTrue(File.Exists(Path.Combine(dataPath, filePath)));
        }

        [Test]
        public void CheckCityData_WhenExists_ReturnsTrue()
        {
            var mockedConfiguration = Substitute.For<IOptions<DatasourceConfiguration>>();
            mockedConfiguration.Value.Returns(new DatasourceConfiguration()
                {CityDataFilePath = _configuration["DataSource:FilePaths:CityDataFilePath"]});

            var result = new CityDataRetriever(mockedConfiguration).GetData(null);
            Assert.AreEqual(result.Count, 9);
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

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            File.Delete(Path.Combine(Directory.GetCurrentDirectory(),
                "appSettings.json"));
            Directory.Delete("Data", true);
        }
    }
}