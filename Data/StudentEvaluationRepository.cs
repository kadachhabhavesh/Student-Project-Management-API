using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class StudentEvaluationRepository
    {
        public readonly SpmContext _context;
        #region StudentEvaluationRepository
        public StudentEvaluationRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region StudentEvaluationList
        public async Task<List<StudentEvaluation>> StudentEvaluationList()
        {
            var studentEvaluationList = await _context.StudentEvaluations
                                            .Include(s => s.Student)
                                            .Include(s => s.Faculty)
                                            .Include(s => s.Faculty.User)
                                            .Include(s => s.Student)
                                            .Include(s => s.Student.User)
                                            .Include(s => s.Project)
                                            .ToListAsync();
            return studentEvaluationList;
        }
        #endregion

        #region GetStudentEvaluationById
        public async Task<StudentEvaluation> GetStudentEvaluationById(int studentEvaluationId)
        {
            StudentEvaluation studentEvaluation = await _context.StudentEvaluations
                                            .Include(s => s.Student)
                                            .Include(s => s.Student.User)
                                            .Include(s => s.Faculty)
                                            .Include(s => s.Project).FirstOrDefaultAsync(s => s.StudentEvaluationsId == studentEvaluationId);
            if (studentEvaluation == null)
            {
                return null;
            }
            return studentEvaluation;
        }
        #endregion

        #region AddStudentEvaluation
        public async Task<bool> AddStudentEvaluation(StudentEvaluation studentEvaluation)
        {
            _context.StudentEvaluations.Add(studentEvaluation);
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

        #region UpdateStudentEvaluation
        public async Task<bool> UpdateStudentEvaluation(StudentEvaluation studentEvaluation)
        {
            _context.StudentEvaluations.Update(studentEvaluation);
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

        #region DeleteStudentEvaluation
        public async Task<bool> DeleteStudentEvaluation(int studentEvaluationId)
        {
            var studentEvaluation = await _context.StudentEvaluations.FindAsync(studentEvaluationId);
            if (studentEvaluation == null)
            {
                return false;
            }
            _context.StudentEvaluations.Remove(studentEvaluation);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
