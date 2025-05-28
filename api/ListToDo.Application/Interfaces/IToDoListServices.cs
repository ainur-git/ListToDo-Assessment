using ListToDo.Core.Entities;
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
        Task<ToDoListReadDto> CreateItemAsync(ToDoListCreateDto dto);
        Task<bool> UpdateItemAsync(int id, ToDoListCreateDto dto);
        Task<bool> DeleteItemAsync(int id);
    }
}
