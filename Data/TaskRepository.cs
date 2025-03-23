using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class TaskRepository
    {
        public readonly SpmContext _context;
        #region TaskRepository
        public TaskRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region TaskList
        public async Task<List<StudentProjectManagementAPI.Models.Task>> TaskList()
        {
            var taskList = await _context.Tasks
                                    .Include(s => s.Project)
                                    .Include(s => s.CreatedByNavigation)
                                    .Include(s => s.AssignedToNavigation)
                                    .Include(s => s.AssignedToNavigation.User)
                                    .ToListAsync();
            return taskList;
        }
        #endregion

        #region GetTaskById
        public async Task<StudentProjectManagementAPI.Models.Task> GetTaskById(int taskId)
        {
            StudentProjectManagementAPI.Models.Task task = await _context.Tasks
                                    .Include(s => s.Project)
                                    .Include(s => s.CreatedByNavigation)
                                    .Include(s => s.AssignedToNavigation)
                                    .FirstOrDefaultAsync(s => s.TaskId == taskId);
            if (task == null)
            {
                return null;
            }
            return task;
        }
        #endregion

        #region AddTask
        public async Task<bool> AddTask(StudentProjectManagementAPI.Models.Task task)
        {
            Console.WriteLine(":::::::::::::::::::::: call");
            _context.Tasks.Add(task);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(":::::::::::");
                Console.WriteLine(ex.Message);
                Console.WriteLine(":::::::::::");
                return false;   
            }
        }

        #endregion

        #region UpdateTask
        public async Task<bool> UpdateTask(StudentProjectManagementAPI.Models.Task task)
        {
            _context.Tasks.Update(task);
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

        #region DeleteTask
        public async Task<bool> DeleteTask(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return false;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}