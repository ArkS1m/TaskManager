using Domain.Entities;
using Domain.Responce;
using Domain.ViewModels.TaskModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        public Task<IBaseResponce<IEnumerable<TaskModel>>> GetTasks();
        public Task<IBaseResponce<TaskModel>> GetTask(int id);
        public Task<IBaseResponce<TaskModel>> GetTaskByName(string name);
        public Task<IBaseResponce<TaskViewModel>> CreateTask(TaskViewModel taskViewModel);
        public Task<IBaseResponce<bool>> DeleteTask(int id);
        public Task<IBaseResponce<TaskModel>> EditTask(int id, TaskViewModel model);
    }
}
