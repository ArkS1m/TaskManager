using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITaskRepository : IBaseRepository<TaskModel>
    {
        Task<TaskModel> GetByName(string name);
    }
}
