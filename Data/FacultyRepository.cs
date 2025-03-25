using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class FacultyRepository
    {
        public readonly SpmContext _context;
        #region FacultyRepository
        public FacultyRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region FacultyList
        public async Task<List<Faculty>> FacultyList()
        {
            var facultyList = await _context.Faculties.Include(s => s.User).ToListAsync();
            return facultyList;
        }
        #endregion

        #region GetFacultyById
        public async Task<Faculty> GetFacultyById(int facultyId)
        {
            Faculty faculty = await _context.Faculties.Include(f => f.User).FirstOrDefaultAsync(s => s.FacultyId == facultyId);
            if (faculty == null)
            {
                return null;
            }
            return faculty;
        }
        #endregion

        #region AddFaculty
        public async Task<bool> AddFaculty(Faculty faculty)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (faculty.User != null)
                {
                    faculty.User.CreatedAt = DateTime.Now;
                    faculty.User.UpdatedAt = DateTime.Now;
                    _context.Users.Add(faculty.User);
                    await _context.SaveChangesAsync();
                }
                faculty.UserId = faculty.User.UserId;
                _context.Faculties.Add(faculty);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        #endregion

        #region UpdateFaculty
        public async Task<bool> UpdateFaculty(Faculty faculty)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (faculty.User != null) {
                    faculty.User.UpdatedAt = DateTime.Now;
                    _context.Users.Update(faculty.User);
                    await _context.SaveChangesAsync();
                }
                _context.Faculties.Update(faculty);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch {
                await transaction.RollbackAsync();    
                return false; 
            }
        }
        #endregion

        #region Deletefaculty
        public async Task<bool> Deletefaculty(int facultyId)
        {
            var faculty = await _context.Faculties.FindAsync(facultyId);
            if (faculty == null)
            {
                return false;
            }
            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region MentorDropDown
        public dynamic MentorDropDown()
        {
            var dropdown = _context.Faculties.Include(f => f.User).Select(f => new { userId = f.User.UserId ,mentorId=f.FacultyId, name=f.User.FirstName+" "+ f.User.LastName }).ToList();
            return dropdown;
        }
        #endregion

        #region FacultyProfile
        public async Task<Object> FacultyProfile(int facultyId)
        {
            var faculty = await _context.Faculties.Include(f => f.User).FirstOrDefaultAsync(f => f.FacultyId == facultyId);


            if (await _context.TeamMembers
                .Include(tm => tm.Project)
                .Include(tm => tm.Project.Mentor)
                .Where(tm => tm.Project.Mentor.FacultyId == facultyId)
                .Select(tm => tm.ProjectId)
                .Distinct()
                .CountAsync() > 0)
            {
                var project = await _context.TeamMembers
                .Include(tm => tm.Project)
                .Where(tm => tm.Project.MentorId == facultyId)
                .GroupBy(tm => tm.Project.ProjectId)
                .Select(t => new
                {
                    ProjectId = t.FirstOrDefault().Project.ProjectId,
                    Title = t.FirstOrDefault().Project.Title,
                    Description = t.FirstOrDefault().Project.Description,
                    StartDate = t.FirstOrDefault().Project.StartDate,
                    EndDate = t.FirstOrDefault().Project.EndDate,
                    Status = t.FirstOrDefault().Project.Status,
                    Mentor = t.FirstOrDefault().Project.Mentor.User.FirstName+" "+t.FirstOrDefault().Project.Mentor.User.LastName,
                    Members = _context.TeamMembers
                        .Where(tm => tm.Project.ProjectId == t.FirstOrDefault().Project.ProjectId)
                        .Select(s => new {
                            StudentId = s.Student.StudentId,
                            Name = s.Student.User.FirstName + " " + s.Student.User.LastName,
                            EnrollmentNo = s.Student.EnrollmentNo,
                            Email = s.Student.User.Email
                        })
                        .ToList(),
                    ProjectEvaluations = _context.Evaluations
                        .Include(tm => tm.Project.Mentor.User)
                        .Where(e => e.ProjectId == t.FirstOrDefault().Project.ProjectId)
                        .Select(e => new {
                            e.ProjectId,
                            Mentor = e.Project.Mentor.User.FirstName+" "+e.Project.Mentor.User.LastName,
                            e.Feedback,
                            e.Score
                        })
                        .ToList(),
                    MembersEvaluations = _context.TeamMembers
                        .Where(tm => tm.Project.ProjectId == t.FirstOrDefault().Project.ProjectId)
                        .Select(tm => new {
                            StudentId = tm.Student.StudentId,
                            Name = tm.Student.User.FirstName + " " + tm.Student.User.LastName,
                            EnrollmentNo = tm.Student.EnrollmentNo,
                            Evaluations = _context.StudentEvaluations
                                .Where(se => se.ProjectId == t.FirstOrDefault().Project.ProjectId && se.StudentId == tm.Student.StudentId)
                                .Select(se => new {
                                    EvaluatedBy = _context.Faculties
                                        .Where(f => f.FacultyId == se.FacultyId)
                                        .Select(f => f.User.FirstName + " " + f.User.LastName)
                                        .FirstOrDefault(),
                                    se.Feedback,
                                    se.Score
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();
                return new
                {
                    faculty,
                    project
                };
            }


            return new { 
                faculty
            };
        }
        #endregion
    } 
}
