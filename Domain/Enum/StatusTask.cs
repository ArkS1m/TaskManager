using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Enum
{
    public enum StatusTask
    {
        [Display(Name = "Назначена")]
        Assigned = 0,
        [Display(Name = "Выполняется")]
        InProgress = 1,
        [Display(Name = "Приостановлена")]
        Suspended = 2,
        [Display(Name = "Завершена")]
        Completed = 3
    }
}
