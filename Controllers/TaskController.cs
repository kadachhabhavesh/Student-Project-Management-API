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
    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;

        #region TaskController
        public TaskController(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        #endregion

        #region TaskList
        [HttpGet]
        public async Task<ActionResult> TaskList()
        {
            var taskList = await _taskRepository.TaskList();
            return Ok(taskList);
        }
        #endregion

        #region GetTaskById
        [HttpGet("{taskId}")]
        public async Task<ActionResult> GetTask(int taskId)
        {
            StudentProjectManagementAPI.Models.Task task = await _taskRepository.GetTaskById(taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        #endregion

        #region AddTask
        [HttpPost]
        public async Task<ActionResult> PostTask(StudentProjectManagementAPI.Models.Task task)
        {
            Console.WriteLine(":::::::::::::::::::::: call-0");
            bool isInserted = await _taskRepository.AddTask(task);
            if (isInserted) return Ok(new { message = "Task inserted successfuly", task });
            else return BadRequest();
        }
        #endregion

        #region UpdateTask
        [HttpPut("{taskId}")]
        public async Task<IActionResult> PutTask(int taskId,StudentProjectManagementAPI.Models.Task task)
        {
            bool isUpdated = await _taskRepository.UpdateTask(task);
            if (isUpdated) return Ok(new { message = "Task updated successfuly", task });
            else return BadRequest();
        }
        #endregion

        #region DeleteTask
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var isDeleted = await _taskRepository.DeleteTask(taskId);
            if (isDeleted) return Ok(new { message = "student deleted successfuly", taskId });
            return NoContent();
        }
        #endregion
    }
}
