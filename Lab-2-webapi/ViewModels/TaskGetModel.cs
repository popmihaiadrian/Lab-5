using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_2_webapi.Models;
using Task = Lab_2_webapi.Models.Task;

namespace Lab_2_webapi.ViewModels
{
    public class TaskGetModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int NumberOfComments { get; set; }

        public static TaskGetModel FromTask(Task task)
        {
            return new TaskGetModel
            {
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.ToString(),
                NumberOfComments = task.Comments.Count
            };
        }
    }
}
