using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using QuizWebAPI.Controllers;
using QuizWebAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuizWebAPI.Tests
{
    public class QuizControllerTests
    {
        private readonly quizdbContext _context;
        private readonly QuizController _controller;

        public QuizControllerTests()
        {
            _context = new quizdbContext();
            _controller = new QuizController(_context, new NullLogger<QuizController>());
        }


        [Fact]
        public async Task Quiz_GetQuiz_ReturnsSuccessValue()
        {
            //Arrange 
            int id = 1;
            
            //Act 
            var result = await _controller.GetQuiz(id);

            //Assert 
            Assert.IsType<ActionResult<IEnumerable>>(result);
        }


        [Fact]
        public async Task Quiz_GetSave_ReturnsSuccessValue()
        {
            //Arrange 
            int id = 1;

            //Act 
            var result = await _controller.GetSave(id);

            //Assert 
            Assert.IsType<ActionResult<IEnumerable>>(result);
        }

        [Fact]
        public async Task Quiz_DoContinue_ReturnsSuccessValue()
        {
            //Arrange 
            String name = "toon";

            //Act 
            var result = await _controller.DoContinue(name);

            //Assert 
            Assert.IsType<ActionResult<TblUser>>(result);
        }


        [Fact]
        public async Task Quiz_DoSummary_ReturnsSuccessValue()
        {
            //Arrange 
            int id = 1;

            //Act 
            var result = await _controller.DoSummary(id);

            //Assert 
            Assert.IsType<ActionResult<TblUser>>(result);
        }


        [Fact]
        public async Task Quiz_DoSave_ReturnsSuccessValue()
        {
            //Arrange 
            TblUserScore[] tblUserScore = new TblUserScore[1];

            //Act 
            var result = await _controller.DoSave(tblUserScore);

            //Assert 
            Assert.IsType<ActionResult<TblUserScore>>(result);
        }



        [Fact]
        public async Task Quiz_DoSubmit_ReturnsSuccessValue()
        {
            //Arrange 
            TblUserScore[] tblUserScore = new TblUserScore[1];

            //Act 
            var result = await _controller.DoSubmit(tblUserScore);

            //Assert 
            Assert.IsType<ActionResult<TblUser>>(result);
        }
    }
}
