using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2_webapi.Models
{

    //DbContext = Unit of work
    public class TasksDbContext :DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> optons) : base(optons)
        {
        }

        //DbSet = Repository
        //DbSet = O tabela din baza de date
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
