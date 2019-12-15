using System.IO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Datasource.Tests
{
    public class BaseTests
    {
        protected string _serviceDirectory;

        protected IConfiguration _configuration;

        private static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        protected static void CopyJsonFiles(string serviceDirectory, string jsonFileName)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), jsonFileName)))
                File.Copy(
                    Path.Combine(serviceDirectory,
                        $@"src{Path.DirectorySeparatorChar}Services{Path.DirectorySeparatorChar}{jsonFileName}"),
                    jsonFileName);
        }

        protected static IConfiguration GetApplicationConfiguration()
        {
            return GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
        }
    }
}