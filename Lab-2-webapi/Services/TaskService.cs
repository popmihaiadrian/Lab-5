using Lab_2_webapi.Models;
using Lab_2_webapi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Lab_2_webapi.Services
{
    public interface ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        //IEnumerable<TaskGetModel> GetAll(DateTime? from=null, DateTime? to=null);
        Task GetById(int id);
        Task Create(Task task);
        Task Upsert(int id, Task task);
        Task Delete(int id);
        PaginatedList<TaskGetModel> GetAll(int page, DateTime? from, DateTime? to);
    }
    public class TaskService : ITaskService
    {
        private Models.TasksDbContext context;
        public TaskService(Models.TasksDbContext context)
        {
            this.context = context;
        }

        public Task Create(Task task)
        {
            context.Tasks.Add(task);
            context.SaveChanges();
            return task;
        }

        public Task Delete(int id)
        {
            var existing = context.Tasks
                .Include(t => t.Comments)
                .FirstOrDefault(task => task.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Tasks.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public IEnumerable<TaskGetModel> GetAll(DateTime? from=null, DateTime? to=null)
        {
                IQueryable<Task> result = context
                .Tasks
                .Include(t => t.Comments);
            if (from == null & to == null)
            {
                return result.Select(t => TaskGetModel.FromTask(t));
            }
            if (from != null)
            {
                result = result.Where(t => t.Deadline >= from);
            }
            if (to != null)
            {
                result = result.Where(t => t.Deadline <= to);
            }
            return result.Select(t => TaskGetModel.FromTask(t));
        }

        public PaginatedList<TaskGetModel> GetAll(int page, DateTime? from, DateTime? to)
        {
            IQueryable<Task> result = context
               .Tasks
               .Include(t => t.Comments);
            PaginatedList<TaskGetModel> paginatedResult = new PaginatedList<TaskGetModel>();
            paginatedResult.CurrentPage = page;

            if (from != null)
            {
                result = result.Where(t => t.Deadline >= from);
            }
            if (to != null)
            {
                result = result.Where(t => t.Deadline <= to);
            }
             
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<TaskGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<TaskGetModel>.EntriesPerPage)
                .Take(PaginatedList<TaskGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(m => TaskGetModel.FromTask(m)).ToList();

            return paginatedResult;
        }

        public Task GetById(int id)
        {
           //sau  context.Tasks.Find()
            return context.Tasks
                .Include(t => t.Comments)
                .FirstOrDefault(t => t.Id == id);
        }

        public Task Upsert(int id, Task task)
        {
            var existing = context.Tasks.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)

            {
                context.Tasks.Add(task);
                context.SaveChanges();
                return task;
            }
                task.Id = id;
            if (task.Status == Task.State.Closed && existing.Status != Task.State.Closed)
                task.ClosedAt = DateTime.Now;
            else if (existing.Status == Task.State.Closed && task.Status != Task.State.Closed)
                task.ClosedAt = null;
            task.Id = id;
            context.Tasks.Update(task);
            context.SaveChanges();
            return task;
        }
    }
}
