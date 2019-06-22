using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2_webapi.Models
{
    public class Task
    {
        public enum TaskImportance
        {
            Low,
            Medium,
            Hight
        }

         public enum State {
            Open,
            InProgress,
            Closed
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }

        [EnumDataType(typeof(TaskImportance))]
        public TaskImportance Imp { get; set; }

        [EnumDataType(typeof(State))]
        public State? Status { get; set; }

        public DateTime? ClosedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
