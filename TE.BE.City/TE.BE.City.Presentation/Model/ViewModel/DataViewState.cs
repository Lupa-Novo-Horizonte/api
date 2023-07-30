using EnumsNET;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Presentation.Model.ViewModel
{
    public class DataViewState
    {
        public DataViewState()
        {
            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
            DdlViewType = "map";
            DdlIssueType = "all";
            DdlIsProblem = "all";

            DdlViewTypeItems = new List<SelectListItem>
            {
                new SelectListItem(){Value = "map", Text = "Mapa"},
                new SelectListItem(){Value = "table", Text = "Tabela"},
                new SelectListItem(){Value = "chart", Text = "Gráfico"}
            };

            DdlIssueTypeItems = new List<SelectListItem>
            {
                new SelectListItem(){Value = TypeIssue.All.ToString(), Text = (TypeIssue.All).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Asphalt.ToString(), Text = (TypeIssue.Asphalt).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Collect.ToString(), Text = (TypeIssue.Collect).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Light.ToString(), Text = (TypeIssue.Light).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Sewer.ToString(), Text = (TypeIssue.Sewer).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Trash.ToString(), Text = (TypeIssue.Trash).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = TypeIssue.Water.ToString(), Text = (TypeIssue.Water).AsString(EnumFormat.Description)}
            };
            
            DdlIsProblemItems = new List<SelectListItem>
            {
                new SelectListItem(){Value = IsProblem.All.ToString(), Text = (TypeIssue.All).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = IsProblem.Problem.ToString(), Text = (IsProblem.Problem).AsString(EnumFormat.Description)},
                new SelectListItem(){Value = IsProblem.NoProblem.ToString(), Text = (IsProblem.NoProblem).AsString(EnumFormat.Description)}
            };
        }

        public string DdlViewType { get; set; }
        public List<SelectListItem> DdlViewTypeItems { get; set; }

        public string DdlIssueType { get; set; }
        public IEnumerable<SelectListItem> DdlIssueTypeItems { get; set; }

        public string DdlIsProblem { get; set; }
        public List<SelectListItem> DdlIsProblemItems { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDate { get; set; }
    }
}
