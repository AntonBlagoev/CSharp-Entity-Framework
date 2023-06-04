namespace TeisterMask.DataProcessor
{
    using Data;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Linq;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.Utilities;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var result = context.Projects
                .Where(p => p.Tasks.Any(x => x.DueDate != null))
                .ToArray()
                .Select(p => new ExportProjectDto
                {
                    TasksCount = p.Tasks.Count,
                    Name = p.Name,
                    HasEndDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
                        .Select(t => new ExportTaskDto 
                        { 
                            Name = t.Name,
                            LabelType = t.LabelType.ToString(),
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                })
                .OrderByDescending(t => t.Tasks.Length)
                .ThenBy(t => t.Name)
                .ToArray();

            return xmlHelper.Serialize(result, "Projects");

        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var result = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .ToArray()
                        .OrderByDescending(et => et.Task.DueDate)
                        .ThenBy(et => et.Task.Name)
                        .Select(t => new
                        {
                            TaskName = t.Task.Name,
                            OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = t.Task.LabelType.ToString(),
                            ExecutionType = t.Task.ExecutionType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();


            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}