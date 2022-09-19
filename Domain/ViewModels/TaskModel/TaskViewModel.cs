using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels.TaskModel
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExecutorsList { get; set; }
        public DateTime RegistrationDate { get; set; }
        public TimeSpan? TimeOfExecution { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string TypeTask { get; set; }
        public string ParentTask { get; set; }
    }
}
