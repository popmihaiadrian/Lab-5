using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_2_webapi.Services;
using Lab_2_webapi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab_2_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        /// <summary>
        /// Gets all comments filtered by a string
        /// </summary>
        /// <param name="filter">The keyword used to search comments</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public IEnumerable<TaskCommentlModel> GetAll([FromQuery]String filter)
        {
            return commentService.GetAll(filter);
        }
    }
}
