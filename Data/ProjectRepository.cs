using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class ProjectRepository
    {
        public readonly SpmContext _context;

        #region ProjectRepository
        public ProjectRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region ProjectList
        public async Task<List<StudentProjectManagementAPI.Models.Project>> ProjectList()
        {
            var ProjectList = await _context.Projects.Include(p => p.Mentor).ToListAsync();
            return ProjectList;
        }
        #endregion

        #region GetProjectById
        public async Task<StudentProjectManagementAPI.Models.Project> GetProjectById(int projectId)
        {
            StudentProjectManagementAPI.Models.Project project = await _context.Projects.Include(s => s.Mentor).FirstOrDefaultAsync(s => s.ProjectId == projectId);
            if (project == null)
            {
                return null;
            }
            return project;
        }
        #endregion

        #region AddProject
        public async Task<bool> AddProject(StudentProjectManagementAPI.Models.Project project)
        {
            _context.Projects.Add(project);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region UpdateProject
        public async Task<bool> UpdateProject(StudentProjectManagementAPI.Models.Project project)
        {
            _context.Projects.Update(project);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region DeleteProject
        public async Task<bool> DeleteProject(int projectId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project == null)
                {
                    return false;
                }
                var evaluations = _context.Evaluations.Where(x => x.ProjectId == projectId);
                _context.Evaluations.RemoveRange(evaluations);
                await _context.SaveChangesAsync();

                var studentEvaluations = _context.StudentEvaluations.Where(x => x.ProjectId == projectId);
                _context.StudentEvaluations.RemoveRange(studentEvaluations);
                await _context.SaveChangesAsync();

                var tasks = _context.Tasks.Where(x => x.ProjectId == projectId);
                _context.Tasks.RemoveRange(tasks);
                await _context.SaveChangesAsync();

                var teamMembers = _context.TeamMembers.Where(x => x.ProjectId == projectId);
                _context.TeamMembers.RemoveRange(teamMembers);
                await _context.SaveChangesAsync(true);

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error while deleting project: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region ProjectDetails
        public async Task<Object> ProjectDetails(int projectId)
        {
            var project = await _context.Projects
                .Select(x => new
                {
                    x.ProjectId,
                    x.Title,
                    x.Description,
                    x.StartDate,
                    x.EndDate,
                    x.Status,
                    progress = (double)_context.Tasks.Count(t => t.ProjectId == projectId && t.Status == "Completed")!=0 ? ((double)_context.Tasks.Count(t => t.ProjectId == projectId && t.Status == "Completed")/ (double)_context.Tasks.Count(t => t.ProjectId == projectId))*100 : 0,
                    TeamMembers = _context.TeamMembers
                        .Where(tm => tm.ProjectId == projectId)
                        .Include(tm => tm.Student.User)
                        .Select(tm => new
                        {
                            tm.TeamMemberId,
                            tm.Student.UserId,
                            tm.Student.StudentId,
                            tm.Student.EnrollmentNo,
                            Name = tm.Student.User.FirstName + " " + tm.Student.User.LastName,
                            tm.Student.User.Email,
                        })
                        .ToList(),


                    ProjectEvalutions = _context.Evaluations
                        .Where(e => e.ProjectId == projectId)
                        .Include(e => e.Faculty.User)
                        .Select(e => new
                        {
                            e.EvaluationId,
                            EvaluatedBy = e.Faculty.User.FirstName + " " + e.Faculty.User.LastName,
                            e.Feedback,
                            e.Score,
                        })
                        .ToList(),


                    TaskDoneByMembers = _context.Tasks
                        .Where(t => t.ProjectId == projectId)
                        .Include(t => t.AssignedToNavigation.User)
                        .Include(t => t.CreatedByNavigation)
                        .Include(t => t.CreatedByNavigation)
                        .ToList(),

                    TeamMembersEvalutions = _context.StudentEvaluations
                        .Where(te => te.ProjectId == projectId)
                        .Include(te => te.Faculty.User)
                        .GroupBy(te => te.StudentId)
                        .Select(te => new
                        {
                            te.FirstOrDefault().Student.StudentId,
                            te.FirstOrDefault().Student.EnrollmentNo,
                            MemberName = te.FirstOrDefault().Student.User.FirstName + " " + te.FirstOrDefault().Student.User.LastName,
                            Evaluation = te
                                .Select(te1 => new
                                {
                                    te1.StudentEvaluationsId,
                                    evaluatedBy = te1.Faculty.User.FirstName + " " + te1.Faculty.User.LastName,
                                    te1.Feedback,
                                    te1.Score
                                })
                                .ToList()
                        })
                        .ToList(),


                    Mentor = _context.Faculties.
                        Include(f => f.User)
                        .FirstOrDefault(f => f.FacultyId == x.MentorId)


                })
                .FirstOrDefaultAsync(x => x.ProjectId == projectId);
            return new { project };
        }
        #endregion

        #region ProjectMembers
        public async Task<List<TeamMember>> ProjectMembers(int projectId)
        {
            var memberList = await _context.TeamMembers.Where(x => x.ProjectId == projectId).Include(x => x.Student).Include(x => x.Student.User).ToListAsync();
            return memberList;
        }
        #endregion

        #region StudentProjectList
        public async Task<List<StudentProjectManagementAPI.Models.Project>> StudentProjectList(int userId)
        {
            var ProjectList = await _context.Projects
                .Include(p => p.Mentor)
                .Where(p => p.TeamMembers.Any(tm => tm.Student.User.UserId == userId))
                .ToListAsync();
            return ProjectList;
        }
        #endregion

    }
}
