using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Responce;
using Domain.ViewModels.TaskModel;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }


        public async Task<IBaseResponce<TaskModel>> GetTask(int id)
        {
            var baseResponce = new BaseResponce<TaskModel>();
            try
            {
                var task = await _taskRepository.Get(id);
                if (task == null)
                {
                    baseResponce.Description = "Task not found";
                    baseResponce.StatusCode = StatusCode.TaskNotFound;
                    return baseResponce;
                }
                baseResponce.StatusCode = StatusCode.OK;
                baseResponce.Data = task;
                return baseResponce;
            }
            catch (Exception ex)
            {
                return new BaseResponce<TaskModel>()
                {
                    Description = $"[GetTask]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponce<TaskModel>> GetTaskByName(string name)
        {
            var baseResponce = new BaseResponce<TaskModel>();
            try
            {
                var task = await _taskRepository.GetByName(name);
                if (task == null)
                {
                    baseResponce.Description = "Task not found";
                    baseResponce.StatusCode = StatusCode.TaskNotFound;
                    return baseResponce;
                }

                baseResponce.Data = task;
                return baseResponce;
            }
            catch (Exception ex)
            {
                return new BaseResponce<TaskModel>()
                {
                    Description = $"[GetTaskByName]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponce<IEnumerable<TaskModel>>> GetTasks()
        {
            var baseResponce = new BaseResponce<IEnumerable<TaskModel>>();
            try
            {
                var tasks = await _taskRepository.Select();
                if(tasks.Count == 0)
                {
                    baseResponce.Description = "No elements found";
                    baseResponce.StatusCode = StatusCode.TasksNotFound;
                    return baseResponce;
                }
                baseResponce.Data = tasks;
                baseResponce.StatusCode = StatusCode.OK;

                return baseResponce;
            }
            catch(Exception ex)
            {
                return new BaseResponce<IEnumerable<TaskModel>>()
                {
                    Description = $"[GetTasks]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponce<TaskViewModel>> CreateTask(TaskViewModel taskViewModel)
        {
            var baseResponce = new BaseResponce<TaskViewModel>();
            try
            {
                var task = new TaskModel()
                {
                    Name = taskViewModel.Name,
                    Description = taskViewModel.Description,
                    RegistrationDate = taskViewModel.RegistrationDate,
                    TypeTask = (StatusTask)Enum.Parse(typeof(StatusTask), taskViewModel.TypeTask)
                };
                if(taskViewModel.ParentTask != "0"){
                    task.ParentTask = _taskRepository.Get(int.Parse(taskViewModel.ParentTask)).Result;
                }

                await _taskRepository.Create(task);
                
            }
            catch (Exception ex)
            {
                return new BaseResponce<TaskViewModel>()
                {
                    Description = $"[CreateTask]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponce;
        }
        public async Task<IBaseResponce<bool>> DeleteTask(int id)
        {
            var baseResponce = new BaseResponce<bool>();
            try
            {
                var task = await _taskRepository.Get(id);
                if (task == null)
                {
                    baseResponce.Description = "Task not found";
                    baseResponce.StatusCode = StatusCode.TaskNotFound;
                    return baseResponce;
                }

                baseResponce.Data = true;
                baseResponce.StatusCode = StatusCode.OK;
                await _taskRepository.Delete(task);

                return baseResponce;
            }
            catch (Exception ex)
            {
                return new BaseResponce<bool>()
                {
                    Description = $"[DeleteTask]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponce<TaskModel>> EditTask(int id, TaskViewModel model)
        {
            var baseResponce = new BaseResponce<TaskModel>();
            try
            {
                var task = await _taskRepository.Get(id);

                if(task == null)
                {
                    baseResponce.StatusCode = StatusCode.TaskNotFound;
                    baseResponce.Description = "Task not found";
                    return baseResponce;
                }
                foreach (var field in typeof(StatusTask).GetFields())
                {
                    if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                    {
                        if (attribute.Name == model.TypeTask)
                            model.TypeTask = field.Name;
                    }
                }

                task.Name = model.Name;
                task.Description = model.Description;
                task.ExecutorsList = model.ExecutorsList;
                task.TypeTask = (StatusTask)Enum.Parse(typeof(StatusTask), model.TypeTask);
                if (task.TypeTask == StatusTask.Completed)
                {
                    task.CompletionDate = DateTime.Now;
                    task.TimeOfExecution = task.CompletionDate - task.RegistrationDate;
                }
                baseResponce.StatusCode = StatusCode.OK;
                await _taskRepository.Update(task);

                return baseResponce;
            }
            catch(Exception ex)
            {
                return new BaseResponce<TaskModel>()
                {
                    Description = $"[DeleteTask]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
