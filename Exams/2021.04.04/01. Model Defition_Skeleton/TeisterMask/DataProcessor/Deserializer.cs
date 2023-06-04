// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Text;
    using TeisterMask.Utilities;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System;
    using Microsoft.VisualBasic;
    using System.Data.SqlTypes;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();
            ImportProjectDto[] projectsDtos = xmlHelper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

            ICollection<Project> validProjects = new HashSet<Project>();

            foreach (ImportProjectDto dto in projectsDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectOpenDate;
                bool isValidProjectOpenDate =
                    DateTime.TryParseExact(dto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);
                if (!isValidProjectOpenDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDueDate = null;
                if (!string.IsNullOrWhiteSpace(dto.DueDate))
                {
                    DateTime projectDueDateTmp;
                    bool isValidProjectDueData =
                        DateTime.TryParseExact(dto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectDueDateTmp);
                    if (!isValidProjectDueData)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    projectDueDate = projectDueDateTmp;
                }


                Project project = new Project()
                {
                    Name = dto.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate,
                };

                foreach (var taskDto in dto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;
                    bool isValidTaskOpenData =
                        DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);
                    if (!isValidTaskOpenData)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    DateTime taskDueDate;
                    bool isValidTaskDueData =
                        DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    if (!isValidTaskDueData)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (projectDueDate.HasValue && taskDueDate > projectDueDate.Value)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (taskOpenDate > taskDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType,
                    };


                    project.Tasks.Add(task);

                }
                validProjects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }
            context.Projects.AddRangeAsync(validProjects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportEmployeeDto[] employeesDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);
            ICollection<Employee> validEmployee = new HashSet<Employee>();
            int[] existingTasks = context.Tasks.Select(t => t.Id).ToArray();

            foreach (ImportEmployeeDto dto in employeesDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee()
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Phone = dto.Phone,
                };

                foreach (var taskId in dto.Tasks.Distinct())
                {
                    if (!existingTasks.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask()
                    {
                        Employee = employee,
                        TaskId = taskId,
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                validEmployee.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRangeAsync(validEmployee);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}