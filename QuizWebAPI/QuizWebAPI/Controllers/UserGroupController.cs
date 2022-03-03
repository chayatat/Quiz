using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizWebAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebAPI.Controllers
{
    [ApiController]
    [Route("api/usergroup")]
    public class UserGroupController : ControllerBase
    {
        private readonly quizdbContext _context;
        private readonly ILogger<UserGroupController> _logger;

        public UserGroupController(quizdbContext context, ILogger<UserGroupController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpGet] //to load user group
        public async Task<ActionResult<IEnumerable>> Get()
        {
            _logger.LogInformation("Start Module");
            try
            {
                var data = await _context.TblUsergroups.ToListAsync();
                _logger.LogInformation("Query successful");
                if (data == null)
                {
                    _logger.LogInformation("Return StatusCode 400");
                    return BadRequest();
                }
                
                _logger.LogInformation("Return StatusCode 200");
                return Ok(data);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
            
        }

    }
}
