using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NLog.Web;
using QuizWebAPI.Controllers;
using QuizWebAPI.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using Xunit;

namespace QuizWebAPI.Tests
{
    public class UserGroupControllerTest
    {
        private readonly quizdbContext _context;
        private readonly UserGroupController _controller;

        public UserGroupControllerTest()
        {
            _context = new quizdbContext();
            _controller = new UserGroupController(_context, new NullLogger<UserGroupController>());
        }

        [Fact]
        public async Task UserGroup_Normal_ReturnsSuccessValue()
        {
            //Arrange 

            //Act 
            var result = await _controller.Get();

            //Assert 
            Assert.IsType<ActionResult<IEnumerable>>(result);
        }
    }
}
