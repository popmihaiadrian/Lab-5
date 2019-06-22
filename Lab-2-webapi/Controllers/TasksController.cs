using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_2_webapi.Models;
using Lab_2_webapi.Services;
using Lab_2_webapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_2_webapi.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskService taskService;
        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        [HttpGet]
        /// GET: api/Tasks

        /// <summary>
        /// Gets all the flowers.
        /// </summary>
        /// <param name="from">Optional, filter by minimum DatePicked.</param>
        /// <param name="to">Optional, filter by maximum DatePicked.</param>
        /// <returns>A list of Flower objects.</returns>
        public PaginatedList<TaskGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery] int page = 1)
        {
           
            page = Math.Max(page, 1);
            return taskService.GetAll(page, from, to);
          
        }








        /// Gets all the tasks.
        /// </summary>

        /// <param name="from">Optional, filter by minimum Deadline</param>
        /// <param name="to">Optional, filter by maximum Deadline</param>
        /// <returns>A list of Task objects</returns>

        //[HttpGet]
        //public IEnumerable<TaskGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to)
        //{
        //    return taskService.GetAll(from, to);
        //}

        // GET: api/Tasks/5
        /// <summary>
        /// Get a task by a given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task with a given ID</returns>
        [HttpGet("{id}", Name = "Get")]

        public IActionResult Get(int id)
        {
            var found = taskService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        /// <summary>
        /// Add a task
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Tasks
        ///     {
        ///         "title": "Todo with comment",
        ///         "description": "description4",
        ///         "dateAdded": "2019-05-06T00:00:00",
        ///         "deadline": "2019-05-15T00:00:00",
        ///         "imp": 1,
        ///         "closedAt": "2019-05-17T00:00:00",
        ///         "comments": [
        ///         	{
        ///         		"text": "morning task",
        ///         		"important": true
        ///
        ///             },
        ///         	{
        ///		
        ///         		"text": "first task",
        ///         		"important": false
        ///         	}
        ///	        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="task">The task to add.</param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Roles = "Admin, Regular")]
        public void Post([FromBody] Models.Task task)
        {
            taskService.Create(task);
        }

        // PUT: api/Tasks/5
        /// <summary>
        /// Update a task with the given ID, or create a new task if the ID does not exist.
        /// </summary>
        /// <param name="id">task ID</param>
        /// <param name="task">The object Task</param>
        /// <returns>The updated task/new created task.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Regular")]
        public IActionResult Put(int id, [FromBody] Models.Task task)
        {
            var result = taskService.Upsert(id, task);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete a task by a given ID
        /// </summary>
        /// <param name="id">ID of the task to be deleted.</param>
        /// <returns>The deleted task object.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Regular")]
        public IActionResult Delete(int id)
        {
            var result = taskService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
