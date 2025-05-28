using ListToDo.Application.Interfaces;
using ListToDo.Core.Entities;
using ListToDo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListToDo.Application.DTO.ItemDto;

namespace ListToDo.Application.Services
{
    public class ToDoItemServices : IToDoItemServices
    {
        private readonly ToDoDbContext _context;

        public ToDoItemServices(ToDoDbContext context)
        {
            _context = context;
        }

        public ToDoItem MapToEntity(ToDoItemCreateDto dto)
        {
            return new ToDoItem
            {
                ListToDoId = dto.ListToDoId,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                IsCompleted = dto.IsCompleted
            };
        }

        public ToDoItemReadDto MapToReadDto(ToDoItem entity)
        {
            return new ToDoItemReadDto
            {
                ItemToDoId = entity.ItemToDoId,
                ListToDoId = entity.ListToDoId,
                Title = entity.Title,
                Description = entity.Description,
                DueDate = entity.DueDate,
                IsCompleted = entity.IsCompleted
            };
        }

        public async Task<IEnumerable<ToDoItemReadDto>> GetAllItemAsync()
        {
            var entities = await _context.ToDoItems.ToListAsync();
            return entities.Select(MapToReadDto);
        }

        public async Task<ToDoItemReadDto> GetItemByIdAsync(int id)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null) return null;
            return MapToReadDto(entity);
        }

        public async Task<ToDoItemReadDto> CreateItemAsync(ToDoItemCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            var entity = MapToEntity(dto);

            _context.ToDoItems.Add(entity);
            await _context.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<bool> UpdateItemAsync(int id, ToDoItemCreateDto dto)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null) return false;

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty");
            }

            // Update properties
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.DueDate = dto.DueDate;
            entity.IsCompleted = dto.IsCompleted;
            entity.ListToDoId = dto.ListToDoId;

            _context.ToDoItems.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var entity = await _context.ToDoItems.FindAsync(id);
            if (entity == null) return false;

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
