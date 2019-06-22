using Lab_2_webapi.Models;
using Lab_2_webapi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Lab_2_webapi.Models.Task;

namespace Lab_2_webapi.Services
{
    

    public interface ICommentService
    {
        IEnumerable<TaskCommentlModel> GetAll(String keyword);
    }

    public class CommentService : ICommentService
    {
        private TasksDbContext context;

        public CommentService(TasksDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TaskCommentlModel> GetAll(String keyword)
        {
            IQueryable<Task> result = context.Tasks.Include(c => c.Comments);

            List<TaskCommentlModel> resultFilteredComments = new List<TaskCommentlModel>();
            List<TaskCommentlModel> resultAllComments = new List<TaskCommentlModel>();

            foreach (Task task in result)
            {
                task.Comments.ForEach(c =>
                {
                    if (c.Text == null || keyword == null)
                    {
                        TaskCommentlModel comment = new TaskCommentlModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            TaskId = task.Id

                        };
                        resultAllComments.Add(comment);
                    }
                    else if (c.Text.Contains(keyword))
                    {
                        TaskCommentlModel comment = new TaskCommentlModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            TaskId = task.Id

                        };
                        resultFilteredComments.Add(comment);

                    }
                });
            }
            if (keyword == null)
            {
                return resultAllComments;
            }
            return resultFilteredComments;
        }
    }
}
