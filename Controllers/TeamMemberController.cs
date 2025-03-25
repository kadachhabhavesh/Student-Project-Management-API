using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using StudentProjectManagementAPI.Data;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamMemberController : ControllerBase
    {
        private readonly TeamMemberRepository _teamMemberRepository;

        #region StudentsController
        public TeamMemberController(TeamMemberRepository TeamMemberRepository)
        {
            _teamMemberRepository = TeamMemberRepository;
        }
        #endregion

        #region TeamMemberList
        [HttpGet]
        public async Task<ActionResult> TeamMemberList()
        {
            var teamMemberList = await _teamMemberRepository.TeamMemberList();
            return Ok(teamMemberList);
        }
        #endregion

        #region GetTeamMemberById
        [HttpGet("{teamMemberId}")]
        public async Task<ActionResult> GetTeamMember(int teamMemberId)
        {
            TeamMember teamMember = await _teamMemberRepository.GetStudentById(teamMemberId);
            if (teamMember == null)
            {
                return NotFound();
            }
            return Ok(teamMember);
        }
        #endregion

        #region AddTeamMember
        [HttpPost]
        public async Task<ActionResult> PostTeamMember(TeamMember teamMember)
        {
            bool isInserted = await _teamMemberRepository.AddTeamMember(teamMember);
            if (isInserted) return Ok(new { message = "TeamMember inserted successfuly", teamMember });
            else return BadRequest();
        }
        #endregion

        #region UpdateTeamMember
        [HttpPut]
        public async Task<IActionResult> PutTeamMember(TeamMember teamMember)
        {
            bool isUpdated = await _teamMemberRepository.UpdateTeamMember(teamMember);
            if (isUpdated) return Ok(new { message = "TeamMember updated successfuly", teamMember });
            else return BadRequest();
        }
        #endregion

        #region DeleteTeamMember
        [HttpDelete("{teamMemberId}")]
        public async Task<IActionResult> DeleteTeamMember(int teamMemberId)
        {
            var isDeleted = await _teamMemberRepository.DeleteTeamMember(teamMemberId);
            if (isDeleted) return Ok(new { message = "TeamMember deleted successfuly", teamMemberId });
            return NoContent();
        }
        #endregion

        #region saveTeamMembers
        [HttpPost("saveTeamMembers")]
        public async Task<IActionResult> saveTeamMembers(int projectId, string membersIds)
        {
            var isSaved = await _teamMemberRepository.saveTeamMembers(projectId, membersIds);
            if (isSaved){
                return Ok(new { Message = "Team Members Saved successfully" });
            }
            else
            {
                return BadRequest(new { Message="member not available" });
            }
        }
        #endregion

        #region UpdateTeamMembers
        [HttpPut("UpdateTeamMembers")]
        public async Task<IActionResult> UpdateTeamMembers(int projectId, string membersIds)
        {
            var isSaved = await _teamMemberRepository.UpdateTeamMembers(projectId, membersIds);
            if (isSaved)
            {
                return Ok(new { Message = "Team Members Update successfully" });
            }
            else
            {
                return BadRequest(new { Message = "members not available" });
            }
        }
        #endregion
    }
}
