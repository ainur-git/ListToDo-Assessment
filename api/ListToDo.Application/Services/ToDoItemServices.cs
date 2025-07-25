using ListToDo.Application.DTO;
using ListToDo.Application.Interfaces;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ItemDto;
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.Application.Services
{
    public class ToDoItemServices : IToDoItemServices
    {
        private readonly ToDoDbContext _context;

        public ToDoItemServices(ToDoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoItemReadDto>> GetAllItemAsync()
        {
            var entities = await _context.ToDoItems.ToListAsync();
            return entities.Adapt<List<ToDoItemReadDto>>();
        }

        public async Task<ToDoItemReadDto> GetItemByIdAsync(int id)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null) return null;
            {
                return entity.Adapt<ToDoItemReadDto>();
            }
        }

        public async Task<ToDoItemReadDto> CreateItemAsync(ToDoItemCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            var entity = dto.Adapt<ToDoItem>();

            _context.ToDoItems.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<ToDoItemReadDto>();
        }

        public async Task<ToDoItemReadDto> UpdateItemAsync(int id, ToDoItemUpdateDto dto)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"No item List with ID{id}");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            // Update properties
            dto.Adapt(entity);

            _context.ToDoItems.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<ToDoItemReadDto>();
        }

        public async Task<ToDoItemReadDto> PatchItemAsync(int id, JsonPatchDocument<ToDoItemUpdateDto> patchDoc)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"No item with ID{id}");
            }
            var dto = entity.Adapt<ToDoItemUpdateDto>();
            patchDoc.ApplyTo(dto);

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            // Update properties
            dto.Adapt(entity);

            _context.ToDoItems.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Adapt<ToDoItemReadDto>();
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null)
            {
                throw new ArgumentException($"No To-Do List with ID{id}");
            }

            _context.ToDoItems.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null) return false;

            entity.IsCompleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
