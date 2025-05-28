using ListToDo.Application.DTO;
using ListToDo.Application.Interfaces;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ListToDo.Application.DTO.ItemDto;

namespace ListToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemServices _toDoItemServices;

        public ToDoItemController(IToDoItemServices toDoItemServices) 
        {
            this._toDoItemServices = toDoItemServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemReadDto>>> GetAllResult()
        {
            var response = await _toDoItemServices.GetAllItemAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemReadDto>> GetItemById(int id)
        {
            var item = await _toDoItemServices.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItemReadDto>> CreateItem(ToDoItemCreateDto dto)
        {
            try
            {
                var createdItem = await _toDoItemServices.CreateItemAsync(dto);
                return CreatedAtAction(nameof(GetItemById), new { id = createdItem.ItemToDoId }, createdItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ToDoItemCreateDto dto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            try
            {
                var updated = await _toDoItemServices.UpdateItemAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var deleted = await _toDoItemServices.DeleteItemAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
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
