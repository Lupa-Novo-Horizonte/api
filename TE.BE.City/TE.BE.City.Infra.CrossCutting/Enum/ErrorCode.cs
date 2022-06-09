using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TE.BE.City.Infra.CrossCutting.Enum
{
    public enum ErrorCode
    {
        [Description("User/password invalid.")]
        UserNotIdentified = 1000,
        [Description("No data was found.")]
        SearchHasNoResult = 1001,
        [Description("Error to create a issue.")]
        CreateIssueFail = 1002,
        [Description("Error to insert contact.")]
        InsertContactFail = 1003,
    }
}
