using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class EvaluationRepository
    {
        public readonly SpmContext _context;
        #region EvaluationRepository
        public EvaluationRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region EvaluationList
        public async Task<List<Evaluation>> EvaluationsList()
        {
            var evaluationList = await _context.Evaluations.Include(s => s.Project).Include(s => s.Faculty).Include(s => s.Faculty.User).ToListAsync();
            return evaluationList;
        }
        #endregion

        #region GetEvaluationById
        public async Task<Evaluation> GetEvaluationById(int evaluationId)
        {
            Evaluation evaluation = await _context.Evaluations.Include(s => s.Project).Include(s=>s.Faculty).Include(s=>s.Faculty.User).FirstOrDefaultAsync(s => s.EvaluationId == evaluationId);
            if (evaluation == null)
            {
                return null;
            }
            return evaluation;
        }
        #endregion

        #region AddEvaluation
        public async Task<bool> AddEvaluation(Evaluation evaluation)
        {
            evaluation.EvaluatedAt = DateOnly.FromDateTime(DateTime.Now);
            _context.Evaluations.Add(evaluation);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(":::::::::::");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(":::::::::::");
                return false;
            }
        }

        #endregion

        #region UpdateEvaluation
        public async Task<bool> UpdateEvaluation(Evaluation evaluation)
        {
            _context.Evaluations.Update(evaluation);
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

        #region DeleteEvaluation
        public async Task<bool> DeleteEvaluation(int evaluationId)
        {
            var evaluation = await _context.Evaluations.FindAsync(evaluationId);
            if (evaluation == null)
            {
                return false;
            }
            _context.Evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
