using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab_2_webapi.Models;

namespace Lab_2_webapi.ViewModels
{
    public class TaskCommentlModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int TaskId { get; set; }
        public object Tasks { get; internal set; }

        //public static implicit operator TaskCommentlModel(TasksDbContext v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
