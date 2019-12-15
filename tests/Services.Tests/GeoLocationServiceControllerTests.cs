 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataSource.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using Models.GeoLocation;
using NSubstitute; 
using Services.Controllers;
using Xunit;

namespace Services.Tests
{
    public class GeoLocationServiceControllerTests
    {

        [Fact]
        public void Constructor_WhenIDataRetrieverArgumentIsNull_ThrowsArgumentNullException()
        {
            var logger = Substitute.For<ILoggerFactory>();
         
            Assert.Throws<ArgumentNullException>(() => new GeoLocationServiceController(logger,null));
        }

        [Fact]
        public void Constructor_WhenILoggerFactoryRetrieverArgumentIsNull_ThrowsArgumentNullException()
        {
            var retriever = Substitute.For<IDataRetriever<GeoLocation>>();

            Assert.Throws<ArgumentNullException>(() => new GeoLocationServiceController(null, retriever));
        }

        [Fact]
        public async void PostAsync_WhenArgumentIsNull_ReturnsBadRequest()
        {

            var logger = Substitute.For<ILoggerFactory>();
            var retriever = Substitute.For<IDataRetriever<GeoLocation>>();
            var geoLocationServiceController = new GeoLocationServiceController(logger, retriever);


            IActionResult result = await geoLocationServiceController.Post(null);

            result.Should().BeOfType(typeof(BadRequestResult)); 
        }


        [Fact]
        public async Task PostAsync_WhenMethodIsCalled_TypeShouldBeOk()
        {

            var logger = Substitute.For<ILoggerFactory>();

            var retriever = Substitute.For<IDataRetriever<GeoLocation>>();
            retriever.GetData().Returns(new List<GeoLocation>());

            var geoLocationServiceController = new GeoLocationServiceController(logger, retriever);


            var result = await geoLocationServiceController.Post("60610");


            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public async void GetTimeZoneAsync_WhenArgumentIsNull_ReturnsBadRequest()
        {

            var logger = Substitute.For<ILoggerFactory>();
            var retriever = Substitute.For<IDataRetriever<GeoLocation>>();
            var geoLocationServiceController = new GeoLocationServiceController(logger, retriever);


            var result = await geoLocationServiceController.GetTimeZone(null);

            result.Should().BeOfType(typeof(BadRequestResult));
        }



        [Fact]
        public async Task GetTimeZoneAsync_WhenMethodIsCalled_TypeShouldBeOk()
        {

            var logger = Substitute.For<ILoggerFactory>();

            var retriever = Substitute.For<IDataRetriever<GeoLocation>>();
            retriever.GetData("60610").Returns(new List<GeoLocation>(){ new GeoLocation(){ timezone = new Timezone()}});

            var geoLocationServiceController = new GeoLocationServiceController(logger, retriever); 


           var result = await geoLocationServiceController.GetTimeZone("60610");

           result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}