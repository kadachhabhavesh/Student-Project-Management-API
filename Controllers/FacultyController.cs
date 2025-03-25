using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FacultyController : ControllerBase
    {
        private readonly FacultyRepository _facultyRepository;

        #region FacultyController
        public FacultyController(FacultyRepository facultyRepository)
        {
            _facultyRepository = facultyRepository;
        }
        #endregion

        #region FacultyList
        [HttpGet]
        public async Task<ActionResult> FacultyList()
        {
            var facultyListList = await _facultyRepository.FacultyList();
            return Ok(facultyListList);
        }
        #endregion

        #region GetFacultyById
        [HttpGet("{facultyId}")]
        public async Task<ActionResult> GetFaculty(int facultyId)
        {
            var faculty = await _facultyRepository.GetFacultyById(facultyId);
            if (faculty == null)
            {
                return NotFound();
            }
            return Ok(faculty);
        }
        #endregion

        #region AddFaculty
        [HttpPost]
        public async Task<ActionResult> PostFaculty(Faculty faculty)
        {
            bool isInserted = await _facultyRepository.AddFaculty(faculty);
            if (isInserted) return Ok(new { message = "faculty inserted successfuly", faculty });
            else return BadRequest();
        }
        #endregion

        #region UpdateFaculty
        [HttpPut("{facultyId}")]
        public async Task<IActionResult> PutFaculty(int facultyId,Faculty faculty)
        {
            bool isUpdated = await _facultyRepository.UpdateFaculty(faculty);
            if (isUpdated) return Ok(new { message = "faculty updated successfuly", faculty });
            else return BadRequest();
        }
        #endregion

        #region DeleteFaculty
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteFaculty(int studentId)
        {
            var isDeleted = await _facultyRepository.Deletefaculty(studentId);
            if (isDeleted) return Ok(new { message = "student deleted successfuly", studentId });
            return NoContent();
        }
        #endregion

        #region MentorDropDown
        [HttpGet("MentorDropDown")]
        public IActionResult MentorDropDown()
        {
            var MentorDropDown = _facultyRepository.MentorDropDown();
            return Ok(MentorDropDown);
        }
        #endregion#region

        #region FacultyProfile
        [HttpGet("FacultyProfile/{facultyId}")]
        public async Task<IActionResult> FacultyProfile(int facultyId)
        {
            try
            {
                var faculty = await _facultyRepository.FacultyProfile(facultyId);
                return Ok(faculty);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
