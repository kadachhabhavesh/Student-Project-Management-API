using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class FileRepository
    {
        public readonly SpmContext _context;
        #region FileRepository
        public FileRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region FileList
        public async Task<List<StudentProjectManagementAPI.Models.File>> FileList()
        {
            var FileList = await _context.Files.Include(s => s.Project).Include(s => s.UploadedByNavigation).ToListAsync();
            return FileList;
        }
        #endregion

        #region GetFileById
        public async Task<StudentProjectManagementAPI.Models.File> GetFileById(int fileId)
        {
            StudentProjectManagementAPI.Models.File file = await _context.Files.Include(s => s.Project).Include(s => s.UploadedByNavigation).FirstOrDefaultAsync(s => s.FileId == fileId);
            if (file == null)
            {
                return null;
            }
            return file;
        }
        #endregion

        #region AddFile
        public async Task<bool> AddFile(StudentProjectManagementAPI.Models.File file)
        {
            _context.Files.Add(file);
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

        #region UpdateFile
        public async Task<bool> UpdateFile(StudentProjectManagementAPI.Models.File file)
        {
            _context.Files.Update(file);
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

        #region DeleteFile
        public async Task<bool> DeleteFile(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file == null)
            {
                return false;
            }
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
