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
    [Route("api")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly quizdbContext _context;
        private readonly ILogger<QuizController> _logger;

        public QuizController(quizdbContext context, ILogger<QuizController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpGet("quiz/{id}")] //to load quiz and choices
        public async Task<ActionResult<IEnumerable>> GetQuiz(int id)
        {
            _logger.LogInformation("Start Module");
            try
            {
                var data = await _context.TblQuestions
                .Where(q => q.UsergroupId == id)
                .OrderBy(q => q.QuestionSort)
                .Join(_context.TblChoices,
                q => q.QuestionId,
                c => c.ChoiceId,
                (q, c) => new { q, c })
                .Select(res => new
                {
                    res.q.QuestionId,
                    res.q.QuestionName,
                    res.q.QuestionSort,
                    res.q.TblChoices
                })
                .ToListAsync();
                _logger.LogInformation("Query successful");

                if (data == null)
                {
                    _logger.LogInformation("Return Content { \"message\":\"UserGroup is not have a question\" }");
                    return Content("{ \"message\":\"UserGroup is not have a question\" }", "application/json");
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

        [HttpGet("load/{id}")] //to load user saved answer
        public async Task<ActionResult<IEnumerable>> GetSave(int id)
        {
            _logger.LogInformation("Start Module");
            try
            {
                var data = await _context.TblUserScores
                    .Where(us => us.UserId == id)
                    .ToListAsync();
                _logger.LogInformation("Query successful");

                if (data == null)
                {
                    _logger.LogInformation("Return StatusCode 200");
                    return Ok();
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

        [HttpPost("save")] //to save user’s answer
        public async Task<ActionResult<TblUserScore>> DoSave([FromBody] TblUserScore[] users)
        {
            _logger.LogInformation("Start Module");
            try
            {
                if (users == null)
                {
                    _logger.LogInformation("Return StatusCode 400");
                    return BadRequest();
                }

                foreach(TblUserScore u in users)
                {
                    if(u.UserScoreId == 0)
                    {
                        u.UserScoreStatus = "Y";
                        await _context.TblUserScores.AddAsync(u);
                    }
                    else
                    {
                        var us = await _context.TblUserScores.FindAsync(u.UserScoreId);
                        us.ChoiceId = u.ChoiceId;
                        us.ChoiceScore = u.ChoiceScore;
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Save successful");
                _logger.LogInformation("Return Content { \"message\":\"Save score success\" }");

                return Content("{ \"message\":\"Save score success\" }", "application/json");
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }


        [HttpPost("submit")] //to submit user’s answer and return summary
        public async Task<ActionResult<TblUser>> DoSubmit([FromBody] TblUserScore[] users)
        {
            _logger.LogInformation("Start Module");
            try
            {
                if (users == null)
                {
                    _logger.LogInformation("Return StatusCode 400");
                    return BadRequest();
                }
                int userid = 0;
                int sumscore = 0;
                foreach (TblUserScore u in users)
                {
                    userid = u.UserId;
                    sumscore += u.ChoiceScore;
                    if (u.UserScoreId == 0)
                    {
                        u.UserScoreStatus = "N";
                        await _context.TblUserScores.AddAsync(u);
                    }
                    else
                    {
                        var us = await _context.TblUserScores.FindAsync(u.UserScoreId);
                        us.ChoiceId = u.ChoiceId;
                        us.ChoiceScore = u.ChoiceScore;
                        us.UserScoreStatus = "N";
                    }
                }

                var user = await _context.TblUsers.Where(u => u.UserId == userid).FirstAsync();
                user.UserStatus = "N";
                user.UserTotalScore = sumscore;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Save successful");
                _logger.LogInformation("Return Content { \"message\":\"Submit score success\" }");

                return Content("{ \"message\":\"Submit score success\" }", "application/json");
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }


        [HttpGet("summary/{id}")] //to load user summary result
        public async Task<ActionResult<TblUser>> DoSummary(int id)
        {
            _logger.LogInformation("Start Module");
            try
            {
                var user = await _context.TblUsers
                    .Where(u => u.UserId == id)
                    .Join(_context.TblUsergroups,
                    u => u.UsergroupId,
                    g => g.UsergroupId,
                    (u,g) => new { u, g })
                    .Select(res => new
                    {
                        res.u.UserId,
                        res.u.UserName,
                        res.u.UserStatus,
                        res.u.UserTotalScore,
                        res.g.UsergroupId,
                        res.g.UsergroupName
                    })
                    .FirstAsync();

                var data = await _context.TblUsers
                    .Where(u => u.UsergroupId == user.UsergroupId)
                    .OrderByDescending(i => i.UserTotalScore)
                    .ToListAsync();
                _logger.LogInformation("Query TblUsers successful");

                if (data == null)
                {
                    _logger.LogInformation("Return StatusCode 204");
                    return BadRequest();
                }


                var quiz = await _context.TblQuestions
                    .Where(q => q.UsergroupId == user.UsergroupId)
                    .ToListAsync();

                var countMax = await _context.TblChoices
                    .GroupBy(c => c.QuestionId)
                    .Select(g => new
                    {
                        id = g.Key,
                        maxscore = g.Max(x => x.ChoiceScore)
                    })
                    .ToListAsync();
                _logger.LogInformation("Query maxscore successful");

                int maxscore = 0;
                foreach (TblQuestion q in quiz)
                {
                    Console.WriteLine(q.QuestionId);
                    foreach(var o in countMax)
                    {
                        Console.WriteLine(o);
                        if(o.id == q.QuestionId)
                        {
                            maxscore += o.maxscore;
                            break;
                        }
                    }
                }


                int rank = 0;
                foreach(TblUser u in data)
                {
                    rank++;
                    if(u.UserId == id)
                    {
                        break;
                    }
                }

                _logger.LogInformation("Return StatusCode 200");
                return Ok(new { user, rank, maxscore } );
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpGet("continue/{name}")]
        public async Task<ActionResult<TblUser>> DoContinue(String name) 
        {
            _logger.LogInformation("Start Module");
            try
            {
                var user = await _context.TblUsers.Where(u => u.UserName == name).FirstOrDefaultAsync();
                _logger.LogInformation("Query successful");
                if (user == null)
                {
                    _logger.LogInformation("Return Content { \"message\":\"Can't find this " + name + " name, please register first.\" }");
                    return Content("{ \"message\":\"Can't find this " + name + " name, please register first.\" }", "application/json");
                }
                _logger.LogInformation("Return StatusCode 200");
                return Ok(user);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving data from database");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

    }
}
