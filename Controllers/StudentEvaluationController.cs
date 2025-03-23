using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentEvaluationController : ControllerBase
    {
        private readonly StudentEvaluationRepository _studentEvaluationRepository;

        #region StudentEvaluationController
        public StudentEvaluationController(StudentEvaluationRepository studentEvaluationRepository)
        {
            _studentEvaluationRepository = studentEvaluationRepository;
        }
        #endregion

        #region StudentEvaluationList
        [HttpGet]
        public async Task<ActionResult> StudentEvaluationList()
        {
            var studentEvaluationList = await _studentEvaluationRepository.StudentEvaluationList();
            return Ok(studentEvaluationList);
        }
        #endregion

        #region GetStudentEvaluationById
        [HttpGet("{studentEvaluationId}")]
        public async Task<ActionResult> GetStudent(int studentEvaluationId)
        {
            StudentEvaluation studentEvaluation = await _studentEvaluationRepository.GetStudentEvaluationById(studentEvaluationId);
            if (studentEvaluation == null)
            {
                return NotFound();
            }
            return Ok(studentEvaluation);
        }
        #endregion

        #region AddStudentEvaluation
        [HttpPost]
        public async Task<ActionResult> PostStudentEvaluation(StudentEvaluation studentEvaluation)
        {
            bool isInserted = await _studentEvaluationRepository.AddStudentEvaluation(studentEvaluation);
            if (isInserted) return Ok(new { message = "StudentEvaluation inserted successfuly", studentEvaluation });
            else return BadRequest();
        }
        #endregion

        #region UpdateStudentEvaluation
        [HttpPut("{studentEvaluationId}")]
        public async Task<IActionResult> PutStudent(int studentEvaluationId, StudentEvaluation studentEvaluation)
        {
            bool isUpdated = await _studentEvaluationRepository.UpdateStudentEvaluation(studentEvaluation);
            if (isUpdated) return Ok(new { message = "StudentEvaluation updated successfuly", studentEvaluation });
            else return BadRequest();
        }
        #endregion

        #region DeleteStudentEvaluation
        [HttpDelete("{studentEvaluationId}")]
        public async Task<IActionResult> DeleteStudentEvaluation(int studentEvaluationId)
        {
            var isDeleted = await _studentEvaluationRepository.DeleteStudentEvaluation(studentEvaluationId);
            if (isDeleted) return Ok(new { message = "StudentEvaluation deleted successfuly", studentEvaluationId });
            return NoContent();
        }
        #endregion
    }
}
