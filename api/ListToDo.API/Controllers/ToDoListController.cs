using Asp.Versioning;
using ListToDo.Application.Interfaces;
using ListToDo.Application.Services;
using ListToDo.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static ListToDo.Application.DTO.ListDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ListToDo.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListServices _toDoListServices;
        private readonly ILogger<ToDoListController> _logger;

        public ToDoListController(IToDoListServices toDoListServices, ILogger<ToDoListController> logger)
        {
            this._toDoListServices = toDoListServices;
            this._logger = logger;
        }

        [HttpGet("info")]
        public IActionResult GetInfo() => Ok("This is ListController v1.0");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListReadDto>>> GetAllResult()
        {
            _logger.LogInformation("Fetching all ToDo lists");
            var lists = await _toDoListServices.GetAllItemAsync();
            if (lists == null || lists.Count() == 0)
            {
                throw new Exception("No To-Do List found in the database.");
            }
            _logger.LogInformation("Successfully fetched {Count} lists", lists.Count());
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoListReadDto>> GetItemById(int id)
        {
            _logger.LogInformation("Fetching ToDo lists");
            var list = await _toDoListServices.GetItemByIdAsync(id);
            if (list == null)
            {
                throw new Exception($"No To-Do List with ID{id}");
            }
            _logger.LogInformation($"Successfully fetched ID:{id} lists");
            return Ok(list);
        }

        [HttpGet("search")]
        public async Task<ActionResult<ToDoListReadDto>> GetItemByTitle([FromQuery] string title)
        {
            var res = await _toDoListServices.GetItemByTitle(title);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoListReadDto>> CreateItem(ToDoListCreateDto dto)
        {
            _logger.LogInformation("Creating new list");  
            var created = await _toDoListServices.CreateItemAsync(dto);
            _logger.LogInformation($"Successfully creating new list");
            return CreatedAtAction(nameof(GetItemById), new { id = created.ListToDoId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ToDoListUpdateDto dto)
        {
            _logger.LogInformation("Updating the list");
            var updated = await _toDoListServices.UpdateItemAsync(id, dto);
            _logger.LogInformation($"Successfully updating the list");
            return Ok(updated);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ToDoListReadDto>> PatchItem(int id, [FromBody] JsonPatchDocument<ToDoListUpdateDto> patchDoc)
        {
            _logger.LogInformation("Patching the list");
            var result = await _toDoListServices.PatchItemAsync(id, patchDoc);
            _logger.LogInformation($"Patching updating the list");
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            _logger.LogInformation("Deleting the list");
            var deleted = await _toDoListServices.DeleteItemAsync(id);
            if (!deleted)
            {
                throw new Exception("List is not deleted");
            }
            _logger.LogInformation($"Successfully deleting the list");
            return Ok();
        }
    }
}
