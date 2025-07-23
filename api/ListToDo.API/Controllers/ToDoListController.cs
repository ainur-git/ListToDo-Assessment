using Asp.Versioning;
using ListToDo.Application.Interfaces;
using ListToDo.Application.Services;
using ListToDo.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
        public class ToDoListController : ControllerBase
    {
        private readonly IToDoListServices _toDoListServices;

        public ToDoListController(IToDoListServices toDoListServices)
        {
            this._toDoListServices = toDoListServices;
        }

        [HttpGet("info")]
        public IActionResult GetInfo() => Ok("This is ListController v1.0");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListReadDto>>> GetAllResult()
        {
            var lists = await _toDoListServices.GetAllItemAsync();
            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoListReadDto>> GetItemById(int id)
        {
            var list = await _toDoListServices.GetItemByIdAsync(id);
            if (list == null) return NotFound();
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
            try
            {
                var created = await _toDoListServices.CreateItemAsync(dto);
                return CreatedAtAction(nameof(GetItemById), new { id = created.ListToDoId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, ToDoListCreateDto dto)
        {
            if (id <= 0) return BadRequest("Invalid ID");

            try
            {
                var updated = await _toDoListServices.UpdateItemAsync(id, dto);
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
            var deleted = await _toDoListServices.DeleteItemAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
