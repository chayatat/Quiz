using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using QuizWebAPI.Controllers;
using QuizWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace QuizWebAPI.Tests
{
    public class RegisterControllerTests
    {
        private readonly quizdbContext _context;
        private readonly RegisterController _controller;

        public RegisterControllerTests()
        {
            _context = new quizdbContext();
            _controller = new RegisterController(_context, new NullLogger<RegisterController>());
        }

        
        [Fact]
        public async Task Register_Post_ReturnsSuccessValue()
        {
            //Arrange 
            TblUser tblUser = new TblUser();
            tblUser.UserName = "dumbo";
            tblUser.UsergroupId = 1;

            //Act 
            var result = await _controller.Post(tblUser);

            //Assert 
            Assert.IsType<ActionResult<TblUser>>(result);
        }

    }
}
