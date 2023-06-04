namespace SoftUni
{
    using Data;
    using SoftUni.Models;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbcontext = new SoftUniContext();

            string result = RemoveTown(dbcontext);
            Console.WriteLine(result);


        }

        //03. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //04. Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .Where(e => e.Salary > 50000)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //05. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.Salary)
                .ThenBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .Where(e => e.Name == "Research and Development")
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //06. Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(newAddress);

            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employee!.Address = newAddress;

            context.SaveChanges();

            string[] employeeAddresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();
            return String.Join(Environment.NewLine, employeeAddresses);
        }

        //07. Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithProjects = context.Employees
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager!.FirstName,
                ManagerLastName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                .Select(ep => new
                {
                    ProjectName = ep.Project.Name,
                    StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndDate = ep.Project.EndDate.HasValue
                        ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished"
                })
                .ToArray()
            })
            .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //08. Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town!.Name,
                    EmployeeCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a?.AddressText}, {a?.TownName} - {a?.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }


        //09. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            p.Project.Name
                        })
                        .OrderBy(p => p.Name)
                        .ToArray()
                })
                .FirstOrDefault();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            //sb.AppendLine(string.Join(Environment.NewLine, employee.Projects.Select(p => p.Name)));

            foreach (var p in employee.Projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }

        // 10. Departments with More Than 5 Employees

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsData = context.Departments
                .Where(e => e.Employees.Count > 5)
                .OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    EmployeesCount = d.Employees.Count,
                    ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                    ManagersData = d.Employees
                    .OrderBy(m => m.FirstName)
                    .ThenBy(m => m.LastName)
                    .Select(m => new
                    {
                        ManagerNameAndJob = m.FirstName + " " + m.LastName + " - " + m.JobTitle
                    })
                    .ToArray()
                })
                .ToArray();

            foreach (var d in departmentsData)
            {
                sb.AppendLine(d.DepartmentName);

                foreach (var m in d.ManagersData)
                {
                    sb.AppendLine(m.ManagerNameAndJob);
                }
            }



            return sb.ToString().TrimEnd();
        }


        // 11. Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projectsInfo = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                })
                .ToArray();

            foreach (var p in projectsInfo)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate);
            }

            return sb.ToString().TrimEnd();
        }

        // 12. Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            string[] departments = new string[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employeeIncreaseSalaries = context.Employees
                .Where(d => departments.Contains(d.Department.Name))
                .ToArray();

            foreach (var e in employeeIncreaseSalaries)
            {
                e.Salary *= 1.12m;
            }

            context.SaveChanges();

            var employeeInfo = context.Employees
				.Where(d => departments.Contains(d.Department.Name))
				.OrderBy(e => e.FirstName)
				.ThenBy(e => e.LastName)
				.Select(e => $"{e.FirstName} {e.LastName} (${e.Salary:F2})")
                .ToArray();

            return string.Join(Environment.NewLine, employeeInfo);
        }

        // 13. Find Employees by First Name Starting With Sa
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employeeInfo = context.Employees
                .Where(e => e.FirstName.Substring(0, 2) == "Sa")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})")
                .ToArray();

            return string.Join(Environment.NewLine, employeeInfo);
        }

        // 14. Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            // Delete all rows from EmployeeProject that refer to Project with Id = 2
            IQueryable<EmployeeProject> epToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);
           
            context.EmployeesProjects.RemoveRange(epToDelete);

            Project projectToDelete = context.Projects.Find(2)!;
            context.Projects.Remove(projectToDelete);
            
            context.SaveChanges();

            string[] projectNames = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();
           
            return String.Join(Environment.NewLine, projectNames);
        }

        // 15. Remove Town

        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .Where(t => t.Name == "Seattle")
                .FirstOrDefault();

            Address[] addressesToDelete = context.Addresses
                .Where(a => a.Town.TownId == townToDelete.TownId)
                .ToArray();

            var employeeAddressToUpdate = context.Employees
                .Where(e => addressesToDelete.Contains(e.Address))
                .ToArray();

            foreach (var e in employeeAddressToUpdate)
            {
                e.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDelete);

            context.Towns.Remove(townToDelete);

            context.SaveChanges();

            return $"{addressesToDelete.Count()} addresses in Seattle were deleted";
        }


    }
}