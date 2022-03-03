using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebAPI.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly quizdbContext _context;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(quizdbContext context, ILogger<RegisterController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpPost] //to register user name
        public async Task<ActionResult<TblUser>> Post(TblUser user)
        {
            _logger.LogInformation("Start Module");
            try
            {
                if (user == null)
                {
                    _logger.LogInformation("Return StatusCode 400");
                    return BadRequest(user);
                }

                var data = await (_context.TblUsers.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync<TblUser>());
                _logger.LogInformation("Query successful");
                if (data != null && data.UserId > 0)
                {
                    _logger.LogInformation("Return Content { \"message\":\"Name is dupilcate\" }");
                    return Content("{ \"message\":\"Name is dupilcate\" }", "application/json");
                }

                user.UserStatus = "Y";
                await _context.TblUsers.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Save successful");
                _logger.LogInformation("Return StatusCode 200");
                return await Task.FromResult(user);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }
    }
}
