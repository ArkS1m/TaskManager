using Domain.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.TagHelpers
{
    public class TasksListTagHelper : TagHelper
    {
        public TaskModel task { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            string listContent = "";
            foreach (TaskModel item in task.SubTasks)
            {
                listContent += "<li>" + item.Name + "</li>";
                if (task.SubTasks.Count != 0)
                    Process(context, output);
            }

            output.Content.SetHtmlContent(listContent);
        }
    }
}
