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
using static ListToDo.Application.DTO.ListDto;

namespace ListToDo.Application.Services
{
    public class ToDoListServices : IToDoListServices 
    {
        private readonly ToDoDbContext _context;
        public ToDoListServices(ToDoDbContext context)
        {
            _context = context;
        }

        public ToDoList MapToEntity(ToDoListCreateDto dto)
        {
            return new ToDoList
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }

        public ToDoListReadDto MapToReadDto(ToDoList entity)
        {
            return new ToDoListReadDto
            {
                ListToDoId = entity.ListToDoId,
                Title = entity.Title,
                Description = entity.Description,
                Items = entity.Items?.Select(item => new ToDoItemReadDto
                {
                    ItemToDoId = item.ItemToDoId,
                    ListToDoId = item.ListToDoId,
                    Title = item.Title,
                    Description = item.Description,
                    DueDate = item.DueDate,
                    IsCompleted = item.IsCompleted
                }).ToList() ?? new List<ToDoItemReadDto>()
            };
        }

        public async Task<IEnumerable<ToDoListReadDto>> GetAllItemAsync()
        {
            var lists = await _context.ToDoLists
                .Include(l => l.Items)
                .ToListAsync();

            return lists.Select(MapToReadDto);
        }

        public async Task<ToDoListReadDto?> GetItemByIdAsync(int id)
        {
            var list = await _context.ToDoLists
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.ListToDoId == id);

            return list == null ? null : MapToReadDto(list);
        }

        public async Task<ToDoListReadDto> CreateItemAsync(ToDoListCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            var entity = MapToEntity(dto);
            _context.ToDoLists.Add(entity);
            await _context.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<bool> UpdateItemAsync(int id, ToDoListCreateDto dto)
        {
            var entity = await _context.ToDoLists.FindAsync(id);
            if (entity == null) return false;

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            entity.Title = dto.Title;
            entity.Description = dto.Description;

            _context.ToDoLists.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var entity = await _context.ToDoLists.FindAsync(id);
            if (entity == null) return false;

            _context.ToDoLists.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
