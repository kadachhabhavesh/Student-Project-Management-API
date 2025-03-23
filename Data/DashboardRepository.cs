using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class DashboardRepository
    {
        private readonly SpmContext _context;
        
        #region DashboardRepository
        public DashboardRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region Dashboard
        public async Task<object> Dashboard() 
        {
            int CompeleteProjects = await _context.Projects.CountAsync(p => p.Status == "Completed");
            int PendingProjects = await _context.Projects.CountAsync(p => p.Status == "Pending");
            int ActiveProjects = await _context.Projects.CountAsync(p => p.Status == "Active");
            int Students = await _context.Students.CountAsync();
            int Faculties = await _context.Faculties.CountAsync();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var projectsDueToday = await _context.Projects.Include(p => p.Mentor).Include(p => p.Mentor.User).Where(p => p.EndDate == today).ToListAsync();

            var tasksDueToday = await _context.Tasks.Include(t => t.AssignedToNavigation).Include(t => t.AssignedToNavigation.User).Include(t => t.Project).Where(t => t.Deadline == today).ToListAsync();
            
            var topStudents =  _context.Students
                .Select(student => new
                {
                    StudentId = student.StudentId,
                    EnrollmentNo = student.EnrollmentNo,
                    Name = student.User.FirstName + " " + student.User.LastName,
                    Email = student.User.Email,
                    Department = student.User.Department,
                    ProjectCount = student.TeamMembers.Count(),
                    Task = student.Tasks.Count(),
                })
                .OrderByDescending(student => student.ProjectCount) 
                .ThenBy(student => student.Task)
                .Take(10)
                .ToList();

                var topProjects = _context.TeamMembers
                    .Include(tm => tm.Project)
                    .Include(p => p.Project.Mentor)
                    .Include(p => p.Project.Mentor.User)
                    .Include(tm => tm.Student)
                    .Include(tm => tm.Student.User)
                .GroupBy(tm => tm.Project.ProjectId)
                .Select(tm => new
                {
                    ProjectTitle = tm.FirstOrDefault().Project.Title,
                    Description = tm.FirstOrDefault().Project.Description,
                    StartDate = tm.FirstOrDefault().Project.StartDate,
                    EndDate = tm.FirstOrDefault().Project.EndDate,
                    Status = tm.FirstOrDefault().Project.Status,
                    Mentor = tm.FirstOrDefault().Project.Mentor.User.FirstName + " " + tm.FirstOrDefault().Project.Mentor.User.LastName,
                    AverageScore = tm.FirstOrDefault().Project.Evaluations.Any() ? tm.FirstOrDefault().Project.Evaluations.Average(e => e.Score) : 0
                })
                .OrderByDescending(p => p.AverageScore)
                .Take(10)
                .ToList();

            return new { 
                Students,
                Faculties,
                projectsDueToday,
                tasksDueToday,
                Projects = new {
                    CompeleteProjects,
                    PendingProjects,
                    ActiveProjects
                },
                topStudents,
                topProjects
            };
        }
        #endregion

        #region FacultyDashboard
        public async Task<object> FacultyDashboard(int userId)
        {
            int CompeleteProjects = await _context.Projects.Where(p => p.Mentor.User.UserId == userId).CountAsync(p => p.Status == "Completed");
            int PendingProjects = await _context.Projects.Where(p => p.Mentor.User.UserId == userId).CountAsync(p => p.Status == "Pending");
            int ActiveProjects = await _context.Projects.Where(p => p.Mentor.User.UserId == userId).CountAsync(p => p.Status == "Active");
            int Students = await _context.Students.Where(s => s.TeamMembers.FirstOrDefault().Project.Mentor.User.UserId == userId).CountAsync();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var projectsDueToday = await _context.Projects.Include(p => p.Mentor).Include(p => p.Mentor.User).Where(p => p.Mentor.User.UserId == userId).Where(p => p.EndDate == today).ToListAsync();

            var tasksDueToday = await _context.Tasks.Include(t => t.AssignedToNavigation).Include(t => t.AssignedToNavigation.User).Include(t => t.Project).Where(t => t.Project.Mentor.User.UserId == userId).Where(t => t.Deadline == today).ToListAsync();

            var topStudents = _context.Students
                .Where(student => student.TeamMembers.FirstOrDefault().Project.Mentor.User.UserId == userId)
                .Select(student => new
                {
                    StudentId = student.StudentId,
                    EnrollmentNo = student.EnrollmentNo,
                    Name = student.User.FirstName + " " + student.User.LastName,
                    Email = student.User.Email,
                    Department = student.User.Department,
                    ProjectCount = student.TeamMembers.Count(),
                    Task = student.Tasks.Count(),
                })
                .OrderByDescending(student => student.ProjectCount)
                .ThenBy(student => student.Task)
                .Take(10)
                .ToList();

            var topProjects = _context.TeamMembers
                .Include(tm => tm.Project)
                .Include(p => p.Project.Mentor)
                .Include(p => p.Project.Mentor.User)
                .Include(tm => tm.Student)
                .Include(tm => tm.Student.User)
                .Where(p => p.Project.Mentor.User.UserId == userId)
            .GroupBy(tm => tm.Project.ProjectId)
            .Select(tm => new
            {
                ProjectTitle = tm.FirstOrDefault().Project.Title,
                Description = tm.FirstOrDefault().Project.Description,
                StartDate = tm.FirstOrDefault().Project.StartDate,
                EndDate = tm.FirstOrDefault().Project.EndDate,
                Status = tm.FirstOrDefault().Project.Status,
                Mentor = tm.FirstOrDefault().Project.Mentor.User.FirstName + " " + tm.FirstOrDefault().Project.Mentor.User.LastName,
                AverageScore = tm.FirstOrDefault().Project.Evaluations.Any() ? tm.FirstOrDefault().Project.Evaluations.Average(e => e.Score) : 0
            })
            .OrderByDescending(p => p.AverageScore)
            .Take(10)
            .ToList();

            return new
            {
                Students,
                projectsDueToday,
                tasksDueToday,
                Projects = new
                {
                    CompeleteProjects,
                    PendingProjects,
                    ActiveProjects
                },
                topStudents,
                topProjects
            };
        }
        #endregion

        #region StudentDashboard
        public async Task<object> StudentDashboard(int userId)
        {
            int CompeleteProjects = await _context.Projects.Where(x => x.TeamMembers.Any(tm => tm.Student.User.UserId == userId)).CountAsync(p => p.Status == "Completed");
            int PendingProjects = await _context.Projects.Where(x => x.TeamMembers.Any(tm => tm.Student.User.UserId == userId)).CountAsync(p => p.Status == "Pending");
            int ActiveProjects = await _context.Projects.Where(x => x.TeamMembers.Any(tm => tm.Student.User.UserId == userId)).CountAsync(p => p.Status == "Active");
            int CompeleteTask = await _context.Tasks.Where(t => t.AssignedToNavigation.User.UserId == userId).CountAsync(t => t.Status == "Completed");
            int PendingTask = await _context.Tasks.Where(t => t.AssignedToNavigation.User.UserId == userId).CountAsync(t => t.Status == "Pending"); ;
            int InProgressTask = await _context.Tasks.Where(t => t.AssignedToNavigation.User.UserId == userId).CountAsync(t => t.Status == "In Progress"); ;
            int Students = await _context.Students.CountAsync();
            int Faculties = await _context.Faculties.CountAsync();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var projectsDueToday = await _context.Projects.Include(p => p.Mentor).Include(p => p.Mentor.User).Where(p => p.TeamMembers.Any(tm => tm.Student.User.UserId == userId)).Where(p => p.EndDate == today).ToListAsync();

            var tasksDueToday = await _context.Tasks.Include(t => t.AssignedToNavigation).Include(t => t.AssignedToNavigation.User).Include(t => t.Project).Where(t => t.AssignedToNavigation.User.UserId == userId).Where(t => t.Deadline == today).ToListAsync();

            var topProjects = _context.TeamMembers
                .Include(tm => tm.Project)
                .Include(p => p.Project.Mentor)
                .Include(p => p.Project.Mentor.User)
                .Include(tm => tm.Student)
                .Include(tm => tm.Student.User)
                .Where(tm => tm.Student.User.UserId == userId)
            .GroupBy(tm => tm.Project.ProjectId)
            .Select(tm => new
            {
                ProjectTitle = tm.FirstOrDefault().Project.Title,
                Description = tm.FirstOrDefault().Project.Description,
                StartDate = tm.FirstOrDefault().Project.StartDate,
                EndDate = tm.FirstOrDefault().Project.EndDate,
                Status = tm.FirstOrDefault().Project.Status,
                Mentor = tm.FirstOrDefault().Project.Mentor.User.FirstName + " " + tm.FirstOrDefault().Project.Mentor.User.LastName,
                AverageScore = tm.FirstOrDefault().Project.Evaluations.Any() ? tm.FirstOrDefault().Project.Evaluations.Average(e => e.Score) : 0
            })
            .OrderByDescending(p => p.AverageScore)
            .Take(10)
            .ToList();

            return new
            {
                Students,
                Faculties,
                projectsDueToday,
                tasksDueToday,
                Projects = new
                {
                    CompeleteProjects,
                    PendingProjects,
                    ActiveProjects
                },
                Tasks = new {
                    CompeleteTask,
                    PendingTask,
                    InProgressTask
                },
                topProjects
            };
        }
        #endregion
    }
}   