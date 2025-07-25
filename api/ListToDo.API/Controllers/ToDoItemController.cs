using Asp.Versioning;
using ListToDo.Application.DTO;
using ListToDo.Application.Interfaces;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static ListToDo.Application.DTO.ItemDto;
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemServices _toDoItemServices;
        private readonly ILogger<ToDoItemController> _logger;

        public ToDoItemController(IToDoItemServices toDoItemServices, ILogger<ToDoItemController> logger) 
        {
            this._toDoItemServices = toDoItemServices;
            this._logger = logger;
        }

        [HttpGet("info")]
        public IActionResult GetInfo() => Ok("This is ItemController v1.0");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemReadDto>>> GetAllResult()
        {
            _logger.LogInformation("Fetching all item in To-Do List");
            var items = await _toDoItemServices.GetAllItemAsync();
            if (items == null || items.Count() == 0)
            {
                throw new Exception("No items found in the database.");
            }
            _logger.LogInformation("Successfully fetch all item");
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemReadDto>> GetItemById(int id)
        {
            _logger.LogInformation("Fetching item from ToDo List");
            var item = await _toDoItemServices.GetItemByIdAsync(id);
            if (item == null)
            {
                throw new Exception($"No items found with ID{id}.");
            }
            _logger.LogInformation($"Successfully fetched ID:{id} irems");
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemReadDto>> CreateItem(ToDoItemCreateDto dto)
        {
            _logger.LogInformation("Creating new item");
            var createdItem = await _toDoItemServices.CreateItemAsync(dto);
            _logger.LogInformation("Successfully creating new item");
            return CreatedAtAction(nameof(GetItemById), new { id = createdItem.ItemToDoId }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ToDoItemUpdateDto dto)
        {
            _logger.LogInformation("Updating the item");
            var updated = await _toDoItemServices.UpdateItemAsync(id, dto);
            _logger.LogInformation($"Successfully updating the item");
            return Ok(updated);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchItem(int id, [FromBody] JsonPatchDocument<ToDoItemUpdateDto> patchDoc)
        {
            _logger.LogInformation("{Patching the item");
            var updated = await _toDoItemServices.PatchItemAsync(id, patchDoc);
            _logger.LogInformation($"Patching updating the item");
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var deleted = await _toDoItemServices.DeleteItemAsync(id);
            if (!deleted)
            {
                throw new Exception("Item is not deleted");
            }
            _logger.LogInformation($"Successfully deleting the item");
            return Ok();
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> Complete(int id)
        {
            var completed = await _toDoItemServices.CompleteAsync(id);
            if (!completed) return NotFound();
            return NoContent();
        }
    }
}
