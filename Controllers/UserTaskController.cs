using Microsoft.AspNetCore.Mvc;
using TimeLog.Models;
using TimeLog.Services;
using MongoDB.Bson;

namespace TimeLog.Controllers;

[ApiController]
[Route("api/UserTasks")]
public class UserTasksController : ControllerBase
{
    private readonly UserService _userService;

    public UserTasksController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}/Tasks")]
    public ActionResult<List<UserTask>> GetTasks(string userId)
    {
        var tasks = _userService.GetTasks(userId);
        return tasks;
    }

    [HttpPost("{userId}/Tasks")]
    public ActionResult<UserTask> AddTask(string userId, UserTask task)
    {
        var addedTask = _userService.AddTask(userId, task);
        if (addedTask == null)
            return NotFound($"User with ID {userId} not found");

        return CreatedAtAction(nameof(GetTasks), new { userId }, addedTask);
    }

    [HttpPut("{userId}/Tasks/{taskId}")]
    public ActionResult<UserTask> UpdateTask(string userId, string taskId, UserTask task)
    {
        var updatedTask = _userService.UpdateTask(userId, taskId, task);
        if (updatedTask == null)
            return NotFound($"User or Task not found");

        return updatedTask;
    }

    [HttpDelete("{userId}/Tasks/{taskId}")]
    public IActionResult DeleteTask(string userId, string taskId)
    {
        var result = _userService.RemoveTask(userId, taskId);
        if (!result)
            return NotFound($"User or Task not found");

        return NoContent();
    }
} 