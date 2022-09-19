using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enum
{
    public enum StatusCode
    {
        TaskNotFound = 0,
        TasksNotFound = 10,
        OK = 200,
        InternalServerError = 500
    }
}
