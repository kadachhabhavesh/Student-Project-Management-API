using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly FileRepository _fileRepository;

        #region FileController
        public FileController(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        #endregion

        #region FileList
        [HttpGet]
        public async Task<ActionResult> FileList()
        {
            var fileList = await _fileRepository.FileList();
            return Ok(fileList);
        }
        #endregion

        #region GetFileById
        [HttpGet("{fileId}")]
        public async Task<ActionResult> GetFile(int fileId)
        {
            StudentProjectManagementAPI.Models.File file = await _fileRepository.GetFileById(fileId);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }
        #endregion

        #region AddFile
        [HttpPost]
        public async Task<ActionResult> PostFile(StudentProjectManagementAPI.Models.File file)
        {
            bool isInserted = await _fileRepository.AddFile(file);
            if (isInserted) return Ok(new { message = "File inserted successfuly", file });
            else return BadRequest();
        }
        #endregion

        #region UpdateFile
        [HttpPut]
        public async Task<IActionResult> PutStudent(StudentProjectManagementAPI.Models.File file)
        {
            bool isUpdated = await _fileRepository.UpdateFile(file);
            if (isUpdated) return Ok(new { message = "File updated successfuly", file });
            else return BadRequest();
        }
        #endregion

        #region DeleteFile
        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            var isDeleted = await _fileRepository.DeleteFile(fileId);
            if (isDeleted) return Ok(new { message = "File deleted successfuly", fileId });
            return NoContent();
        }
        #endregion
    }
}
