using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExecutorsList { get; set; }
        public DateTime RegistrationDate { get; set; }
        public StatusTask TypeTask { get; set; }
        public TimeSpan? TimeOfExecution { get; set; }
        public DateTime? CompletionDate { get; set; }
        public TaskModel? ParentTask { get; set; }
        public ICollection<TaskModel> SubTasks { get; set; }
        public TaskModel()
        {
            SubTasks = new List<TaskModel>();
        }
    }
}
