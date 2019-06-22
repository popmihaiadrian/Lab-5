using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2_webapi.Models
{
    public class TasksDbSeeder
    {
        public static void Initialize(TasksDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            context.Tasks.AddRange(
                new Task
                {
                    Title = "Todo1",
                    Description = "description1",

                },

                new Task
                {
                    Title = "Todo2",
                    Description = "description2"
                }
            );
            context.SaveChanges(); //commit transactions
        }
    }
}
