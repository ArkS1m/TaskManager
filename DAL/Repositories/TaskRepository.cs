using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;
        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(TaskModel entity)
        {
            await _db.Task.AddAsync(entity);
            await _db.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> Delete(TaskModel entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<TaskModel> Get(int id)
        {
            return await _db.Task.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TaskModel> GetByName(string name)
        {
            return await _db.Task.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<TaskModel>> Select()
        {
            return await _db.Task.ToListAsync();
        }

        public async Task<TaskModel> Update(TaskModel entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
