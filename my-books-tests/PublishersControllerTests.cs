using Microsoft.EntityFrameworkCore;
using my_books.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using my_books.Controllers;
using Microsoft.Extensions.Logging;
using my_books.Data.Services;
using my_books.Data.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.ViewModels;

namespace my_books_tests
{
    public class PublishersControllerTests
    {
        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "BookDbControllerTest")
            .Options;

        AppDbContext context;
        private PublishersService publishersService;
        private PublishersController publishersController;

        [OneTimeSetUp]
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated();

            SeedDatabase();
            publishersService = new PublishersService(context);
            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>());
        }

        [Test, Order(1)]
        public void HttpGet_GetAllPublishers_WithSortBy_SearchStr_PageNum_ReturnOk_Test()
        {
            IActionResult actionResult1stPage = publishersController.GetAllPublishers("name_desc", "publisher", 1);
            Assert.That(actionResult1stPage, Is.TypeOf<OkObjectResult>());
            var actionResult1stPageData = (actionResult1stPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResult1stPageData.First().Name, Is.EqualTo("Publisher 6"));
            Assert.That(actionResult1stPageData.First().Id, Is.EqualTo(6));
            Assert.That(actionResult1stPageData.Count, Is.EqualTo(5));

            IActionResult actionResult2ndPage = publishersController.GetAllPublishers("name_desc", "publisher", 2);
            Assert.That(actionResult2ndPage, Is.TypeOf<OkObjectResult>());
            var actionResult2ndPageData = (actionResult2ndPage as OkObjectResult).Value as List<Publisher>;
            Assert.That(actionResult2ndPageData.First().Name, Is.EqualTo("Publisher 1"));
            Assert.That(actionResult2ndPageData.First().Id, Is.EqualTo(1));
            Assert.That(actionResult2ndPageData.Count, Is.EqualTo(1));
        }

        [Test, Order(2)]
        public void HttpGet_GetPublisherById_ReturnOk_Test()
        {
            int publisherId = 1;
            IActionResult actionResult = publishersController.GetPublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
            var publisherData = (actionResult as OkObjectResult).Value as Publisher;
            Assert.That(publisherData.Id, Is.EqualTo(1));
            Assert.That(publisherData.Name, Is.EqualTo("publisher 1").IgnoreCase);
        }

        [Test, Order(3)]
        public void HttpGet_GetPublisherById_ReturnNotFound_Test()
        {
            int publisherId = 99;
            IActionResult actionResult = publishersController.GetPublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<NotFoundResult>()); //не NotFoundObjectResult
                                                                    //т.к в скобках передаются данные и нет
        }

        [Test, Order(4)]
        public void HttpPost_AddPublisher_ReturnsCreated_Test()
        {
            var newPublisherVM = new PublisherVM()
            {
                Name = "New Publisher"
            };
            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);
            Assert.That(actionResult, Is.TypeOf<CreatedResult>());
        }

        [Test, Order(5)]
        public void HttpPost_AddPublisher_ReturnsBadRequest_Test()
        {
            var newPublisherVM = new PublisherVM()
            {
                Name = "123 New Publisher"
            };
            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(6)]
        public void HttpDelete_DeletePublisherById_ReturnsOk_Test()
        {
            int publisherId = 6;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HttpDelete_DeletePublisherById_ReturnsBadRequest_Test()
        {
            int publisherId = 6;
            IActionResult actionResult = publishersController.DeletePublisherById(publisherId);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var publishers = new List<Publisher>
            {
                    new Publisher() {
                        Id = 1,
                        Name = "Publisher 1"
                    },
                    new Publisher() {
                        Id = 2,
                        Name = "Publisher 2"
                    },
                    new Publisher() {
                        Id = 3,
                        Name = "Publisher 3"
                    },
                    new Publisher() {
                        Id = 4,
                        Name = "Publisher 4"
                    },
                    new Publisher() {
                        Id = 5,
                        Name = "Publisher 5"
                    },
                    new Publisher() {
                        Id = 6,
                        Name = "Publisher 6"
                    },
            };
            context.Publishers.AddRange(publishers);
            context.SaveChanges();
        }
    }
}
