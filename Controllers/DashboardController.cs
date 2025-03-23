using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentProjectManagementAPI.Data;

namespace StudentProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepository;
        public DashboardController(DashboardRepository dashboardRepository)
        {
            this._dashboardRepository = dashboardRepository;
        }

        #region Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var obj = await _dashboardRepository.Dashboard();
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the dashboard data.", Details = ex.Message });
            }
        }
        #endregion

        #region FacultyDashboard
        [HttpGet("FacultyDashboard/{userId}")]
        public async Task<IActionResult> FacultyDashboard(int userId)
        {
            try
            {
                var obj = await _dashboardRepository.FacultyDashboard(userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the dashboard data.", Details = ex.Message });
            }
        }
        #endregion

        #region StudentDashboard
        [HttpGet("StudentDashboard/{userId}")]
        public async Task<IActionResult> StudentDashboard(int userId)
        {
            try
            {
                var obj = await _dashboardRepository.StudentDashboard(userId);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the dashboard data.", Details = ex.Message });
            }
        }
        #endregion
    }
}
