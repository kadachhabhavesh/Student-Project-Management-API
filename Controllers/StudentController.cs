using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly StudentRepository _studentRepository;

        #region StudentsController
        public StudentsController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        #endregion

        #region StudentList
        [HttpGet]
        public async Task<ActionResult> StudentList()
        {
            var studentList = await _studentRepository.StudentList();
            return Ok(studentList);
        }
        #endregion

        #region GetStudentById
        [HttpGet("{studentId}")]
        public async Task<ActionResult> GetStudent(int studentId)
        {
            Student student = await _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        #endregion

        #region AddStudent
        [HttpPost]
        public async Task<ActionResult> PostStudent(Student student)
        {
            bool isInserted = await _studentRepository.AddStudent(student);
            if(isInserted) return Ok(new { message="student inserted successfuly",student });
            else return BadRequest();
        }
        #endregion

        #region UpdateStudent
        [HttpPut("{studentId}")]
        public async Task<IActionResult> PutStudent(int studentId,Student student)
        {
            bool isUpdated = await _studentRepository.UpdateStudent(student);
            if (isUpdated) return Ok(new { message = "student updated successfuly", student });
            else return BadRequest();
        }
        #endregion

        #region DeleteStudent
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var isDeleted = await _studentRepository.DeleteStudent(studentId);
            if (isDeleted) return Ok(new { message = "student deleted successfuly", studentId });
            return NotFound();
        }
        #endregion

        #region StudentProfile
        [HttpGet("StudentProfile/{studentId}")]
        public async Task<IActionResult> StudentProfile(int studentId)
        {
            try
            {
                var student = await _studentRepository.StudentProfile(studentId);
                Console.WriteLine(student);
                return Ok(student);
            } catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}