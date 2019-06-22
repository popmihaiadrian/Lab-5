using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2_webapi.ViewModels
{
    public class TaskPostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public DateTime ClosedAt { get; set; }

       
    }
}
