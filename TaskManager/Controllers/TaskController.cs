using DAL.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Nodes;
using Domain.ViewModels.TaskModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        public ActionResult GetTasks()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var responce = await _taskService.GetTasks();
            if(responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }
            return RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetTask(int id)
        { 
            var responce = await _taskService.GetTask(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var task = new TaskViewModel
                {
                    Id = responce.Data.Id,
                    Name = responce.Data.Name,
                    Description = responce.Data.Description,
                    ExecutorsList = responce.Data.ExecutorsList,
                    RegistrationDate = responce.Data.RegistrationDate,
                    TypeTask = responce.Data.TypeTask.GetType().GetMember(responce.Data.TypeTask.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName()
                };
                if (responce.Data.CompletionDate != null)
                {
                    task.CompletionDate = responce.Data.CompletionDate;
                }
                return Json(task);
            }
            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task Delete(int id)
        {
            await _taskService.DeleteTask(id);
        }
        [HttpGet]
        public async Task<IActionResult> Save(int id)
        {
            if (id == 0)
            {
                return View();
            }
            var responce = await _taskService.GetTask(id);
            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(responce.Data);
            }

            return RedirectToAction("Error");
        }
        [HttpPost]
        public async Task<IActionResult> Save(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id == 0)
                {
                    foreach (var field in typeof(StatusTask).GetFields())
                    {
                        if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                        {
                            if (attribute.Name == model.TypeTask)
                                model.TypeTask = field.Name;
                        }
                    }
                    if (model.ParentTask != "0")
                        model.ParentTask = _taskService.GetTasks().Result.Data.ToList().ElementAt(int.Parse(model.ParentTask) - 1).Id.ToString();
                    model.RegistrationDate = DateTime.Now;
                    await _taskService.CreateTask(model);
                }
                else
                {
                    await _taskService.EditTask(model.Id, model);
                }
            }
            return RedirectToAction("GetTasks");
        }
        [HttpGet]
        public JsonResult GetTaskTypes()
        {
            List<string> TypesToSend = new List<string>();
            var enums = Enum.GetValues(typeof(StatusTask));
            foreach (var elem in enums)
                TypesToSend.Add(elem.GetType().GetMember(elem.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName());

            return Json(TypesToSend);
        }
        [HttpGet]
        public async Task<JsonResult> GetTaskParents()
        {
            List<string> ParentsToSend = new List<string>();
            var tasks = await _taskService.GetTasks();
            if (tasks.Data.Count() != 0)
            {
                foreach (var task in tasks.Data)
                    ParentsToSend.Add(task.Name);
            }

            return Json(ParentsToSend);
        }

        public async Task<JsonResult> GetNodes()
        {
            List<TreeNode> taskNodes = new List<TreeNode>();

            var tasks = await _taskService.GetTasks();
            if (tasks.Data != null)
            {
                foreach (var task in tasks.Data)
                {
                    var taskTemp = new TreeNode
                    {
                        id = task.Id.ToString(),
                        parent = task.ParentTask == null ? "#" : task.ParentTask.Id.ToString(),
                        text = task.Name,
                        icon = "fa fa-folder",

                    };
                    taskNodes.Add(taskTemp);
                }
            }
            return Json(taskNodes);
        }

    }
}
