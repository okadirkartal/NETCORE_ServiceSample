
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataSource.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using NSubstitute;
using Services.Controllers;
using Xunit;

namespace Services.Tests
{
    public class WeatherServiceControllerTests
    {

        [Fact]
        public void Constructor_WhenDataRetrieverArgumentIsNull_ThrowsArgumentNullException()
        {
            var logger = Substitute.For<ILoggerFactory>();

            Assert.Throws<ArgumentNullException>(() => new WeatherServiceController(logger, null));
        }

        [Fact]
        public void Constructor_WhenILoggerFactoryRetrieverArgumentIsNull_ThrowsArgumentNullException()
        {
            var retriever = Substitute.For<IDataRetriever<WeatherInfo>>();

            Assert.Throws<ArgumentNullException>(() => new WeatherServiceController(null, retriever));
        }

        [Fact]
        public async void GetAsync_WhenArgumentIsNull_ReturnsBadRequest()
        {

            var logger = Substitute.For<ILoggerFactory>();
            var retriever = Substitute.For<IDataRetriever<WeatherInfo>>();
            var weatherServiceController = new WeatherServiceController(logger, retriever);


            IActionResult result = await weatherServiceController.Get(null);

            result.Should().BeOfType(typeof(BadRequestResult));
        }


        [Fact]
        public async Task GetAsync_WhenTMethodIsCalled_TypeShouldBeOk()
        {

            var logger = Substitute.For<ILoggerFactory>();

            var retriever = Substitute.For<IDataRetriever<WeatherInfo>>();
            retriever.GetData().Returns(new List<WeatherInfo>());

            var weatherServiceController = new WeatherServiceController(logger, retriever);


            var result = await weatherServiceController.Get("745044");


            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}