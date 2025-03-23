using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class UserRepository
    {
        public readonly SpmContext _context;
        #region UserRepository
        public UserRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region UserList
        public async Task<List<User>> UserList()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }
        #endregion

        #region GetUserById
        public async Task<User> GetUserById(int userId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(s => s.UserId == userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        #endregion

        #region AddUser
        public async Task<bool> AddUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _context.Users.Add(user);
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

        #region UpdateUser
        public async Task<bool> UpdateUser(User user)
        {
            user.UpdatedAt = DateTime.Now;
            _context.Users.Update(user);
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

        #region DeleteUser
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
        }
        #endregion

        #region GetUserByEmail
        public async Task<User> GetUserByEmail(string Email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            return user;
        }
        #endregion

        #region GetStudentFromUserId
        public async Task<Student> GetStudentFromUserId(int? userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            return student;
        }
        #endregion

        #region GetFacultyFromUserId
        public async Task<Faculty> GetFacultyFromUserId(int? userId)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(s => s.UserId == userId);
            return faculty;
        }
        #endregion
    }
}
