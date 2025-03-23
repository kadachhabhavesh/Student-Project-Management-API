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
    public class EvaluationController : ControllerBase
    {
        private readonly EvaluationRepository _evaluationRepository;

        #region EvaluationController
        public EvaluationController(EvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }
        #endregion

        #region EvaluationList
        [HttpGet]
        public async Task<ActionResult> EvaluationList()
        {
            var evaluationList = await _evaluationRepository.EvaluationsList();
            return Ok(evaluationList);
        }
        #endregion

        #region GetEvaluationById
        [HttpGet("{evaluationId}")]
        public async Task<ActionResult> GetEvaluation(int evaluationId)
        {
            Evaluation evaluation = await _evaluationRepository.GetEvaluationById(evaluationId);
            if (evaluation == null)
            { 
                return NotFound();
            }
            return Ok(evaluation);
        }
        #endregion

        #region AddEvaluation
        [HttpPost]
        public async Task<ActionResult> PostEvaluation(Evaluation evaluation)
        {
            bool isInserted = await _evaluationRepository.AddEvaluation(evaluation);
            if (isInserted) return Ok(new { message = "Evaluation inserted successfuly", evaluation });
            else return BadRequest();
        }
        #endregion

        #region UpdateEvaluation
        [HttpPut("{evaluationId}")]
        public async Task<IActionResult> PutEvaluation(int evaluationId,Evaluation evaluation)
        {
            bool isUpdated = await _evaluationRepository.UpdateEvaluation(evaluation);
            if (isUpdated) return Ok(new { message = "Evaluation updated successfuly", evaluation });
            else return BadRequest();
        }
        #endregion

        #region DeleteEvaluation
        [HttpDelete("{evaluationId}")]
        public async Task<IActionResult> DeleteEvaluation(int evaluationId)
        {
            var isDeleted = await _evaluationRepository.DeleteEvaluation(evaluationId);
            if (isDeleted) return Ok(new { message = "student deleted successfuly", evaluationId });
            return NoContent();
        }
        #endregion
    }
}
