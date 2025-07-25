using ListToDo.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.Application.Interfaces
{
    public interface IToDoListServices
    {
        Task<IEnumerable<ToDoListReadDto>> GetAllItemAsync();
        Task<ToDoListReadDto> GetItemByIdAsync(int id);
        Task<IEnumerable<ToDoListReadDto>> GetItemByTitle(string title);
        Task<ToDoListReadDto> CreateItemAsync(ToDoListCreateDto dto);
        Task<ToDoListReadDto> UpdateItemAsync(int id, ToDoListUpdateDto dto);
        Task<ToDoListReadDto> PatchItemAsync(int id, JsonPatchDocument<ToDoListUpdateDto> patchDoc);
        Task<bool> DeleteItemAsync(int id);
    }
}
