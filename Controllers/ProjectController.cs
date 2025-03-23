using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectRepository _projectRepository;

        #region StudentsController
        public ProjectController(ProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        #endregion

        #region ProjectList
        [HttpGet]
        public async Task<ActionResult> ProjectList()
        {
            var projectList = await _projectRepository.ProjectList();
            return Ok(projectList);
        }
        #endregion

        #region GetProjectById
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetProject(int projectId)
        {
            StudentProjectManagementAPI.Models.Project project = await _projectRepository.GetProjectById(projectId);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
        #endregion

        #region AddProject
        [HttpPost]
        public async Task<ActionResult> PostProject(StudentProjectManagementAPI.Models.Project project)
        {
            bool isInserted = await _projectRepository.AddProject(project);
            if (isInserted) return Ok(new { message = "Project inserted successfuly", project });
            else return BadRequest();
        }
            #endregion

        #region UpdateProject
        [HttpPut("{projectId}")]
        public async Task<IActionResult> PutStudent(int projectId,StudentProjectManagementAPI.Models.Project project)
        {
        bool isUpdated = await _projectRepository.UpdateProject(project);
        if (isUpdated) return Ok(new { message = "Project updated successfuly", project });
            else return BadRequest();
        }
        #endregion

        #region DeleteProject
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var isDeleted = await _projectRepository.DeleteProject(projectId);
            if (isDeleted) return Ok(new { message = "Project deleted successfuly", projectId });
            return NoContent();
        }
        #endregion

        #region ProjectDetails
        [HttpGet("ProjectDetails/{projectId}")]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {
            var project = await _projectRepository.ProjectDetails(projectId);
            return Ok(project);
        }
        #endregion

        #region ProjectMembers
        [HttpGet("ProjectMembers/{projectId}")]
        public async Task<IActionResult> actionResult(int projectId)
        {
            try
            {
                var memberList = await _projectRepository.ProjectMembers(projectId);
                return Ok(memberList);
            }
            catch (Exception ex) { 
                return NotFound("Member Not found");
            }
        }
        #endregion

        #region StudentProjectList
        [HttpGet("StudentProjectList/{userId}")]
        public async Task<ActionResult> StudentProjectList(int userId)
        {
            var projectList = await _projectRepository.StudentProjectList(userId);
            return Ok(projectList);
        }
        #endregion

    }
}
