using ListToDo.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ItemDto;

namespace ListToDo.Application.Interfaces
{
    public interface IToDoItemServices
    {
        Task<IEnumerable<ToDoItemReadDto>> GetAllItemAsync();
        Task<ToDoItemReadDto> GetItemByIdAsync(int id);
        Task<ToDoItemReadDto> CreateItemAsync(ToDoItemCreateDto dto);
        Task<ToDoItemReadDto> UpdateItemAsync(int id, ToDoItemUpdateDto dto);
        Task<ToDoItemReadDto> PatchItemAsync(int id, JsonPatchDocument<ToDoItemUpdateDto> patchDoc);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> CompleteAsync(int id);

    }
}
